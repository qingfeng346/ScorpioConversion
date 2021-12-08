using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Scorpio.Commons;

public partial class TableBuilder {
    private const string KEYWORD_FILENAME   = "/FileName";      //名字,不填默认文件或者sheet名字
    private const string KEYWORD_TAGS       = "/Tags";          //标签,有标签时需要比较传入的标签是否导出
    private const string KEYWORD_AUTOINC    = "/AutoInc";       //ID自动递增生成
    private const string KEYWORD_PACKAGE    = "/Package";       //命名空间,不填默认使用Language配置
    private const string KEYWORD_SPAWN      = "/Spawn";         //是否是派生类
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

    private PackageParser mParser = null;                       //自定义类
    private List<RowData> mDatas = new List<RowData>();         //Excel内容

    public string Name { get; private set; }

    public string FileName { get; private set; }                                    //文件名字
    public List<string> Tags { get; private set; } = new List<string>();            //标签列表
    public bool AutoInc { get; private set; } = false;                              //是否是自增长的表
    public string PackageName { get; private set; }                                 //命名空间
    public string Spawn { get; private set; }                                       //派生类名字
    public bool IsSpawn => !string.IsNullOrEmpty(Spawn);
    public string Group { get; private set; }                                       //是否是Group类型表
    public string GroupSort { get; private set; }                                   //Group类型的排序字段
    public string LayoutMD5 { get; private set; }                                   //文件结构MD5
    public string Source { get; private set; }                                      //来源
    public int DataCount => mDatas.Count;                                           //数据数量
    public PackageClass PackageClass { get; private set; }                          //表结构
    public HashSet<IPackage> CustomTypes { get; } = new HashSet<IPackage>();        //自定义类,枚举列表

    public TableBuilder SetFileName(string name) { FileName = name; return this; }
    public TableBuilder SetSource(string source) { Source = source; return this; }

    public bool Parse(DataTable dataTable) {
        mParser = Config.Parser;
        LoadLayout(dataTable);
        if (!CheckTags()) { return false; }
        LoadData(dataTable);
        CheckData();
        Generate();
        return true;
    }

    //解析文件结构
    void LoadLayout(DataTable dataTable) {
        PackageClass = new PackageClass();
        var rows = dataTable.Rows;
        for (var i = 0; i < rows.Count; ++i) {
            var row = rows[i];
            var keyCell = row[0].GetDataString().Trim();
            if (keyCell.IsEmptyString() || keyCell == KEYWORD_BEGIN || keyCell == KEYWORD_END) { continue; }
            var valueCell = row[1].GetDataString().Trim();
            switch (keyCell) {
                case KEYWORD_FILENAME:      FileName = valueCell; break;
                case KEYWORD_AUTOINC:       AutoInc = true; break;
                case KEYWORD_PACKAGE:       PackageName = valueCell; break;
                case KEYWORD_SPAWN:         Spawn = valueCell; break;
                case KEYWORD_GROUP:         Group = valueCell; break;
                case KEYWORD_GROUP_SORT:    GroupSort = valueCell; break;
                case KEYWORD_TAGS:          Tags = new List<string>(Util.Split(valueCell)); break;
                default:                    ParseHead(keyCell, row); break;
            }
        }
    }
    ClassField GetField(int index) {
        for (var i = PackageClass.Fields.Count; i <= index; ++i) {
            PackageClass.Fields.Add(new ClassField(mParser));
        }
        return PackageClass.Fields[index];
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
                        field.Name = value.ParseFlag(out var invalid, out var l10n);
                        field.IsInvalid |= invalid;
                        field.IsL10N |= l10n;
                        break;
                    }
                case KEYWORD_TYPE: {
                        var type = value.ParseFlag(out var invalid, out var l10n);
                        field.IsInvalid |= invalid;
                        field.IsL10N |= l10n;
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
        for (var i = 0; i < PackageClass.Fields.Count; ++i) {
            var field = PackageClass.Fields[i];
            if (!field.IsValid) { continue; }
            try {
                data.Values.Add(row[i + 1].GetDataString());
            } catch (Exception e) {
                throw new Exception($"列:{field.Name} 行:{rowNumber + 1} 解析出错 : {e}");
            }
        }
        mDatas.Add(data);
    }

    //检测所有数据
    void CheckData() {
        //删除无效字段
        PackageClass.Name = FileName;
        PackageClass.Fields.RemoveAll(field => !field.IsValid);
        for (var i = 0; i < PackageClass.Fields.Count;++i) {
            var field = PackageClass.Fields[i];
            if (!field.Name.IsEmptyString() && field.Type.IsEmptyString()) {
                throw new Exception($"列:{(i + 1).GetLineName()}({field.Name}) 类型为空");
            } else if (field.Name.IsEmptyString() && !field.Type.IsEmptyString()) {
                throw new Exception($"列:{(i + 1).GetLineName()}名字为空,类型为{field.Type}");
            } else if (field.IsL10N && (!field.IsString || field.IsArray)) {
                throw new Exception($"列:{(i + 1).GetLineName()}({field.Name}) L10N字段只支持string类型");
            }
        }
        if (AutoInc) {
            PackageClass.Fields.Insert(0, new ClassField(mParser) { Name = "AutoID", Type = "int", Comment = "AutoID" });
            var autoID = 0;
            foreach (var data in mDatas) {
                data.Key = (autoID++).ToString();
                data.Values.Insert(0, data.Key);
            }
        }
        //计算文件结构MD5
        var builder = new StringBuilder();
        for (var i = 0; i < PackageClass.Fields.Count; ++i) {
            PackageClass.Fields[i].Index = i;
            builder.Append(PackageClass.Fields[i].Type).Append(":");
            builder.Append(PackageClass.Fields[i].IsArray ? "1" : "0").Append(":");
        }
        LayoutMD5 = ScorpioUtil.GetMD5FromString(builder.ToString());
        if (PackageClass.Fields.Count == 0) throw new Exception($"有效字段数量为0");
        if (Spawn.IsEmptyString()) {
            Name = FileName;
        } else {
            if (Config.SpawnsList.TryGetValue(Spawn, out var spawnMD5) && spawnMD5 != LayoutMD5) {
                throw new Exception("派生类结构不同");
            }
            Config.SpawnsList[Spawn] = LayoutMD5;
            Name = Spawn;
        }
        CustomTypes.Clear();
        PackageClass.Fields.ForEach((field) => {
            if (!field.IsBasic) {
                if (field.IsEnum) {
                    CustomTypes.Add(field.CustomEnum);
                } else {
                    CustomTypes.Add(field.CustomType);
                }
            }
        });
        if (string.IsNullOrEmpty(FileName)) {
            throw new Exception($"FileName 为空");
        }
    }
    void Generate() {
        var l10nDatas = Config.L10NDatas;
        using (var writer = new TableWriter()) {
            writer.WriteInt32(mDatas.Count);        //数据数量
            writer.WriteString(LayoutMD5);          //文件结构MD5
            writer.WriteClass(PackageClass.Fields);              //表结构
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
            var keys = new HashSet<string>();
            foreach (var data in mDatas) {
                if (keys.Contains(data.Key)) {
                    throw new Exception($"ID有重复项[{data.Key}], 行:[{data.RowNumber}]");
                }
                keys.Add(data.Key);
                for (var i = 0; i < PackageClass.Fields.Count; ++i) {
                    var field = PackageClass.Fields[i];
                    if (field.IsL10N) {
                        l10nDatas.Add(new L10NData {
                            Key = $"{this.Name}.{data.Key}.{field.Name}", // skip the signature of '$'
                            Hint = data.Values[i]
                        });
                    }
                    try {
                        if (!field.MaxValue.IsEmptyString() && Convert.ToDouble(field.MaxValue) < Convert.ToDouble(data.Values[i])) {
                            throw new DataException($"行:{data.RowNumber} {field.Name} 值超过限定最大值");
                        }
                        if (!field.MinValue.IsEmptyString() && Convert.ToDouble(field.MinValue) > Convert.ToDouble(data.Values[i])) {
                            throw new DataException($"行:{data.RowNumber} {field.Name} 值低于限定最小值");
                        }
                        if (field.IsL10N && !Config.WriteL10N) {
                            field.Write(writer, new RowValue() { value = "" });
                        } else {
                            field.Write(writer, data.Values[i]);
                        }
                    } catch (Exception e) {
                        var builder = new StringBuilder();
                        for (var m = 0; m < data.Values.Count; m ++) {
                            builder.Append(data.Values[m].value + ",");
                        }
                        throw new Exception($"列:{field.Name} 行:{data.RowNumber}({builder}) : {e}");
                    }
                }
            }
            Config.LanguageConfig.Generate(this, writer.ToArray());
        }
    }
}