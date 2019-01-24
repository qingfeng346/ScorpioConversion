using System;
using System.Diagnostics;
using System.Collections.Generic;
using Scorpio.Commons;
using System.IO;
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
        static void Main(string[] args)
        {
            try {
                Logger.SetLogger(new LogHelper());
                var command = CommandLine.Parse(args);
                var package = command.GetValue("-package");
                var files = command.GetValue("-files");
                var data = command.GetValue("-data");
                var name = command.GetValue("-name");
                var cs = command.GetValue("-cs");
                var java = command.GetValue("-java");
                var sco = command.GetValue("-sco");
                if (string.IsNullOrEmpty(files))
                    throw new Exception("找不到 files 参数");
                var languageDirectory = new Dictionary<Language, string>();
                if (!string.IsNullOrWhiteSpace(cs)) languageDirectory.Add(Language.CSharp, cs);
                if (!string.IsNullOrWhiteSpace(java)) languageDirectory.Add(Language.Java, java);
                if (!string.IsNullOrWhiteSpace(sco)) languageDirectory.Add(Language.Scorpio, sco);
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
                                if (sheet.SheetName.Trim().IsInvalid()) { continue; }
                                var className = string.IsNullOrWhiteSpace(name) || name.ToLower() == "file" ? fileName : sheet.SheetName.Trim();
                                new TableBuilder().Parse(sheet, package, className, false, data, languageDirectory, null);
                            }
                        }
                    } catch (Exception e) {
                        Logger.error($"file [{fullFile}] is error : " + e.ToString());
                    } finally {
                        File.Delete(tempFile);
                    }
                }
            } catch (Exception e) {
                Logger.error($"exec is error : " + e.ToString());
            }
        }
    }
}
