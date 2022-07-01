using System;
using System.Collections.Generic;
using Scorpio.Commons;
namespace Scorpio.Conversion.Engine {
    public abstract class BaseManager<T, ST> 
        where T : IDisposable 
        where ST : T {
        public class BaseType {
            public Type type;                   //类型
            public ScriptValue scriptValue;     //脚本类
        }
        protected Dictionary<string, BaseType> values = new Dictionary<string, BaseType>();
        protected abstract string Name { get; }
        public void Add(string name, Type value) {
            name = name.ToLowerInvariant();
            Logger.info($"添加[{Name}] {name} - {value}");
            values[name] = new BaseType() { type = value, scriptValue = ScriptValue.Null };
        }
        public void Add(string name, ScriptValue scriptValue) {
            name = name.ToLowerInvariant();
            Logger.info($"添加[{Name}] {name} - {scriptValue}");
            values[name] = new BaseType() { type = typeof(ST), scriptValue = scriptValue };
        }
        public T Get(string name, params object[] args) {
            name = name.ToLowerInvariant();
            if (values.TryGetValue(name, out var baseType)) {
                if (baseType.scriptValue.IsNull) {
                    return (T)Activator.CreateInstance(baseType.type, args);
                } else {
                    return (T)Activator.CreateInstance(baseType.type, baseType.scriptValue, args);
                }
            }
            throw new System.Exception($"找不到[{Name}] : {name}");
        }
    }
}

