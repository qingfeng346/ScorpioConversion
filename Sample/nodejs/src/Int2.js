

class Int2 {
    
    /*   默认值() */
    getvalue1() { return this._value1; }
    ID() { return this._value1; }
    /*   默认值() */
    getvalue2() { return this._value2; }
    
    GetData(key) {
        if ("value1" == key) return this._value1;
        if ("value2" == key) return this._value2;
        return null;
    }
    
    IsInvalid() { return false; }
    
    Set(value) {
        this._value1 = value._value1;
        this._value2 = value._value2;
    }
    
    ToString() {
        return `{
            value1 : ${_value1}, 
            value2 : ${_value2}
            }`;
    }
    
    static Read(fileName, reader) {
        let ret = new Int2();
        ret._value1 = reader.ReadInt32();
        ret._value2 = reader.ReadInt32();
        return ret;
    }
}
exports.Int2 = Int2;
