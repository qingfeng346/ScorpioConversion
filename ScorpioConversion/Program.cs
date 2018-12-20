using System;
using System.Diagnostics;
using Scorpio.Commons;
namespace ScorpioConversion
{
    public class LogHelper : ILogger {
        public void info(string value) {
            Console.WriteLine(value);
        }
        public void warn(string value) {
            Console.WriteLine("[warn]" + value);
        }
        public void error(string value) {
            Console.WriteLine("[error]" + value);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Logger.SetLogger(new LogHelper());
            Logger.info("=====================================");
        }
    }
}
