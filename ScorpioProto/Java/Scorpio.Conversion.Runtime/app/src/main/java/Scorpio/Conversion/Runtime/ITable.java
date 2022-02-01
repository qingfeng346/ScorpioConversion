package Scorpio.Conversion.Runtime;

public interface ITable {
    IData GetValueObject(Object ID) throws Exception;
    boolean ContainsObject(Object ID);
    int Count();
}