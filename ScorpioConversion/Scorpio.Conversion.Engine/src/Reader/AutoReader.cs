using System;

namespace Scorpio.Conversion.Engine {
    public class AutoReader : Attribute {
        public string Name { get; private set; }
        public AutoReader(string name) {
            Name = name;
        }
    }
}