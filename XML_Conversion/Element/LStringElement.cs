using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
class LStringElement : StringElement
{
    public override ElementType Type { get { return ElementType.LSTRING; } }
    public override void WriteValueByType_impl(string strValue, BinaryWriter writer)
    {
        Util.WriteString(writer, "");
    }
    public override string GetReadMemory_impl(PROGRAM program, string strName)
    {
        switch (program)
        {
            case PROGRAM.CS:
            case PROGRAM.JAVA:
                return string.Format("TableUtil.ReadLString(reader, fileName,\"{0}\",{1})", strName, "Data.ID()");
            case PROGRAM.PHP:
                return "$reader->readString()";
        }
        return base.GetReadMemory_impl(program, strName);
    }
}