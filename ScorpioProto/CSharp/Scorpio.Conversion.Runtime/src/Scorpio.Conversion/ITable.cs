using System.Collections;
namespace Scorpio.Conversion {
    public interface ITable {
        IData GetValueObject(object key);
        bool ContainsObject(object ID);
        IDictionary GetDatas();
        int Count();
    }
}
