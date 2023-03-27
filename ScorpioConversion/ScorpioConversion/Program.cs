using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using Scorpio.Commons;
using ExcelDataReader;
using System.Data;
using System.Diagnostics;
using System.Text;
using MiniExcelLibs;
using Scorpio.Conversion.Engine;

namespace Scorpio.Conversion {
    class Program {
        private static readonly string HelpBuild = $@"
转换excel文件
    --config|-confg             sco(https://github.com/qingfeng346/Scorpio-CSharp)配置文件
    --files|-files              Excel文件路径,多文件空格隔开
    --paths|-paths              Excel文件夹(仅扫描 xls|xlsx|csv 文件),多路径空格隔开
    --mergefiles|-mergefiles    Merge Excel文件路径
    --mergepaths|-mergepaths    Merge Excel文件夹
    --tags|-tags                需要过滤的tags,多tag空格隔开
    --branch|-branch            分支列表 /BeginBranch 使用
    --library|-library          额外处理程序
    --name|-name                输出文件使用文件名或者sheet名字,默认file, 选项 [file,sheet]
    --info|-info                Build信息
";
        private static readonly string HelpDecompile = $@"
反编译文件
    --files|-files      需要反编译的文件,多文件空格隔开
    --output|-output    导出目录,默认当前目录
    --library|-library  额外处理程序
    --reader|-reader    反序列化Reader,默认DefaultReader
";
        private readonly static string[] ParameterConfig = new[] { "--config", "-config" };                 //配置文件
        private readonly static string[] ParameterFiles = new[] { "--files", "-files" };                    //所有的excel文件
        private readonly static string[] ParameterPaths = new[] { "--paths", "-paths" };                    //所有的excel文件目录
        private readonly static string[] ParameterMergeFiles = new[] { "--mergefiles", "-mergefiles" };     //所有的merge excel文件
        private readonly static string[] ParameterMergePaths = new[] { "--mergepaths", "-mergepaths" };     //所有的merge excel文件目录
        private readonly static string[] ParameterTags = new[] { "--tags", "-tags" };                       //需要过滤的文件tags
        private readonly static string[] ParameterInfo = new[] { "--info", "-info" };                       //Build信息
        private readonly static string[] ParameterOutput = new[] { "--output", "-output" };                 //输出目录
        private readonly static string[] ParameterReader = new[] { "--reader", "-reader" };                 //反编译的Reader
        private readonly static string[] ParameterBranch = new[] { "--branch", "-branch" };                 //分支列表 /BeginBranch 使用
        private readonly static string[] ParameterLibrary = new[] { "--library", "-library" };              //额外处理程序
        static void Main(string[] args) {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var perform = new Perform();
            perform.AddExecute("build", HelpBuild, Build);
            perform.AddExecute("decompile", HelpDecompile, Decompile);
            try {
                perform.Start(args);
            } catch (System.Exception e) {
                Console.Error.WriteLine(e);
                Environment.Exit(1);
            }
        }
        static void LoadAssembly(CommandLine command) {
            foreach (var path in command.GetValues(ParameterLibrary)) {
                foreach (var file in Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly)) {
                    var assembly = Assembly.LoadFile(file);
                    GeneratorManager.Instance.Add(assembly);
                    ReaderManager.Instance.Add(assembly);
                    WriterManager.Instance.Add(assembly);
                    HandlerManager.Instance.Add(assembly);
                }
            }
            foreach (var assemblyName in typeof(Program).Assembly.GetReferencedAssemblies()) {
                var assembly = Assembly.Load(assemblyName);
                GeneratorManager.Instance.Add(assembly);
                ReaderManager.Instance.Add(assembly);
                WriterManager.Instance.Add(assembly);
                HandlerManager.Instance.Add(assembly);
            }
        }
        static void Decompile(Perform perform, CommandLine command, string[] args) {
            LoadAssembly(command);
            var files = new List<string>();
            files.AddRange(command.GetValues(ParameterFiles));
            var output = perform.GetPath(ParameterOutput);
            if (!Directory.Exists(output)) { Directory.CreateDirectory(output); }
            var reader = command.GetValueDefault(ParameterReader, "default");
            foreach (var file in files) {
                var tempFile = Path.GetTempFileName();
                try {
                    File.Copy(file, tempFile, true);
                    new TableDecompile().Decompile(tempFile, Path.GetFileNameWithoutExtension(file), output, reader);
                    logger.info($"反编译 {file} 完成");
                } catch (System.Exception e) {
                    logger.error($"文件 [{file}] 反编译出错 : " + e.ToString());
                } finally {
                    File.Delete(tempFile);
                }
            }
        }
        static void Build(Perform perform, CommandLine command, string[] args) {
            LoadAssembly(command);
            Config.Initialize(command.GetValues(ParameterConfig), 
                              command.GetValues(ParameterFiles), 
                              command.GetValues(ParameterPaths), 
                              command.GetValues(ParameterMergeFiles),
                              command.GetValues(ParameterMergePaths),
                              command.GetValues(ParameterTags),
                              command.GetValues(ParameterBranch),
                              command.GetValue(ParameterInfo));
            if (Config.Files.Count == 0) throw new System.Exception("至少选择一个excel文件");
            var successTables = new List<TableBuilder>();
            var successSpawns = new SortedDictionary<string, List<TableBuilder>>();
            var mergeFiles = new Dictionary<string, List<Tuple<string, string>>>();
            logger.info("开始计算MergeFile");
            foreach (var file in Config.MergeFiles) {
                var tempFile = Path.GetTempFileName();
                try {
                    File.Copy(file, tempFile, true);
                    var sheetNames = file.IsCsv() ? new[] { Path.GetFileNameWithoutExtension(file) } : MiniExcel.GetSheetNames(tempFile).ToArray();
                    for (var i = 0; i < sheetNames.Length; ++i) {
                        var sheetName = sheetNames[i].Trim();
                        if (sheetName.IsMergeSheet()) {
                            var name = sheetName.GetMergeSheet();
                            if (!mergeFiles.TryGetValue(name, out var mergeFile)) {
                                mergeFiles.Add(name, mergeFile = new List<Tuple<string, string>>());
                            }
                            logger.info($"检测到Merge数据 {file} - {sheetName} ==> {name}");
                            mergeFile.Add(new Tuple<string, string>(file, sheetNames[i]));
                        }
                    }
                } catch (System.Exception e) {
                    throw new System.Exception($"计算MergeFile [{file}] 出错 : {e}");
                } finally {
                    File.Delete(tempFile);
                }
            }
            foreach (var file in Config.Files) {
                var tempFile = Path.GetTempFileName();
                try {
                    File.Copy(file, tempFile, true);
                    using var fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read);
                    var isCsv = file.IsCsv();
                    var sheetNames = isCsv ? new[] { Path.GetFileNameWithoutExtension(file) } : MiniExcel.GetSheetNames(fileStream).ToArray();
                    for (var i = 0; i < sheetNames.Length; ++i) {
                        var sheetName = sheetNames[i].Trim();
                        if (sheetName.IsInvalid()) { continue; }
                        var dataTable = isCsv ? ExcelReaderFactory.CreateCsvReader(fileStream).AsDataSet().Tables[0] : fileStream.QueryAsDataTable(false, sheetName);
                        try {
                            var stopwatch = Stopwatch.StartNew();
                            var builder = new TableBuilder();
                            builder.SetFileName(sheetName);
                            builder.SetSource($"{file} - {sheetName}");
                            var mergeDataTables = new List<DataTable>();
                            if (mergeFiles.TryGetValue(sheetName, out var merges)) {
                                foreach (var merge in merges) {
                                    var mergeTempFile = Path.GetTempFileName();
                                    try {
                                        File.Copy(merge.Item1, mergeTempFile, true);
                                        using (var mergeStream = new FileStream(mergeTempFile, FileMode.Open, FileAccess.Read)) {
                                            mergeDataTables.Add(merge.Item1.IsCsv() ? ExcelReaderFactory.CreateCsvReader(mergeStream).AsDataSet().Tables[0] : mergeStream.QueryAsDataTable(false, merge.Item2));
                                        }
                                    } finally {
                                        File.Delete(mergeTempFile);
                                    }
                                }
                            }
                            if (!builder.Parse(dataTable, mergeDataTables)) {
                                continue;
                            }
                            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                            logger.info(string.Format("File:{0,-20} Sheet:{1,-20} 解析完成  有效列:{2,-5}  有效行:{3,-5}  耗时:{4,-5}", Path.GetFileName(file).Breviary(18), sheetName.Breviary(18), builder.PackageClass.Fields.Count, builder.DataCount, elapsedMilliseconds > 10000 ? $"{elapsedMilliseconds / 1000}秒" : $"{elapsedMilliseconds}毫秒"));
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
                            throw new System.Exception($"文件:[{Path.GetFileName(file)}] Sheet:[{sheetName}] 解析出错 : {e}");
                        }
                    }
                    //using var excelReader = file.IsCsv() ? ExcelReaderFactory.CreateCsvReader(fileStream) : ExcelReaderFactory.CreateReader(fileStream);
                    //foreach (DataTable dataTable in excelReader.AsDataSet().Tables) {
                    //    var sheetName = file.IsCsv() ? Path.GetFileNameWithoutExtension(file) : dataTable.TableName;
                    //    if (sheetName.IsInvalid()) { continue; }
                    //    sheetName = sheetName.Trim();
                    //    try {
                    //        var stopwatch = Stopwatch.StartNew();
                    //        var builder = new TableBuilder();
                    //        builder.SetFileName(sheetName);
                    //        builder.SetSource($"{file} - {sheetName}");
                    //        if (!builder.Parse(dataTable)) {
                    //            continue;
                    //        }
                    //        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    //        Logger.info(string.Format("File:{0,-20} Sheet:{1,-20} 解析完成  有效列:{2,-5}  有效行:{3,-5}  耗时:{4,-5}", Path.GetFileName(file).Breviary(18), sheetName.Breviary(18), builder.PackageClass.Fields.Count, builder.DataCount, elapsedMilliseconds > 10000 ? $"{elapsedMilliseconds / 1000}秒" : $"{elapsedMilliseconds}毫秒"));
                    //        if (builder.IsSpawn) {
                    //            if (successSpawns.TryGetValue(builder.Spawn, out var array)) {
                    //                array.Add(builder);
                    //            } else {
                    //                successSpawns[builder.Spawn] = new List<TableBuilder>() { builder };
                    //            }
                    //        } else {
                    //            successTables.Add(builder);
                    //        }
                    //    } catch (System.Exception e) {
                    //        throw new System.Exception($"文件:[{Path.GetFileName(file)}] Sheet:[{sheetName}] 解析出错 : {e}");
                    //    }
                    //}
                } catch (System.Exception e) {
                    throw new System.Exception($"文件 [{file}] 执行出错 : {e}");
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