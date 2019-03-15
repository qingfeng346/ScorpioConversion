using System;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Diagnostics;
using System.Collections.Generic;
using Scorpio.Commons;

using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
namespace ScorpioConversion
{
    public class LogHelper : ILogger {
        public void info(string value) {
            value += "\n";
            Console.WriteLine(value);
            Debugger.Log(0, null, value);
        }
        public void warn(string value) {
            value = "[warn]" + value + "\n";
            Console.WriteLine(value);
            Debugger.Log(0, null, value);
        }
        public void error(string value) {
            value = "[error]" + value + "\n";
            Console.WriteLine(value);
            Debugger.Log(0, null, value);
        }
    }
    class Program
    {
        const int READ_LENGTH = 8192;
        static void Main(string[] args)
        {
            try {
                Logger.SetLogger(new LogHelper());
                var command = CommandLine.Parse(args);
                var type = command.GetValue("-type");           //操作类型 默认转换excel install 自动拷贝 ScorpioProto库
                var package = command.GetValue("-package");     //默认 命名空间
                var files = command.GetValue("-files");         //需要转换的文件 多文件[,]隔开
                var data = command.GetValue("-data");           //data文件输出目录 多目录[,]隔开
                var name = command.GetValue("-name");           //名字使用文件名或者sheet名字
                var config = command.GetValue("-config");       //配置文件路径 多路径[,]隔开
                var spawn = command.GetValue("-spawn");         //派生文件列表 多个Key[,]隔开
                var languageDirectory = new Dictionary<Language, string>();
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
                if ("install".Equals(type)) {
                    if (languageDirectory.Count == 0) {
                        throw new Exception("请至少选择一种语言");
                    }
                    Install(languageDirectory);
                } else {
                    if (string.IsNullOrEmpty(files))
                        throw new Exception("找不到 files 参数");
                    var parser = new PackageParser();
                    if (config != null) {
                        foreach (var dir in config.Split(",")) {
                            parser.Parse(dir);
                        }
                    }
                    foreach (var file in files.Split(",")) {
                        var fullFile = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, file));
                        var fileName = Path.GetFileNameWithoutExtension(file).Trim();
                        var extension = Path.GetExtension(file);
                        var tempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp" + extension);
                        try {
                            File.Copy(fullFile, tempFile, true);
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
                                    new TableBuilder().Parse(sheet, package, name.IsEmptyString() || name.ToLower() == "file" ? fileName : sheet.SheetName.Trim(), spawn, data, languageDirectory, parser);
                                }
                            }
                        } catch (Exception e) {
                            Logger.error($"file [{fullFile}] is error : " + e.ToString());
                        } finally {
                            File.Delete(tempFile);
                        }
                    }
                }
            } catch (Exception e) {
                Logger.error($"exec is error : " + e.ToString());
            }
        }
        static void Install(Dictionary<Language, string> languages) {
            var request = (HttpWebRequest)HttpWebRequest.Create("https://github.com/qingfeng346/ScorpioProto/archive/master.zip");
            var fileName = $"{Environment.CurrentDirectory}/{DateTime.Now.Ticks}";
            try {
                Logger.info("开始下载库文件...");
                using (var response = request.GetResponse()) {
                    using (var stream = response.GetResponseStream()) {
                        var bytes = new byte[READ_LENGTH];
                        using (var fileStream = new FileStream($"{fileName}.zip", FileMode.CreateNew)) {
                            while (true) {
                                var readSize = stream.Read(bytes, 0, READ_LENGTH);
                                if (readSize <= 0) { break; }
                                fileStream.Write(bytes, 0, readSize);
                            }
                        }
                    }
                }
                Logger.info("开始解压文件...");
                ZipFile.ExtractToDirectory($"{fileName}.zip", fileName, true);
                foreach (var language in languages) {
                    var pathName = language.Key == Language.Go ? "scorpioproto" : "ScorpioProto";
                    var sourceDir = $"{fileName}/ScorpioProto-master/{language.Key}/src/{pathName}/";
                    var targets = language.Value.Split(",");
                    foreach (var target in targets) {
                        var targetDir = $"{target}/{pathName}";
                        Logger.info($"拷贝目录 {sourceDir} -> {targetDir}");
                        FileUtil.DeleteFiles(targetDir, "*", true);
                        FileUtil.CopyFolder(sourceDir, targetDir, "*");
                    }
                }
            } finally {
                //FileUtil.DeleteFile($"{fileName}.zip");
                //FileUtil.DeleteFiles(fileName, "*", true);
            }
        }
    }
}
