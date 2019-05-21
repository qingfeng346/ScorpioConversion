
const TestEnum = require('./TestEnum').TestEnum
const Int3 = require('./Int3').Int3

class DataTest {
    
    /*   默认值() */
    getTestID() { return this._TestID; }
    ID() { return this._TestID; }
    /*   默认值() */
    gettestEnum() { return this._testEnum; }
    /*   默认值() */
    getTestDate() { return this._TestDate; }
    
    GetData(key) {
        if ("TestID" == key) return this._TestID;
        if ("testEnum" == key) return this._testEnum;
        if ("TestDate" == key) return this._TestDate;
        return null;
    }
    
    IsInvalid() { return false; }
    
    Set(value) {
        this._TestID = value._TestID;
        this._testEnum = value._testEnum;
        this._TestDate = value._TestDate;
    }
    
    ToString() {
        return `{
            TestID : ${_TestID}, 
            testEnum : ${_testEnum}, 
            TestDate : ${ScorpioUtil.ToString(_TestDate)}
            }`;
    }
    
    static Read(fileName, reader) {
        let ret = new DataTest();
        ret._TestID = reader.ReadInt32();
        ret._testEnum = reader.ReadInt32();
        {
            let list = new Array();
            let number = reader.ReadInt32();
            for (let i = 0; i < number; ++i) { list.push(Int3.Read(fileName, reader)); }
            ret._TestDate = list;
        }
        return ret;
    }
}
exports.DataTest = DataTest;
