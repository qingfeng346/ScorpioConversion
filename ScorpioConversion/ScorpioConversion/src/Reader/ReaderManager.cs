using System;
using System.Reflection;

namespace Scorpio.Conversion {
    public class ReaderManager : BaseManager<IReader> {
        public static ReaderManager Instance { get; } = new ReaderManager();
        private static readonly Type TypeBase = typeof(IReader);
        protected override string Name => "Reader";
        public void Add(Assembly assembly) {
            foreach (var type in assembly.GetTypes()) {
                if (type.IsInterface || type.IsAbstract || !TypeBase.IsAssignableFrom(type)) { continue; }
                var auto = type.GetCustomAttribute<AutoReader>();
                if (auto == null) { continue; }
                Add(auto.Name, type, auto.Args);
            }
        }
    }
}