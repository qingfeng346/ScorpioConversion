const fs = require('fs')
const DefaultReader = require("./ScorpioProto/DefaultReader")
const TableManager = require('./src/TableManager')
const TableTest = require("./src/TableTest")
global.GetReader = function(name) {
    return new DefaultReader(fs.readFileSync(`../${name}.data`))
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