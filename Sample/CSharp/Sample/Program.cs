using System;
using System.Collections.Generic;
using Scorpio.Table;
using ScorpioProtoTest;
using System.IO;
namespace Sample {
    class TableDis : TableUtil.ITableUtil {
        public byte[] GetBuffer(string resource) {
            string path = Path.Combine(Environment.CurrentDirectory, "../../Data/" + resource + ".data");
            return File.ReadAllBytes(path);
        }
        public void Warning(string str) {
            Console.WriteLine(str);
        }
    }
    class Program {
        static void Main(string[] args) {
            try
            {
                TableUtil.SetTableUtil(new TableDis());
                Console.WriteLine("======================下面代码是c#读取Data数据=============================");
                var tableManger = new TableManager();
                //输出 Test 表 ID 为 10001 的数据 的 TestString 字段
                Console.WriteLine(tableManger.GetTest().GetElement(10001).getTestString());
                //输出 Test 表的数据数量
                Console.WriteLine(tableManger.GetTest().Count());
                //数据关键字表 Spawn_Test1 ID 为 10000 的 TestString 字段
                Console.WriteLine(tableManger.GetSpawns(TableManager.Spawn.Spawn_Test1).GetElement(10000).getTestString());
                //也可以使用字符串获取关键字表
                Console.WriteLine(tableManger.GetSpawns_Spawn("Test1").GetElement(10000).getTestString());

                Console.WriteLine("======================下面代码是java消息序列化代码=============================");
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
                Console.WriteLine("数据长度 " + bytes.Length);
                //反序列化协议
                Msg_C2G_Test deser = Msg_C2G_Test.Deserialize(bytes);
                Console.WriteLine(deser.getValue2());
                Console.WriteLine(deser.getValue3().Count + "   " + deser.getValue3()[2]);
                //HasSign函数可以返回此协议是否给某字段赋值了 传入的值为 协议原型定义的索引
                Console.WriteLine(deser.HasSign(3));


                Console.WriteLine("======================下面代码是Sco脚本读取Data数据=============================");
                //剩下的是使用c#版sco的代码 要使用sco 请加入 SCORPIO_PROTO_SCO 编译符号 并引用 Scorpio.dll或导入Scorpio源码 ScorpioProto里有sco的代码
                Scorpio.Script script = new Scorpio.Script();
                script.LoadLibrary();
                //请先引入 TableUtil
                script.SetObject("TableUtil", typeof(Scorpio.Table.TableUtil));
                var files = Directory.GetFiles("../../Sco", "*", SearchOption.AllDirectories);
                foreach (var file in files) {
                    script.LoadFile(file);
                }
                script.LoadString(@"
//输出 Test 表 ID 为 10001 的数据 的 TestString 字段 (注意 脚本里面是直接使用的变量字段名 c#里面是使用的函数)
print(TableManager.GetTest().GetElement(10001).TestString)
 //输出 Test 表的数据数量
print(TableManager.GetTest().Count());
//数据关键字表 Spawn_Test1 ID 为 10000 的 TestString 字段 如果想动态获取请使用TableManager[""GetSpawn_"" + ""Test1""]
print(TableManager.GetSpawn_Test1().GetElement(10000).TestString);
//数据关键字表 Spawn_Test2 ID 为 10000 的 TestString 字段
print(TableManager.GetSpawn_Test2().GetElement(10000).TestString);
");
                Console.WriteLine("======================下面代码是Sco消息序列化代码=============================");
                //请先引入 ScorpioSerializer
                script.SetObject("ScorpioSerializer", typeof(Scorpio.Commons.ScorpioSerializer));
                script.LoadString("" + 
    		    //先初始化数据Table 要跟序列化的数据字段名字对上 例如要使用 Msg_C2G_Test
    		    "var ser = {Value2 = \"123123\", Value3 = [1,2,3,4,5,6]}" +
    		    //序列化数据 第一个参数传入 Script句柄  _SCRIPT为Script句柄, 第二个参数为 数据内容, 第三个参数为 数据的模板
    		    "var bytes = ScorpioSerializer.Serialize(_SCRIPT, ser, \"Msg_C2G_Test\")" + 
    		    //输出数据长度
    		    "print(\"数据长度\" + bytes.Length)" +
    		    //反序列化数据 第一个参数为Script句柄, 第二个参数为数据, 第三个参数为数据模板, 第四个参数固定为true
    		    "var deser = ScorpioSerializer.Deserialize(_SCRIPT, bytes, \"Msg_C2G_Test\", true)" +
    		    //输出数据 sco 暂时不支持 HasSign 的判断
    		    "print(deser.Value2)" + 
    		    "print(array.count(deser.Value3) + \"   \" + deser.Value3[2])"
    );

            } catch (System.Exception ex) {
                Console.WriteLine("Error : " + ex.ToString());
            }
            Console.ReadKey();
        }
    }
}
