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
using System.Resources;

namespace Scorpio.Conversion {
    class Program {
//        const int READ_LENGTH = 8192;
//        private static string HelpRegister = @"
//注册运行程序到环境变量";
//        private static string HelpInstall = $@"
//载入对应语言的库文件
//    -version|-v     版本号，默认为当前程序版本
//{HelpLanguages}";
//        private static string HelpReset = $@"
//刷新表注释
//    -files          需要转换的excel文件,多文件[{Extend.Separator}]隔开
//    -paths          需要转换的excel文件目录 多路径[{Extend.Separator}]隔开
//    -config         配置文件路径 多路径[{Extend.Separator}]隔开
//";
//        private static string HelpExecute = $@"
//命令列表
//    register        注册运行程序到环境变量
//    install         载入对应语言的库文件
//    reset           刷新表注释
//    [other]         转换excel文件
//        -package        命名空间,默认 sco
//        -files          需要转换的excel文件,多文件[{Extend.Separator}]隔开
//        -paths          需要转换的excel文件目录 多路径[{Extend.Separator}]隔开
//        -tags           需要过滤的文件tags 多tag[{Extend.Separator}]隔开
//        -data           数据文件输出目录 多目录[{Extend.Separator}]隔开
//        -suffix         数据文件后缀 默认[.data]
//        -fileSuffix     程序文件后缀 默认各语言默认后缀
//        -name           输出文件使用文件名或者sheet名字,默认file, 选项 [file,sheet]
//        -config         配置文件路径 多路径[{Extend.Separator}]隔开
//        -l10n           输出L10N配置文件
//{HelpLanguages}
//";
        private static readonly string HelpBuild = $@"
转换excel文件
    --config|-confg     sco(https://github.com/qingfeng346/Scorpio-CSharp)配置文件
    --files|-files      Excel文件路径,多文件空格隔开
    --paths|-paths      Excel文件夹(仅扫描 xls|xlsx|xlsb|csv 文件),多路径空格隔开
    --name|-name        输出文件使用文件名或者sheet名字,默认file, 选项 [file,sheet]
    --tags|-tags        需要过滤的tags,多tag空格隔开
    --info|-info        Build信息
";
        private static readonly string HelpDecompile = $@"
反编译文件
    --files|-files      需要反编译的文件,多文件空格隔开
    --output|-output    导出目录,默认当前目录
    --reader|-reader    反序列化Reader,默认DefaultReader
";
    //    static string HelpLanguages {
    //        get {
    //            return "";
    ////            var builder = new StringBuilder(@"
    ////支持语言列表");
    ////            foreach (Language language in Enum.GetValues(typeof(Language))) {
    ////                builder.Append($@"
    ////    -l{language.GetInfo().extension.ToLower()} | -{language.ToString().ToLower()}           {language.ToString()} 语言输出目录,多目录[{Util.Separator}]隔开");
    ////            }
    ////            return builder.ToString();
    //        }
    //    }
        private readonly static string[] ParameterConfig = new[] { "--config", "-config" };     //配置文件
        private readonly static string[] ParameterFiles = new[] { "--files", "-files" };        //所有的excel文件
        private readonly static string[] ParameterPaths = new[] { "--paths", "-paths" };        //所有的excel文件目录,会
        private readonly static string[] ParameterTags = new[] { "--tags", "-tags" };           //需要过滤的文件tags
        private readonly static string[] ParameterName = new[] { "--name", "-name" };           //导出名字使用文件名还是sheet名
        private readonly static string[] ParameterInfo = new[] { "--info", "-info" };           //Build信息
        private readonly static string[] ParameterOutput = new[] { "--output", "-output" };     //输出目录
        private readonly static string[] ParameterReader = new[] { "--reader", "-reader" };     //反编译的Reader
        static void Main(string[] args) {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var assembly = typeof(Program).Assembly;
            GeneratorManager.Instance.Add(assembly);
            ReaderManager.Instance.Add(assembly);
            WriterManager.Instance.Add(assembly);
            HandlerManager.Instance.Add(assembly);
            var perform = new Perform();
            //perform.AddExecute("register", HelpRegister, Register);
            //perform.AddExecute("install", HelpInstall, Install);
            //perform.AddExecute("reset", HelpReset, Reset);
            //perform.AddExecute("format", HelpReset, Format);
            perform.AddExecute("build", HelpBuild, Build);
            perform.AddExecute("decompile", HelpDecompile, Decompile);
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
        static void Decompile(Perform perform, CommandLine command, string[] args) {
            var files = new List<string>();
            files.AddRange(command.GetValues(ParameterFiles));
            var output = perform.GetPath(ParameterOutput);
            if (!Directory.Exists(output)) { Directory.CreateDirectory(output); }
            var reader = command.GetValueDefault(ParameterReader, "default");
            foreach (var file in files) {
                var tempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
                try {
                    File.Copy(file, tempFile, true);
                    new TableDecompile().Decompile(tempFile, Path.GetFileNameWithoutExtension(file), output, reader);
                    Logger.info($"反编译 {file} 完成");
                } catch (System.Exception e) {
                    Logger.error($"文件 [{file}] 反编译出错 : " + e.ToString());
                } finally {
                    File.Delete(tempFile);
                }
            }
        }
        static void Build(Perform perform, CommandLine command, string[] args) {
            Config.Initialize(command.GetValues(ParameterConfig), 
                              command.GetValues(ParameterFiles), 
                              command.GetValues(ParameterPaths), 
                              command.GetValues(ParameterTags), 
                              command.GetValue(ParameterInfo));
            var useFileName = command.GetValueDefault(ParameterName, "sheet").ToLower() == "file";
            if (Config.Files.Count == 0) throw new System.Exception("至少选择一个excel文件");
            var successTables = new List<TableBuilder>();
            var successSpawns = new SortedDictionary<string, List<TableBuilder>>();
            foreach (var file in Config.Files) {
                var tempFile = Path.GetTempFileName();
                try {
                    File.Copy(file, tempFile, true);
                    using var fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read);
                    using var excelReader = ExcelReaderFactory.CreateReader(fileStream);
                    foreach (DataTable dataTable in excelReader.AsDataSet().Tables) {
                        var sheetName = dataTable.TableName;
                        if (sheetName.IsInvalid()) { continue; }
                        try {
                            var stopwatch = Stopwatch.StartNew();
                            var builder = new TableBuilder();
                            builder.SetFileName(useFileName ? Path.GetFileNameWithoutExtension(file) : sheetName.Trim());
                            builder.SetSource($"{file} - {sheetName}");
                            if (!builder.Parse(dataTable)) {
                                continue;
                            }
                            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                            Logger.info(string.Format("File:{0,-20} Sheet:{1,-20} 解析完成  有效列:{2,-5}  有效行:{3,-5}  耗时:{4,-5}", Path.GetFileName(file).Breviary(18), sheetName.Breviary(18), builder.PackageClass.Fields.Count, builder.DataCount, elapsedMilliseconds > 10000 ? $"{elapsedMilliseconds / 1000}秒" : $"{elapsedMilliseconds}毫秒"));
                            if (builder.IsSpawn) {
                                if (successSpawns.TryGetValue(builder.Spawn, out var array)) {
                                    array.Add(builder);
                                } else {
                                    successSpawns[builder.Spawn] = new List<TableBuilder>() { builder };
                                }
                            } else {
                                successTables.Add(builder);
                            }
                        } catch (System.Exception e) {
                            Logger.error($"文件:[{Path.GetFileName(file)}] Sheet:[{sheetName}] 解析出错 : {e}");
                        }
                    }
                } catch (System.Exception e) {
                    Logger.error($"文件 [{file}] 执行出错 : {e}");
                } finally {
                    File.Delete(tempFile);
                }
            }
            successTables.Sort((a, b) => { return a.Name.CompareTo(b.Name); });
            foreach (var pair in successSpawns) {
                pair.Value.Sort((a, b) => { return a.FileName.CompareTo(b.FileName); });
            }
            Config.BuildInfo.GenerateCustom(Config.Parser);
            Config.BuildInfo.Handle(successTables, successSpawns, command);
        }
    }
}