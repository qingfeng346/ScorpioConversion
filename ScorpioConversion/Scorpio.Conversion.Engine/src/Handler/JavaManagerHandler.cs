
using Scorpio.Commons;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Scorpio.Conversion {
    [AutoHandler("JavaManager")]
    public class JavaManagerHandler : IHandler {
        public void Handle(LanguageInfo languageInfo, List<TableBuilder> successTables, SortedDictionary<string, List<TableBuilder>> successSpawns, List<L10NData> l10NDatas, CommandLine command) {
            var builder = new StringBuilder();
            builder.Append($@"
package {languageInfo.package};
import Scorpio.Conversion.IReader;
public abstract class TableManagerBase {{
    public abstract IReader GetReader(String name) throws Exception;");
            successTables.ForEach(table => {
                builder.Append($@"
    private Table{table.Name} _table{table.Name} = null;
    public Table{table.Name} get{table.Name}() throws Exception {{
        if (this._table{table.Name} == null) {{
            IReader reader = GetReader(""{table.Name}"");
            this._table{table.Name} = new Table{table.Name}().Initialize(""{table.Name}"", reader);
            reader.Close();
        }}
        return this._table{table.Name};
    }}");
            });
            foreach (var pair in successSpawns) {
                pair.Value.ForEach((table) => {
                    builder.Append($@"
    private Table{table.Name} _table{table.FileName} = null;
    public Table{table.Name} get{table.Name}{table.FileName}() throws Exception {{
        if (this._table{table.FileName} == null) {{
            IReader reader = GetReader(""{table.FileName}"");
            this._table{table.FileName} = new Table{table.Name}().Initialize(""{table.FileName}"", reader);
            reader.Close();
        }}
        return this._table{table.FileName};
    }}");
                });
                builder.Append($@"
    public Table{pair.Key} get{pair.Key}(String name) throws Exception {{");
                pair.Value.ForEach((table) => {
                    builder.Append($@"
        if (""{table.FileName}"".equals(name)) return get{table.Name}{table.FileName}();");
                });
                builder.Append(@"
        return null;
    }");
            }
            builder.Append(@"
}");
            FileUtil.CreateFile(Path.Combine(ScorpioUtil.CurrentDirectory, languageInfo.codeOutput, languageInfo.package.Replace(".", "/"), $"TableManagerBase.{languageInfo.codeSuffix}"), builder.ToString());
        }
        public void Dispose() { }
    }
}
