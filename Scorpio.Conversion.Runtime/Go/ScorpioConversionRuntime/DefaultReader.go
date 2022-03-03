package ScorpioConversionRuntime

import (
	"bytes"
	"encoding/binary"
	"io"
	"time"
)

type DefaultReader struct {
	Buffer *bytes.Buffer
}

func (reader *DefaultReader) ReadBool() bool {
	return reader.ReadInt8() == int8(1)
}

// ReadInt8 读取一个 int8
func (reader *DefaultReader) ReadInt8() int8 {
	var ret int8
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadUInt8 读取一个 uint8
func (reader *DefaultReader) ReadUInt8() uint8 {
	var ret uint8
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadInt16 读取一个 int16
func (reader *DefaultReader) ReadInt16() int16 {
	var ret int16
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadUInt16 读取一个 uint16
func (reader *DefaultReader) ReadUInt16() uint16 {
	var ret uint16
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadInt32 读取一个 int32
func (reader *DefaultReader) ReadInt32() int32 {
	var ret int32
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadUInt32 读取一个 uint32
func (reader *DefaultReader) ReadUInt32() uint32 {
	var ret uint32
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadInt64 读取一个 int64
func (reader *DefaultReader) ReadInt64() int64 {
	var ret int64
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadUInt64 读取一个 uint64
func (reader *DefaultReader) ReadUInt64() uint64 {
	var ret uint64
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadFloat 读取一个 float
func (reader *DefaultReader) ReadFloat() float32 {
	var ret float32
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadDouble 读取一个 double
func (reader *DefaultReader) ReadDouble() float64 {
	var ret float64
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadString 读取一个 string
func (reader *DefaultReader) ReadString() string {
	var length = reader.ReadUInt16()
	var bs = make([]byte, length)
	io.ReadFull(reader.Buffer, bs)
	return string(bs)
}

// ReadDateTime 读取一个 time
func (reader *DefaultReader) ReadDateTime() time.Time {
	return time.Unix(reader.ReadInt64()/1000, 0)
}

func (reader *DefaultReader) Close() {

}
