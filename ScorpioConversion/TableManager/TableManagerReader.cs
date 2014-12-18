using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public partial class TableManager
{
    public void CreateReaderCS()
    {
        PROGRAM program = PROGRAM.CS;
        string strDataBase = @"using System;
using System.IO;
public class TableReader
{
    MemoryStream stream;
    BinaryReader reader;
    public TableReader(byte[] buffer) {
        stream = new MemoryStream(buffer);
        reader = new BinaryReader(stream);
    }
    public bool ReadBool() {
        return ReadInt8() == 1;
    }
    public sbyte ReadInt8() {
        return reader.ReadSByte();
    }
    public short ReadInt16() {
        return reader.ReadInt16();
    }
    public int ReadInt32() {
        return reader.ReadInt32();
    }
    public long ReadInt64() {
        return reader.ReadInt64();
    }
    public float ReadFloat() {
        return reader.ReadSingle();
    }
    public double ReadDouble() {
        return reader.ReadDouble();
    }
    public String ReadString() {
        return TableUtil.ReadString(reader);
    }
    public void Close() {
        stream.Close();
        reader.Close();
    }
}";
        FileUtil.CreateFile("TableReader.cs", strDataBase, true, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
    }
    public void CreateReaderJAVA()
    {
        PROGRAM program = PROGRAM.JAVA;
        string strDataBase = @"package table;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
public class TableReader
{
    ByteBuffer reader;
    public TableReader(byte[] buffer) {
        reader = ByteBuffer.wrap(buffer).order(ByteOrder.LITTLE_ENDIAN);
    }
    public boolean ReadBool() {
        return ReadInt8() == 1;
    }
    public byte ReadInt8() {
        return reader.get();
    }
    public short ReadInt16() {
        return reader.getShort();
    }
    public int ReadInt32() {
        return reader.getInt();
    }
    public long ReadInt64() {
        return reader.getLong();
    }
    public float ReadFloat() {
        return reader.getFloat();
    }
    public double ReadDouble() {
        return reader.getDouble();
    }
    public String ReadString() {
        return TableUtil.ReadString(reader);
    }
    public void Close() {
    }
}";
        FileUtil.CreateFile("TableReader.java", strDataBase, true, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
    }
    public void CreateReaderCPP()
    {
        PROGRAM program = PROGRAM.CPP;
        string strDataBase = @"
public class TableReader
{
private:
    char * reader;
    char * data;
public:
    TableReader(char * buffer) {
        this->reader = buffer;
        this->data = buffer;
    }
    bool ReadBool() {
        return ReadInt8() == 1;
    }
    public byte ReadInt8() {
        char ret = *((char*)reader);
        reader += sizeof(char);
        return ret;
    }
    short ReadInt16() {
        short ret = *((short*)reader);
        reader += sizeof(short);
        return ret;
    }
    int ReadInt32() {
        int ret = *((int*)reader);
        reader += sizeof(int);
        return ret;
    }
    long ReadInt64() {
        long ret = *((long*)reader);
        reader += sizeof(long);
        return ret;
    }
    float ReadFloat() {
        float ret = *((float*)reader);
        reader += sizeof(float);
        return ret;
    }
    double ReadDouble() {
        double ret = *((double*)reader);
        reader += sizeof(double);
        return ret;
    }
    string ReadString() {
        return TableUtil.ReadString(reader);
    }
    void Close() {
        delete data;
    }
}";
        FileUtil.CreateFile("TableReader.h", strDataBase, true, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
    }
    public void CreateReaderPHP()
    {
        PROGRAM program = PROGRAM.PHP;
        string strDataBase = @"
class TableReader
{
    ByteBuffer $reader;
    public TableReader($buffer) {
        $reader = new ByteBuffer($buffer);
        $reader->order(ByteBuffer::LITTLE_ENDIAN);
    }
    public boolean ReadBool() {
        return ReadInt8() == 1;
    }
    public byte ReadInt8() {
        return reader.get();
    }
    public short ReadInt16() {
        return reader.getShort();
    }
    public int ReadInt32() {
        return reader.getInt();
    }
    public long ReadInt64() {
        return reader.getLong();
    }
    public float ReadFloat() {
        return reader.getFloat();
    }
    public double ReadDouble() {
        return reader.getDouble();
    }
    public String ReadString() {
        return TableUtil.ReadString(reader);
    }
    public void Close() {
    }
}";
        FileUtil.CreateFile("TableReader.java", strDataBase, true, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
    }
}

