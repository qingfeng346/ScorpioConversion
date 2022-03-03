#本文件为自动生成，请不要手动修改


from Int3 import *;

class DataTestCsv:
    
    def __init__(this, fileName, reader):
        this.TestID = reader.ReadInt32()
        this.ID = this.TestID
        this.testEnum = reader.ReadInt32()
        list = []
        number = reader.ReadInt32()
        for i in range(0, number):
            list.append(Int3(fileName, reader))

        this.TestDate = list
        this.TestDateTime = reader.ReadDateTime()
        this.TestInt = reader.ReadInt32()
    
    def GetData(this, key):
        if "TestID" == key:
            return this.TestID
        if "testEnum" == key:
            return this.testEnum
        if "TestDate" == key:
            return this.TestDate
        if "TestDateTime" == key:
            return this.TestDateTime
        if "TestInt" == key:
            return this.TestInt
        return None

    
    def Set(this, value):
        this.TestID = value.TestID
        this.testEnum = value.testEnum
        this.TestDate = value.TestDate
        this.TestDateTime = value.TestDateTime
        this.TestInt = value.TestInt
    
    def __str__(this):
        return "TestID:" + str(this.TestID) + "," + "testEnum:" + str(this.testEnum) + "," + "TestDate:" + str(this.TestDate) + "," + "TestDateTime:" + str(this.TestDateTime) + "," + "TestInt:" + str(this.TestInt)

