using System;
using System.Collections.Generic;
using System.Text;

class FStringElement : StringElement
{
    public override ElementType Type { get { return ElementType.FSTRING; } }
    public override string GetReadMemory_impl(PROGRAM program, string strName)
    {
        switch (program)
        {
            case PROGRAM.CS:
                return "TableUtil.ReadFString(reader)";
            case PROGRAM.JAVA:
                return "TableUtil.ReadFString(reader)";
            case PROGRAM.PHP:
                return "$reader->readString()";
        }
        return base.GetReadMemory_impl(program, strName);
    }
}