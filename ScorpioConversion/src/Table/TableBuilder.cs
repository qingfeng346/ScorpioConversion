
using System;
using System.Collections.Generic;
using System.Text;
using Scorpio;
using Scorpio.Commons;
using NPOI.SS.UserModel;

public class TableBuilder {
    private const string KEYWORD_PACKAGE = "/Package";                  //命名空间,不填默认为空
    private const string KEYWORD_FILENAME = "/FileName";                //导出data文件名字,不填默认文件或者sheet名字

    private const string KEYWORD_COMMENT = "/Comment";                  //注释
    private const string KEYWORD_NAME = "/Name";                        //字段名
    private const string KEYWORD_TYPE = "/Type";                        //字段类型
    private const string KEYWORD_ATTRIBUTE = "/Attribute";              //字段属性
    private const string KEYWORD_DEFAULT = "/Default";                  //字段默认值

    private const string KEYWORD_BEGIN = "/Begin";                      //数据开始
    private const string KEYWORD_END = "/End";                          //数据结束

    private string mPackageName = "";                                   //命名空间
    private string mName = "";                                          //导出data文件名字
    private string mSuffix = "";                                        //data文件后缀
    private List<string> mSpawns = new List<string>();                  //派生类

    private PackageParser mParser = null;                               //自定义类
    private List<FieldClass> mFields = new List<FieldClass>();          //Excel结构
    private List<RowData> mDatas = new List<RowData>();                 //Excel内容
    private List<string> mUsedCustoms = new List<string>();             //正在转换的表已使用的自定义类

    private string mDataDirectory;                                      //data文件输出目录
    private Dictionary<Language, string> mLanguageDirectory;            //所有语言输出目录

    public bool IsSpawn { get; set; }
    public string DataClassName { get; set; }
    public string TableClassName { get; set; }
    public TableBuilder SetSuffix(string value) { mSuffix = value; return this; }
    public TableBuilder SetName(string value) { mName = value; return this; }
    public TableBuilder SetPackageName(string value) { mPackageName = value; return this; }
    public TableBuilder SetSpawn(string value) { if (!value.IsEmptyString()) { mSpawns.AddRange(value.Split(",")); } return this; }
    public void Parse(ISheet sheet, string dataDirectory, Dictionary<Language, string> languageDirectory, PackageParser parser) {
        mParser = parser;
        mDataDirectory = dataDirectory;
        mLanguageDirectory = languageDirectory;
        LoadLayout(sheet);
        LoadData(sheet);
        CheckData();
        CreateDataFile();
        CreateLanguageFile();
    }
    //解析文件结构
    void LoadLayout(ISheet sheet) {
        mFields.Clear();
        for (var i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i) {
            var row = sheet.GetRow(i);
            if (row == null) { continue; }
            var keyCell = row.GetCellString(0);
            if (keyCell.IsEmptyString() || keyCell == KEYWORD_BEGIN || keyCell == KEYWORD_END) { continue; }
            if (keyCell == KEYWORD_PACKAGE) {
                mPackageName = row.GetCellString(1);
            } else if (keyCell == KEYWORD_FILENAME) {
                mName = row.GetCellString(1, mName);
            } else {
                ParseHead(keyCell, row);
            }
        }
    }
    FieldClass GetField(int index) {
        for (var i = mFields.Count; i <= index; ++i) {
            mFields.Add(new FieldClass(mParser) { Index = index });
        }
        return mFields[index];
    }
    //解析文件头
    void ParseHead(string key, IRow row) {
        for (var i = 1; i < row.LastCellNum; ++i) {
            var value = row.GetCellString(i);
            var field = GetField(i - 1);
            if (key == KEYWORD_NAME) {
                field.Name = value;
                if (value.IsInvalid()) {
                    field.Valid = false;
                }
            } else if (key == KEYWORD_TYPE) {
                if (value.IsArrayType()) {
                    field.Array = true;
                    field.Type = value.GetFinalType();
                } else {
                    field.Array = false;
                    field.Type = value;
                }
            } else if (key == KEYWORD_COMMENT) {
                field.Comment = value;
            } else if (key == KEYWORD_DEFAULT) {
                field.Default = value;
            } else if (key == KEYWORD_ATTRIBUTE) {
                Script script = new Script();
                script.LoadLibrary();
                script.LoadString(value);
                field.Attribute = script.GetGlobalTable();
            } else {
                throw new Exception($"不能识别的Key : {key}");
            }
        }
    }
    //解析文件数据
    void LoadData(ISheet sheet) {
        mDatas.Clear();
        var begin = false;
        for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i) {
            var row = sheet.GetRow(i);
            if (row == null) { continue; }
            var keyCell = row.GetCellString(0);
            if (keyCell == KEYWORD_BEGIN || keyCell == KEYWORD_END) {
                begin = keyCell == KEYWORD_BEGIN;
                ParseRow(row);
            } else if (begin) {
                ParseRow(row);
            }
        }
    }
    //解析一行数据
    void ParseRow(IRow row) {
        var data = new RowData() { RowNumber = row.RowNum + 1, Key = row.GetCellString(1) };
        if (data.Key.IsEmptyString()) {
            return;
        }
        for (var i = 0; i < mFields.Count; ++i) {
            var field = mFields[i];
            if (field.Valid) {
                var value = row.GetCellString(i + 1);
                data.Values.Add(new RowValue() { value = value.IsEmptyString() ? field.Default : value });
            }
        }
        mDatas.Add(data);
    }
    string GetClassMD5Code() {
        var builder = new StringBuilder();
        for (int i = 0; i < mFields.Count; ++i) {
            builder.Append(mFields[i].Type).Append(":");
            builder.Append(mFields[i].Array ? "1" : "0").Append(":");
        }
        return Scorpio.Commons.Util.GetMD5FromString(builder.ToString());
    }
    ValueList ReadValue(string value) {
        return new ValueParser($"[{value}]").GetObject() as ValueList;
    }
    //检测所有数据
    void CheckData() {
        mFields.RemoveAll((_) => { return !_.Valid; });
        if (mPackageName.IsEmptyString() || mName.IsEmptyString())
            throw new Exception($"PackageName 或 Name 不能为空  PackageName : {mPackageName}  Name : {mName}");
        var spawn = mSpawns.Find((str) => mName.StartsWith($"{str}_") );
        if (spawn.IsEmptyString()) {
            DataClassName = $"Data{mName}";
            TableClassName = $"Table{mName}";
        } else {
            IsSpawn = true;
            DataClassName = $"Data{spawn}";
            TableClassName = $"Table{spawn}";
        }
    }
    //生成data文件
    void CreateDataFile() {
        if (mDataDirectory.IsEmptyString())
            return;
        using (var writer = new TableWriter()) {
            writer.WriteInt32(mDatas.Count);            //数据数量
            writer.WriteString(GetClassMD5Code());      //文件结构MD5
            writer.WriteInt32(mFields.Count);           //字段数量
            foreach (var field in mFields) {
                if (field.IsBasic) {
                    writer.WriteInt8(0);
                    writer.WriteInt8((sbyte)field.BasicType.Index);
                } else {
                    writer.WriteInt8(1);
                    writer.WriteString(field.Type);
                }
                writer.WriteBool(field.Array);
            }
            writer.WriteInt32(0);                       //自定义类数量
            var keys = new List<string>();
            foreach (var data in mDatas) {
                if (keys.Contains(data.Key)) {
                    throw new Exception($"ID有重复项[{data.Key}], 行:[{data.RowNumber}]");
                }
                keys.Add(data.Key);
                for (var i = 0; i < mFields.Count; ++i) {
                    var field = mFields[i];
                    var value = data.Values[i].value;
                    if (!field.Array && (field.IsBasic || field.IsEnum)) {
                        WriteField(writer, new ValueString(value), field);
                    } else {
                        WriteField(writer, ReadValue(value), field);
                    }
                }
            }
            FileUtil.CreateFile(mName + "." + mSuffix, writer.ToArray(), mDataDirectory.Split(","));
        }
    }
    void WriteField(TableWriter writer, IValue value, FieldClass field) {
        if (field.IsBasic) {
            if (field.Array) {
                var list = value as ValueList;
                if (list.IsEmptyValue()) {
                    writer.WriteInt32(0);
                } else {
                    writer.WriteInt32(list.Count);
                    for (var i = 0; i < list.Count; ++i) {
                        field.BasicType.WriteValue(writer, list[i].Value);
                    }
                }
            } else {
                field.BasicType.WriteValue(writer, value.Value);
            }
        } else if (field.IsEnum) {
            if (field.Array) {
                var list = value as ValueList;
                if (list.IsEmptyValue()) {
                    writer.WriteInt32(0);
                } else {
                    writer.WriteInt32(list.Count);
                    for (var i = 0; i < list.Count; ++i) {
                        writer.WriteInt32(field.GetEnumValue(list[i].Value));
                    }
                }
            } else {
                writer.WriteInt32(field.GetEnumValue(value.Value));
            }
        } else {
            WriteCustom(writer, value as ValueList, field.CustomType, field.Array);
        }
    }
    void WriteCustom(TableWriter writer, ValueList list, PackageClass classes, bool array) {
        if (array) {
            if (list.IsEmptyValue()) {
                writer.WriteInt32(0);
            } else {
                writer.WriteInt32(list.Count);
                for (var i = 0; i < list.Count; ++i) {
                    WriteCustom(writer, list[i] as ValueList, classes, false);
                }
            }
        } else {
            if (list.IsEmptyValue()) {
                for (var i = 0; i < classes.Fields.Count; ++i)
                    WriteField(writer, new ValueString(""), classes.Fields[i]);
            } else {
                var count = list.Count;
                if (count != classes.Fields.Count)
                    throw new Exception($"填写字段数量与数据机构字段数量不一致 需要数量 {classes.Fields.Count}  填写数量{count}");
                for (var i = 0; i < count; ++i)
                    WriteField(writer, list[i], classes.Fields[i]);
            }
        }
    }
    void CreateLanguageFile() {
        foreach (var pair in mLanguageDirectory) {
            CreateDataClass(pair.Key, DataClassName, mFields, pair.Value);
            if (mParser != null) {
                foreach (var tab in mParser.Tables) {
                    CreateDataClass(pair.Key, tab.Key, tab.Value.Fields, pair.Value);
                }
                foreach (var en in mParser.Enums) {
                    CreateEnumClass(pair.Key, en.Value, pair.Value);
                }
            }
            {
                var extension = pair.Key.GetInfo().extension;
                var generate = Activator.CreateInstance(Type.GetType("GenerateTable" + pair.Key)) as IGenerate;
                generate.PackageName = mPackageName;
                generate.ClassName = TableClassName;
                generate.Language = pair.Key;
                generate.Package = new PackageClass() { Fields = mFields };
                var str = generate.Generate();
                str = str.Replace("__KeyType", mFields[0].GetLanguageType(pair.Key));
                str = str.Replace("__KeyName", mFields[0].Name);
                str = str.Replace("__TableName", TableClassName);
                str = str.Replace("__DataName", DataClassName);
                str = str.Replace("__MD5", GetClassMD5Code());
                var fileName = $"{TableClassName}.{extension}";
                if (pair.Key == Language.Java) {
                    fileName = string.Join("/", mPackageName.Split(".")) + "/" + fileName;
                }
                FileUtil.CreateFile(fileName, str, pair.Value.Split(","));
            }
        }
    }
    void CreateDataClass(Language language, string className, List<FieldClass> fields, string path) {
        var generate = Activator.CreateInstance(Type.GetType($"GenerateData{language}")) as IGenerate;
        generate.PackageName = mPackageName;
        generate.ClassName = className;
        generate.Language = language;
        generate.Package = new PackageClass() { Fields = fields };
        generate.Parameter = true;
        var fileName = $"{className}.{language.GetInfo().extension}";
        if (language == Language.Java) {
            fileName = string.Join("/", mPackageName.Split(".")) + "/" + fileName;
        }
        FileUtil.CreateFile(fileName, generate.Generate(), path.Split(","));
    }
    void CreateEnumClass(Language language, PackageEnum enums, string path) {
        var generate = Activator.CreateInstance(Type.GetType($"GenerateEnum{language}")) as IGenerate;
        generate.PackageName = mPackageName;
        generate.Language = language;
        generate.ClassName = enums.Name;
        generate.Package = enums;
        generate.Parameter = true;
        var fileName = $"{enums.Name}.{language.GetInfo().extension}";
        if (language == Language.Java) {
            fileName = string.Join("/", mPackageName.Split(".")) + "/" + fileName;
        }
        FileUtil.CreateFile(fileName, generate.Generate(), path.Split(","));
    }
}
