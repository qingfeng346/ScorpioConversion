ConversionUtil = importType("Scorpio.Conversion.ConversionUtil")
DefaultReader = importType("Scorpio.Conversion.DefaultReader")
require("sco/TableManager.sco")
function Main() {
    TableManager.getTest().Datas().forEach((key, value) => {
        print(value.toString())
    })
    TableManager.getSpawnTest1().Datas().forEach((key, value) => {
        print(value.toString())
    })
}
Main()