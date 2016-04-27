using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using ScorpioConversion;

public class TinyPNGConfig {
    public string ApiKey;
    public string SourcePath;
    public string TargetPath;
}
public class TinyPNGInput {
    public int size;
    public string type;
}
public class TinyPNGOutput {
    public int size;
    public string type;
    public int width;
    public int height;
    public float ratio;
    public string url;
}
public class TinyPNGResponseData {
    public TinyPNGInput input;
    public TinyPNGOutput output;
    public string error;
    public string message;
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
public class ConfigKey
{
    //PathConfig
    public const string CodeDirectory = "CodeDirectory";            //CS代码生成目录
    public const string DataDirectory = "DataDirectory";            //CS数据文件生成目录
    public const string TransformDirectory = "TransformDirectory";  //转换文件目录
    public const string RollbackDirectory = "RollbackDirectory";    //反转文件目录
    public const string DatabaseConfigDirectory = "DatabaseConfigDirectory";    //拷贝数据库配置目录
    //InitConfig
    public const string SpawnList = "SpawnList";                    //批量处理关键字
    public const string Create = "Create";                          //是否默认生成
    public const string Compress = "Compress";                      //data文件是否使用
    public const string Languages = "Languages";                    //所有多国语言
    public const string PackageName = "PackageName";                //PackageName
    public const string TableConfigPath = "TableConfigPath";        //Table配置路径
    public const string TableFolderPath = "TableFolderPath";        //转换Table文件夹目录
    public const string MessagePath = "MessagePath";                //协议路径
    public const string DatabasePath = "DatabasePath";              //数据库配置路径
    //TableConfig
    public const string Array = "Array";                            //是否是数组形式
    //LanguageConfig
    public const string AllLanguage = "AllLanguage";
    public const string TranslationDirectory = "TranslationDirectory";
    public const string LanguageDirectory = "LanguageDirectory";
}
public static partial class ConversionUtil
{
    public static string CurrentDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; } }
    private class AutoConfig
    {
        public PROGRAM program;     //语言枚举
        public string key;          //key值
        public ConfigFile file;     //保存文件
    }
    private static Dictionary<ConfigFile, Config> m_Configs = new Dictionary<ConfigFile, Config>();
    private static Dictionary<object, AutoConfig> m_AutoConfigs = new Dictionary<object, AutoConfig>();
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
            TextBox textBox = (TextBox)sender;
            AutoConfig config = m_AutoConfigs[sender];
            SetConfig(config.program, config.key, textBox.Text, config.file);
        }
    }
    public static void Bind(CheckBox textBox, string key, ConfigFile file)
    {
        Bind(textBox, PROGRAM.NONE, key, file);
    }
    public static void Bind(CheckBox textBox, PROGRAM program, string key, ConfigFile file)
    {
        if (m_AutoConfigs.ContainsKey(textBox))
            return;
        AutoConfig config = new AutoConfig() { program = program, key = key, file = file };
        textBox.Checked = Util.ToBoolean(GetConfig(program, key, file), false);
        textBox.CheckedChanged += new System.EventHandler(CheckedChanged);
        m_AutoConfigs[textBox] = config;
    }
    private static void CheckedChanged(object sender, EventArgs e)
    {
        if (m_AutoConfigs.ContainsKey(sender))
        {
            AutoConfig config = m_AutoConfigs[sender];
            SetConfig(config.program, config.key, ((CheckBox)sender).Checked ? "true" : "false", config.file);
        }
    }
    private static Config GetConfig(ConfigFile file)
    {
        Config config = null;
        if (!m_Configs.ContainsKey(file)) {
            config = new Config(CurrentDirectory + file.ToString() + ".ini", true);
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
    public static void SetToolTip(Control control, string text)
    {
        var tips = new ToolTip();
        tips.ShowAlways = true;
        tips.InitialDelay = 1;
        tips.IsBalloon = true;
        tips.SetToolTip(control, text);
    }
}

