using System;
using System.Reflection;

namespace Scorpio.Conversion.Engine {
    public class HandlerManager : BaseManager<IHandler, ScriptHandler> {
        public static HandlerManager Instance { get; } = new HandlerManager();
        private static readonly Type TypeBase = typeof(IHandler);
        protected override string Name => "Handler";
        public void Add(Assembly assembly) {
            foreach (var type in assembly.GetTypes()) {
                if (type.IsInterface || type.IsAbstract || !TypeBase.IsAssignableFrom(type)) { continue; }
                var auto = type.GetCustomAttribute<AutoHandler>();
                if (auto == null) { continue; }
                Add(auto.Name, type);
            }
        }
    }
}