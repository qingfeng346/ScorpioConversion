using System;
namespace Scorpio.Conversion {
    public class AutoGenerator : Attribute {
        public string Name { get; private set; }
        public object[] Args { get; private set; }
        public AutoGenerator(string name, params object[] args) {
            this.Name = name;
            this.Args = args;
        }
    }
}
