using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace ScorpioMessage
{
    public class GenerateScorpio : IGenerate
    {
        public GenerateScorpio(string className, string package, List<PackageField> fields) :
            base(className, package, fields, Code.NONE)
        { }
        public override string Generate()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"__ClassName = [");
            foreach (var field in m_Fields)
            {
                string str = @"
    { Index = __Index, Name = ""__Name"", Type = ""__Type"", Array = __Array },";
                str = str.Replace("__Index", field.Index.ToString());
                str = str.Replace("__Name", field.Name);
                str = str.Replace("__Type", field.Type);
                str = str.Replace("__Array", field.Array ? "true" : "false");
                builder.Append(str);
            }
            builder.Append(@"
]");
            builder = builder.Replace("__ClassName", m_ClassName);
            return builder.ToString();
        }
    }
}
