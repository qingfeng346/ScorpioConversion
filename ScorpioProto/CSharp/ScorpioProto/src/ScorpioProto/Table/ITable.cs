using System.Collections;
namespace ScorpioProto.Table {
    public interface ITable {
        IData GetValueObject(object key);
        bool ContainsObject(object ID);
        IDictionary GetDatas();
        int Count();
    }
}
