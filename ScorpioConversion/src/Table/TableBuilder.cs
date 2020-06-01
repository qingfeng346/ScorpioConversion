using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Scorpio.Commons;
using ScorpioConversion.Table;

public partial class TableBuilder {
    private const string KEYWORD_PACKAGE    = "/Package";       //命名空间,不填默认为空
    private const string KEYWORD_FILENAME   = "/FileName";      //导出data文件名字,不填默认文件或者sheet名字
    private const string KEYWORD_TAGS       = "/Tags";          //tags
    private const string KEYWORD_AUTOINC    = "/AutoInc";       //ID自动递增生成
    private const string KEYWORD_SPAWN      = "/Spawn";         //派生类
    private const string KEYWORD_GROUP      = "/Group";         //Group类型的表,值为Group的字段名
    private const string KEYWORD_GROUP_SORT = "/GroupSort";     //Group类型的排序字段

    private const string KEYWORD_COMMENT    = "/Comment";       //注释
    private const string KEYWORD_NAME       = "/Name";          //字段名
    private const string KEYWORD_TYPE       = "/Type";          //字段类型
    private const string KEYWORD_ATTRIBUTE  = "/Attribute";     //字段属性
    private const string KEYWORD_DEFAULT    = "/Default";       //字段默认值
    private const string KEYWORD_MAX        = "/Max";           //字段最大值
    private const string KEYWORD_MIN        = "/Min";           //字段最小值

    private const string KEYWORD_BEGIN      = "/Begin";         //数据开始
    private const string KEYWORD_END        = "/End";           //数据结束

    private PackageParser mParser = null; //自定义类
    private List<RowData> mDatas = new List<RowData>(); //Excel内容

    private string mDataDirectory; //data文件输出目录
    private Dictionary<Language, string> mLanguageDirectory; //所有语言输出目录

    public bool IsSpawn => !string.IsNullOrEmpty(Spawn);

    public string PackageName { get; private set; }                                 //命名空间
    public string DataClassName { get; private set; }                               //DataClass名字
    public string TableClassName { get; private set; }                              //TableClass名字
    public string DataFileName { get; private set; }                                //数据文件名字
    public string Spawn { get; private set; }                                       //派生类名字
    public string Group { get; private set; }                                       //是否是Group类型表
    public string GroupSort { get; private set; }                                   //Group类型的排序字段
    public List<string> Tags { get; private set; } = new List<string>();            //表标签列表
    public bool AutoInc { get; private set; } = false;                              //是否是自增长的表
    public string LayoutMD5 { get; private set; }                                   //文件结构MD5
    public int DataCount => mDatas.Count;                                           //数据数量
    public List<FieldClass> Fields { get; } = new List<FieldClass>();               //表结构
    public HashSet<IPackage> CustomTypes { get; } = new HashSet<IPackage>();        //自定义类,枚举列表

    public TableBuilder SetPackageName(string value) {
        PackageName = value;
        return this;
    }

    public TableBuilder SetName(string value) {
        DataFileName = value;
        return this;
    }

    public bool Parse(DataTable dataTable, IList<L10NData> l10nDatas) {
        mParser = Config.Parser;
        mDataDirectory = Config.DataDirectory;
        mLanguageDirectory = Config.LanguageDirectory;
        LoadLayout(dataTable);
        if (!CheckTags()) { return false; }
        LoadData(dataTable);
        CheckData();
        CreateDataFile(l10nDatas);
        CreateLanguageFile();
        return true;
    }

    //解析文件结构
    void LoadLayout(DataTable dataTable) {
        Fields.Clear();
        var rows = dataTable.Rows;
        for (var i = 0; i < rows.Count; ++i) {
            var row = rows[i];
            var keyCell = row[0].GetDataString().Trim();
            if (keyCell.IsEmptyString() || keyCell == KEYWORD_BEGIN || keyCell == KEYWORD_END) { continue; }
            var valueCell = row[1].GetDataString().Trim();
            switch (keyCell) {
                case KEYWORD_AUTOINC:       AutoInc = true; break;
                case KEYWORD_PACKAGE:       PackageName = valueCell; break;
                case KEYWORD_FILENAME:      DataFileName = valueCell; break;
                case KEYWORD_SPAWN:         Spawn = valueCell; break;
                case KEYWORD_GROUP:         Group = valueCell; break;
                case KEYWORD_GROUP_SORT:    GroupSort = valueCell; break;
                case KEYWORD_TAGS:          Tags = new List<string>(valueCell.Split(ScorpioConversion.Util.Separator)); break;
                default:                    ParseHead(keyCell, row); break;
            }
        }
    }
    FieldClass GetField(int index) {
        for (var i = Fields.Count; i <= index; ++i) {
            Fields.Add(new FieldClass(mParser));
        }
        return Fields[index];
    }
    //解析文件头
    void ParseHead(string key, DataRow row) {
        for (var i = 1; i < row.ItemArray.Length; ++i) {
            var value = row.ItemArray[i].GetDataString().Trim();
            var field = GetField(i - 1);
            switch (key) {
                case KEYWORD_MAX: if (!value.IsEmptyString()) field.MaxValue = value; break;
                case KEYWORD_MIN: if (!value.IsEmptyString()) field.MinValue = value; break;
                case KEYWORD_COMMENT: field.Comment = value; break;
                case KEYWORD_DEFAULT: field.Default = value; break;
                case KEYWORD_NAME: {
                        field.Name = value.ParseFlag(out var invalid, out var l10n, out var noOut);
                        field.IsInvalid |= invalid;
                        field.IsL10N |= l10n;
                        field.IsNoOut |= noOut;
                        break;
                    }
                case KEYWORD_TYPE: {
                        var type = value.ParseFlag(out var invalid, out var l10n, out var noOut);
                        field.IsInvalid |= invalid;
                        field.IsL10N |= l10n;
                        field.IsNoOut |= noOut;
                        if (type.IsArrayType()) {
                            field.IsArray = true;
                            field.Type = type.GetFinalType();
                        } else {
                            field.IsArray = false;
                            field.Type = type;
                        }
                        break;
                    }
                case KEYWORD_ATTRIBUTE:
                    //Script script = new Script();
                    //script.LoadLibrary();
                    //script.LoadString(value);
                    //field.Attribute = script.GetGlobalTable();
                    break;
                default:
                    if (key.StartsWith('/')) {
                        throw new Exception($"不能识别的Key : {key}");
                    }
                    break;
            }
        }
    }

    //检测所有 Tags
    bool CheckTags() {
        return Config.ContainsTags(Tags);
    }
    //解析文件数据
    void LoadData(DataTable dataTable) {
        mDatas.Clear();
        var begin = false;
        var rows = dataTable.Rows;
        for (var i = 0; i < rows.Count; ++i) {
            var row = rows[i];
            var keyCell = row[0].GetDataString().Trim();
            if (keyCell == KEYWORD_BEGIN || keyCell == KEYWORD_END) {
                begin = keyCell == KEYWORD_BEGIN;
                ParseRow(i, row);
            } else if (begin) {
                ParseRow(i, row);
            }
        }
    }
    //解析一行数据
    void ParseRow(int rowNumber, DataRow row) {
        var firstValue = row[1].GetDataString();
        //首列不填数据为无效行
        if (firstValue.IsEmptyString()) { return; }
        var data = new RowData() { RowNumber = rowNumber + 1 };
        if (!AutoInc) { data.Key = firstValue; }
        for (var i = 0; i < Fields.Count; ++i) {
            var field = Fields[i];
            if (!field.IsValid) { continue; }
            try {
                data.Values.Add(row[i + 1].GetDataString());
            } catch (Exception e) {
                throw new Exception($"列:{field.Name} 行:{rowNumber + 1} 解析出错 : {e.ToString()}");
            }
        }
        mDatas.Add(data);
    }

    //检测所有数据
    void CheckData() {
        for (var i = 0; i < Fields.Count;++i) {
            var field = Fields[i];
            if (field.IsInvalid) { continue; }
            if (!field.Name.IsEmptyString() && field.Type.IsEmptyString()) {
                throw new Exception($"列:{field.Name} 类型为空");
            } else if (field.Name.IsEmptyString() && !field.Type.IsEmptyString()) {
                throw new Exception($"列:{(i + 1).GetLineName()} 名字为空,类型为{field.Type}");
            }
        }
        //删除无效字段
        Fields.RemoveAll(field => !field.IsValid);
        if (AutoInc) {
            Fields.Insert(0, new FieldClass(mParser) { Name = "AutoID", Type = "int", Comment = "AutoID" });
            var autoID = 0;
            foreach (var data in mDatas) {
                data.Key = (autoID++).ToString();
                data.Values.Insert(0, data.Key);
            }
        }
        //计算文件结构MD5
        var builder = new StringBuilder();
        for (var i = 0; i < Fields.Count; ++i) {
            Fields[i].Index = i;
            builder.Append(Fields[i].Type).Append(":");
            builder.Append(Fields[i].IsArray ? "1" : "0").Append(":");
        }
        LayoutMD5 = Util.GetMD5FromString(builder.ToString());

        var errorL10N = Fields.FirstOrDefault(_ => _.IsL10N && !_.IsString && !_.IsArray);
        if (errorL10N != null) throw new InvalidDataException($"{errorL10N.Name}: L10N 字段只能为string类型");

        if (PackageName.IsEmptyString() || DataFileName.IsEmptyString())
            throw new Exception($"PackageName 或 Name 不能为空  PackageName : {PackageName}  Name : {DataFileName}");
        if (Fields.Count == 0)
            throw new Exception($"有效字段为0");

        if (Spawn.IsEmptyString()) {
            DataClassName = $"Data{DataFileName}";
            TableClassName = $"Table{DataFileName}";
        } else {
            if (Config.SpawnsList.TryGetValue(Spawn, out var spawnMD5) && spawnMD5 != LayoutMD5) {
                throw new Exception($"派生类结构不同 ${DataFileName}");
            }
            Config.SpawnsList[Spawn] = LayoutMD5;
            DataFileName = $"{Spawn}_{DataFileName}";
            DataClassName = $"Data{Spawn}";
            TableClassName = $"Table{Spawn}";
        }
        CustomTypes.Clear();
        Fields.ForEach((field) => {
            if (!field.IsBasic) {
                if (field.IsEnum) {
                    CustomTypes.Add(field.CustomEnum);
                } else {
                    CustomTypes.Add(field.CustomType);
                }
            }
        });
    }

    //生成data文件
    void CreateDataFile(ICollection<L10NData> l10NDatas) {
        if (mDataDirectory.IsEmptyString())
            return;
        using (var writer = new TableWriter()) {
            writer.WriteInt32(mDatas.Count);        //数据数量
            writer.WriteString(LayoutMD5);          //文件结构MD5
            writer.WriteClass(Fields);              //表结构
            writer.WriteInt32(CustomTypes.Count);   //自定义类数量
            foreach (var customType in CustomTypes) {
                if (customType is PackageEnum) {
                    writer.WriteString((customType as PackageEnum).Name);
                    writer.WriteInt8(1);
                    writer.WriteEnum((customType as PackageEnum).Fields);
                } else {
                    writer.WriteString((customType as PackageClass).Name);
                    writer.WriteInt8(2);
                    writer.WriteClass((customType as PackageClass).Fields);
                }
            }
            var keys = new List<string>();
            foreach (var data in mDatas) {
                if (keys.Contains(data.Key)) {
                    throw new Exception($"ID有重复项[{data.Key}], 行:[{data.RowNumber}]");
                }
                keys.Add(data.Key);
                for (var i = 0; i < Fields.Count; ++i) {
                    var field = Fields[i];
                    if (field.IsL10N) {
                        l10NDatas.Add(new L10NData {
                            Key = $"{this.DataFileName}.{data.Key}.{field.Name}", // skip the signature of '$'
                            Hint = data.Values[i]
                        });
                    }
                    try {
                        if (!field.MaxValue.IsEmptyString() &&
                            Convert.ToDouble(field.MaxValue) < Convert.ToDouble(data.Values[i])) {
                            throw new DataException($"行:{data.RowNumber} {field.Name} 值超过限定最大值");
                        }

                        if (!field.MinValue.IsEmptyString() &&
                            Convert.ToDouble(field.MinValue) > Convert.ToDouble(data.Values[i])) {
                            throw new DataException($"行:{data.RowNumber} {field.Name} 值低于限定最小值");
                        }
                        if (field.IsL10N && !Config.WriteL10N) {
                            field.Write(writer, new RowValue() { value = "" });
                        } else {
                            field.Write(writer, data.Values[i]);
                        }
                    } catch (Exception e) {
                        throw new Exception($"行:{data.RowNumber} 列:{field.Name}  {e.ToString()}");
                    }
                }
            }

            FileUtil.CreateFile($"{DataFileName}.{Config.Suffix}", writer.ToArray(),
                mDataDirectory.Split(ScorpioConversion.Util.Separator));
        }
    }

    //void WriteField(TableWriter writer, IValue value, FieldClass field) {
    //    if (field.IsBasic) {
    //        if (field.IsArray) {
    //            var list = value as ValueList;
    //            if (list.IsEmptyValue()) {
    //                writer.WriteInt32(0);
    //            } else {
    //                writer.WriteInt32(list.Count);
    //                for (var i = 0; i < list.Count; ++i) {
    //                    field.BasicType.WriteValue(writer, list[i].Value);
    //                }
    //            }
    //        } else {
    //            field.BasicType.WriteValue(writer, value.Value);
    //        }
    //    } else if (field.IsEnum) {
    //        if (field.IsArray) {
    //            var list = value as ValueList;
    //            if (list.IsEmptyValue()) {
    //                writer.WriteInt32(0);
    //            } else {
    //                writer.WriteInt32(list.Count);
    //                for (var i = 0; i < list.Count; ++i) {
    //                    writer.WriteInt32(field.GetEnumValue(list[i].Value));
    //                }
    //            }
    //        } else {
    //            writer.WriteInt32(field.GetEnumValue(value.Value));
    //        }
    //    } else {
    //        WriteCustom(writer, value as ValueList, field.CustomType, field.IsArray);
    //    }
    //}
    //void WriteCustom(TableWriter writer, ValueList list, PackageClass classes, bool array) {
    //    if (array) {
    //        if (list.IsEmptyValue()) {
    //            writer.WriteInt32(0);
    //        } else {
    //            writer.WriteInt32(list.Count);
    //            for (var i = 0; i < list.Count; ++i) {
    //                WriteCustom(writer, list[i] as ValueList, classes, false);
    //            }
    //        }
    //    } else {
    //        if (list.IsEmptyValue()) {
    //            for (var i = 0; i < classes.Fields.Count; ++i)
    //                WriteField(writer, new ValueString(""), classes.Fields[i]);
    //        } else {
    //            var count = list.Count;
    //            if (count != classes.Fields.Count)
    //                throw new Exception($"字段数量与{classes.Name}需求数量不一致 需要:{classes.Fields.Count} 填写数量:{count} ");
    //            for (var i = 0; i < count; ++i)
    //                WriteField(writer, list[i], classes.Fields[i]);
    //        }
    //    }
    //}
    void CreateLanguageFile() {
        foreach (var pair in mLanguageDirectory) {
            ScorpioConversion.Util.CreateDataClass(pair.Key, PackageName, DataClassName, Fields, pair.Value,
                Config.FileSuffix);
            ScorpioConversion.Util.CreateTableClass(pair.Key, PackageName, TableClassName, DataClassName, LayoutMD5,
                Fields, pair.Value, Config.FileSuffix);
        }
    }
}