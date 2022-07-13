require("sco/TableManager.sco")
File = importType("System.IO.File")
DefaultReader = importType("Scorpio.Conversion.Runtime.DefaultReader")
function Main() {
    TableManager.GetReader = function(fileName) {
        return new DefaultReader(File.OpenRead("../../${fileName}.data"), true)
    }
    print("=======================Sco-Test=======================");
    TableManager.getTest().Datas().forEach((key, value) => {
        print(value.toString())
    })
    print("=======================Sco-SpawnTest1=======================");
    TableManager.getSpawnTest1().Datas().forEach((key, value) => {
        print(value.toString())
    })
    print("=======================Sco-TestCsv=======================");
    TableManager.getTestCsv().Datas().forEach((key, value) => {
        print(value.toString())
    })
}
Main()