using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using NPOI.SS.UserModel;
using Scorpio;
using Scorpio.Commons;
using ExcelDataReader;
using System.Data;
using System.Diagnostics;

namespace ScorpioConversion {
    class Program {
        const int READ_LENGTH = 8192;
        private static string HelpRegister = @"
注册运行程序到环境变量";
        private static string HelpInstall = $@"
载入对应语言的库文件
    -version|-v     版本号，默认为当前程序版本
{HelpLanguages}";
        private static string HelpReset = $@"
刷新表注释
    -files          需要转换的excel文件,多文件[{Util.Separator}]隔开
    -paths          需要转换的excel文件目录 多路径[{Util.Separator}]隔开
    -config         配置文件路径 多路径[{Util.Separator}]隔开
";
        private static string HelpExecute = $@"
命令列表
    register        注册运行程序到环境变量
    install         载入对应语言的库文件
    reset           刷新表注释
    [other]         转换excel文件
        -package        命名空间,默认 sco
        -files          需要转换的excel文件,多文件[{Util.Separator}]隔开
        -paths          需要转换的excel文件目录 多路径[{Util.Separator}]隔开
        -tags           需要过滤的文件tags 多tag[{Util.Separator}]隔开
        -data           数据文件输出目录 多目录[{Util.Separator}]隔开
        -suffix         数据文件后缀 默认[.data]
        -fileSuffix     程序文件后缀 默认各语言默认后缀
        -name           输出文件使用文件名或者sheet名字,默认file, 选项 [file,sheet]
        -config         配置文件路径 多路径[{Util.Separator}]隔开
        -l10n           输出L10N配置文件
{HelpLanguages}
";
        static string HelpLanguages {
            get {
                return "";
    //            var builder = new StringBuilder(@"
    //支持语言列表");
    //            foreach (Language language in Enum.GetValues(typeof(Language))) {
    //                builder.Append($@"
    //    -l{language.GetInfo().extension.ToLower()} | -{language.ToString().ToLower()}           {language.ToString()} 语言输出目录,多目录[{Util.Separator}]隔开");
    //            }
    //            return builder.ToString();
            }
        }
        static void Main(string[] args) {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var perform = new Perform();
            //perform.AddExecute("register", HelpRegister, Register);
            //perform.AddExecute("install", HelpInstall, Install);
            //perform.AddExecute("reset", HelpReset, Reset);
            //perform.AddExecute("format", HelpReset, Format);
            //perform.AddExecute("decompile", HelpReset, Decompile);
            perform.AddExecute("", HelpExecute, Execute);
            perform.Start(args, null, null);
        }
        //static void Register(Perform perform, CommandLine command, string[] args) {
        //    Scorpio.Commons.Util.RegisterApplication($"{Scorpio.Commons.Util.BaseDirectory}/{AppDomain.CurrentDomain.FriendlyName}");
        //}
        //static void Install(Perform perform, CommandLine command, string[] args) {
        //    Config.Initialize(command);
        //    if (Config.LanguageDirectory.Count == 0) { throw new Exception("至少选择一种语言"); }
        //    var version = command.GetValueDefault(new string[] { "-version", "-v" }, "");
        //    if (string.IsNullOrWhiteSpace(version)) { version = $"v{Version.version}"; }
        //    var fileName = Path.GetFullPath($"{Environment.CurrentDirectory}/{System.Guid.NewGuid().ToString("N")}");
        //    try {
        //        if (!Download(version, $"{fileName}.zip")) {
        //            Logger.error($"版本[{version}]库文件下载失败,开始下载master分支");
        //            version = "master";
        //            if (!Download(version, $"{fileName}.zip")) {
        //                throw new Exception("库文件下载失败");
        //            }
        //        }
        //        Logger.info("开始解压文件...");
        //        ZipFile.ExtractToDirectory($"{fileName}.zip", fileName, true);
        //        //foreach (var language in Config.LanguageDirectory) {
        //        //    var pathName = language.Key == Language.Go ? "scorpioproto" : "ScorpioProto";
        //        //    var sourceDir = $"{fileName}/ScorpioConversion-{version}/ScorpioProto/{language.Key}/src/{pathName}/";
        //        //    var targets = language.Value.Split(Util.Separator);
        //        //    foreach (var target in targets) {
        //        //        var targetDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, $"{target}/{pathName}"));
        //        //        Logger.info($"拷贝目录 {sourceDir} -> {targetDir}");
        //        //        FileUtil.DeleteFolder(targetDir, null, true);
        //        //        FileUtil.CopyFolder(sourceDir, targetDir, null, true);
        //        //    }
        //        //}
        //    } finally {
        //        FileUtil.DeleteFile($"{fileName}.zip");
        //        FileUtil.DeleteFolder(fileName, null, true);
        //    }
        //}
        //static bool Download(string version, string fileName) {
        //    //return Scorpio.Commons.Util.Download($"https://github.com/qingfeng346/ScorpioConversion/archive/{version}.zip", fileName);
        //    return true;
        //}
        //static void Reset(Perform perform, CommandLine command, string[] args) {
        //    Config.Initialize(command);
        //    var fileList = Config.FileList; //所有要生成的excel文件
        //    if (fileList.Count == 0) throw new Exception("至少选择一个excel文件");
        //    foreach (var file in fileList) {
        //        try {
        //            IWorkbook workbook = null;
        //            using (var fileStream = new FileStream(file.file, FileMode.Open, FileAccess.Read)) {
        //                workbook = file.GetWorkbook(fileStream);
        //            }
        //            var enumSheets = new HashSet<string>();
        //            for (var i = 0; i < workbook.NumberOfSheets; ++i) {
        //                var sheet = workbook.GetSheetAt(i);
        //                if (sheet.SheetName.IsInvalid()) { continue; }
        //                try {
        //                    if (new TableBuilder().Reset(workbook, sheet, enumSheets)) {
        //                        Logger.info(string.Format("File:{0,-20} Sheet:{1,-20} Reset Over", file.fileName.Breviary(18), sheet.SheetName.Breviary(18)));
        //                    }
        //                } catch (Exception e) {
        //                    Logger.error($"文件:[{file.fileName}] Sheet:[{sheet.SheetName}] 解析出错 : " + e.ToString());
        //                }
        //            }
        //            foreach (var enumSheet in enumSheets) {
        //                workbook.SetSheetHidden(workbook.GetSheetIndex(enumSheet), SheetState.VeryHidden);
        //            }
        //            using (var fileStream = new FileStream(file.file, FileMode.Create, FileAccess.ReadWrite)) {
        //                workbook.Write(fileStream);
        //            }
        //        } catch (Exception e) {
        //            Logger.error($"文件 [{file.fileName}] 执行出错 : " + e.ToString());
        //        }
        //    }
        //}
        //static void Format(Perform perform, CommandLine command, string[] args) {
        //    Config.Initialize(command);
        //    var fileList = Config.FileList; //所有要生成的excel文件
        //    if (fileList.Count == 0) throw new Exception("至少选择一个excel文件");
        //    foreach (var file in fileList) {
        //        try {
        //            IWorkbook workbook = null;
        //            using (var fileStream = new FileStream(file.file, FileMode.Open, FileAccess.Read)) {
        //                workbook = file.GetWorkbook(fileStream);
        //            }
        //            for (var i = 0; i < workbook.NumberOfSheets; ++i) {
        //                var sheet = workbook.GetSheetAt(i);
        //                if (sheet.SheetName.IsInvalid()) { continue; }
        //                try {
        //                    TableBuilder.Format(workbook, sheet);
        //                    Logger.info(string.Format("File:{0,-20} Sheet:{1,-20} Format Over", file.fileName.Breviary(18), sheet.SheetName.Breviary(18)));
        //                } catch (Exception e) {
        //                    Logger.error($"文件:[{file.fileName}] Sheet:[{sheet.SheetName}] 解析出错 : " + e.ToString());
        //                }
        //            }
        //            using (var fileStream = new FileStream(file.file, FileMode.Create, FileAccess.ReadWrite)) {
        //                workbook.Write(fileStream);
        //            }
        //        } catch (Exception e) {
        //            Logger.error($"文件 [{file.fileName}] 执行出错 : " + e.ToString());
        //        }
        //    }
        //}
        //static void Decompile(Perform perform, CommandLine command, string[] args) {
        //    var suffix = command.GetValueDefault("-suffix", "data");        //数据文件后缀 默认.data
        //    var output = perform.GetPath("-output");                         //输出目录
        //    var files = new List<string>();
        //    //需要转换的文件 多文件[{Util.Separator}]隔开
        //    Util.Split(command.GetValue("-files"), (file) => files.Add(Path.GetFullPath($"{Environment.CurrentDirectory}/{file}")));
        //    //需要转换的文件目录 多路径[{Util.Separator}]隔开
        //    Util.Split(command.GetValue("-paths"), (path) => files.AddRange(Directory.GetFiles(Path.GetFullPath($"{Environment.CurrentDirectory}/{path}"))));
        //    Logger.info($"输出目录 {output}");
        //    if (!Directory.Exists(output)) { Directory.CreateDirectory(output); }
        //    foreach (var file in files) {
        //        if (!file.EndsWith(suffix)) { continue; }
        //        var tempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
        //        try {
        //            File.Copy(file, tempFile, true);
        //            new TableDecompile().Decompile(tempFile, Path.GetFileNameWithoutExtension(file), output);
        //            Logger.info($"反编译 {file} 完成");
        //        } catch (Exception e) {
        //            Logger.error($"文件 [{file}] 反编译出错 : " + e.ToString());
        //        } finally {
        //            File.Delete(tempFile);
        //        }
        //    }
        //}
        static void Execute(Perform perform, CommandLine command, string[] args) {
            Config.Initialize(command);
            GeneratorManager.Instance.Add(typeof(Program).Assembly);
            var fileList = Config.FileList; //所有要生成的excel文件
            if (fileList.Count == 0) throw new Exception("至少选择一个excel文件");
            var successTables = new List<TableBuilder>();
            var successSpawns = new SortedDictionary<string, List<TableBuilder>>();
            foreach (var file in fileList) {
                var tempFile = Path.GetTempFileName();
                try {
                    File.Copy(file.file, tempFile, true);
                    using (var fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read)) {
                        using (var excelReader = file.GetExcelReader(fileStream)) {
                            foreach (DataTable dataTable in excelReader.AsDataSet().Tables) {
                                var sheetName = dataTable.TableName;
                                if (sheetName.IsInvalid()) { continue; }
                                try {
                                    var stopwatch = Stopwatch.StartNew();
                                    var builder = new TableBuilder();
                                    builder.SetFileName(Config.IsFileName ? file.fileNameWithoutExtension : sheetName.Trim());
                                    builder.SetSource($"{file.file} - {sheetName}");
                                    if (!builder.Parse(dataTable)) {
                                        continue;
                                    }
                                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                                    Logger.info(string.Format("File:{0,-20} Sheet:{1,-20} 解析完成  有效列:{2,-5}  有效行:{3,-5}  耗时:{4,-5}", file.fileName.Breviary(18), sheetName.Breviary(18), builder.PackageClass.Fields.Count, builder.DataCount, elapsedMilliseconds > 10000 ? $"{elapsedMilliseconds / 1000}秒" : $"{elapsedMilliseconds}毫秒"));
                                    if (builder.IsSpawn) {
                                        if (successSpawns.TryGetValue(builder.Spawn, out var array)) {
                                            array.Add(builder);
                                        } else {
                                            successSpawns[builder.Spawn] = new List<TableBuilder>() { builder };
                                        }
                                    } else {
                                        successTables.Add(builder);
                                    }
                                } catch (Exception e) {
                                    Logger.error($"文件:[{file.fileName}] Sheet:[{sheetName}] 解析出错 : {e}");
                                }
                            }
                        }
                    }
                } catch (Exception e) {
                    Logger.error($"文件 [{file.fileName}] 执行出错 : {e}");
                } finally {
                    File.Delete(tempFile);
                }
            }
            Config.LanguageConfig.GenerateCustom(Config.Parser);
            //if (!l10nOutput.IsEmptyString()) {
            //    File.WriteAllText(Path.Combine(Environment.CurrentDirectory, l10nOutput, "./l10n.json"),
            //        Newtonsoft.Json.JsonConvert.SerializeObject(l10nData), new UTF8Encoding(false));
            //}
            //successTables.Sort((a, b) => { return a.DataFileName.CompareTo(b.DataFileName); });
            //foreach (var pair in successSpawns) {
            //    pair.Value.Sort((a, b) => { return a.DataFileName.CompareTo(b.DataFileName); });
            //}
            //var parser = Config.Parser;
            //var script = parser.Script;
            //foreach (var pair in Config.LanguageDirectory) {
            //    var language = pair.Key;
            //    //foreach (var table in parser.Tables) {
            //    //    Util.CreateDataClass(language, Config.PackageName, table.Key, table.Value.Fields, pair.Value, Config.FileSuffix);
            //    //}
            //    //foreach (var @enum in parser.Enums) {
            //    //    Util.CreateEnumClass(language, Config.PackageName, @enum.Value, pair.Value, Config.FileSuffix);
            //    //}
            //}
            //if (script.HasGlobal("BuildOver")) {
            //    script.GetGlobal("BuildOver").call(ScriptValue.Null, successTables, successSpawns, command, parser);
            //}
        }
    }
}