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
// const Int2 = require("./src/Int2").Int2
// const fs = require('fs')
// const ScorpioReader = require('./ScorpioProto/ScorpioReader')
// const DataTest = require("./src/DataTest").DataTest
// var data = new DataTest()
// data._TestID = 100
// console.log(data.getTestID())
// let a = DataTest.Read()
// console.log(a.getTestID())

// var a = new Array()
// a.push(100)
// console.log(a.length)
// for (let i = 0; i < 10; ++i) {
//     console.log(i)
// }
// var a = {}
// a["100"] = 200
// if (a["200"] == null) {
//     console.log("true")
// } else {
//     console.log("false")
// }
// let t = new TableTest()
// t.Initialize("", new ScorpioReader(fs.readFileSync("../Test.data")))
// let a = 0;
// class TestClass1 {
//     constructor() {
//         this.num = 11111
//     }
// }
// class TestClass2 {
//     constructor() {
//         this.num = 22222
//     }
//     test() {
//         console.log(this.num)
//     }
// }
// let t1 = new TestClass1()
// let t2 = new TestClass2()
// t1.call = t2.test.bind(t2)
// t1.call()
// let a = 100
// Number.prototype.test = () => {
//     console.log("==============================")
// }
// a.test()
// let b = {}
// Object.prototype.test = () => {
//     console.log("111111111111111111111")
// }
// b.test()
// function t() {

// }
// let c = new t()
// t.prototype.test = () => {
//     console.log("111111111111111111111")
// }
// c.test()
// class eeee {
    
//     function www() {

//     }
// }
// // var f = new eeee()
// // eeee.prototype.fffff = () => {}
// // f.fffff()
// eeee.www = () => {

// }

