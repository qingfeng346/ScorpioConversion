using System;
using System.Reflection;

namespace Scorpio.Conversion {
    public class WriterManager : BaseManager<IWriter> {
        public static WriterManager Instance { get; } = new WriterManager();
        private static readonly Type TypeBase = typeof(IWriter);
        protected override string Name => "Writer";
        public void Add(Assembly assembly) {
            foreach (var type in assembly.GetTypes()) {
                if (type.IsInterface || type.IsAbstract || !TypeBase.IsAssignableFrom(type)) { continue; }
                var auto = type.GetCustomAttribute<AutoWriter>();
                if (auto == null) { continue; }
                Add(auto.Name, type, auto.Args);
            }
        }
    }
}