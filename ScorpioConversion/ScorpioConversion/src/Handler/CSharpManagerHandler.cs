
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
        public Table{table.Name} {table.Name} {{
            get {{
                if (this._table{table.Name} == null) {{
                    using var reader = GetReader(""{table.Name}"");
                    this._table{table.Name} = new Table{table.Name}().Initialize(""{table.Name}"", reader);
                }}
                return this._table{table.Name};
            }}
        }}");
            });
            foreach (var pair in successSpawns) {
                pair.Value.ForEach((table) => {
                    builder.Append($@"
        private Table{table.Name} _table{table.FileName} = null;
        public Table{table.Name} {table.Name}{table.FileName} {{
            get {{
                if (this._table{table.FileName} == null) {{
                    using var reader = GetReader(""{table.FileName}"");
                    this._table{table.FileName} = new Table{table.Name}().Initialize(""{table.FileName}"", reader);
                }}
                return this._table{table.FileName};
            }}
        }}");
                });
                builder.Append($@"
        public Table{pair.Key} Get{pair.Key}(string name) {{");
                pair.Value.ForEach((table) => {
                    builder.Append($@"
            if (name == ""{table.FileName}"") return {table.Name}{table.FileName};");
                });
                builder.Append(@"
            return null;
        }");
            }
            builder.Append(@"
    }
}");
            FileUtil.CreateFile(languageInfo.GetCodePath("TableManager"), builder.ToString());
        }
        public void Dispose() { }
    }
}
