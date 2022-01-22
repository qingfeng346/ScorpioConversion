using System;
using System.Reflection;

namespace Scorpio.Conversion {
    public class GeneratorManager : BaseManager<IGenerator> {
        public static GeneratorManager Instance { get; } = new GeneratorManager();
        private static readonly Type TypeBase = typeof(IGenerator);
        protected override string Name { get; } = "Generator";
        public void Add(Assembly assembly) {
            foreach (var type in assembly.GetTypes()) {
                if (type.IsInterface || type.IsAbstract || !TypeBase.IsAssignableFrom(type)) { continue; }
                var auto = type.GetCustomAttribute<AutoGenerator>();
                if (auto == null) { continue; }
                Add(auto.Name, type, auto.Args);
            }
        }
        public void Add(string name, ScriptValue scriptValue) {
            Add(name, typeof(ScriptGenerator), scriptValue);
        }
    }
}