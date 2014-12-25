using System;
using System.Collections.Generic;
using System.Text;

public class IValue
{
}
public class ValueString : IValue
{
    public string value;
}
public class ValueList : IValue
{
    public List<IValue> values = new List<IValue>();
}

