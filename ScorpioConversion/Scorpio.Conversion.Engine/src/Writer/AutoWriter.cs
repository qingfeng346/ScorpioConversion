using System;

namespace Scorpio.Conversion.Engine {
    public class AutoWriter : Attribute {
        public string Name { get; private set; }
        public AutoWriter(string name) {
            Name = name;
        }
    }
}