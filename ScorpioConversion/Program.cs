using System;
using System.Diagnostics;
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
            Logger.SetLogger(new LogHelper());
            if (args.Length == 0) { return; }
            var files = args[0].Split(",");
            foreach (var file in files) {
                var fullFile = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, file));
                var fileName = Path.GetFileNameWithoutExtension(file);
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
                            new TableBuilder().Parse(sheet, fileName, false, null);
                        }
                    }
                } catch (Exception e) {
                    Logger.error($"file [{fullFile}] is error : " + e.ToString());
                } finally {
                    File.Delete(tempFile);
                }
            }
        }
    }
}
