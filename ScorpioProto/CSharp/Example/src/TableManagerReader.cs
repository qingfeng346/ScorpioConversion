using Scorpio.Conversion;
using System.IO;
namespace Datas {
    public partial class TableManager {
        public static TableManager Instance { get; } = new TableManager();
        IReader GetReader(string name) {
            return new DefaultReader(File.OpenRead("../../Test.data"), true);
        }
    }
}