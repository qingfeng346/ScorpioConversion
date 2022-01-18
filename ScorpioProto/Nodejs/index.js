const fs = require('fs')
const DefaultReader = require("./ScorpioProto/DefaultReader")
const TableTest = require("./src/TableTest")
function main() {
    let table = new TableTest()
    table.Initialize("Test", new DefaultReader(fs.readFileSync("../Test.data")))
    table.Datas().forEach((value) => {
        console.log(value.toString())
    })
}
main()