using System;
using System.Collections.Generic;
using Scorpio.Table;
using ScorpioProtoTest;
using System.IO;
namespace Sample {
    class Program {
        class TableDis : TableUtil.ITableUtil {
            public byte[] GetBuffer(string resource) {
                string path = Path.Combine(Environment.CurrentDirectory, "../../Data/" + resource + ".data");
                return File.ReadAllBytes(path);
            }
            public void Warning(string str) {
                Console.WriteLine(str);
            }
            public string GetLanguage(string str) {
                return "";
            }
        }
        static void log(object obj) {
            Console.WriteLine(obj);
        }
        static void Main(string[] args) {
            try {
                TableUtil.SetTableUtil(new TableDis());
                var tableManger = new TableManager();

                log("======================下面代码是c#读取Data数据=============================");
                log("**********下面是中文**********");
                tableManger.setLanguage("CN");
                //获得 Test 表 ID 为 10001 的数据
                log(tableManger.GetTest().GetElement(10000).ToString());
                //获得 关键字表 Spawn_Test1 ID 为 10000 的数据
                log(tableManger.GetSpawns(TableManager.Spawn.Spawn_Test1).GetElement(10000).ToString());
                //获得 关键字表 Spawn_Test2 ID 为 10000 的数据
                log(tableManger.GetSpawns_Spawn("Test2").GetElement(10000).ToString());
                log("**********下面是英文**********");
                tableManger.Reset();
                tableManger.setLanguage("EN");
                log(tableManger.GetTest().GetElement(10000).ToString());
                log(tableManger.GetSpawns(TableManager.Spawn.Spawn_Test1).GetElement(10000).ToString());
                log(tableManger.GetSpawns_Spawn("Test2").GetElement(10000).ToString());


                log("======================下面代码是c#消息序列化代码=============================");
                Msg_C2G_Test ser = new Msg_C2G_Test();
                ser.setValue2("123123");
                List<int> list = new List<int>();
                list.Add(1);
                list.Add(2);
                list.Add(3);
                list.Add(4);
                list.Add(5);
                list.Add(6);
                ser.setValue3(list);
                //序列化协议
                byte[] bytes = ser.Serialize();
                log("序列化后数据长度 : " + bytes.Length + " 数据内容 : " + ser.ToString());
                //反序列化协议
                Msg_C2G_Test deser = Msg_C2G_Test.Deserialize(bytes);
                log(deser.getValue2());
                log(deser.getValue3().Count + "   " + deser.getValue3()[2]);
                //HasSign函数可以返回此协议是否给某字段赋值了 传入的值为 协议原型定义的索引
                log(deser.HasSign(3));
                log("解析后的数据内容 : " + deser.ToString());

                log("======================下面代码是Sco脚本读取Data数据=============================");
                //剩下的是使用c#版sco的代码 要使用sco 请加入 SCORPIO_PROTO_SCO 编译符号 并引用 Scorpio.dll或导入Scorpio源码 ScorpioProto里有sco的代码
                Scorpio.Script script = new Scorpio.Script();
                script.LoadLibrary();
                //请先引入 ScorpioSerializer
                script.SetObject("ScorpioSerializer", typeof(Scorpio.Commons.ScorpioSerializer));
                var files = Directory.GetFiles("../../Sco", "*", SearchOption.AllDirectories);
                foreach (var file in files) {
                    script.LoadFile(file);
                }
                script.LoadFile("../../Sample.sco");
            } catch (System.Exception ex) {
                log("Error : " + ex.ToString());
            }
            Console.ReadKey();
        }
    }
}
