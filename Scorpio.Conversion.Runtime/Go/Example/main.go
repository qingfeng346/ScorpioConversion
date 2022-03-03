package main

import (
	"fmt"

	"github.com/qingfeng346/ScorpioConversion/Scorpio.Conversion.Runtime/Go/ScorpioConversionRuntime"
)

func main() {
	fmt.Println("hello")
	var reader = ScorpioConversionRuntime.DefaultReader{}
	reader.Close()
}
