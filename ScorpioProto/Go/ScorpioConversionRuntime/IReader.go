package ScorpioConversionRuntime

import "time"

type IReader interface {
	ReadBool() bool
	ReadInt8() int8
	ReadUInt8() uint8
	ReadInt16() int16
	ReadUInt16() uint16
	ReadInt32() int32
	ReadUInt32() uint32
	ReadInt64() int64
	ReadUInt64() uint64
	ReadFloat() float32
	ReadDouble() float64
	ReadString() string
	ReadDateTime() time.Time
	Close()
}
