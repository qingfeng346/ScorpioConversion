using Scorpio.Commons;
using System.Collections.Generic;

namespace Scorpio.Conversion {
    public class ScriptHandler : IHandler {
        public ScriptValue Value { get; private set; }
        public ScriptHandler(ScriptValue value) {
            Value = value;
        }
        public bool Call(string functionName, out ScriptValue ret, params object[] args) {
            var func = Value.GetValue(functionName);
            if (func.valueType == ScriptValue.scriptValueType) {
                try {
                    ret = func.call(Value, args);
                    return true;
                } catch (System.Exception e) {
                    throw new System.Exception($"ScriptInstance Call is error Type:ScriptHandler  Function:{functionName} error:{e}");
                }
            }
            ret = ScriptValue.Null;
            return false;
        }
        public void Handle(List<TableBuilder> successTables, SortedDictionary<string, List<TableBuilder>> successSpawns, List<L10NData> l10NDatas, CommandLine command) {
            Call("Handle", out _, successTables, successSpawns, l10NDatas, command);
        }
        public void Dispose() { }
    }
}
