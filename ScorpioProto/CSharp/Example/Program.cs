using System;
using System.IO;
using Datas;
using Scorpio.Conversion;
using Scorpio;
using Scorpio.Userdata;

public class Program {
    private class GetReader : ScorpioHandle {
        public ScriptValue Call(ScriptValue obj, ScriptValue[] Parameters, int length) {
            return ScriptValue.CreateValue(new DefaultReader(File.OpenRead($"../../{Parameters[0].ToString()}.data"), true));
        }
    }
    public static void Main(string[] args) {
        try {
            foreach (var pair in TableManager.Instance.Test.Datas()) {
                Console.WriteLine(pair.Value.ToString());
            }
            foreach (var pair in TableManager.Instance.SpawnTest1.Datas()) {
                Console.WriteLine(pair.Value.ToString());
            }
            TypeManager.PushAssembly(typeof(IData).Assembly);
            var script = new Script();
            script.LoadLibraryV1();
            script.PushSearchPath("./sco");
            script.SetGlobal("GetReader", script.CreateFunction(new GetReader()));
            script.LoadFile("./sco/Main.sco");
        } catch (Exception ex) {
            Console.WriteLine(ex);
        }
    }
}