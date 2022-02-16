#本文件为自动生成，请不要手动修改


from Int2 import *;

class Int3:
    
    def __init__(this, fileName, reader):
        list = []
        number = reader.ReadInt32()
        for i in range(0, number):
            list.append(Int2(fileName, reader))

        this.Value1 = list
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

