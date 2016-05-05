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
            Console.ReadKey();
        }
    }
}
