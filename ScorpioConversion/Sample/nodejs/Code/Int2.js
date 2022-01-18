

class Int2 {
    
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
            Value1 : ${_Value1}, 
            Value2 : ${_Value2}
            }`;
    }
    
    static Read(fileName, reader) {
        let ret = new Int2();
        ret._Value1 = reader.ReadInt32();
        ret._Value2 = reader.ReadInt32();
        return ret;
    }
}
exports.Int2 = Int2;
