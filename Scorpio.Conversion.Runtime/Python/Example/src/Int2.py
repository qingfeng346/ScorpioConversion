#本文件为自动生成，请不要手动修改


class Int2:
    
    def __init__(this, fileName, reader):
        this.Value1 = reader.ReadInt32()
        this.Value2 = reader.ReadInt32()
    
    def GetData(this, key):
        if "Value1" == key:
            return this.Value1
        if "Value2" == key:
            return this.Value2
        return None

    
    def Set(this, value):
        this.Value1 = value.Value1
        this.Value2 = value.Value2
    
    def __str__(this):
        return "Value1:" + str(this.Value1) + "," + "Value2:" + str(this.Value2)

