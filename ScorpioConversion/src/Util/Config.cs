using ScorpioConversion;
using System;
using System.IO;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using ExcelDataReader;
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
    public IWorkbook GetWorkbook(FileStream fileStream) {
        if (fileName.EndsWith(".xls")) {
            return new HSSFWorkbook(fileStream);
        } else if (fileName.EndsWith(".xlsx")) {
            return new XSSFWorkbook(fileStream);
        } else {
            throw new Exception($"不支持的文件后缀 : {fileName}");
        }
    }
    public IExcelDataReader GetExcelReader(FileStream fileStream) {
        return ExcelReaderFactory.CreateReader(fileStream);
    }
    public override string ToString() {
        return fileName;
    }
}
public class Config {
    public static string Suffix;                                            //data文件后缀
    public static string FileSuffix;                                        //程序文件后缀
    public static List<string> Tags;                                        //标签列表
    public static List<ExcelFile> FileList;                                 //所有要生成的excel文件
    public static string PackageName;                                       //默认Package名字
    public static Dictionary<string, string> LanguageDirectory;             //所有语言的输出目录
    public static string DataDirectory;                                     //数据文件输出目录
    public static bool WriteL10N;                                           //是否写入l10n字段的值
    public static bool IsFileName;                                          //默认名字是否使用文件名字
    public static Dictionary<string, string> SpawnsList;                    //派生类列表
    public static PackageParser Parser;                                     //配置文件解析
    public static void Initialize(Scorpio.Commons.CommandLine command) {
        SpawnsList = new Dictionary<string, string>();               //派生文件列表 多个Key[{Util.Separator}]隔开
        Suffix = command.GetValueDefault("-suffix", "data");        //数据文件后缀 默认.data
        FileSuffix = command.GetValueDefault("-fileSuffix", "");    //生成的程序文件后缀名
        Tags = new List<string>(command.GetValueDefault("-tags", "").Split(Util.Separator));        //需要过滤的文件tags 多tag[{Util.Separator}]隔开
        Tags.Remove("");
        Parser = new PackageParser();
        Util.Split(command.GetValue("-config"), (file) => Parser.Parse(file) );

        var files = new List<string>();
        //需要转换的文件 多文件[{Util.Separator}]隔开
        Util.Split(command.GetValue("-files"), (file) => {
            if (File.Exists(file))
            {
                files.Add(file);
            } else
            {
                files.Add($"{Environment.CurrentDirectory}/{file}");
            }
        });
        //需要转换的文件目录 多路径[{Util.Separator}]隔开
        Util.Split(command.GetValue("-paths"), (path) => files.AddRange(Directory.GetFiles($"{Environment.CurrentDirectory}/{path}", "*", SearchOption.TopDirectoryOnly)));

        FileList = new List<ExcelFile>();
        files.ForEach((file) => { if (file.IsExcel()) { FileList.Add(new ExcelFile(file)); } });

        var wl10n = command.GetValueDefault("-wl10n", "");
        WriteL10N = wl10n.IsEmptyString() ? false : Convert.ToBoolean(wl10n);

        LanguageDirectory = new Dictionary<string, string>();
        foreach (Language language in Enum.GetValues(typeof(Language))) {
            //各语言文件输出目录 多目录[,]隔开
            var dir = command.GetValue("-l" + language.GetInfo().extension.ToLower());
            if (string.IsNullOrWhiteSpace(dir)) {
                dir = command.GetValue("-" + language.ToString().ToLower());
            }
            if (!string.IsNullOrWhiteSpace(dir)) {
                LanguageDirectory[language] = dir;
            }
        }
        PackageName = command.GetValueDefault("-package", "scov");
        DataDirectory = command.GetValue("-data");
        IsFileName = command.GetValueDefault("-name", "file").ToLower() == "file";      //名字使用文件名或者sheet名字
    }
    public static bool ContainsTags(List<string> tags) {
        if (Tags.Count == 0 || tags.Count == 0) { return true; }
        return tags.FindIndex(_ => !_.IsEmptyString() && Tags.Contains(_)) >= 0;
    }
}
