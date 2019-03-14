using System;
using System.Collections.Generic;
using System.Text;

public class TemplateTypescript {
    public const string Head = @"import { ScorpioReader } from '../ScorpioProto/Commons/ScorpioReader'
import { ScorpioUtil } from '../ScorpioProto/Commons/ScorpioUtil'
import { TableUtil } from '../ScorpioProto/Table/TableUtil'
import { IData } from '../ScorpioProto/Table/IData'
import { ITable } from '../ScorpioProto/Table/ITable'";

    public const string Table = @"export class __TableName extends ITable {
	private FILE_MD5_CODE:string = ""__MD5"";
    private m_count:number = 0;
    private m_dataArray:{[key:__KeyType]:__DataName} = {};
    public Initialize(l10n:{[key:string]:string}, fileName:string, buffer:any):__TableName {
        let reader = new ScorpioReader(buffer)
        let iRow = TableUtil.ReadHead(reader, fileName, this.FILE_MD5_CODE);
        for (let i = 0; i < iRow; ++i) {
            let pData = __DataName.Read(l10n, fileName, reader);
            if (this.Contains(pData.ID())) {
                this.m_dataArray[pData.ID()].Set(pData);
            } else {
                this.m_dataArray[pData.ID()] = pData;
            }
        }
        this.m_count = Object.getOwnPropertyNames(this.m_dataArray).length;
        return this;
    }
    public GetValue(ID:__KeyType):__DataName|null {
        if (this.Contains(ID)) return this.m_dataArray[ID];
        TableUtil.Warning(""__DataName key is not exist "" + ID);
        return null;
    }
    public Contains(ID:__KeyType):boolean {
        return this.m_dataArray[ID] != null;
    }
    public Datas():{[key:__KeyType]:__DataName} {
        return this.m_dataArray;
    }
    public GetValueObject(ID:any):IData|null {
        return this.GetValue(ID as __KeyType);
    }
    public ContainsObject(ID:any):boolean {
        return this.Contains(ID as __KeyType);
    }
    public GetDatas():any {
        return this.Datas();
    }
    public Count():number {
        return this.m_count;
    }
}
";
}
public class GenerateDataTypescript : IGenerate {
    protected override string Generate_impl() {
        return $@"{TemplateTypescript.Head}
export class {ClassName} extends IData {{
    private m_IsInvalid:boolean = false;
    {AllFields()}
    {FuncGetData()}
    {FuncIsInvalid()}
    {FuncSet()}
    {FuncToString()}
    {FuncRead()}
}}";
    }
    string AllFields() {
        var builder = new StringBuilder();
        var first = true;
        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            if (field.Array) { languageType = $"Array<{languageType}>"; }
            builder.Append($@"
    private _{field.Name}:any = null;
    /* {field.Comment}  默认值({field.Default}) */
    get{field.Name}():{languageType} {{ return this._{field.Name}; }}");
            if (first && (bool)Parameter) {
                first = false;
                builder.Append($@"
    ID():{languageType} {{ return this._{field.Name}; }}");
            }
        }
        return builder.ToString();
    }
    string FuncGetData() {
        var builder = new StringBuilder();
        builder.Append(@"
    GetData(key:string):any {");
        foreach (var field in Fields) {
            builder.Append($@"
        if (""{field.Name}"" == key) return this._{field.Name};");
        }
        builder.Append(@"
        return null;
    }");
        return builder.ToString();
    }
    string FuncIsInvalid() {
        return @"
    IsInvalid():boolean { return this.m_IsInvalid; }
    private CheckInvalid():boolean {
        return false;
    }";
    }
    string FuncSet() {
        var builder = new StringBuilder();
        builder.Append($@"
    Set(value:{ClassName}) {{");
        foreach (var field in Fields) {
            builder.Append($@"
        this._{field.Name} = value._{field.Name};");
        }
        builder.Append(@"
    }");
        return builder.ToString();
    }
    string FuncToString() {
        var builder = new StringBuilder();
        builder.Append(@"
    ToString() {
        return `{");
        var count = Fields.Count;
        for (int i = 0; i < count; ++i) {
            var field = Fields[i];
            var toString = field.Array ? $"ScorpioUtil.ToString(this._{field.Name})" : $"this._{field.Name}";
            builder.Append($@"
            {field.Name} : ${{{toString}}}");
            if (i != count - 1) {
                builder.Append(", ");
            }
        }
        builder.Append(@"
            }`;
    }");
        return builder.ToString();
    }
    string FuncRead() {
        var builder = new StringBuilder();
        builder.Append($@"
    public static Read(l10n:{{[key:string]:string}}, fileName:string, reader:ScorpioReader):{ClassName} {{
        var ret = new {ClassName}();");
        foreach (var field in Fields) {
            var languageType = field.GetLanguageType(Language);
            var fieldRead = "";
            if (field.Attribute != null && field.Attribute.GetValue("Language").LogicOperation()) {
                fieldRead = $@"TableUtil.Readl10n(l10n, fileName + ""_{field.Name}_"" + ret.ID(), reader)";
            } else if (field.IsBasic) {
                fieldRead = $"reader.Read{field.BasicType.Name}()";
            } else if (field.IsEnum) {
                fieldRead = $"<{languageType}>reader.ReadInt32()";
            } else {
                fieldRead = $"{languageType}.Read(l10n, fileName, reader)";
            }
            if (field.Array) {
                builder.Append($@"
        {{
            let list = new Array<{languageType}>();
            let number = reader.ReadInt32();
            for (let i = 0; i < number; ++i) {{ list.push({fieldRead}); }}
            ret._{field.Name} = list;
        }}");
            } else {
                builder.Append($@"
        ret._{field.Name} = {fieldRead};");
            }
        }
        builder.Append(@"
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }");
        return builder.ToString();
    }
}
public class GenerateTableTypescript : IGenerate {
    protected override string Generate_impl() {
        return $@"{TemplateTypescript.Head}
import {{ __DataName }} from './__DataName';
{TemplateTypescript.Table}";
    }
}

