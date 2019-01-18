using System;
using System.Collections.Generic;
using System.Text;
using Scorpio;
using NPOI.SS.UserModel;

public class TableBuilder {
    private const string KEYWORD_PACKAGE = "/Package";                  //命名空间,不填默认为空
    private const string KEYWORD_CLASSNAME = "/ClassName";              //生成类的名字,不填默认为传入的默认名字

    private const string KEYWORD_COMMENT = "/Comment";                  //注释
    private const string KEYWORD_NAME = "/Name";                        //字段名
    private const string KEYWORD_TYPE = "/Type";                        //字段类型
    private const string KEYWORD_ATTRIBUTE = "/Attribute";              //字段属性
    private const string KEYWORD_DEFAULT = "/Default";                  //字段默认值

    private const string KEYWORD_BEGIN = "/Begin";                      //数据开始
    private const string KEYWORD_END = "/End";                          //数据结束

    private string mClassName = "";                                     //生成类的名字
    private string mPackage = "";                                       //命名空间
    private PackageParser mParser = null;                               //自定义类
    private List<PackageField> mFields = new List<PackageField>();      //Excel结构
    private List<RowData> mDatas = new List<RowData>();                 //Excel内容
    private readonly List<string> mUsedCustoms = new List<string>();    //正在转换的表已使用的自定义类
    public void Parse(ISheet sheet, string className, bool isSpawn, PackageParser parser) {
        mClassName = string.IsNullOrWhiteSpace(className) ? sheet.SheetName : className;
        mParser = parser;
        LoadLayout(sheet);
        LoadData(sheet);
        mFields.RemoveAll((_) => { return !_.Valid; });
        CreateDataFile();
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
                mPackage = row.GetCellString(1);
            } else if (keyCell == KEYWORD_CLASSNAME) {
                mClassName = row.GetCellString(1, mClassName);
            } else {
                ParseHead(keyCell, row);
            }
        }
    }
    PackageField GetField(int index) {
        for (var i = mFields.Count; i <= index; ++i) {
            mFields.Add(new PackageField());
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
                field.Attribute = (script.LoadString($"return {{{value}}}") as ScriptTable) ?? script.CreateTable();
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
            if (mFields[i].Valid) {
                data.Values.Add(row.GetCellString(i + 1));
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
    void CreateDataFile() {
        var writer = new TableWriter();
        writer.WriteInt32(mDatas.Count);
        writer.WriteString(GetClassMD5Code());
        writer.WriteInt32(mFields.Count);
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
        var keys = new List<string>();
        foreach (var data in mDatas) {
            if (keys.Contains(data.Key)) {
                throw new Exception($"ID有重复项[{data.Key}], 行:[{data.RowNumber}]");
            }
            keys.Add(data.Key);
            for (var i = 0; i < mFields.Count; ++i) {
                var value = data.Values[i];

            }
        }
    }
}
