using System;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Diagnostics;
using System.Collections.Generic;
using Scorpio;
using Scorpio.Commons;
using System.Text;

using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
namespace ScorpioConversion {
    public class LogHelper : ILogger {
        public void info(string value) {
            Console.WriteLine(value);
            Debugger.Log(0, null, value + "\n");
        }
        public void warn(string value) {
            value = "[warn]" + value;
            Console.WriteLine(value);
            Debugger.Log(0, null, value + "\n");
        }
        public void error(string value) {
            value = "[error]" + value;
            Console.WriteLine(value);
            Debugger.Log(0, null, value + "\n");
        }
    }
    class Program {
        const int READ_LENGTH = 8192;
        private const string HelpRegister = @"
注册运行程序到环境变量";
        private static string HelpInstall = $@"
载入对应语言的库文件
{HelpLanguages}";
        private static string HelpExecute = $@"
命令列表
    register        注册运行程序到环境变量
    install         载入对应语言的库文件
    [other]         转换excel文件
        -package        命名空间,默认 sco
        -files          需要转换的excel文件,多文件,隔开
        -paths          需要转换的excel文件目录 多路径[,]隔开
        -data           数据文件输出目录 多目录[,]隔开
        -suffix         数据文件后缀 默认 .data
        -name           输出文件使用文件名或者sheet名字,默认file, 选项 [file,sheet]
        -config         配置文件路径 多路径[,]隔开
        -spawns         派生文件列表 多个Key[,]隔开
{HelpLanguages}
";
        static string HelpLanguages {
            get {
                var builder = new StringBuilder(@"
    支持语言列表");
                foreach (Language language in Enum.GetValues(typeof(Language))) {
                    builder.Append($@"
        -l{language.GetInfo().extension.ToLower()} | -{language.ToString().ToLower()}           {language.ToString()} 语言输出目录,多目录[,]隔开");
                }
                return builder.ToString();
            }
        }
        static void Main(string[] args) {
            try {
                Logger.SetLogger(new LogHelper());
                Scorpio.Commons.Util.PrintSystemInfo();
                Logger.info("Scov Version : " + Version.version);
                Logger.info("Build Date : " + Version.date);
                var command = CommandLine.Parse(args);
                var type = command.GetValue("-type","-t");              //操作类型 默认转换excel install 自动拷贝 ScorpioProto库
                if (type == null || type.Length == 0) { type = command.Type; }
                if (type == null) { type = ""; }
                var languageDirectory = new Dictionary<Language, string>();     //各语言文件生成目录
                foreach (Language language in Enum.GetValues(typeof(Language))) {
                    //各语言文件输出目录 多目录[,]隔开
                    var dir = command.GetValue("-l" + language.GetInfo().extension.ToLower());
                    if (string.IsNullOrWhiteSpace(dir)) {
                        dir = command.GetValue("-" + language.ToString().ToLower());
                    }
                    if (!string.IsNullOrWhiteSpace(dir)) {
                        languageDirectory.Add(language, dir);
                    }
                }
                switch (type) {
                    case "register": Register(); return;                        //注册命令行
                    case "install": Install(languageDirectory); return;         //引入各语言需要的库文件
                    default: Convert(languageDirectory, command); return;       //转换文件
                }
            } catch (Exception e) {
                Logger.error($"exec is error : " + e.ToString());
            }
        }
        static void Register() {
            Scorpio.Commons.Util.RegisterApplication(Scorpio.Commons.Util.BaseDirectory + "/" + AppDomain.CurrentDomain.FriendlyName);
        }
        static void Install(Dictionary<Language, string> languageDirectory) {
            if (languageDirectory.Count == 0) { throw new Exception("请至少选择一种语言"); }
#if DEBUG
            var version = "master";
#else
            var version = $"v{Version.version}";
#endif
            var fileName = Path.GetFullPath($"{Environment.CurrentDirectory}/{System.Guid.NewGuid().ToString("N")}");
            try {
                if (!Download(version, $"{fileName}.zip")) {
                    version = "master";
                    if (!Download(version, $"{fileName}.zip")) {
                        throw new Exception("库文件下载失败");
                    }
                }
                Logger.info("开始解压文件...");
                ZipFile.ExtractToDirectory($"{fileName}.zip", fileName, true);
                foreach (var language in languageDirectory) {
                    var pathName = language.Key == Language.Go ? "scorpioproto" : "ScorpioProto";
                    var sourceDir = $"{fileName}/ScorpioConversion-{version}/ScorpioProto/{language.Key}/src/{pathName}/";
                    var targets = language.Value.Split(",");
                    foreach (var target in targets) {
                        var targetDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, $"{target}/{pathName}"));
                        Logger.info($"拷贝目录 {sourceDir} -> {targetDir}");
                        FileUtil.DeleteFiles(targetDir, "*", true);
                        FileUtil.CopyFolder(sourceDir, targetDir, "*");
                    }
                }
            } finally {
                FileUtil.DeleteFile($"{fileName}.zip");
                FileUtil.DeleteFiles(fileName, "*", true);
            }
        }
        static bool Download(string version, string fileName) {
            var request = (HttpWebRequest)HttpWebRequest.Create($"https://github.com/qingfeng346/ScorpioConversion/archive/{version}.zip");
            Logger.info($"开始下载库文件... : {version}");
            try {
                using (var response = request.GetResponse()) {
                    using (var stream = response.GetResponseStream()) {
                        var bytes = new byte[READ_LENGTH];
                        using (var fileStream = new FileStream(fileName, FileMode.CreateNew)) {
                            while (true) {
                                var readSize = stream.Read(bytes, 0, READ_LENGTH);
                                if (readSize <= 0) { break; }
                                fileStream.Write(bytes, 0, readSize);
                            }
                            return true;
                        }
                    }
                }
            } catch (System.Exception e) {
                Logger.error("下载库文件失败 : {0}", e.Message);
            }
            return false;
        }
        static void Convert(Dictionary<Language, string> languageDirectory, CommandLine command) {
            if (languageDirectory.Count == 0) { throw new Exception("至少选择一种语言"); }
            var packageName = command.GetValueDefault("-package", "scov");  //默认 命名空间
            var files = command.GetValue("-files");                         //需要转换的文件 多文件[,]隔开
            var paths = command.GetValue("-paths");                         //需要转换的文件目录 多路径[,]隔开
            var data = command.GetValue("-data");                           //数据文件输出目录 多目录[,]隔开
            var suffix = command.GetValueDefault("-suffix", "data");        //数据文件后缀 默认 .data
            var name = command.GetValueDefault("-name", "file");            //名字使用文件名或者sheet名字
            var config = command.GetValue("-config");                       //配置文件路径 多路径[,]隔开
            var spawns = command.GetValue("-spawns");                       //派生文件列表 多个Key[,]隔开
            var fileList = new List<string>();                              //所有要生成的excel文件
            var spawnList = new Dictionary<string, string>();               //派生文件列表
            var parser = new PackageParser();
            var script = parser.Script;
            if (!config.IsEmptyString()) {
                foreach (var dir in config.Split(",")) {
                    parser.Parse(dir);
                }
            }
            if (!files.IsEmptyString()) {
                foreach (var file in files.Split(",")) {
                    fileList.Add(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, file)));
                }
            }
            if (!paths.IsEmptyString()) {
                foreach (var path in paths.Split(",")) {
                    foreach (var file in Directory.GetFiles(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, path)), "*", SearchOption.AllDirectories)) {
                        if (!file.Contains("~$") && (file.EndsWith(".xls") || file.EndsWith(".xlsx"))) {
                            fileList.Add(file);
                        }
                    }
                }
            }
            if (fileList.Count == 0)
                throw new Exception("至少选择一个excel文件");
            if (!spawns.IsEmptyString()) {
                foreach (var spawn in spawns.Split(",")) {
                    spawnList[spawn] = "";
                }
            }
            var successTables = new List<string>();
            var successSpawns = new Dictionary<string, List<string>>();
            var isFileName = name.ToLower() == "file";
            foreach (var file in fileList) {
                var fileName = Path.GetFileNameWithoutExtension(file).Trim();
                var extension = Path.GetExtension(file);
                var tempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Guid.NewGuid().ToString("N") + extension);
                try {
                    File.Copy(file, tempFile, true);
                    using (var fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read)) {
                        IWorkbook workbook = null;
                        if (extension.EndsWith(".xls")) {
                            workbook = new HSSFWorkbook(fileStream);
                        } else if (extension.EndsWith(".xlsx")) {
                            workbook = new XSSFWorkbook(fileStream);
                        }
                        if (workbook == null) {
                            throw new Exception("不支持的文件后缀 : " + extension);
                        }
                        for (var i = 0; i < workbook.NumberOfSheets; ++i) {
                            var sheet = workbook.GetSheetAt(i);
                            if (sheet.SheetName.IsInvalid()) { continue; }
                            try {
                                var builder = new TableBuilder();
                                builder.SetPackageName(packageName);
                                builder.SetName(isFileName ? fileName : sheet.SheetName.Trim());
                                builder.SetSuffix(suffix);
                                builder.SetSpawn(spawnList);
                                builder.Parse(sheet, data, languageDirectory, parser);
                                Logger.info($"文件:{file} Sheet:{sheet.SheetName} 解析完成");
                                if (builder.IsSpawn) {
                                    if (successSpawns.ContainsKey(builder.Spawn)) {
                                        successSpawns[builder.Spawn].Add(builder.Name);
                                    } else {
                                        successSpawns[builder.Spawn] = new List<string>() { builder.Name };
                                    }
                                } else {
                                    successTables.Add(builder.Name);
                                }
                            } catch (Exception e) {
                                Logger.error($" 文件:{file} Sheet:{sheet.SheetName} 解析出错 : " + e.ToString());
                            }
                        }
                    }
                } catch (Exception e) {
                    Logger.error($" 文件 [{file}] 执行出错 : " + e.ToString());
                } finally {
                    File.Delete(tempFile);
                }
            }
            foreach (var pair in languageDirectory) {
                var language = pair.Key;
                foreach (var table in parser.Tables) {
                    ScorpioConversion.Util.CreateDataClass(language, packageName, table.Key, table.Value.Fields, pair.Value);
                }
                foreach (var @enum in parser.Enums) {
                    ScorpioConversion.Util.CreateEnumClass(language, packageName, @enum.Value, pair.Value);
                }
            }
            if (script.HasGlobal("BuildOver")) {
                script.GetGlobal("BuildOver").call(ScriptValue.Null, successTables, successSpawns, command, parser);
            }
        }
    }
}
