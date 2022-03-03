#本文件为自动生成，请不要手动修改


from Int2 import *;

class DataSpawn:
    
    def __init__(this, fileName, reader):
        this.ID = reader.ReadInt32()
        this.TestInt = reader.ReadInt32()
        this.TestString = reader.ReadString()
        this.TestLanguage = reader.ReadString()
        this.TestBool = reader.ReadBool()
        this.TestInt2 = Int2(fileName, reader)
        this.TestEnumName = reader.ReadInt32()
    
    def GetData(this, key):
        if "ID" == key:
            return this.ID
        if "TestInt" == key:
            return this.TestInt
        if "TestString" == key:
            return this.TestString
        if "TestLanguage" == key:
            return this.TestLanguage
        if "TestBool" == key:
            return this.TestBool
        if "TestInt2" == key:
            return this.TestInt2
        if "TestEnumName" == key:
            return this.TestEnumName
        return None

    
    def Set(this, value):
        this.ID = value.ID
        this.TestInt = value.TestInt
        this.TestString = value.TestString
        this.TestLanguage = value.TestLanguage
        this.TestBool = value.TestBool
        this.TestInt2 = value.TestInt2
        this.TestEnumName = value.TestEnumName
    
    def __str__(this):
        return "ID:" + str(this.ID) + "," + "TestInt:" + str(this.TestInt) + "," + "TestString:" + str(this.TestString) + "," + "TestLanguage:" + str(this.TestLanguage) + "," + "TestBool:" + str(this.TestBool) + "," + "TestInt2:" + str(this.TestInt2) + "," + "TestEnumName:" + str(this.TestEnumName)

