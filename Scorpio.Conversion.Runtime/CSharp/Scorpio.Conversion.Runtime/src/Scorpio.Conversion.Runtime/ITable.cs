using System.Collections;
namespace Scorpio.Conversion.Runtime {
    public interface ITable {
        IData GetValueObject(object ID);
        bool ContainsObject(object ID);
        IDictionary GetDatas();
        int Count();
    }
}
