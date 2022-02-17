require("sco/TableManager.sco")
File = importType("System.IO.File")
DefaultReader = importType("Scorpio.Conversion.Runtime.DefaultReader")
function Main() {
    TableManager.GetReader = function(fileName) {
        return new DefaultReader(File.OpenRead("../../${fileName}.data"), true)
    }
    TableManager.getTest().Datas().forEach((key, value) => {
        print(value.toString())
    })
    TableManager.getSpawnTest1().Datas().forEach((key, value) => {
        print(value.toString())
    })
    TableManager.getTestCsv().Datas().forEach((key, value) => {
        print(value.toString())
    })
}
Main()