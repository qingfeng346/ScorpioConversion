
using Scorpio.Commons;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Scorpio.Conversion {
    [AutoHandler("ScorpioManager")]
    public class ScorpioManagerHandler : IHandler {
        public void Handle(LanguageInfo languageInfo, List<TableBuilder> successTables, SortedDictionary<string, List<TableBuilder>> successSpawns, List<L10NData> l10NDatas, CommandLine command) {
            var builder = new StringBuilder();
            foreach (var pair in Config.Parser.Tables) {
                builder.Append($@"
#import ""{pair.Value.Name}.{languageInfo.codeSuffix}""");
            }
            foreach (var pair in Config.Parser.Enums) {
                builder.Append($@"
#import ""{pair.Value.Name}.{languageInfo.codeSuffix}""");
            }
            successTables.ForEach(table => {
builder.Append($@"
#import ""Table{table.Name}.{languageInfo.codeSuffix}""
#import ""Data{table.Name}.{languageInfo.codeSuffix}""");
            });
            foreach (var pair in successSpawns) {
                builder.Append($@"
#import ""Table{pair.Key}.{languageInfo.codeSuffix}""
#import ""Data{pair.Key}.{languageInfo.codeSuffix}""");
            }
            builder.Append($@"
TableManager = {{");
            successTables.ForEach(table => {
                builder.Append($@"
    get{table.Name}(ID) {{
        if (this._table{table.Name} == null) {{
            var reader = GetReader(""{table.Name}"");
            this._table{table.Name} = new Table{table.Name}().Initialize(""{table.Name}"", reader);
            reader.Close()
        }}
        return ID == null ? this._table{table.Name} : this._table{table.Name}(ID);
    }}");
            });
            foreach (var pair in successSpawns) {
                pair.Value.ForEach((table) => {
                    builder.Append($@"
    get{table.Name}{table.FileName}(ID) {{
        if (this._table{table.FileName} == null) {{
            var reader = GetReader(""{table.FileName}"");
            this._table{table.FileName} = new Table{table.Name}().Initialize(""{table.FileName}"", reader);
            reader.Close()
        }}
        return ID == null ? this._table{table.Name} : this._table{table.Name}(ID);
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
}");
            FileUtil.CreateFile(languageInfo.GetCodePath("TableManager"), builder.ToString());
        }
        public void Dispose() { }
    }
}
