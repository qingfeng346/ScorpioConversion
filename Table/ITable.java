package Scorpio.Table;
public abstract class ITable
{
    public abstract boolean Contains(Integer ID);
    public abstract int Count();
    public abstract IData GetValue(Integer key);
}

