using Scorpio.Commons;
using System.Collections.Generic;

namespace Scorpio.Conversion.Engine {
    public class ScriptHandler : IHandler, ScriptBase {
        public ScriptValue Value { get; set; }
        public ScriptHandler(ScriptValue value, object[] args) {
            Value = value.call(ScriptValue.Null, args);
        }
        public void Handle(LanguageInfo languageInfo, List<TableBuilder> successTables, SortedDictionary<string, List<TableBuilder>> successSpawns, List<L10NData> l10NDatas, CommandLine command) {
            this.Call("Handle", languageInfo, successTables, successSpawns, l10NDatas, command);
        }
        public void Dispose() { }
    }
}
