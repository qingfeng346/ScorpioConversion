
using Scorpio.Commons;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Scorpio.Conversion {
    [AutoHandler("C#Manager")]
    public class CSharpManagerHandler : IHandler {
        public void Handle(LanguageInfo languageInfo, List<TableBuilder> successTables, SortedDictionary<string, List<TableBuilder>> successSpawns, List<L10NData> l10NDatas, CommandLine command) {
            var builder = new StringBuilder();
            builder.Append($@"
namespace {languageInfo.package} {{
    public partial class TableManager {{");
            successTables.ForEach(table => {
                builder.Append($@"
        private Table{table.Name} _table{table.Name} = null;
        public Table{table.Name} get{table.Name}() {{
            if (this._table{table.Name} == null)
                this._table{table.Name} = new Table{table.Name}().Initialize(""{table.Name}"", GetReader(""{table.Name}""));
            return this._table{table.Name};
        }}");
            });
            builder.Append(@"
    }
}");
            FileUtil.CreateFile(Path.Combine(ScorpioUtil.CurrentDirectory, languageInfo.codeOutput, $"TableManager.{languageInfo.codeSuffix}"), builder.ToString());
        }
        public void Dispose() { }
    }
}
