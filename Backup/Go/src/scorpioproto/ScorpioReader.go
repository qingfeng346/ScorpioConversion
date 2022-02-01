package scorpioproto

import (
	"bytes"
	"encoding/binary"
	"io"
	"time"
)

// ScorpioReader 二进制读取类
type ScorpioReader struct {
	Buffer *bytes.Buffer
}

// ReadBool 读取一个 bool
func (reader *ScorpioReader) ReadBool() bool {
	return reader.ReadInt8() == int8(1)
}

// ReadInt8 读取一个 int8
func (reader *ScorpioReader) ReadInt8() int8 {
	var ret int8
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadUInt8 读取一个 uint8
func (reader *ScorpioReader) ReadUInt8() uint8 {
	var ret uint8
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadInt16 读取一个 int16
func (reader *ScorpioReader) ReadInt16() int16 {
	var ret int16
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadUInt16 读取一个 uint16
func (reader *ScorpioReader) ReadUInt16() uint16 {
	var ret uint16
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadInt32 读取一个 int32
func (reader *ScorpioReader) ReadInt32() int32 {
	var ret int32
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadUInt32 读取一个 uint32
func (reader *ScorpioReader) ReadUInt32() uint32 {
	var ret uint32
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadInt64 读取一个 int64
func (reader *ScorpioReader) ReadInt64() int64 {
	var ret int64
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadUInt64 读取一个 uint64
func (reader *ScorpioReader) ReadUInt64() uint64 {
	var ret uint64
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadFloat 读取一个 float
func (reader *ScorpioReader) ReadFloat() float32 {
	var ret float32
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadDouble 读取一个 double
func (reader *ScorpioReader) ReadDouble() float64 {
	var ret float64
	binary.Read(reader.Buffer, binary.LittleEndian, &ret)
	return ret
}

// ReadString 读取一个 string
func (reader *ScorpioReader) ReadString() string {
	length := reader.ReadUInt16()
	bs := make([]byte, length)
	io.ReadFull(reader.Buffer, bs)
	return string(bs)
}

// ReadDateTime 读取一个 time
func (reader *ScorpioReader) ReadDateTime() time.Time {
	return time.Unix(reader.ReadInt64()/1000, 0)
}
