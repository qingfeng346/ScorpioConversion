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
//字段数据
public class Variable
{
    public string strFieldType;                                 //字段类型
    public List<Element> fieldTypes = new List<Element>();      //字段类型数组
    public string strFieldName;                                 //字段名字
    public string strFieldNote;                                 //字段注释
    public Element eElement;                                    //变量类型
    public bool bArray;                                         //是否是array字段
}
//自定义类
public class CustomClass
{
    public List<Element> fieldTypes = new List<Element>();
    public CustomClass(List<Element> types)
    {
        fieldTypes = types;
    }
}
//多国语言
public class Language
{
    public string Key = "";             //Key值(等于 [表名]_[字段名]_[行ID])
    public string Value = "";           //多国语言值
    public int Row = -1;                //在Translation表所在行
    public int ChangType = 0;           //比较旧的Translation是否有改变
    public Language(string key, string value)
    {
        Key = key;
        Value = value;
    }
}
//一个表的多国语言
public class Table
{
    //Key 值
    public Dictionary<string, Language> Languages = new Dictionary<string, Language>();
}
public partial class TableManager
{
    public static TableManager instance = null;
    public static TableManager GetInstance()
    {
        if (instance == null)
            instance = new TableManager();
        return instance;
    }

    private Dictionary<String, CustomClass> mCustomClasses = new Dictionary<String, CustomClass>();
    private List<Variable> mArrayVariable = new List<Variable>();       //列数组
    private List<string> mTableEnumList = new List<string>();           //tableEnum
    private Dictionary<string, Table> mKeyWords = new Dictionary<string, Table>();           //多国语言数组
    private int mMaxRow = -1;                                           //最大行数
    private int mMaxColumn = -1;                                        //最大列数
    private List<List<string>> mDataTable = new List<List<string>>();   //Excel表数据
    public string mStrFiler = "";                                       //文件名称
    private string mStrTableClass = "";                                 //table类名称
    private string mStrDataClass = "";                                  //data类名称
    private string mStrFieldClass = "";                                 //field类名称
    private string mStrMD5Code = "";                                    //文件MD5码
    private List<TableClass> mClasses = new List<TableClass>();         //所有普通table类
    private Dictionary<string, SpawnsClass> mSpawnsClasses = new Dictionary<string, SpawnsClass>();     //所有关键字table类
    private bool mArray = false;                                        //是否是数组
    private bool mSpawns = false;                                       //是否是关键字类
    private string mSpawnsName = "";                                    //关键字名称
    private string mSpawnsFileName = "";                                //关键字文件名
    private string mKeyElement = "";                                    //Key值类型

    private bool mGetManager = false;                                   //是否生成Manager
    private bool mGetLanguage = false;                                  //是否生成多国语言文件
    private bool mGetCustom = false;                                    //是否生成自定义类
    private bool mGetBase = false;                                      //是否生成基础类

    private Dictionary<PROGRAM, ProgramInfo> mProgramInfos = new Dictionary<PROGRAM, ProgramInfo>();    //语言配置
    private class TableClass
    {
        public string strFiler;
        public string strTableClass;
        public Dictionary<PROGRAM, ProgramInfo> mProgramInfos = new Dictionary<PROGRAM, ProgramInfo>();
        public bool IsCreate(PROGRAM program)
        {
            return mProgramInfos[program].Create;
        }
        public TableClass(string filer, string tableClass, Dictionary<PROGRAM, ProgramInfo> infos)
        {
            strFiler = filer;
            strTableClass = tableClass;
            foreach (var pair in infos) {
                mProgramInfos[pair.Key] = pair.Value;
            }
        }
    }
    private class SpawnsClass
    {
        public string strMD5 = null;    //文件结构MD5
        public string strTableClass;    //table名字
        public Dictionary<PROGRAM, ProgramInfo> mProgramInfos = new Dictionary<PROGRAM, ProgramInfo>();
        public List<string> list = new List<string>();      //此关键字的所有文件
        public SpawnsClass(string tableClass, Dictionary<PROGRAM, ProgramInfo> infos)
        {
            strTableClass = tableClass;
            foreach (var pair in infos) {
                mProgramInfos[pair.Key] = pair.Value;
            }
        }
        public bool IsCreate(PROGRAM program)
        {
            return mProgramInfos[program].Create;
        }
        public void AddString(string str)
        {
            list.Add(str);
        }
    }
    private string GetClassMD5Code()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < mArrayVariable.Count; ++i)
        {
            builder.Append(mArrayVariable[i].bArray ? "1" : "0").Append(":");
            builder.Append(mArrayVariable[i].strFieldType).Append(":");
            builder.Append(mArrayVariable[i].fieldTypes.Count).Append(":");
            foreach (var element in mArrayVariable[i].fieldTypes) {
                builder.Append(element.Type).Append(":");
            }
            builder.Append(mArrayVariable[i].strFieldName).Append(",");
        }
        return FileUtil.GetMD5FromString(builder.ToString());
    }
    private void Initialize(string fileName)
    {
        string[] spawnsList = Util.GetConfig(ConfigKey.SpawnList, ConfigFile.InitConfig).Split(';');
        mStrFiler = Path.GetFileNameWithoutExtension(fileName);
        string strSection = mStrFiler;
        mSpawnsName = "";
        mSpawnsFileName = "";
        foreach (string spawns in spawnsList)
        {
            if (string.IsNullOrEmpty(spawns)) continue;
            mSpawns = mStrFiler.StartsWith(spawns + "_");
            if (mSpawns)
            {
                mSpawnsName = spawns;
                strSection = spawns;
                int startPosition = spawns.Length + 1;
                mSpawnsFileName = mStrFiler.Substring(startPosition, mStrFiler.Length - startPosition);
                break;
            }
        }
        mArray = Util.ToBoolean(Util.GetConfig(strSection, ConfigKey.Array, ConfigFile.TableConfig));
        mProgramInfos.Clear();
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
    public void Transform(List<string> fileNames,bool bGetManager, bool bGetLanguage, bool bGetCustom, bool bGetBase)
    {
        try {
            if (fileNames == null || fileNames.Count <= 0) {
                Logger.error("请选择要转换的文件");
                return;
            }
            Util.InitializeProgram();
            CodeProvider.GetInstance().Initialize();
            fileNames.Sort();
            Progress.Count = fileNames.Count;
            mGetManager = bGetManager;
            mGetLanguage = bGetLanguage;
            mGetCustom = bGetCustom;
            mGetBase = bGetBase;
            mClasses.Clear();
            mSpawnsClasses.Clear();
            mTableEnumList.Clear();
            mKeyWords.Clear();
            int ValidCount = 0;
            for (int i = 0; i < fileNames.Count; ++i)
            {
                Progress.Current = (i + 1);
                try {
                    string fileName = fileNames[i];
                    LoadFile(fileName);
                    Initialize(fileName);
                    if (mSpawns) {
                        mStrTableClass = "MT_Table_" + mSpawnsName;
                        mStrDataClass = "MT_Data_" + mSpawnsName;
                        mStrFieldClass = "Field" + mSpawnsName;
                        if (!mSpawnsClasses.ContainsKey(mSpawnsName))
                            mSpawnsClasses.Add(mSpawnsName, new SpawnsClass(mStrTableClass, mProgramInfos));
                        mSpawnsClasses[mSpawnsName].AddString(mStrFiler);
                    } else {
                        mStrTableClass = "MT_Table_" + mStrFiler;
                        mStrDataClass = "MT_Data_" + mStrFiler;
                        mStrFieldClass = "Field" + mStrFiler;
                        mClasses.Add(new TableClass(mStrFiler, mStrTableClass, mProgramInfos));
                    }
                    Transform_impl();
                    ++ValidCount;
                } catch (System.Exception ex) {
                    string error = string.Format("{0}文件出错\r\n{1}", fileNames[i], ex.ToString());
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
                    if (mGetManager && info.CreateManager != null)
                        info.CreateManager.Invoke(this, null);
                    if (mGetCustom && info.CreateCustom != null)
                        info.CreateCustom.Invoke(this, null);
                    if (mGetBase)
                    {
                        if (info.CreateBase != null)
                            info.CreateBase.Invoke(this, null);
                        if (info.CreateReader != null)
                            info.CreateReader.Invoke(this, null);
                    }
                }
                if (mGetLanguage)
                    CreateLanguage();
                Logger.warn("转换结束");
            }
        } catch (System.Exception ex) {
            Logger.error("转换文件出错 " + ex.ToString());
        }
    }
    private void Transform_impl()
    {
        mArrayVariable.Clear();
        mCustomClasses.Clear();
        for (int i = 0; i < mMaxColumn; ++i) {
            Variable variable = new Variable();
            for (int j = 0; j <= 2; ++j) {
                string str = mDataTable[j][i].ToString();
                if (j == 0) {
                    variable.strFieldNote = str;
                } else if (j == 1) {
                    if (string.IsNullOrEmpty(str))
                        goto next;
                    variable.strFieldName = str;
                } else if (j == 2) {
                    string strFieldType = "";
                    if (str.StartsWith(Util.ArrayString)) {
                        int iFinalIndex = Util.ArrayString.Length;
                        strFieldType = str.Substring(iFinalIndex, str.Length - iFinalIndex);
                        variable.bArray = true;
                    } else {
                        strFieldType = str;
                        variable.bArray = false;
                    }
                    Element element = Util.GetElement(strFieldType);
                    if (element == null)
                        throw new System.Exception(string.Format("第 {0:d} 列的字段类型不能识别 数据内容为 : \"{1}\"", i, str));
                    if (i == 0)
                    {
                        mKeyElement = element.ToString();
                        if (variable.bArray == true || (element.Type != ElementType.INT32 && element.Type != ElementType.STRING))
                            throw new System.Exception("第一列的数据类型必须为int32型或者string型");
                    }
                    variable.strFieldType = strFieldType;
                    variable.eElement = element;
                    if (element is ClassElement)
                    {
                        Type[] types = Util.GetElementTypes((element as ClassElement).GetClassType());
                        variable.fieldTypes.Clear();
                        foreach (Type t in types) {
                            variable.fieldTypes.Add(Util.GetPrimitiveElement(t));
                        }
                        if (!mCustomClasses.ContainsKey(strFieldType))
                            mCustomClasses.Add(strFieldType, new CustomClass(variable.fieldTypes));
                    } else {
                        variable.fieldTypes.Clear();
                        variable.fieldTypes.Add(element);
                    }
                }
            }
            if (!mTableEnumList.Contains(variable.strFieldName))
                mTableEnumList.Add(variable.strFieldName);
            mArrayVariable.Add(variable);
        }
    next:
        if (mArrayVariable.Count <= 0)
            throw new System.Exception("字段个数小于等于0");
        mStrMD5Code = GetClassMD5Code();
        if (!string.IsNullOrEmpty(mSpawnsName))
        {
            string md5 = mSpawnsClasses[mSpawnsName].strMD5;
            if (string.IsNullOrEmpty(md5))
                mSpawnsClasses[mSpawnsName].strMD5 = mStrMD5Code;
            else if (md5 != mStrMD5Code)
                throw new System.Exception(string.Format("关键字文件[{0}]结构跟之前文件不一致", mStrFiler));
        }
        Progress.Value = 0f;
        Create();
    }
    private void Create()
    {
        int iColums = mArrayVariable.Count;                 //数据列数
        int iRows = mDataTable.Count;                       //数据行数
        TableWriter writer = new TableWriter();
        writer.WriteInt32((int)(iRows - 3));                //写入行数
        writer.WriteInt32((int)iColums);                    //写入列数

        //////////////////////////////////////////////////////////////////////////
        Type[] types = CodeProvider.GetInstance().GetTypes();
        if (types != null && mCustomClasses.Count > 0)
        {
            writer.WriteInt32(mCustomClasses.Count);
            foreach (Type type in types)
            {
                if (!mCustomClasses.ContainsKey(type.Name))
                    continue;
                FieldInfo[] fieldInfos = type.GetFields();
                List<Variable> variables = new List<Variable>();
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    Variable variable = new Variable();
                    variable.strFieldType = fieldInfo.FieldType.Name;
                    variable.strFieldName = fieldInfo.Name;
                    variable.strFieldNote = "";
                    variable.bArray = false;
                    variables.Add(variable);
                }
                string strMD5 = Util.GetDataMD5Code(variables);
                writer.WriteString(strMD5);
            }
        }
        else
        {
            writer.WriteInt32(0);
        }
        //////////////////////////////////////////////////////////////////////////
        writer.WriteString(mStrMD5Code);     //写入文件MD5码
        for (int i = 0; i < iColums; ++i)
        {
            Element element = Util.GetElement(mArrayVariable[i].strFieldType);
            int index = (int)element.Type;
            int array = (int)(mArrayVariable[i].bArray ? 1 : 0);
            writer.WriteInt32(index);
            writer.WriteInt32(array);
            if (element.Type == ElementType.CLASS)
            {
                writer.WriteString(mArrayVariable[i].strFieldType);
                writer.WriteInt32(mCustomClasses[mArrayVariable[i].strFieldType].fieldTypes.Count);
                foreach (Element e in mCustomClasses[mArrayVariable[i].strFieldType].fieldTypes)
                    writer.WriteInt32((int)e.Type);
            }
        }
        List<string> keys = new List<string>();
        int count = 0;
        for (int i = 3; i < iRows; ++i)
        {
            string id = "";
            for (int j = 0; j < iColums; ++j)
            {
                var l = mDataTable[i];
                object val = l[j];
                try {
                    string str = Convert.ToString(val);
                    if (string.IsNullOrEmpty(str)) str = Util.EmptyString;    //数据为空默认成####(非法值)
                    if (j == 0) {
                        if (keys.Contains(str)) {
                            throw new Exception(string.Format("ID有重复项【{0}】", str));
                        } else if (Util.IsEmptyString(str)) {
                            throw new Exception("ID不能为####");
                        }
                        id = str;
                        keys.Add(str);
                    }
                    Element element = mArrayVariable[j].eElement;
                    if (element != null) {
                        if (element.Type == ElementType.LSTRING && !Util.IsEmptyString(str)) {
                            if (!mKeyWords.ContainsKey(mStrFiler))
                                mKeyWords[mStrFiler] = new Table();
                            var key = string.Format("{0}_{1}_{2}",mStrFiler, mArrayVariable[j].strFieldName, id);
                            mKeyWords[mStrFiler].Languages[key] = new Language(key, str);
                        }
                        element.WriteValueByType(str, writer, mArrayVariable[j].bArray);
                    }
                    else
                        throw new Exception("找不到对应的Element " + mArrayVariable[j].strFieldType);
                } catch (System.Exception ex) {
                    throw new SystemException(string.Format("【{0}】行 【{1}】列   填错了 数据内容为 : type : {2}  val : {3}   {4}", i + 1, Util.GetLineName(j + 1), val.GetType(),val.ToString(),ex.ToString()));
                }
            }
            count++;
        }
        writer.Seek(0);
        writer.WriteInt32(count);
        Create_impl(writer.ToArray());
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
            if (info.CreateCode == null) continue;
            string strData = Util.GetDataClass(program, mStrDataClass, mArrayVariable, mArray, true, false);
            string strTable = GetTableClass(program);
            string strField = Util.GetFieldClass(program, mStrFieldClass, mArrayVariable);
            info.CreateCode.Invoke(this, new object[] { strData, strTable, strField, info });
        }
    }
    public void CreateCodeCS(string strData, string strTable, string strField, ProgramInfo info)
    {
        string strStream = @"using System;
using System.IO;
using System.Collections.Generic;
#pragma warning disable 0219";
        FileUtil.CreateFile(info.GetFile(mStrTableClass), strStream + strField + strData + strTable, true, info.CodeDirectory.Split(';'));
    }
    public void CreateCodeJAVA(string strData, string strTable, string strField, ProgramInfo info)
    {
        string strStream = @"package table;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.HashMap;
@SuppressWarnings(""unused"")";
        FileUtil.CreateFile(info.GetFile(mStrFieldClass), strStream + strField, false, info.CodeDirectory.Split(';'));
        FileUtil.CreateFile(info.GetFile(mStrDataClass), strStream + strData, false, info.CodeDirectory.Split(';'));
        FileUtil.CreateFile(info.GetFile(mStrTableClass), strStream + strTable, false, info.CodeDirectory.Split(';'));
    }
    public void CreateCodePHP(string strData, string strTable, string strField, ProgramInfo info)
    {
        string strStream = @"<?php
require_once 'TableUtil.php';";
        Type[] types = CodeProvider.GetInstance().GetTypes();
        if (types != null && types.Length > 0)
        {
            strStream += @"
require_once 'Custom.php';";
        }
        FileUtil.CreateFile(info.GetFile(mStrTableClass), strStream + strData + strTable, false, info.CodeDirectory.Split(';'));
    }
    public void CreateCodeCPP(string strData, string strTable, string strField, ProgramInfo info)
    {
        string strStream = @"#pragma once
#include <string>
#include <vector>
#include <map>
using namespace::std;
#pragma warning disable 0219";
        FileUtil.CreateFile(info.GetFile(mStrTableClass), strStream + strData + strTable, true, info.CodeDirectory.Split(';'));
    }
}
