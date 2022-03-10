package main

import (
	"fmt"

	"./datas"
)

func main() {
	tableManager := new(datas.TableManager)
	for _, v := range tableManager.GetSpawnTest1().Datas() {
		fmt.Println(v.String())
	}
	// buf, err := ioutil.ReadFile("../../Test.data")
	// if err != nil {
	// 	fmt.Println(err)
	// } else {
	// 	buff := bytes.NewBuffer(buf)
	// 	reader := &ScorpioConversionRuntime.DefaultReader{buff}
	// 	ttt := &datas.TableTest{}
	// 	ttt.Initialize("Test", reader)
	// 	for k, v := range ttt.Datas() {
	// 		fmt.Println(k, v.String())
	// 	}
	// }
}
