using System;
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
                var tableManger = new TableManager();
                //输出 Test 表 ID 为 10001 的数据 的 TestString 字段
                Console.WriteLine(tableManger.GetTest().GetElement(10001).getTestString());
                //输出 Test 表的数据数量
                Console.WriteLine(tableManger.GetTest().Count());
                //数据关键字表 Spawn_Test1 ID 为 10000 的 TestString 字段
                Console.WriteLine(tableManger.GetSpawns(TableManager.Spawn.Spawn_Test1).GetElement(10000).getTestString());
                //也可以使用字符串获取关键字表
                Console.WriteLine(tableManger.GetSpawns_Spawn("Test1").GetElement(10000).getTestString());

                Console.WriteLine("======================华丽的分割线=============================");
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
            } catch (System.Exception ex) {
                Console.WriteLine("Error : " + ex.ToString());
            }
            Console.ReadKey();
        }
    }
}
