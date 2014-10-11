using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

class ClassElement : Element
{
    public override ElementType Type { get { return ElementType.CLASS; } }
    private Type mClassType;
    public Type GetClassType()
    {
        return mClassType;
    }
    public void Initialize(Type type)
    {
        mClassType = type;
    }
    protected override string GetVariable_impl(PROGRAM program)
    {
        return mClassType.Name;
    }
    public override void WriteValueByType_impl(string strValue, BinaryWriter writer)
    {
        Type[] types = Util.GetElementTypes(mClassType);
        if (Util.IsEmptyString(strValue)) {
            for (int i = 0; i < types.Length; ++i) {
                Element element = Util.GetPrimitiveElement(types[i]);
                element.WriteValueByType("####", writer, false);
            }
        } else {
            string[] values = strValue.Split(':');
            if (values.Length != types.Length)
                throw new Exception(string.Format("参数个数填写错误 请填写对应数量的参数   need : {0}   real : {1}", types.Length, values.Length));
            for (int i = 0; i < types.Length; ++i) {
                try {
                    Element element = Util.GetPrimitiveElement(types[i]);
                    element.WriteValueByType(values[i], writer, false);
                } catch (System.Exception ex) {
                    throw new Exception(string.Format("第【{0}】个参数填写错误（从1开始数） 需要类型【{1}】  实际值【{2}】   {3}", i + 1, types[i].Name, values[i], ex.ToString()));
                }
            }
        }
    }
    public override string GetReadMemory_impl(PROGRAM program, string strName)
    {
        if (program == PROGRAM.PHP)
            return string.Format("{0}::ReadMemory($reader, $fileName)", mClassType.Name);
        return string.Format("{0}.ReadMemory(reader, fileName)", mClassType.Name);
    }
}