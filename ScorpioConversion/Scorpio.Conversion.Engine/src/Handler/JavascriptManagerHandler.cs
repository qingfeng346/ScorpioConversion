
using Scorpio.Commons;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Scorpio.Conversion.Engine;

namespace Scorpio.Conversion.Engine {
    [AutoHandler("JavascriptManager")]
    public class JavascriptManagerHandler : IHandler {
        public void Handle(LanguageInfo languageInfo, List<TableBuilder> successTables, SortedDictionary<string, List<TableBuilder>> successSpawns, List<L10NData> l10NDatas, CommandLine command) {
            var builder = new StringBuilder();
            successTables.ForEach(table => {
builder.Append($@"
const Table{table.Name} = require(""./Table{table.Name}"")");
            });
            foreach (var pair in successSpawns) {
                builder.Append($@"
const Table{pair.Key} = require(""./Table{pair.Key}"")");
            }
            builder.Append($@"
class TableManager {{");
            successTables.ForEach(table => {
                builder.Append($@"
    get{table.Name}() {{
        if (this._table{table.Name} == null) {{
            var reader = this.GetReader(""{table.Name}"");
            this._table{table.Name} = new Table{table.Name}().Initialize(""{table.Name}"", reader);
            reader.Close()
        }}
        return this._table{table.Name};
    }}");
            });
            foreach (var pair in successSpawns) {
                pair.Value.ForEach((table) => {
                    builder.Append($@"
    get{table.Name}{table.FileName}() {{
        if (this._table{table.FileName} == null) {{
            var reader = this.GetReader(""{table.FileName}"");
            this._table{table.FileName} = new Table{table.Name}().Initialize(""{table.FileName}"", reader);
            reader.Close()
        }}
        return this._table{table.FileName};
    }}");
                });
                builder.Append($@"
    get{pair.Key}(name) {{");
                pair.Value.ForEach((table) => {
                    builder.Append($@"
        if (name == ""{table.FileName}"") {{ return this.get{table.Name}{table.FileName}(); }}");
                });
                builder.Append(@"
        return null;
    }");
            }
            builder.Append(@"
}
module.exports = new TableManager();");
            FileUtil.CreateFile(languageInfo.GetCodePath("TableManager"), builder.ToString());
        }
        public void Dispose() { }
    }
}
