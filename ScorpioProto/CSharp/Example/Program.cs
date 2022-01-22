using System;
using System.IO;
using Datas;
using Scorpio.Conversion;
using Scorpio;
using Scorpio.Userdata;

public class Program {
    public static void Main(string[] args) {
        try {

            // var table = new TableTest();
            // using (var file = File.OpenRead("../../Test.data")) {
            //     table.Initialize("Test", new DefaultReader(file));
            // }
            foreach (var pair in TableManager.Instance.getTest().Datas()) {
                Console.WriteLine(pair.Value.ToString());
            }
            // TypeManager.PushAssembly(typeof(IData).Assembly);
            // var script = new Script();
            // script.LoadLibraryV1();
            // script.LoadFile("./sco/Main.sco");
            // using (var file = File.OpenRead("../..//Test.data")) {
            //     script.call("Main", new DefaultReader(file));
            // }
        } catch (Exception ex) {
            Console.WriteLine(ex);
        }
    }
}