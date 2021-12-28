using System;
using System.Reflection;
using System.Collections.Generic;
using Scorpio.Commons;
public abstract class BaseManager<T> {
    public class BaseType {
        public Type type;       //类型
        public object[] args;   //构造函数默认参数
    }
    protected Dictionary<string, BaseType> values = new Dictionary<string, BaseType>();
    protected abstract string Name { get; }
    public void Add(string name, Type value, params object[] args) {
        Logger.info($"添加[{Name}] {name} - {value}");
        values[name] = new BaseType() { type = value, args = args };
    }
    public T Get(string name, params object[] args) {
        if (values.TryGetValue(name, out var baseType)) {
            var arg = new List<object>(baseType.args);
            arg.AddRange(args);
            return (T)Activator.CreateInstance(baseType.type, arg.ToArray());
        }
        throw new Exception($"找不到[{Name}] : {name}");
    }
}
