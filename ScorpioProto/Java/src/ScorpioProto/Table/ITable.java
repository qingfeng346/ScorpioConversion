package ScorpioProto.Table;
public interface ITable <Key, Value> {
    boolean ContainsObject(Object ID);
    IData GetValueObject(Object ID) throws Exception;
    int Count();
}