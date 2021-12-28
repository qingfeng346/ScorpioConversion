using System;
public class AutoWriter : Attribute {
    public string Name { get; private set; }
    public object[] Args { get; private set; }
    public AutoWriter(string name, params object[] args) {
        this.Name = name;
        this.Args = args;
    }
}
