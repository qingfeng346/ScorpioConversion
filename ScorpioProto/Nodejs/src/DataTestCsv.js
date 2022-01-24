//本文件为自动生成，请不要手动修改

const Int3 = require('./Int3')

class DataTestCsv {
    
    constructor(fileName, reader) {
        this.TestID = reader.ReadInt32()
        this.ID = this.TestID
        this.testEnum = reader.ReadInt32()
        {
            let list = []
            let number = reader.ReadInt32()
            for (let i = 0; i < number; i++) { 
                list.push(new Int3(fileName, reader))
            }
            this.TestDate = list
        }
        this.TestDateTime = reader.ReadDateTime()
        this.TestInt = reader.ReadInt32()
    }
    
    GetData(key) {
        if ("TestID" == key) { return this.TestID }
        if ("testEnum" == key) { return this.testEnum }
        if ("TestDate" == key) { return this.TestDate }
        if ("TestDateTime" == key) { return this.TestDateTime }
        if ("TestInt" == key) { return this.TestInt }
        return null;
    }
    
    Set(value) {
        this.TestID = value.TestID
        this.testEnum = value.testEnum
        this.TestDate = value.TestDate
        this.TestDateTime = value.TestDateTime
        this.TestInt = value.TestInt
    }
    
    toString() {
        return "TestID:" + this.TestID + "," + "testEnum:" + this.testEnum + "," + "TestDate:" + this.TestDate + "," + "TestDateTime:" + this.TestDateTime + "," + "TestInt:" + this.TestInt
    }
}
module.exports = DataTestCsv;
