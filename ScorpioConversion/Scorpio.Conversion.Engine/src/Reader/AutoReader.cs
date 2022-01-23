using System;

namespace Scorpio.Conversion {
    public class AutoReader : Attribute {
        public string Name { get; private set; }
        public object[] Args { get; private set; }
        public AutoReader(string name, params object[] args) {
            Name = name;
            Args = args;
        }
    }
}