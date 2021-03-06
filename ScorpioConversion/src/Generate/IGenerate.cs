﻿using System;
using System.Collections.Generic;
using System.Text;

public abstract class IGenerate {
    public string PackageName { get; set; }
    public string ClassName { get; set; }
    public IPackage Package { get; set; }
    public object Parameter { get; set; }
    public Language Language { get; set; }

    public PackageEnum Enums { get { return Package as PackageEnum; } }
    public List<FieldClass> Fields { get { return (Package as PackageClass).Fields; } }

    public string Generate() { return Generate_impl(); }
    protected abstract string Generate_impl();
}
