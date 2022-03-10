
using Scorpio.Commons;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Scorpio.Conversion.Engine;

namespace Scorpio.Conversion.Engine {
    [AutoHandler("GoManager")]
    public class GoManagerHandler : IHandler {
        public void Handle(LanguageInfo languageInfo, List<TableBuilder> successTables, SortedDictionary<string, List<TableBuilder>> successSpawns, List<L10NData> l10NDatas, CommandLine command) {
            var builder = new StringBuilder();
            builder.Append($@"package {languageInfo.package}
type TableManager struct {{");
            successTables.ForEach(table => {
                builder.Append($@"
    _table{table.Name} *Table{table.Name}");
            });
            foreach (var pair in successSpawns) {
                pair.Value.ForEach((table) => {
                    builder.Append($@"
    _table{table.FileName} *Table{table.Name}");
                });
            }
            builder.Append(@"
}");
            successTables.ForEach(table => {
                builder.Append($@"
func (tableManager *TableManager) Get{table.Name}() *Table{table.Name} {{
    if (tableManager._table{table.Name} == nil) {{
        reader := GetReader(""{table.Name}"")
        tableManager._table{table.Name} = new(Table{table.Name})
        tableManager._table{table.Name}.Initialize(""{table.Name}"", reader)
        reader.Close()
    }}
    return tableManager._table{table.Name}
}}");
            });
            foreach (var pair in successSpawns) {
                pair.Value.ForEach((table) => {
                    builder.Append($@"
func (tableManager *TableManager) Get{table.Name}{table.FileName}() *Table{table.Name} {{
    if (tableManager._table{table.FileName} == nil) {{
        reader := GetReader(""{table.FileName}"")
        tableManager._table{table.FileName} = new(Table{table.Name})
        tableManager._table{table.FileName}.Initialize(""{table.FileName}"", reader)
        reader.Close()
    }}
    return tableManager._table{table.FileName}
}}");
                });
                builder.Append($@"
func (tableManager *TableManager) Get{pair.Key}(name string) *Table{pair.Key} {{");
                pair.Value.ForEach((table) => {
                    builder.Append($@"
    if (name == ""{table.FileName}"") {{
        return tableManager.Get{table.Name}{table.FileName}();
    }}");
                });
                builder.Append(@"
    return nil;
}");
            }
            FileUtil.CreateFile(languageInfo.GetCodePath("TableManager"), builder.ToString());
        }
        public void Dispose() { }
    }
}
