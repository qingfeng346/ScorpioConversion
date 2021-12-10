using System;
public class BaseAttribute : Attribute {
    public string Name { get; private set; }
    public bool AutoAdd { get; private set; }
    public BaseAttribute(string name, bool autoAdd = true) {
        Name = name;
        AutoAdd = autoAdd;
    }
}
