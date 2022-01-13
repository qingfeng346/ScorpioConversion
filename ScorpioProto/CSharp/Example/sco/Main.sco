ConversionUtil = importType("Scorpio.Conversion.ConversionUtil")
require("sco/TableTest.sco")
require("sco/DataTest.sco")
require("sco/Int3.sco")
require("sco/Int2.sco")
function Main(reader) {
    var table = new TableTest()
    table.Initialize("Test", reader)
    table.Datas().forEach((key, value) => {
        print(value.toString())
    })
}