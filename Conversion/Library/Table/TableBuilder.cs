using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Threading;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using Scorpio;
public partial class TableBuilder {
    public delegate void ExecuteFieldDelegate(PackageField field, string id, ref string value);
    private const int START_ROW = 5;        //数据起始行
    private class RowData {
        public int RowNumber = 0;
        public List<string> Values = new List<string>();
        public string this[int index] { get { return Values[index]; } }
    }
    private List<PackageField> mFields = new List<PackageField>();              //列数组
    private Dictionary<string, List<PackageField>> mCustoms = new Dictionary<string, List<PackageField>>();         //所有自定义类
    private Dictionary<string, List<PackageEnum>> mEnums = new Dictionary<string, List<PackageEnum>>();             //所有自定义枚举
    private List<string> mUsedCustoms = new List<string>();                     //正在转换的表已使用的自定义类
    private int mMaxRow = -1;                                                   //最大行数
    private int mMaxColumn = -1;                                                //最大列数
    private string mPackage = "";                                               //Package名字
    private string mSpawnList = "";                                             //关键字列表
    private List<RowData> mDataTable = new List<RowData>();                     //Excel表数据
    private string mStrFiler = "";                                              //文件名称(去掉后缀名)
    private bool mSpawns = false;                                               //是否是关键字类
    private string mSpawnsName = "";                                            //关键字名称
    private string mKeyName = "";                                               //ID名称（表第一个字段名字）
    public ExecuteFieldDelegate ExecuteField;
    public string Filer { get { return mStrFiler; } }
    private string TableClassName { get { return "Table" + (mSpawns ? mSpawnsName : mStrFiler); } }     //Table类名称
    private string DataClassName { get { return "Data" + (mSpawns ? mSpawnsName : mStrFiler); } }       //Data类名称
    private Dictionary<PROGRAM, ProgramInfo> mProgramInfos = new Dictionary<PROGRAM, ProgramInfo>();    //所有生成语言配置
    private List<TableClass> mNormalClasses = new List<TableClass>();                                   //所有普通table类
    private Dictionary<string, SpawnsClass> mSpawnsClasses = new Dictionary<string, SpawnsClass>();     //所有关键字table类

    private class TableClass
    {
        public string Filer { get; private set; }       //Filter
        public string Class { get; private set; }       //类名称
        public string KeyName { get; private set; }     //ID名称
        public Dictionary<PROGRAM, ProgramInfo> Info { get; private set; }
        public TableClass(string filer, string clazz, string keyName, Dictionary<PROGRAM, ProgramInfo> info)
        {
            Filer = filer;
            Class = clazz;
            KeyName = keyName;
            Info = info;
        }
        public bool IsCreate(PROGRAM program)
        {
            return Info[program].Create;
        }
    }
    private class SpawnsClass
    {
        public string Filer { get; private set; }       //Filter
        public string Class { get; private set; }       //类名称
        public string KeyName { get; private set; }     //ID名称
        public string MD5 { get; private set; }         //文件结构MD5
        public Dictionary<PROGRAM, ProgramInfo> Info { get; private set; }
        public List<string> Files = new List<string>(); //此关键字的所有文件
        public SpawnsClass(string filer, string clazz, string keyName, string md5, Dictionary<PROGRAM, ProgramInfo> info)
        {
            Filer = filer;
            Class = clazz;
            KeyName = keyName;
            MD5 = md5;
            Info = info;
        }
        public void AddString(string str)
        {
            Files.Add(str);
        }
        public bool IsCreate(PROGRAM program)
        {
            return Info[program].Create;
        }
    }
    private List<TableClass> GetNormalClasses(PROGRAM program)
    {
        List<TableClass> ret = new List<TableClass>();
        foreach (var clazz in mNormalClasses)
        {
            if (clazz.IsCreate(program)) ret.Add(clazz);
        }
        return ret;
    }
    private List<SpawnsClass> GetSpawnsClasses(PROGRAM program)
    {
        List<SpawnsClass> ret = new List<SpawnsClass>();
        foreach (var pair in mSpawnsClasses)
        {
            if (pair.Value.IsCreate(program)) ret.Add(pair.Value);
        }
        return ret;
    }
    private string GetClassMD5Code()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < mFields.Count; ++i)
        {
            builder.Append(mFields[i].Name).Append(":");
            builder.Append(mFields[i].Type).Append(":");
            builder.Append(mFields[i].Array ? "1" : "0").Append(":");
        }
        return FileUtil.GetMD5FromString(builder.ToString());
    }
    //获得枚举值
    private int GetEnumValue(string enumName, string value)
    {
        var enums = mEnums[enumName];
        foreach (var pair in enums) {
            if (pair.Name == value)
                return pair.Index;
        }
        throw new Exception(string.Format("枚举:{0} 找不到枚举值:{1}", enumName, value));
    }
    //获得枚举列表
    private string[] GetEnumList(string enumName)
    {
        var enums = mEnums[enumName];
        List<string> ret = new List<string>();
        foreach (var pair in enums)
            ret.Add(pair.Name);
        return ret.ToArray();
    }
    //获得枚举注释
    private string GetEnumComment(string enumName)
    {
        var enums = mEnums[enumName];
        string ret = "";
        foreach (var pair in enums)
            ret += (pair.Name + " = " + pair.Index + "\n");
        return ret;
    }
    //初始化此文件的配置
    private void Initialize(string fileName)
    {
        string[] spawnsList = mSpawnList.Split(';');
        mStrFiler = Path.GetFileNameWithoutExtension(fileName);
        string strSection = mStrFiler;
        foreach (string spawns in spawnsList) {
            if (string.IsNullOrEmpty(spawns)) continue;
            mSpawns = mStrFiler.StartsWith(spawns + "_");
            if (mSpawns) {
                mSpawnsName = spawns;
                strSection = spawns;
                break;
            }
        }
        mProgramInfos = new Dictionary<PROGRAM, ProgramInfo>();
        var infos = Util.GetProgramInfos();
        foreach (var pair in infos) {
            mProgramInfos[pair.Key] = pair.Value.Clone();
        }
    }
    //读取文件内容
    private void LoadFile(string fileFullName)
    {
        IWorkbook workbook = new HSSFWorkbook(new FileStream(fileFullName, FileMode.Open, FileAccess.Read));
        ISheet sheet = workbook.GetSheetAt(0);
        mDataTable.Clear();
        mMaxColumn = -1;
        mMaxRow = -1;
        for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i) {
            IRow row = sheet.GetRow(i);
            if (row == null) row = sheet.CreateRow(i);
            string firstValue = Util.GetCellString(row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK));
            if (i >= START_ROW && string.IsNullOrEmpty(firstValue)) continue;       //如果第一个值为空 则直接跳过
            List<string> values = new List<string>();
            for (int j = 0; j < (mMaxColumn == -1 ? row.LastCellNum : mMaxColumn); ++j) {
                string val = Util.GetCellString(row.GetCell(j, MissingCellPolicy.CREATE_NULL_AS_BLANK));
                if (mMaxColumn == -1 && (string.IsNullOrEmpty(val) || val.Equals("Comment")))
                    break;
                values.Add(val);
            }
            if (mMaxColumn == -1) mMaxColumn = values.Count;
            mDataTable.Add(new RowData() { RowNumber = i + 1, Values = values });
        }
        mMaxRow = mDataTable.Count;
    }
    //解析文件结构
    private void ParseLayout()
    {
        mFields.Clear();
        mUsedCustoms.Clear();
        for (int i = 0; i < mMaxColumn; ++i) {
            PackageField field = new PackageField();
            field.Index = i;
            field.Comment = mDataTable[0][i];       //第0行是注释
            field.Name = mDataTable[1][i];          //第1行是字段名
            field.Default = mDataTable[3][i];       //第3行是字段默认值
            var attribute = mDataTable[2][i];       //第2行是字段属性
            Scorpio.Script script = new Scorpio.Script();
            script.LoadLibrary();
            if (string.IsNullOrEmpty(attribute)) {
                field.Attribute = script.CreateTable();
            } else {
                field.Attribute = (script.LoadString("return {" + attribute + "}") as ScriptTable) ?? script.CreateTable();
            }
            string columnType = mDataTable[4][i];   //第4行是字段类型
            string fieldType = "";
            if (columnType.StartsWith(Util.ArrayString)) {
                field.Array = true;
                int iFinalIndex = Util.ArrayString.Length;
                fieldType = columnType.Substring(iFinalIndex, columnType.Length - iFinalIndex);
            } else {
                field.Array = false;
                fieldType = columnType;
            }
            field.Type = fieldType;
            bool basic = BasicUtil.HasType(fieldType);
            if (!basic && !mCustoms.ContainsKey(fieldType) && !mEnums.ContainsKey(fieldType))
                throw new System.Exception(string.Format("第 {0:d} 数据内容为 列的字段类型不能识别 : \"{1}\"", i, columnType));
            if (i == 0 && (field.Array == true || field.Info.BasicIndex != BasicEnum.INT32))
                throw new System.Exception("第一列的数据类型必须为int32类型");
            field.Enum = mEnums.ContainsKey(fieldType);
            //保存使用的自定义结构
            if (!basic && !field.Enum && !mUsedCustoms.Contains(fieldType))
                mUsedCustoms.Add(fieldType);
            mFields.Add(field);
        }
        if (mFields.Count == 0)
            throw new System.Exception("字段个数为0");
        mKeyName = mFields[0].Name;
    }
    //刷新表注释
    private void RefreshNote(string fileFullName)
    {
        IWorkbook workbook = new HSSFWorkbook(new FileStream(fileFullName, FileMode.Open, FileAccess.ReadWrite));
        ISheet sheet = workbook.GetSheetAt(0);
        sheet.CreateFreezePane(2, START_ROW);
        for (int i = 0; i < mFields.Count;++i ) {
            var filed = mFields[i];
            if (filed.Enum) {
                CellRangeAddressList reg = new CellRangeAddressList(START_ROW, 65535, i, i);
                var con = DVConstraint.CreateExplicitListConstraint(GetEnumList(filed.Type));
                HSSFDataValidation dataValidate = new HSSFDataValidation(reg, con);
                dataValidate.CreatePromptBox(filed.Type, GetEnumComment(filed.Type));
                sheet.AddValidationData(dataValidate);
            }
        }
        FileStream stream = new FileStream(fileFullName, FileMode.Create);
        workbook.Write(stream);
        stream.Close();
    }
    public void Transform(string files, string configPath, string package, string spawnList, bool generateManager, bool refreshNote, Dictionary<PROGRAM, ProgramConfig> programConfigs)
    {
        try {
            List<string> fileNames = new List<string>(files.Split(';'));
            if (fileNames.Count == 0)
                throw new Exception("请选择要转换的文件");
            configPath = FileUtil.GetFullPath(configPath);
            Util.InitializeProgram(programConfigs);
            Util.ParseStructure(configPath, null, mEnums, mCustoms, null, null, null);
            fileNames.Sort();
            mNormalClasses.Clear();
            mSpawnsClasses.Clear();
            mPackage = package;
            mSpawnList = spawnList;
            Progress.Count = fileNames.Count;
            int ValidCount = 0;
            for (int i = 0; i < fileNames.Count; ++i) {
                string fileName = fileNames[i];
                Logger.info("正在转换文件 {0}/{1} [{2}]", i + 1, fileNames.Count, fileName);
                string tempFileName = Util.CurrentDirectory + "temp.xls";
                FileUtil.DeleteFile(tempFileName);
                File.Copy(fileName, tempFileName);
                Progress.Current = (i + 1);
                try {
                    Initialize(fileName);
                    LoadFile(tempFileName);
                    ParseLayout();
                    if (refreshNote) RefreshNote(fileName);
                    if (mSpawns) {
                        if (!mSpawnsClasses.ContainsKey(mSpawnsName))
                            mSpawnsClasses.Add(mSpawnsName, new SpawnsClass(mSpawnsName, TableClassName, mKeyName, GetClassMD5Code(), mProgramInfos));
                        else if (mSpawnsClasses[mSpawnsName].MD5 != GetClassMD5Code())
                            throw new System.Exception("关键字文件[" + fileName + "]结构跟之前文件不一致");
                        mSpawnsClasses[mSpawnsName].AddString(mStrFiler);
                    } else {
                        mNormalClasses.Add(new TableClass(mStrFiler, TableClassName, mKeyName, mProgramInfos));
                    }
                    Transform_impl();
                    ++ValidCount;
                } catch (System.Exception ex) {
                    Logger.error(string.Format("转换{0}文件出错\r\n{1}", fileName, ex.ToString()));
                    continue;
                }
                FileUtil.DeleteFile(tempFileName);
            }
            if (ValidCount > 0) {
                if (generateManager) {
                    foreach (var pair in Util.GetProgramInfos()) {
                        var info = pair.Value;
                        if (info.Create) {
                            if (info.CreateTableManager == null) {
                                Logger.error("找不到生成TableManager的函数 " + pair.Key);
                            } else {
                                info.CreateTableManager.Invoke(this, null);
                            }
                        }
                    }
                }
            }
        } catch (System.Exception ex) {
            Logger.error("转换文件出错 " + ex.ToString());
        }
    }
    private void WriteFields(TableWriter writer, List<PackageField> fields)
    {
        writer.WriteInt32(fields.Count);
        for (int i = 0; i < fields.Count; ++i) {
            var field = fields[i];
            var basic = BasicUtil.GetType(field.Type);
            if (basic != null) {
                writer.WriteByte(0);
                writer.WriteByte((sbyte)basic.BasicIndex);
                writer.WriteBool(field.Array);
            } else {
                writer.WriteByte(1);
                writer.WriteString(field.Type);
                writer.WriteBool(field.Array);
            }
        }
    }
    //生成data文件数据
    private void Transform_impl() {
        int iColums = mMaxColumn;                   //数据列数
        int iRows = mMaxRow;                        //数据行数
        TableWriter writer = new TableWriter();
        writer.WriteInt32(0);                       //写入行数(最后计算出有效数据然后写入)
        writer.WriteString(GetClassMD5Code());      //写入文件MD5码
        WriteFields(writer, mFields);               //写入表结构
        writer.WriteInt32(mUsedCustoms.Count);      //写入用到的自定义类数量
        foreach (var key in mUsedCustoms) {
            writer.WriteString(key);
            WriteFields(writer, mCustoms[key]);
        }
        List<string> keys = new List<string>();
        int count = 0;
        for (int i = START_ROW; i < iRows; ++i) {
            string ID = "";
            for (int j = 0; j < iColums; ++j) {
                string value = mDataTable[i][j];
                var field = mFields[j];
                try {
                    if (string.IsNullOrEmpty(value)) {
                        value = string.IsNullOrEmpty(field.Default) ? Util.EmptyString : field.Default;    //数据为空设置为默认值
                    }
                    if (j == 0) {
                        if (keys.Contains(value)) {
                            throw new Exception(string.Format("ID有重复项[{0}]", value));
                        } else if (Util.IsEmptyString(value)) {
                            throw new Exception("ID字段不能为空");
                        }
                        ID = value;
                        keys.Add(value);
                    }
                    if (ExecuteField != null) ExecuteField(field, ID, ref value);
                    var basic = BasicUtil.GetType(field.Type);
                    if (basic != null || field.Enum) {
                        if (field.Array)
                            WriteField(writer, Util.ReadValue(mEnums, mDataTable[i].RowNumber, Util.GetLineName(j), value, false, true), field);
                        else
                            WriteField(writer, value, field);
                    } else {
                        WriteCustom(writer, Util.ReadValue(mEnums, mDataTable[i].RowNumber, Util.GetLineName(j), value, true, field.Array), mCustoms[field.Type], field.Array);
                    }
                } catch (System.Exception ex) {
                    throw new SystemException(string.Format("[{0}]行[{1}]列出错 数据内容为[{2}] : {3}", mDataTable[i].RowNumber, Util.GetLineName(j), value, ex.ToString()));
                }
            }
            count++;
        }
        writer.Seek(0);
        writer.WriteInt32(count);
        Create_impl(writer.ToArray());
    }
    private void WriteCustom(TableWriter writer, ScriptArray list, List<PackageField> fields, bool array)
    {
        if (array) {
            if (Util.IsEmptyValue(list)) {
                writer.WriteInt32(0);
            } else {
                int count = list.Count();
                writer.WriteInt32(count);
                for (int i = 0; i < count; ++i) {
                    WriteCustom(writer, list.GetValue(i) as ScriptArray, fields, false);
                }
            }
        } else {
            if (Util.IsEmptyValue(list)) {
                for (int i = 0; i < fields.Count; ++i)
                    WriteField(writer, "", fields[i]);
            } else {
                int count = list.Count();
                if (count != fields.Count)
                    throw new Exception(string.Format("填写字段数量与数据机构字段数量不一致 需要数量 {0}  填写数量{1}", fields.Count, count));
                for (int i = 0; i < count; ++i)
                    WriteField(writer, list.GetValue(i), fields[i]);
            }
        }
    }
    private void WriteField(TableWriter writer, object value, PackageField field)
    {
        var basic = BasicUtil.GetType(field.Type);
        if (basic != null) {
            if (field.Array) {
                var list = value as ScriptArray;
                if (Util.IsEmptyValue(list)) {
                    writer.WriteInt32(0);
                } else {
                    int count = list.Count();
                    writer.WriteInt32(count);
                    for (int i = 0;i < count; ++i) {
                        basic.WriteValue(writer, list.GetValue(i).ToString());
                    }
                }
            } else {
                basic.WriteValue(writer, value.ToString());
            }
        } else if (field.Enum) {
            if (field.Array) {
                var list = value as ScriptArray;
                if (Util.IsEmptyValue(list)) {
                    writer.WriteInt32(0);
                } else {
                    int count = list.Count();
                    writer.WriteInt32(count);
                    for (int i = 0; i < count; ++i) {
                        writer.WriteInt32(GetEnumValue(field.Type, value.ToString()));
                    }
                }
            } else {
                writer.WriteInt32(GetEnumValue(field.Type, value.ToString()));
            }
        } else {
            WriteCustom(writer, value as ScriptArray, mCustoms[field.Type], field.Array);
        }
    }
    private void Create_impl(byte[] buffer)
    {
        byte[] bytes = GZipUtil.Compress(buffer);
        foreach (var pair in mProgramInfos) {
            PROGRAM program = pair.Key;
            ProgramInfo info = pair.Value;
            if (!info.Create) continue;
            info.CreateData(mStrFiler, info.Compress ? bytes : buffer);
            CreateTable(info);
            CreateCustom(DataClassName, mPackage, mFields, info, true);
            foreach (var custom in mCustoms)
                CreateCustom(custom.Key, mPackage, custom.Value, info, false);
            foreach (var custom in mEnums)
                info.CreateFile(custom.Key, info.GenerateEnum.Generate(custom.Key, mPackage, custom.Value));
        }
    }
    /// <summary> 生成Table类的代码 </summary>
    private void CreateTable(ProgramInfo info) {
        var str = info.GenerateTable.Generate(mPackage);
        str = str.Replace("__TableName", TableClassName);
        str = str.Replace("__DataName", DataClassName);
        str = str.Replace("__KeyName", mKeyName);
        str = str.Replace("__KeyType", BasicUtil.GetType(BasicEnum.INT32).GetCode(info.Code));
        str = str.Replace("__MD5", GetClassMD5Code());
        info.CreateFile(TableClassName, str);
    }
    private void CreateCustom(string name, string package, List<PackageField> fields, ProgramInfo info, bool conID)
    {
        info.CreateFile(name, info.GenerateData.Generate(name, package, fields, conID));
    }
}
