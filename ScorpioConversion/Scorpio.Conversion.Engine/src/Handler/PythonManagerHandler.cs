
using Scorpio.Commons;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Conversion.Engine {
    [AutoHandler("PythonManager")]
    public class PythonManagerHandler : IHandler {
        public void Handle(LanguageInfo languageInfo, List<TableBuilder> successTables, SortedDictionary<string, List<TableBuilder>> successSpawns, List<L10NData> l10NDatas, CommandLine command) {
            var builder = new StringBuilder();
            foreach (var pair in Config.Parser.Classes) {
                builder.Append($@"
from {pair.Value.Name} import *");
            }
            successTables.ForEach(table => {
builder.Append($@"
from Table{table.Name} import *
from Data{table.Name} import *");
            });
            foreach (var pair in successSpawns) {
                builder.Append($@"
from Table{pair.Key} import *
from Data{pair.Key} import *");
            }
            builder.Append($@"
class TableManager:");
            successTables.ForEach(table => {
                builder.Append($@"
    _table{table.Name} = None
    def get{table.Name}(this):
        if this._table{table.Name} == None:
            reader = this.GetReader(""{table.Name}"")
            this._table{table.Name} = Table{table.Name}().Initialize(""{table.Name}"", reader)
            reader.Close()
        return this._table{table.Name}
");
            });
            foreach (var pair in successSpawns) {
                pair.Value.ForEach((table) => {
                    builder.Append($@"
    _table{table.FileName} = None
    def get{table.Name}{table.FileName}(this):
        if this._table{table.FileName} == None:
            reader = this.GetReader(""{table.FileName}"")
            this._table{table.FileName} = Table{table.Name}().Initialize(""{table.FileName}"", reader)
            reader.Close()
        return this._table{table.FileName}
");
                });
                builder.Append($@"
    def get{pair.Key}(this, name):");
                pair.Value.ForEach((table) => {
                    builder.Append($@"
        if name == ""{table.FileName}"":
            return this.get{table.Name}{table.FileName}()");
                });
                builder.Append(@"
        return None
");
            }
            builder.Append(@"
");
            FileUtil.CreateFile(languageInfo.GetCodePath("TableManager"), builder.ToString());
        }
        public void Dispose() { }
    }
}
