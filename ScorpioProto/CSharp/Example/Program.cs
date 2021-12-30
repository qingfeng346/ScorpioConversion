using System;
using System.IO;
using Datas;
using Scorpio.Conversion;
public class Program {
    public static void Main(string[] args) {
        var table = new TableTest();
        using (var file = File.OpenRead("Test.data")) {
            table.Initialize("Test", new DefaultReader(file));
        }
        int a = 0;
    }
}