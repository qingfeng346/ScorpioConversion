//本文件为自动生成，请不要手动修改

class Int3 {
    
    constructor(fileName, reader) {
        var list = []
        var number = reader.ReadInt32()
        for (var i = 0; i < number; i++) { 
            list.add(new Int2(fileName, reader))
        }
        this.Value1 = list
        this.Value2 = reader.ReadInt32()
    }
    
    GetData(key) {
        if ("Value1" == key) { return this.Value1 }
        if ("Value2" == key) { return this.Value2 }
        return null;
    }
    
    Set(value) {
        this.Value1 = value.Value1
        this.Value2 = value.Value2
    }
    
    toString() {
        return "Value1:" + this.Value1 + "," + "Value2:" + this.Value2
    }
}
