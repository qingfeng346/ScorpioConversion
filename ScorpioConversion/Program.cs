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
            Console.WriteLine(value);
            Debugger.Log(0, null, value);
        }
        public void warn(string value) {
            Console.WriteLine("[warn] " + value);
            Debugger.Log(0, null, "[warn] " + value);
        }
        public void error(string value) {
            Console.WriteLine("[error] " + value);
            Debugger.Log(0, null, "[error] " + value);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) { return; }
            Logger.SetLogger(new LogHelper());
            var files = args[0].Split(",");
            var baseTemp = Path.GetFullPath("E:/ScorpioConversion/ScorpioConversion/temp");
            foreach (var file in files) {
                var temp = baseTemp + Path.GetExtension(file);
                try {
                    File.Copy(file, temp, true);
                    if (temp.EndsWith(".xlsx")) { continue; }
                    IWorkbook workbook = new HSSFWorkbook(new FileStream(temp, FileMode.Open, FileAccess.Read));
                    for (var i = 0; i < workbook.NumberOfSheets; ++i) {
                        var sheet = workbook.GetSheetAt(i);
                        if (sheet.SheetName.Trim().StartsWith("!")) { continue; }
                        new TableBuilder().Parse(workbook.GetSheetAt(i), Path.GetFileNameWithoutExtension(file), false, null);
                    }
                } catch (Exception e) {
                    Logger.error($"file [{file}] is error : " + e.ToString());
                } finally {
                    File.Delete(temp);
                }
            }
        }
    }
}
