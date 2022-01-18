
const Int2 = require('./Int2').Int2

class Int3 {
    
    /*   默认值() */
    getValue1() { return this._Value1; }
    ID() { return this._Value1; }
    /*   默认值() */
    getValue2() { return this._Value2; }
    
    GetData(key) {
        if ("Value1" == key) return this._Value1;
        if ("Value2" == key) return this._Value2;
        return null;
    }
    
    IsInvalid() { return false; }
    
    Set(value) {
        this._Value1 = value._Value1;
        this._Value2 = value._Value2;
    }
    
    ToString() {
        return `{
            Value1 : ${ScorpioUtil.ToString(_Value1)}, 
            Value2 : ${_Value2}
            }`;
    }
    
    static Read(fileName, reader) {
        let ret = new Int3();
        {
            let list = new Array();
            let number = reader.ReadInt32();
            for (let i = 0; i < number; ++i) { list.push(Int2.Read(fileName, reader)); }
            ret._Value1 = list;
        }
        ret._Value2 = reader.ReadInt32();
        return ret;
    }
}
exports.Int3 = Int3;
