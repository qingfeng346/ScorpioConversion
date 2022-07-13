using System;
using System.IO;
using Datas;
using Scorpio.Conversion.Runtime;
using Scorpio;
using Scorpio.Userdata;

public class Program {
    public static void Main(string[] args) {
        try {
            Console.WriteLine("=======================Test=======================");
            foreach (var pair in TableManager.Instance.Test.Datas()) {
                Console.WriteLine(pair.Value.ToString());
            }
            Console.WriteLine("=======================SpawnTest1=======================");
            foreach (var pair in TableManager.Instance.SpawnTest1.Datas()) {
                Console.WriteLine(pair.Value.ToString());
            }
            Console.WriteLine("=======================TestCsv=======================");
            foreach (var pair in TableManager.Instance.TestCsv.Datas()) {
                Console.WriteLine(pair.Value.ToString());
            }
            TypeManager.PushAssembly(typeof(IData).Assembly);
            var script = new Script();
            script.LoadLibraryV1();
            script.PushSearchPath("./sco");
            script.LoadFile("./sco/Main.sco");
        } catch (Exception ex) {
            Console.WriteLine(ex);
        }
    }
}