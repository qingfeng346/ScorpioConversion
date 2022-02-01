const fs = require('fs')
const DefaultReader = require("../Scorpio.Conversion.Runtime/index").DefaultReader
const TableManager = require('./src/TableManager')
global.GetReader = function(name) {
    return new DefaultReader(fs.readFileSync(`../../${name}.data`))
}
function main() {
    TableManager.getTest().Datas().forEach((value) => {
        console.log(value.toString())
    })
    TableManager.getSpawnTest1().Datas().forEach((value) => {
        console.log(value.toString())
    })
    // let table = new TableTest()
    // table.Initialize("Test", new DefaultReader(fs.readFileSync("../Test.data")))
    // table.Datas().forEach((value) => {
    //     console.log(value.toString())
    // })
}
main()