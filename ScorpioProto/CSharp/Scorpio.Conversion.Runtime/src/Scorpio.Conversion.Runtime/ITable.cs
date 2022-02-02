using System.Collections;
namespace Scorpio.Conversion.Runtime {
    public interface ITable {
        IData GetValueObject(object key);
        bool ContainsObject(object ID);
        IDictionary GetDatas();
        int Count();
    }
}
