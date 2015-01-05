using System;
using System.Data;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
public partial class TableBuilder
{
    private List<PackageField> mFields = new List<PackageField>();              //列数组
    private Dictionary<string, List<PackageField>> mCustoms = new Dictionary<string, List<PackageField>>();             //所有自定义类
    private List<string> mUsedCustoms = new List<string>();                     //正在转换的表已使用的自定义类
    private int mMaxRow = -1;                                                   //最大行数
    private int mMaxColumn = -1;                                                //最大列数
    private string mPackage = "";                                               //Package名字
    private List<List<string>> mDataTable = new List<List<string>>();           //Excel表数据
    private string mStrFiler = "";                                              //文件名称(去掉后缀名)
    private bool mSpawns = false;                                               //是否是关键字类
    private string mSpawnsName = "";                                            //关键字名称
    private string mKeyName = "";                                               //ID名称（表第一个字段名字）
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
    //初始化此文件的配置
    private void Initialize(string fileName)
    {
        string[] spawnsList = Util.GetConfig(ConfigKey.SpawnList, ConfigFile.InitConfig).Split(';');
        mStrFiler = Path.GetFileNameWithoutExtension(fileName);
        string strSection = mStrFiler;
        foreach (string spawns in spawnsList)
        {
            if (string.IsNullOrEmpty(spawns)) continue;
            mSpawns = mStrFiler.StartsWith(spawns + "_");
            if (mSpawns)
            {
                mSpawnsName = spawns;
                strSection = spawns;
                break;
            }
        }
        mProgramInfos = new Dictionary<PROGRAM, ProgramInfo>();
        for (int i=(int)PROGRAM.NONE + 1;i < (int)PROGRAM.COUNT;++i)
        {
            PROGRAM program = (PROGRAM)i;
            var info = Util.GetProgramInfo(program);
            string code = Util.GetConfig(strSection, program.ToString() + ConfigKey.CodeDirectory, ConfigFile.TableConfig);
            string data = Util.GetConfig(strSection, program.ToString() + ConfigKey.DataDirectory, ConfigFile.TableConfig);
            string create = Util.GetConfig(strSection, program.ToString() + ConfigKey.Create, ConfigFile.TableConfig);
            ProgramInfo ret = info.Clone();
            ret.CodeDirectory = string.IsNullOrEmpty(code) ? info.CodeDirectory : info.CodeDirectory + "/" + code;
            ret.DataDirectory = string.IsNullOrEmpty(data) ? info.DataDirectory : info.DataDirectory + "/" + data;
            ret.Create = string.IsNullOrEmpty(create) ? info.Create : Util.ToBoolean(create);
            ret.Extension = info.Extension;
            mProgramInfos[program] = ret;
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
        for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i)
        {
            IRow row = sheet.GetRow(i);
            if (row == null) continue;
            ICell key = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (key == null || key.CellType == CellType.Blank)
                continue;
            List<string> values = new List<string>();
            for (int j = 0; j < (mMaxColumn == -1 ? row.LastCellNum : mMaxColumn); ++j)
            {
                ICell cell = row.GetCell(j, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                if (mMaxColumn == -1) {
                    if (cell == null || cell.CellType == CellType.Blank)
                        break;
                    cell.SetCellType(CellType.String);
                    string val = cell.StringCellValue;
                    if (string.IsNullOrEmpty(val) || val.Equals("Comment"))
                        break;
                    values.Add(val);
                } else {
                    cell.SetCellType(CellType.String);
                    string val = cell.StringCellValue;
                    values.Add(val);
                }
            }
            if (mMaxColumn == -1) mMaxColumn = values.Count;
            mDataTable.Add(values);
        }
        mMaxRow = mDataTable.Count;
    }
    //解析文件结构
    private void ParseLayout()
    {
        mFields.Clear();
        mUsedCustoms.Clear();
        for (int i = 0; i < mMaxColumn; ++i)
        {
            PackageField field = new PackageField();
            field.Index = i;
            field.Note = mDataTable[0][i];
            field.Name = mDataTable[1][i];
            string columnType = mDataTable[2][i];
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
            if (!basic && !mCustoms.ContainsKey(fieldType))
                throw new System.Exception(string.Format("第 {0:d} 列的字段类型不能识别 数据内容为 : \"{1}\"", i, columnType));
            if (i == 0 && (field.Array == true || field.Type != "int32"))
                throw new System.Exception("第一列的数据类型必须为int32类型");
            if (!basic) mUsedCustoms.Add(fieldType);
            mFields.Add(field);
        }
        if (mFields.Count == 0)
            throw new System.Exception("字段个数为0");
        mKeyName = mFields[0].Name;
    }
    public void Transform(List<string> fileNames, bool getManager)
    {
        try {
            if (fileNames == null || fileNames.Count <= 0) {
                Logger.error("请选择要转换的文件");
                return;
            }
            Util.InitializeProgram();
            mCustoms = Util.ParseStructure("Custom");
            fileNames.Sort();
            mNormalClasses.Clear();
            mSpawnsClasses.Clear();
            mPackage = Util.GetConfig(ConfigKey.PackageName, ConfigFile.InitConfig);
            Progress.Count = fileNames.Count;
            int ValidCount = 0;
            for (int i = 0; i < fileNames.Count; ++i)
            {
                string fileName = fileNames[i];
                Progress.Current = (i + 1);
                try {
                    Initialize(fileName);
                    LoadFile(fileName);
                    ParseLayout();
                    if (mSpawns) {
                        if (!mSpawnsClasses.ContainsKey(mSpawnsName))
                            mSpawnsClasses.Add(mSpawnsName, new SpawnsClass(mSpawnsName, TableClassName, mKeyName, GetClassMD5Code(), mProgramInfos));
                        else if (mSpawnsClasses[mSpawnsName].MD5 != GetClassMD5Code())
                            throw new System.Exception("关键字文件[" + fileName + "]结构跟之前文件不一致");
                        mSpawnsClasses[mSpawnsName].AddString(mStrFiler);
                    } else {
                        mNormalClasses.Add(new TableClass(mStrFiler, TableClassName, mKeyName, mProgramInfos));
                    }
                    Progress.Value = 0f;
                    Transform_impl();
                    ++ValidCount;
                } catch (System.Exception ex) {
                    string error = string.Format("转换{0}文件出错\r\n{1}", fileName, ex.ToString());
                    Logger.error(error);
                    MessageBox.Show(error);
                    continue;
                }
            }
            if (ValidCount > 0)
            {
                for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT; ++i)
                {
                    PROGRAM program = (PROGRAM)i;
                    ProgramInfo info = mProgramInfos[program];
                    if (getManager) info.CreateTableManager.Invoke(this, null);
                }
                Logger.warn("转换结束");
            }
        } catch (System.Exception ex) {
            string error = "转换文件出错 " + ex.ToString();
            Logger.error(error);
            MessageBox.Show(error);
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
    private void Transform_impl()
    {
        int iColums = mMaxColumn;                   //数据列数
        int iRows = mMaxRow;                        //数据行数
        TableWriter writer = new TableWriter();
        writer.WriteInt32(0);                       //写入行数(最后计算出有效数据然后写入)
        writer.WriteString(GetClassMD5Code());      //写入文件MD5码
        WriteFields(writer, mFields);               //写入表结构
        writer.WriteInt32(mUsedCustoms.Count);      //写入用到的自定义类数量
        foreach (var key in mUsedCustoms)
        {
            writer.WriteString(key);
            WriteFields(writer, mCustoms[key]);
        }
        List<string> keys = new List<string>();
        int count = 0;
        for (int i = 3; i < iRows; ++i) {
            for (int j = 0; j < iColums; ++j) {
                string value = mDataTable[i][j];
                try {
                    if (string.IsNullOrEmpty(value)) value = Util.EmptyString;    //数据为空默认成####(非法值)
                    if (j == 0) {
                        if (keys.Contains(value)) {
                            throw new Exception(string.Format("ID有重复项[{0}]", value));
                        } else if (Util.IsEmptyString(value)) {
                            throw new Exception("ID不能为####");
                        }
                        keys.Add(value);
                    }
                    var field = mFields[j];
                    var basic = BasicUtil.GetType(field.Type);
                    if (basic != null) {
                        if (field.Array) {
                            string[] strs = value.Split(',');
                            writer.WriteInt32(strs.Length);
                            foreach (var str in strs)
                                basic.WriteValue(writer, str);
                        } else {
                            basic.WriteValue(writer, value);
                        }
                    } else {
                        WriteCustom_impl(writer, ValueUtil.ReadValue(value) as ValueList, mCustoms[field.Type], field.Array);
                    }
                } catch (System.Exception ex) {
                    throw new SystemException(string.Format("[{0}]行[{1}]列出错 数据内容为[{2}] : {3}", i + 1, Util.GetLineName(j + 1), value, ex.ToString()));
                }
            }
            count++;
        }
        writer.Seek(0);
        writer.WriteInt32(count);
        Create_impl(writer.ToArray());
    }
    private void WriteCustom_impl(TableWriter writer, ValueList list, List<PackageField> fields, bool array)
    {
        if (list == null) throw new Exception("数据结构错误");
        if (array) {
            writer.WriteInt32(list.values.Count);
            for (int i = 0; i < list.values.Count;++i ) {
                WriteCustom_impl(writer, list.values[i] as ValueList, fields, false);
            }
        } else {
            for (int i = 0; i < list.values.Count;++i )
                WriteOneField(writer, list.values[i], fields[i]);
        }
    }
    private void WriteOneField(TableWriter writer, IValue value, PackageField field)
    {
        var basic = BasicUtil.GetType(field.Type);
        if (basic != null) {
            if (field.Array) {
                var vals = (value as ValueList).values;
                writer.WriteInt32(vals.Count);
                foreach (var v in vals)
                    basic.WriteValue(writer, (v as ValueString).value);
            } else {
                basic.WriteValue(writer, (value as ValueString).value);
            }
        } else {
            WriteCustom_impl(writer, value as ValueList, mCustoms[field.Type], field.Array);
        }
    }
    public void Create_impl(byte[] buffer)
    {
        byte[] bytes = GZipUtil.Compress(buffer);
        for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT; ++i)
        {
            PROGRAM program = (PROGRAM)i;
            ProgramInfo info = mProgramInfos[program];
            if (!info.Create) continue;
            FileUtil.CreateFile(mStrFiler + ".data", info.Compress ? bytes : buffer, false, info.DataDirectory.Split(';'));
            string strHead = info.HeadTemplate.Replace("__Package", mPackage);
            FileUtil.CreateFile(info.GetFile(TableClassName), strHead.Replace("__Content", GetTableClass(program)), info.Bom, info.CodeDirectory.Split(';'));
            CreateCustom(DataClassName, mPackage, mFields, info, strHead, true);
            foreach (var custom in mCustoms)
                CreateCustom(custom.Key, mPackage, custom.Value, info, strHead, false);
        }
    }
    /// <summary> 获得Table类的代码 </summary>
    private string GetTableClass(PROGRAM program)
    {
        var template = Util.GetProgramInfo(program).TableTemplate;
        template = template.Replace("__TableName", TableClassName);
        template = template.Replace("__DataName", DataClassName);
        template = template.Replace("__KeyName", mKeyName);
        template = template.Replace("__KeyType", BasicUtil.GetType(BasicEnum.INT32).GetCode(program));
        template = template.Replace("__MD5", GetClassMD5Code());
        return template.ToString();
    }
    private void CreateCustom(string name, string package, List<PackageField> fields, ProgramInfo info, string head, bool conID)
    {
        FileUtil.CreateFile(info.GetFile(name), head.Replace("__Content", info.GenerateTable.Generate(name, package, fields, conID)), info.Bom, info.CodeDirectory.Split(';'));
    }
}
