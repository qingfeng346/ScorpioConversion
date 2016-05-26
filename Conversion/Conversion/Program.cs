using System;
using System.IO;
using System.Collections.Generic;
namespace Conversion {
    public class LibLogger : ILogger {
        public void info(string value) {
            Console.WriteLine(value);
        }
        public void warn(string value) {
            Console.WriteLine(value);
        }
        public void error(string value) {
            Console.WriteLine(value);
        }
    }
    class Program {
        static Dictionary<string, string> Args = new Dictionary<string, string>();
        static string Get(string key) {
            return Args.ContainsKey(key) ? Args[key] : "";
        }
        static Dictionary<PROGRAM, ProgramConfig> GetProgramConfig() {
            Dictionary<PROGRAM, ProgramConfig> configs = new Dictionary<PROGRAM, ProgramConfig>();
            for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT; ++i) {
                PROGRAM program = (PROGRAM)i;
                configs.Add(program, new ProgramConfig() {
                    CodeDirectory = Get("-" + program.ToString().ToLower() + "code"),
                    DataDirectory = Get("-" + program.ToString().ToLower() + "data"),
                    Create = Args.ContainsKey("-" + program.ToString().ToLower() + "code") ? "true" : "false",
                    Compress = Get("-" + program.ToString().ToLower() + "com"),
                });
            }
            return configs;
        }
        static void Main(string[] args) {
            Logger.SetLogger(new LibLogger());
            try {
                int length = args.Length;
                string key = "";
                for (var i = 0;i < length;++i) {
                    var arg = args[i];
                    if (string.IsNullOrEmpty(key)) {
                        if (!arg.StartsWith("-")) throw new Exception("参数值只能是单个参数");
                        key = arg;
                    } else {
                        Args[key] = arg;
                        key = "";
                    }
                }
                if (!Args.ContainsKey("-t") || !Args.ContainsKey("-file") || !Args.ContainsKey("-package")) {
                    throw new Exception("[-t -file -package] 三个参数是必填项");
                }
                int t = int.Parse(Args["-t"]);
                string fileArg = Args["-file"];
                if (t == 0 || t == 1) {
                    var files = t == 1 ? string.Join(";", Directory.GetFiles(fileArg, "*.xls", SearchOption.AllDirectories)) : fileArg;
                    new TableBuilder().Transform(files, Get("-config"), Args["-package"], Get("-spawn"), Util.ToBoolean(Get("-manager"), false), false, GetProgramConfig());
                }
                Console.WriteLine("执行成功");
                Console.ReadKey();
                return;
            } catch (System.Exception ex) {
                Console.WriteLine("Exec is error : " + ex.ToString());
            }
            string hint = @"
-t              (必填)类型 0转换单个表 1转换文件夹
-file           (必填)根据类型填不同数据
-config         table表配置 自定义结构
-package        (必填)生成的包名
-spawn          关键字列表 多关键字用;隔开
-manager        是否生成tablemanager 默认为false
";
            for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT; ++i) {
                PROGRAM program = (PROGRAM)i;
                string str = @"-__programcode       __program 代码生成路径
-__programdata      __program Data生成路径
-__programcom       __program Data是否压缩
";
                hint += str.Replace("__program", program.ToString().ToLower());
            }
            hint += @"
    注：code和Data路径如果是多路径，请使用【;】隔开";
            Console.WriteLine(hint);
            Console.ReadKey();
        }
    }
}
