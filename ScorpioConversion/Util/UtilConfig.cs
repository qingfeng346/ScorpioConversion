using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using XML_Conversion;

public class DefaultInfo : Attribute
{
    public string Extension;        //文件后缀
    public Type GenerateType;       //转换类
    public DefaultInfo(string ex, Type type)
    {
        Extension = ex;
        GenerateType = type;
    }
}
//语言数量
public enum PROGRAM
{
    NONE = -1,      //无语言
    [DefaultInfo("cs", typeof(GenerateCSharp))]
    CSharp,         //C#(CSharp) 语言
    [DefaultInfo("java", typeof(GenerateJava))]
    Java,           //Java 语言
    [DefaultInfo("js", typeof(GenerateScorpio))]
    Scorpio,        //Scorpio 脚本
    //[DefaultInfo("h", null)]
    //Cpp,
    COUNT,
}
//所有配置文件
public enum ConfigFile
{
    PathConfig,
    InitConfig,
    TableConfig,
    LanguageConfig,
    CunstomConfig,
}
//一种语言的信息
public class ProgramInfo
{
    private static readonly Type TYPE_MANAGER = typeof(TableManager);
    public bool Create;                 //默认是否生成
    public string CodeDirectory;        //代码输出目录
    public string DataDirectory;        //文件输出目录
    public bool Compress;               //data文件是否压缩
    public string Extension;            //扩展名
    public IGenerate Generate;          //生成代码类
    public MethodInfo CreateCode;       //生成代码函数
    public MethodInfo CreateManager;    //生成TableManager文件
    private ProgramInfo() { }
    public ProgramInfo(PROGRAM program)
    {
        CreateCode = TYPE_MANAGER.GetMethod("CreateCode" + program.ToString());
        CreateManager = TYPE_MANAGER.GetMethod("CreateManager" + program.ToString());
    }
    public string GetFile(string filter)
    {
        return filter + "." + Extension;
    }
    public ProgramInfo Clone()
    {
        ProgramInfo ret = new ProgramInfo();
        ret.Create = Create;
        ret.CodeDirectory = CodeDirectory;
        ret.DataDirectory = DataDirectory;
        ret.Compress = Compress;
        ret.Extension = Extension;
        ret.Generate = Generate;
        ret.CreateCode = CreateCode;
        ret.CreateManager = CreateManager;
        return ret;
    }
}
public class ConfigKey
{
    //PathConfig
    public const string CodeDirectory = "CodeDirectory";            //CS代码生成目录
    public const string DataDirectory = "DataDirectory";            //CS数据文件生成目录
    public const string TransformDirectory = "TransformDirectory";  //转换文件目录
    public const string RollbackDirectory = "RollbackDirectory";    //反转文件目录
    //InitConfig
    public const string SpawnList = "SpawnList";                    //批量处理关键字
    public const string Create = "Create";                          //是否默认生成
    public const string Compress = "Compress";                      //data文件是否使用
    public const string Languages = "Languages";                    //所有多国语言
    //TableConfig
    public const string Array = "Array";                            //是否是数组形式
    //LanguageConfig
    public const string AllLanguage = "AllLanguage";
    public const string TranslationDirectory = "TranslationDirectory";
    public const string LanguageDirectory = "LanguageDirectory";
}
public static partial class Util
{
    public delegate PROGRAM GetProgram();
    private class AutoConfig
    {
        public PROGRAM program;
        public string key;
        public ConfigFile file;
    }
    private static Dictionary<ConfigFile, Config> m_Configs = new Dictionary<ConfigFile, Config>();
    private static Dictionary<PROGRAM, ProgramInfo> m_ProgramInfos = new Dictionary<PROGRAM, ProgramInfo>();
    private static Dictionary<object, AutoConfig> m_AutoConfigs = new Dictionary<object, AutoConfig>();
    public static string BaseDirectory { get { return System.AppDomain.CurrentDomain.BaseDirectory; } }
    public static void Bind(TextBox textBox, string key, ConfigFile file)
    {
        Bind(textBox, PROGRAM.NONE, key, file);
    }
    public static void Bind(TextBox textBox, PROGRAM program, string key, ConfigFile file)
    {
        if (m_AutoConfigs.ContainsKey(textBox))
            return;
        AutoConfig config = new AutoConfig() { program = program, key = key, file = file };
        textBox.Text = GetConfig(program, key, file);
        textBox.TextChanged += new System.EventHandler(TextChanged);
        m_AutoConfigs[textBox] = config;
    }
    private static void TextChanged(object sender, EventArgs e)
    {
        if (m_AutoConfigs.ContainsKey(sender))
        {
            AutoConfig config = m_AutoConfigs[sender];
            SetConfig(config.program, config.key, ((TextBox)sender).Text, config.file);
        }
    }
    public static bool ToBoolean(string str)
    {
        return ToBoolean(str, false);
    }
    public static bool ToBoolean(string str, bool def)
    {
        if (string.IsNullOrEmpty(str)) return def;
        switch (str.ToLower())
        {
            case "1":
            case "true":
                return true;
            case "0":
            case "false":
                return false;
            default:
                throw new Exception("字符串不能转换为bool " + str);
        }
    }
    private static Config GetConfig(ConfigFile file)
    {
        Config config = null;
        if (!m_Configs.ContainsKey(file)) {
            config = new Config(BaseDirectory + file.ToString() + ".ini", true);
            m_Configs.Add(file, config);
        } else {
            config = m_Configs[file];
        }
        return config;
    }
    public static string GetConfig(string key, ConfigFile file)
    {
        return GetConfig(PROGRAM.NONE, key, file);
    }
    public static void SetConfig(string key, string value, ConfigFile file)
    {
        SetConfig(PROGRAM.NONE, key, value, file);
    }
    public static string GetConfig(PROGRAM program, string key, ConfigFile file)
    {
        Config config = GetConfig(file);
        return config.Get(program == PROGRAM.NONE ? "" : program.ToString(), key);
    }
    public static void SetConfig(PROGRAM program, string key, string value, ConfigFile file)
    {
        Config config = GetConfig(file);
        config.Set(program == PROGRAM.NONE ? "" : program.ToString(), key, value);
        config.Save(false);
    }
    public static string GetConfig(string section, string key, ConfigFile file)
    {
        Config config = GetConfig(file);
        return config.Get(section, key);
    }
    public static void InitializeProgram()
    {
        m_ProgramInfos.Clear();
        for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT;++i )
        {
            PROGRAM program = (PROGRAM)i;
            ProgramInfo info = new ProgramInfo(program);
            string CodeDirectory = GetConfig(program, ConfigKey.CodeDirectory, ConfigFile.PathConfig);
            string DataDirectory = GetConfig(program, ConfigKey.DataDirectory, ConfigFile.PathConfig);
            info.CodeDirectory = string.IsNullOrEmpty(CodeDirectory) ? BaseDirectory + program.ToString() : CodeDirectory;
            info.DataDirectory = string.IsNullOrEmpty(DataDirectory) ? BaseDirectory + program.ToString() : DataDirectory;
            info.Create = ToBoolean(GetConfig(program, ConfigKey.Create, ConfigFile.PathConfig), true);
            info.Compress = ToBoolean(GetConfig(program, ConfigKey.Compress, ConfigFile.InitConfig));
            DefaultInfo defaultInfo = (DefaultInfo)Attribute.GetCustomAttribute(program.GetType().GetMember(program.ToString())[0], typeof(DefaultInfo));
            info.Extension = defaultInfo.Extension;
            info.Generate = (IGenerate)System.Activator.CreateInstance(defaultInfo.GenerateType);
            m_ProgramInfos.Add(program, info);
        }
    }
    public static ProgramInfo GetProgramInfo(PROGRAM program)
    {
        if (m_ProgramInfos.ContainsKey(program))
            return m_ProgramInfos[program];
        return null;
    }
    public static void SetToolTip(Control control, string text)
    {
        var tips = new ToolTip();
        tips.ShowAlways = true;
        tips.InitialDelay = 1;
        tips.IsBalloon = true;
        tips.SetToolTip(control, text);
    }
}

