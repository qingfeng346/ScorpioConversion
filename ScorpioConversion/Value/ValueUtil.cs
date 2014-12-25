using System;
using System.Collections.Generic;
using System.Text;


public static class ValueUtil
{
    public static IValue ReadValue(string value)
    {
        string temp = "[" + value + "]";
        return ReadValue_impl(ref temp);
    }
    public static IValue ReadValue_impl(ref string value)
    {
        if (value.StartsWith("[")) {
            value = value.Substring(1);
            ValueList ret = new ValueList();
            while (!value.StartsWith("]")) {
                if (value.StartsWith(","))
                    value = value.Substring(1);
                ret.values.Add(ReadValue_impl(ref value));
            }
            value = value.Substring(1);
            return ret;
        } else {
            ValueString ret = new ValueString();
            int index1 = value.IndexOf(',');
            int index2 = value.IndexOf("]");
            if (index1 == -1 && index2 == -1) {
                ret.value = value;
                value = "";
            } else if (index1 == -1 || index2 < index1) {
                ret.value = value.Substring(0, index2);
                value = value.Substring(index2);
            } else {
                ret.value = value.Substring(0, index1);
                value = value.Substring(index1 + 1);
            }
            return ret;
        }
    }
}

