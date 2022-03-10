package datas

import (
	"bytes"
	"fmt"
	"io/ioutil"

	"github.com/qingfeng346/ScorpioConversion/Scorpio.Conversion.Runtime/Go/ScorpioConversionRuntime"
)

func GetReader(name string) ScorpioConversionRuntime.IReader {
	buf, err := ioutil.ReadFile("../../" + name + ".data")
	if err != nil {
		fmt.Println(err)
		return nil
	}
	buff := bytes.NewBuffer(buf)
	return &ScorpioConversionRuntime.DefaultReader{buff}
}
