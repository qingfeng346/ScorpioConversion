package main

import (
	"bytes"
	"fmt"
	"io/ioutil"
	"scorpioproto"
	"tabletest"
)

func main() {
	buf, err := ioutil.ReadFile("../../Test.data")
	if err != nil {
		fmt.Println(err)
	} else {
		buff := bytes.NewBuffer(buf)
		reader := &scorpioproto.ScorpioReader{buff}
		// reader := &ScorpioProto.ScorpioReader{bytes.NewBuffer(buf)}
		ttt := new(tabletest.TableTest)
		ttt.Initialize("Test", reader)
		v := ttt.GetValue(1000000)
		list := v.GetTestDate()
		for it := list.Front(); it != nil; it = it.Next() {
			vvvvvvvv := it.Value
			fmt.Println(vvvvvvvv)
		} 
		// fmt.Println(v.GetTestDate())
		// a := 0;
		// list := v.GetTestArrayString()
		// for it := list.Front(); it != nil; it = it.Next() {
		// 	fmt.Println(it.Value)
		// }
		// fmt.Println(v.GetTestDateTime().String())
		// fmt.Println(v.GetTestArrayString())

	}
	// fmt.Println("hello world")
	// scorpio.TestFunc()
	// fmt.Println("1111")
	// fmt.Println("2222" + strconv.FormatInt(200, 10))
	// a := map[string]string{}
	// a["100"] = "100"
	// a["200"] = "200"
	// fmt.Println(len(a))
	// if _, ok := a["300"]; ok {
	// 	fmt.Println("true")
	// } else {
	// 	fmt.Println("false")
	// }
	// a := list.New()
	// a.PushBack("eee" + 100)
	// fmt.Println(a.Len())
	// c := TestData{"ttttttt"}
	// fmt.Println(c.Aaa)
	// Test1(&c)
	// fmt.Println(c.Aaa)
	// for index := 0; index < count; index++ {
	// }
}
