using System;
namespace Scorpio.Conversion.Engine {
    public class AutoHandler : Attribute {
        public string Name { get; private set; }
        public AutoHandler(string name) {
            this.Name = name;
        }
    }
}
