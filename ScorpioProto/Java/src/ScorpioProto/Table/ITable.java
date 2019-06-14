package ScorpioProto.Table;

import java.util.HashMap;

public interface ITable <Key, Value> {
    boolean ContainsObject(Object ID);
    IData GetValueObject(Object ID);
    int Count();
    
    boolean Contains(Key ID);
    Value GetValue(Key ID);
    HashMap<Key, Value> Datas();
}