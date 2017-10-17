using System;
using System.Collections.Generic;
#if MONO_GTK
using RichTextBox = global::Gtk.TextView;
using TextBox = global::Gtk.Entry;
using CheckBox = global::Gtk.ToggleButton;
#else
using System.Windows.Forms;
#endif
using ScorpioConversion;
public static class Extends
{
	public static bool GetChecked(CheckBox checkBox) {
#if MONO_GTK
		return checkBox.Active;
#else
		return checkBox.Checked;
#endif
	}
	public static void SetChecked(CheckBox checkBox, bool check) {
#if MONO_GTK
		checkBox.Active = check;
#else
        checkBox.Checked = check;
#endif
	}
	public static string GetText(RichTextBox textBox) {
#if MONO_GTK
		return textBox.Buffer.Text;
#else
		return textBox.Text;
#endif
	}
	public static void SetText(RichTextBox textBox, string text) {
#if MONO_GTK
		textBox.Buffer.Text = text;
#else
		textBox.Text = text;
#endif
	}
	public static void RegisterEvent(RichTextBox textBox, System.EventHandler handler) {
#if MONO_GTK
		textBox.Buffer.Changed += handler;
#else
		textBox.TextChanged += handler;
#endif	
	}
	public static void RegisterEvent(TextBox textBox, System.EventHandler handler) {
#if MONO_GTK
		textBox.Changed += handler;
#else
		textBox.TextChanged += handler;
#endif	
	}
	public static void RegisterEvent(CheckBox textBox, System.EventHandler handler) {
#if MONO_GTK
		textBox.Toggled += handler;
#else
		textBox.CheckedChanged += handler;
#endif
	}
}
//所有配置文件
public enum ConfigFile {
    PathConfig,
    InitConfig,
    LanguageConfig,
    TinyConfig,
}
public class ConfigKey {
    //PathConfig
    public const string CodeDirectory = "CodeDirectory";            //语言代码生成目录
    public const string DataDirectory = "DataDirectory";            //语言数据文件生成目录
    public const string Create = "Create";                          //语言是否默认生成
    public const string Compress = "Compress";                      //语言data文件是否使用

    public const string TransformDirectory = "TransformDirectory";  	//转换文件目录
    public const string RollbackDirectory = "RollbackDirectory";        //反转文件目录

    //InitConfig
    public const string DatabaseConfigDirectory = "DatabaseConfigDirectory";        //数据库配置路径
    public const string SpawnList = "SpawnList";                    //批量处理关键字
    public const string PackageName = "PackageName";                //PackageName
    public const string TableConfigPath = "TableConfigPath";        //Table配置路径
    public const string TableFolderPath = "TableFolderPath";        //转换Table文件夹目录
    public const string MessagePath = "MessagePath";                //协议路径

    //LanguageConfig
    public const string AllLanguage = "AllLanguage";
    public const string TranslationDirectory = "TranslationDirectory";
    public const string LanguageDirectory = "LanguageDirectory";

    //TinyConfig
    public const string TinyApiKey = "ApiKey";
    public const string TinySourcePath = "TinySourcePath";
    public const string TinyTargetPath = "TinyTargetPath";
}
public static partial class ConversionUtil {
    public static string CurrentDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; } }
    public static string WorkspaceDirectory { get; set; }
    public static Dictionary<PROGRAM, ProgramConfig> GetProgramConfig() {
        Dictionary<PROGRAM, ProgramConfig> configs = new Dictionary<PROGRAM, ProgramConfig>();
        for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT; ++i) {
            PROGRAM program = (PROGRAM)i;
            configs.Add(program, new ProgramConfig() {
                CodeDirectory = ConversionUtil.GetConfig(program, ConfigKey.CodeDirectory, ConfigFile.PathConfig),
                DataDirectory = ConversionUtil.GetConfig(program, ConfigKey.DataDirectory, ConfigFile.PathConfig),
                Create = ConversionUtil.GetConfig(program, ConfigKey.Create, ConfigFile.PathConfig),
                Compress = ConversionUtil.GetConfig(program, ConfigKey.Compress, ConfigFile.PathConfig),
            });
        }
        return configs;
    }
    private class AutoConfig {
        public PROGRAM program;     //语言枚举
        public string key;          //key值
        public ConfigFile file;     //保存文件
    }
    private static Dictionary<ConfigFile, ScorpioIni> m_Configs = new Dictionary<ConfigFile, ScorpioIni>();
    private static Dictionary<object, AutoConfig> m_AutoConfigs = new Dictionary<object, AutoConfig>();
    public static void Cleanup() {
        m_Configs.Clear();
        m_AutoConfigs.Clear();
    }
    public static string GetPath(string path) {
        return System.IO.Path.Combine(WorkspaceDirectory, path);
    }
    public static void Bind(RichTextBox textBox, string key, ConfigFile file) {
        Bind(textBox, PROGRAM.NONE, key, file);
    }
    public static void Bind(RichTextBox textBox, PROGRAM program, string key, ConfigFile file) {
        if (m_AutoConfigs.ContainsKey(textBox))
            return;
        AutoConfig config = new AutoConfig() { program = program, key = key, file = file };
        Extends.SetText(textBox, GetConfig(program, key, file).Replace(";", "\n"));
        Extends.RegisterEvent(textBox, new System.EventHandler(RichTextChanged));
        m_AutoConfigs[textBox] = config;
    }
    private static void RichTextChanged(object sender, EventArgs e) {
        if (m_AutoConfigs.ContainsKey(sender)) {
            RichTextBox textBox = (RichTextBox)sender;
            AutoConfig config = m_AutoConfigs[sender];
			SetConfig(config.program, config.key, Extends.GetText(textBox).Replace("\n", ";"), config.file);
        }
    }
    public static void Bind(TextBox textBox, string key, ConfigFile file) {
        Bind(textBox, PROGRAM.NONE, key, file);
    }
    public static void Bind(TextBox textBox, PROGRAM program, string key, ConfigFile file) {
        if (m_AutoConfigs.ContainsKey(textBox))
            return;
        AutoConfig config = new AutoConfig() { program = program, key = key, file = file };
        textBox.Text = GetConfig(program, key, file);
        Extends.RegisterEvent(textBox, new System.EventHandler(TextChanged));
        m_AutoConfigs[textBox] = config;
    }
    private static void TextChanged(object sender, EventArgs e) {
        if (m_AutoConfigs.ContainsKey(sender)) {
            TextBox textBox = (TextBox)sender;
            AutoConfig config = m_AutoConfigs[sender];
            SetConfig(config.program, config.key, textBox.Text, config.file);
        }
    }
    public static void Bind(CheckBox textBox, string key, ConfigFile file) {
        Bind(textBox, PROGRAM.NONE, key, file);
    }
    public static void Bind(CheckBox textBox, PROGRAM program, string key, ConfigFile file) {
        if (m_AutoConfigs.ContainsKey(textBox))
            return;
        AutoConfig config = new AutoConfig() { program = program, key = key, file = file };
        Extends.SetChecked(textBox, Util.ToBoolean(GetConfig(program, key, file), false));
        Extends.RegisterEvent(textBox, new System.EventHandler(CheckedChanged));
        m_AutoConfigs[textBox] = config;
    }
    private static void CheckedChanged(object sender, EventArgs e) {
        if (m_AutoConfigs.ContainsKey(sender)) {
            AutoConfig config = m_AutoConfigs[sender];
			SetConfig(config.program, config.key, Extends.GetChecked((CheckBox)sender) ? "true" : "false", config.file);
        }
    }
    private static ScorpioIni GetConfig(ConfigFile file) {
        ScorpioIni config = null;
        if (!m_Configs.ContainsKey(file)) {
            config = new ScorpioIni(WorkspaceDirectory + file.ToString() + ".ini", System.Text.Encoding.UTF8);
            m_Configs.Add(file, config);
        } else {
            config = m_Configs[file];
        }
        return config;
    }
    public static string GetConfig(string key, ConfigFile file) {
        return GetConfig(PROGRAM.NONE, key, file);
    }
    public static void SetConfig(string key, string value, ConfigFile file) {
        SetConfig(PROGRAM.NONE, key, value, file);
    }
    public static string GetConfig(PROGRAM program, string key, ConfigFile file) {
        ScorpioIni config = GetConfig(file);
        return config.Get(program == PROGRAM.NONE ? "" : program.ToString(), key);
    }
    public static void SetConfig(PROGRAM program, string key, string value, ConfigFile file) {
        ScorpioIni config = GetConfig(file);
        config.Set(program == PROGRAM.NONE ? "" : program.ToString(), key, value);
        FileUtil.CreateFile(WorkspaceDirectory + file.ToString() + ".ini", config.GetString());
    }
    public static string GetConfig(string section, string key, ConfigFile file) {
        ScorpioIni config = GetConfig(file);
        return config.Get(section, key);
    }
	public static void SetToolTip(object control, string text) {
        
    }
}
