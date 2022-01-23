﻿using System;
using System.Collections.Generic;
using Scorpio.Commons;
namespace Scorpio.Conversion {
    public abstract class BaseManager<T> where T : IDisposable {
        public class BaseType {
            public Type type;       //类型
            public object[] args;   //构造函数默认参数
        }
        protected Dictionary<string, BaseType> values = new();
        protected abstract string Name { get; }
        public void Add(string name, Type value, params object[] args) {
            name = name.ToLowerInvariant();
            Logger.info($"添加[{Name}] {name} - {value}");
            values[name] = new BaseType() { type = value, args = args };
        }
        public T Get(string name, params object[] args) {
            name = name.ToLowerInvariant();
            if (values.TryGetValue(name, out var baseType)) {
                var arg = new List<object>(baseType.args);
                arg.AddRange(args);
                return (T)Activator.CreateInstance(baseType.type, arg.ToArray());
            }
            throw new System.Exception($"找不到[{Name}] : {name}");
        }
    }
}

