using System;
using System.Collections.Generic;
using System.Text;

public abstract class IGenerate {
    public string PackageName { get; set; }
    public string ClassName { get; set; }
    public object Parameter { get; set; }
    public Language Language { get; set; }
    public string Generate() {
        return Generate_impl();
    }
    protected abstract string Generate_impl();
}
