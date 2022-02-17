from struct import pack, unpack
import time
class DefaultReader:
    def __init__(self, base_stream):
        self.base_stream = base_stream
        
    def readBytes(self, length):
        return self.base_stream.read(length)

    def unpack(self, fmt, length = 1):
        return unpack(fmt, self.readBytes(length))[0]

    def ReadBool(self):
        return self.ReadInt8() == 1

    def ReadInt8(self):
        return self.unpack('b')

    def ReadUInt8(self):
        return self.unpack('B')

    def ReadInt16(self):
        return self.unpack('h', 2)

    def ReadUInt16(self):
        return self.unpack('H', 2)

    def ReadInt32(self):
        return self.unpack('i', 4)

    def ReadUInt32(self):
        return self.unpack('I', 4)

    def ReadInt64(self):
        return self.unpack('q', 8)

    def ReadUInt64(self):
        return self.unpack('Q', 8)

    def ReadFloat(self):
        return self.unpack('f', 4)

    def ReadDouble(self):
        return self.unpack('d', 8)

    def ReadString(self):
        length = self.ReadUInt16()
        return self.readBytes(length).decode("utf-8")

    def ReadDateTime(self):
        # return datetime.fromtimestamp(int(self.ReadInt64()))
        # return self.ReadInt64()
        return time.localtime(self.ReadInt64() / 1000)
        
    def ReadBytes(self):
        length = self.ReadInt32()
        return self.readBytes(length)

    def ReadHead(this):
        number = this.ReadInt32()           #字段数量
        for i in range(0, number):
            if this.ReadInt8() == 0:        #基础类型
                this.ReadInt8()             #基础类型索引
            else:                           #自定义类
                this.ReadString()           #自定义类名称
            this.ReadBool()                 #是否是数组
            this.ReadString()               #字段名称

        customNumber = this.ReadInt32()             #自定义类数量
        for i in range(0, customNumber):
            this.ReadString()                       #读取自定义类名字
            if this.ReadInt8() == 1:
                number = this.ReadInt32()
                for j in range(0, number):
                    this.ReadString()
                    this.ReadInt32()
            else:
                number = this.ReadInt32()           #字段数量
                for j in range(0, number):
                    if this.ReadInt8() == 0:        #基础类型
                        this.ReadInt8()             #基础类型索引
                    else:                           #自定义类
                        this.ReadString()           #自定义类名称
                    this.ReadBool()                 #是否是数组
                    this.ReadString()

    def Close(self):
        pass