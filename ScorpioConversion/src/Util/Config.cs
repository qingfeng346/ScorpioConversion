using ScorpioConversion;
using System.IO;
using System.Collections.Generic;
using ExcelDataReader;
using Scorpio.Commons;
using Newtonsoft.Json;
public class ExcelFile {
    public string file;
    public string fileName;
    public string fileNameWithoutExtension;
    public string extension;
    public ExcelFile(string file) {
        this.file = file;
        this.fileName = Path.GetFileName(file);
        this.fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
        this.extension = Path.GetExtension(file);
    }
    public IExcelDataReader GetExcelReader(FileStream fileStream) {
        return ExcelReaderFactory.CreateReader(fileStream);
    }
    public override string ToString() {
        return fileName;
    }
}
public class Config {
    public static HashSet<string> Tags { get; private set; }                    //标签列表
    public static List<ExcelFile> FileList { get; private set; }                //所有要生成的excel文件
    public static LanguageConfig LanguageConfig { get; private set; }           //所有语言配置
    public static bool WriteL10N { get; private set; }                          //是否写入l10n字段的值
    public static bool IsFileName { get; private set; }                         //默认名字是否使用文件名字
    public static Dictionary<string, string> SpawnsList { get; private set; }   //派生类MD5列表
    public static List<L10NData> L10NDatas { get; private set; }                //所有的翻译字段
    public static PackageParser Parser { get; private set; }                    //配置文件解析
    public static void Initialize(string[] configs, string[] files, string[] paths, string[] tags, string name) {
        SpawnsList = new Dictionary<string, string>();                                    //派生文件列表 多个Key[{Util.Separator}]隔开
        L10NDatas = new List<L10NData>();
        Tags = new HashSet<string>(tags);                               //需要过滤的文件tags 多tag[{Util.Separator}]隔开
        Parser = new PackageParser();
        foreach (var config in configs) {
            Parser.Parse(config);
        }
        FileList = new List<ExcelFile>();

        files.ForEach((file) => {
            if (file.IsExcel())
                FileList.Add(new ExcelFile(file));
        });
        var files = new List<string>(Util.Split(command.GetValue("-files")));
        files = files.ConvertAll(file => Path.Combine(ScorpioUtil.CurrentDirectory, file));
        //需要转换的文件目录 多路径[{Util.Separator}]隔开
        Util.Split(command.GetValueDefault("-paths", ""), (path) => {
            files.AddRange(Directory.GetFiles(Path.Combine(ScorpioUtil.CurrentDirectory, path), "*", SearchOption.AllDirectories));
        });
        
        LanguageConfig = JsonConvert.DeserializeObject<LanguageConfig>(FileUtil.GetFileString(Path.Combine(ScorpioUtil.CurrentDirectory, command.GetValue("-lang"))));
        WriteL10N = command.GetValueDefault("-wl10n", "").ToBoolean();
        IsFileName = command.GetValueDefault("-name", "file").ToLower() == "file";      //名字使用文件名或者sheet名字
    }
    public static bool ContainsTags(List<string> tags) {
        if (Tags.Count == 0 || tags.Count == 0) { return true; }
        return Tags.Overlaps(tags);
    }
}
