using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

public class DefaultInfo : Attribute
{
    public string Extension;        //文件后缀
    public Type GenerateTable;      //转换Table类
    public Type GenerateMessage;    //转换Message类
    public Type GenerateEnum;       //转换Enum类
    public Type GenerateConst;      //转换Const类
    public bool Bom;                //文件是否有bom头
    public DefaultInfo(string ex, Type table, Type message, Type @enum, Type @const, bool bom)
    {
        Extension = ex;
        GenerateTable = table;
        GenerateMessage = message;
        GenerateEnum = @enum;
        GenerateConst = @const;
        Bom = bom;
    }
}
//语言数量
public enum PROGRAM
{
    NONE = -1,      //无语言
    [DefaultInfo("cs", typeof(GenerateTableCSharp), typeof(GenerateMessageCSharp), typeof(GenerateEnumCSharp), typeof(GenerateConstCSharp), true)]
    CSharp,         //C#(CSharp) 语言
    [DefaultInfo("java", typeof(GenerateTableJava), typeof(GenerateMessageJava), typeof(GenerateEnumJava), typeof(GenerateConstJava), false)]
    Java,           //Java 语言
    [DefaultInfo("sco", typeof(GenerateTableScorpio), typeof(GenerateMessageScorpio), typeof(GenerateEnumScorpio), typeof(GenerateConstScorpio), false)]
    Scorpio,        //Scorpio 脚本
    COUNT,
}
//一种程序语言的动态配置
public class ProgramConfig
{
    private string mCodeDirectory;
    private string mDataDirectory;
    public string CodeDirectory {
        get { return mCodeDirectory; }
        set {
            string[] dirs = value.Split(';');
            for (int i = 0; i < dirs.Length; ++i) {
                dirs[i] = FileUtil.GetFullPath(dirs[i]);
            }
            mCodeDirectory = string.Join(";", dirs);
        }
    }
    public string DataDirectory {
        get { return mDataDirectory; }
        set {
            string[] dirs = value.Split(';');
            for (int i = 0; i < dirs.Length; ++i) {
                dirs[i] = FileUtil.GetFullPath(dirs[i]);
            }
            mDataDirectory = string.Join(";", dirs);
        }
    }
    public string Create;
    public string Compress;
}
//一种语言的信息
public class ProgramInfo
{
    private static readonly Type TYPE_TABLE_BUILDER = typeof(TableBuilder);
    private static readonly Type TYPE_MESSAGE_BUILDER = typeof(MessageBuilder);
    public PROGRAM Code;                        //语言枚举
    public bool Create;                         //默认是否生成
    public string CodeDirectory;                //代码输出目录
    public string DataDirectory;                //文件输出目录
    public bool Compress;                       //data文件是否压缩
    public string Extension;                    //扩展名
    public IGenerate GenerateTable;             //Table生成代码类
    public IGenerate GenerateMessage;           //Message生成代码类
    public IGenerate GenerateEnum;              //Enum生成代码类
    public IGenerate GenerateConst;             //const生成代码类
    public bool Bom;                            //是否有bom文件头
    public MethodInfo CreateTableManager;       //生成TableManager文件
    public MethodInfo CreateMessageManager;     //生成TableManager文件
    public ProgramInfo(PROGRAM code)
    {
        Code = code;
        CreateTableManager = TYPE_TABLE_BUILDER.GetMethod("CreateManager" + code.ToString());
        CreateMessageManager = TYPE_MESSAGE_BUILDER.GetMethod("CreateManager" + code.ToString());
    }
    public string GetFile(string filter) { return filter + "." + Extension; }
    public string TableTemplate { 
        get {
            string file = Util.CurrentDirectory + "/Template/Table." + Extension;
            return FileUtil.FileExist(file) ? FileUtil.GetFileString(file) : "";
        }
    }
    public string HeadTemplate {
        get {
            string file = Util.CurrentDirectory + "/Template/Head." + Extension;
            return FileUtil.FileExist(file) ? FileUtil.GetFileString(file) : "";
        }
    }
    public void CreateFile(string name, string context) {
        if (string.IsNullOrEmpty(CodeDirectory)) return;
        FileUtil.CreateFile(GetFile(name), context, Bom, CodeDirectory.Split(';'));
    }
    public void CreateData(string name, byte[] data) {
        if (string.IsNullOrEmpty(DataDirectory)) return;
        FileUtil.CreateFile(name + ".data", data, false, DataDirectory.Split(';'));
    }
    public ProgramInfo Clone()
    {
        ProgramInfo ret = new ProgramInfo(Code);
        ret.Create = Create;
        ret.CodeDirectory = CodeDirectory;
        ret.DataDirectory = DataDirectory;
        ret.Compress = Compress;
        ret.Extension = Extension;
        ret.GenerateTable = GenerateTable;
        ret.GenerateMessage = GenerateMessage;
        ret.GenerateEnum = GenerateEnum;
        ret.GenerateConst = GenerateConst;
        ret.CreateTableManager = CreateTableManager;
        ret.CreateMessageManager = CreateMessageManager;
        return ret;
    }
}

public static partial class Util
{
    private static Dictionary<PROGRAM, ProgramInfo> m_ProgramInfos = new Dictionary<PROGRAM, ProgramInfo>();
    public static void InitializeProgram(Dictionary<PROGRAM, ProgramConfig> configs)
    {
        m_ProgramInfos.Clear();
        for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT;++i ) {
            PROGRAM program = (PROGRAM)i;
            if (!configs.ContainsKey(program)) continue;
            var config = configs[program];
            ProgramInfo info = new ProgramInfo(program);
            info.CodeDirectory = config.CodeDirectory;
            info.DataDirectory = config.DataDirectory;
            info.Create = ToBoolean(config.Create, true);
            info.Compress = ToBoolean(config.Compress, false);
            DefaultInfo defaultInfo = (DefaultInfo)Attribute.GetCustomAttribute(program.GetType().GetMember(program.ToString())[0], typeof(DefaultInfo));
            info.Extension = defaultInfo.Extension;
            info.GenerateTable = (IGenerate)System.Activator.CreateInstance(defaultInfo.GenerateTable);
            info.GenerateMessage = (IGenerate)System.Activator.CreateInstance(defaultInfo.GenerateMessage);
            info.GenerateEnum = (IGenerate)System.Activator.CreateInstance(defaultInfo.GenerateEnum);
            info.GenerateConst = (IGenerate)System.Activator.CreateInstance(defaultInfo.GenerateConst);
            info.Bom = defaultInfo.Bom;
            m_ProgramInfos.Add(program, info);
        }
    }
    public static ProgramInfo GetProgramInfo(PROGRAM program)
    {
        if (m_ProgramInfos.ContainsKey(program))
            return m_ProgramInfos[program];
        return null;
    }
    public static Dictionary<PROGRAM, ProgramInfo> GetProgramInfos()
    {
        return m_ProgramInfos;
    }
}

