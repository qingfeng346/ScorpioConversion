
const Int2 = require('./Int2').Int2
const TestEnum = require('./TestEnum').TestEnum

class DataSpawn {
    
    /* 测试ID 此值必须唯一 而且必须为int型  默认值() */
    getID() { return this._ID; }
    ID() { return this._ID; }
    /* int类型  默认值() */
    getTestInt() { return this._TestInt; }
    /* string类型  默认值() */
    getTestString() { return this._TestString; }
    /* 测试多国语言  默认值() */
    getTestLanguage() { return this._TestLanguage; }
    /* bool类型  默认值() */
    getTestBool() { return this._TestBool; }
    /* 自定义类型 根据ExcelConfig下 table.sco文件定义的Int2解析 类型为table_后面的名字 格式为 , 隔开  默认值() */
    getTestInt2() { return this._TestInt2; }
    /* 自定义枚举  默认值() */
    getTestEnumName() { return this._TestEnumName; }
    
    GetData(key) {
        if ("ID" == key) return this._ID;
        if ("TestInt" == key) return this._TestInt;
        if ("TestString" == key) return this._TestString;
        if ("TestLanguage" == key) return this._TestLanguage;
        if ("TestBool" == key) return this._TestBool;
        if ("TestInt2" == key) return this._TestInt2;
        if ("TestEnumName" == key) return this._TestEnumName;
        return null;
    }
    
    IsInvalid() { return false; }
    
    Set(value) {
        this._ID = value._ID;
        this._TestInt = value._TestInt;
        this._TestString = value._TestString;
        this._TestLanguage = value._TestLanguage;
        this._TestBool = value._TestBool;
        this._TestInt2 = value._TestInt2;
        this._TestEnumName = value._TestEnumName;
    }
    
    ToString() {
        return `{
            ID : ${_ID}, 
            TestInt : ${_TestInt}, 
            TestString : ${_TestString}, 
            TestLanguage : ${_TestLanguage}, 
            TestBool : ${_TestBool}, 
            TestInt2 : ${_TestInt2}, 
            TestEnumName : ${_TestEnumName}
            }`;
    }
    
    static Read(fileName, reader) {
        let ret = new DataSpawn();
        ret._ID = reader.ReadInt32();
        ret._TestInt = reader.ReadInt32();
        ret._TestString = reader.ReadString();
        ret._TestLanguage = reader.ReadString();
        ret._TestBool = reader.ReadBool();
        ret._TestInt2 = Int2.Read(fileName, reader);
        ret._TestEnumName = reader.ReadInt32();
        return ret;
    }
}
exports.DataSpawn = DataSpawn;
