namespace Scorpio.Conversion.Engine {
    public abstract class ScriptBase {
        public ScriptValue Value { get; private set; }
        public ScriptBase(ScriptValue value, object[] args) {
            Value = value.call(ScriptValue.Null, args);
        }
        public bool Call(string functionName, out ScriptValue ret, params object[] args) {
            var func = Value.GetValue(functionName);
            if (func.valueType == ScriptValue.scriptValueType) {
                try {
                    ret = func.call(Value, args);
                    return true;
                } catch (System.Exception e) {
                    throw new System.Exception($"Call is error Type:{GetType()}  Function:{functionName} error:{e}");
                }
            }
            ret = ScriptValue.Null;
            return false;
        }
    }
}
