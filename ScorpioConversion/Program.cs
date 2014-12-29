using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace ScorpioConversion
{
    static class Program
    {
        private static string launch;      //启动方式
        private static string Path;
        private static string Package;
        private static string CSOut;
        private static string JavaOut;
        private static string ScoOut;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //string[] args = Environment.GetCommandLineArgs();
            //try {
            //    for (int i = 0; i < args.Length; ++i) {
            //        if (args[i] == "-p") {
            //            Package = args[i + 1];
            //        } else if (args[i] == "-m") {
            //            Path = args[i + 1];
            //        } else if (args[i] == "-co") {
            //            CSOut = args[i + 1];
            //        } else if (args[i] == "-jo") {
            //            JavaOut = args[i + 1];
            //        } else if (args[i] == "-so") {
            //            ScoOut = args[i + 1];
            //        }
            //    }
            //} catch (System.Exception ex) {
            //    Console.WriteLine("参数出错 -p [package] -m [sco配置目录] -co [cs生成目录] -jo [java生成目录] -so [Sco脚本生成目录] error : " + ex.ToString());
            //    goto exit;
            //}
            //if (args.Length == 0 || args[0] == "") {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
        //    } else {
        //        try {

        //        } catch (System.Exception ex) {
        //            Console.WriteLine("error : " + ex.ToString());
        //        }
        //    }
        //exit:
        //    Console.WriteLine("生成完成");
        //    Console.ReadKey();
        }
    }
}
