using System;
namespace Scorpio.Conversion.Engine {
    public class AutoGenerator : Attribute {
        public string Name { get; private set; }
        public AutoGenerator(string name) {
            this.Name = name;
        }
    }
}
