package ScorpioConversionRuntime

type ITable interface {
	GetValueObject(ID interface{}) IData
	ContainsObject(ID interface{}) bool
	Count() int
}
