using Scorpio.Conversion.Runtime;
using System.IO;
namespace Datas {
    public partial class TableManager {
        public static TableManager Instance { get; } = new TableManager();
        IReader GetReader(string name) {
            return new DefaultReader(File.OpenRead($"../../{name}.data"), true);
        }
    }
}