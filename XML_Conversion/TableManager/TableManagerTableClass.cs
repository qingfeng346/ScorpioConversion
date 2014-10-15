using System;
using System.Collections.Generic;
using System.Text;


public partial class TableManager
{
    /// <summary>
    /// 获得Table类的构造函数
    /// </summary>
    private string GetTableStructureFunc(PROGRAM program)
    {
        string str = "";
        switch (program)
        {
            case PROGRAM.CS:
                str += @"
    public __TableClass (__StringElement fileName) {
        this.fileName = fileName;
    }";
                break;
            case PROGRAM.JAVA:
                str += @"
    public __TableClass (__StringElement fileName) {
        this.fileName = fileName;
    }";
                break;
            case PROGRAM.CPP:
                str += @"
    public: __TableClass (__StringElement fileName) {
        this->fileName = fileName;
    }";
                break;
            case PROGRAM.PHP:
                str += @"
    public function __construct ($fileName) {
        $this->fileName = $fileName;
    }";
                break;
        }
        return str;
    }
    /// <summary>
    /// 获得Table类的Initialize函数
    /// </summary>
    private string GetTableInitialize(PROGRAM program)
    {
        string str = "";
        if (program == PROGRAM.CS)
        {
            str += @"
    public void Initialize() {
        m_dataArray.Clear();
        byte[] buffer = TableUtil.GetBuffer(fileName);
        TableReader reader = new TableReader(buffer);
        __IntElement iRow = __IntElement.Read();
        __IntElement iColums = __IntElement.Read();
        __IntElement iCodeNum = __IntElement.Read();";
            Type[] types = CodeProvider.GetInstance().GetTypes();
            if (types != null && mCustomClasses.Count > 0)
            {
                foreach (Type type in types)
                {
                    if (!mCustomClasses.ContainsKey(type.Name))
                        continue;
                    string temp = @"
        __StringElement strClassMD5Code__Key = __StringElement.Read();
        if (strClassMD5Code__Key != __Key.MD5)
            throw new System.Exception(""文件["" + fileName + ""]的自定义类[__Key]已被修改"");";
                    str += temp.Replace("__Key", type.Name);
                }
            }
            str += @"
        __StringElement strMD5Code = __StringElement.Read();
        if (strMD5Code != FILE_MD5_CODE)
            throw new System.Exception(""文件["" + fileName + ""]版本验证失败"");
        for (int i = 0; i < iColums; ++i) {
            __IntElement index = __IntElement.Read();
            __IntElement.Read();
            if (index == TableUtil.CLASS_VALUE) {
                __StringElement.Read();
                __IntElement nCount = __IntElement.Read();
                for (__IntElement k = 0; k < nCount; ++k) {
                    __IntElement.Read();
                }
            }
        }
        for (__IntElement i = 0; i < iRow; ++i) {
            __DataClass pData = __DataClass.ReadMemory(reader, fileName);
            if (Contains(pData.ID()))
                throw new System.Exception(""文件["" + fileName + ""]有重复项 ID : "" + pData.ID());
            m_dataArray.Add(pData.ID(), pData);
        }
        reader.Close()
    }";
        }
        else if (program == PROGRAM.JAVA)
        {
            str += @"
    public void Initialize() throws Exception {
        m_dataArray.clear();
        byte[] buffer = TableUtil.GetBuffer(fileName);
        TableReader reader = new TableReader(buffer);
        __IntElement iRow = __IntElement.Read();
        __IntElement iColums = __IntElement.Read();
        __IntElement iCodeNum = __IntElement.Read();";
            Type[] types = CodeProvider.GetInstance().GetTypes();
            if (types != null && mCustomClasses.Count > 0)
            {
                foreach (Type type in types)
                {
                    if (!mCustomClasses.ContainsKey(type.Name))
                        continue;
                    string temp = @"
        __StringElement strClassMD5Code__Key = __StringElement.Read();
        if (!strClassMD5Code__Key.equals(__Key.MD5))
            throw new Exception(""文件"" + fileName + ""的自定义类[__Key]已被修改"");";
                    str += temp.Replace("__Key", type.Name);
                }
            }
            str += @"
        __StringElement strMD5Code = __StringElement.Read();
        if (!strMD5Code.equals(FILE_MD5_CODE))
            throw new Exception(""文件"" + fileName + ""版本验证失败"");
        for (__IntElement i = 0; i < iColums; ++i) {
            __IntElement index = __IntElement.Read();
            __IntElement.Read();
            if (index.equals(TableUtil.CLASS_VALUE)) {
                __StringElement.Read();
                __IntElement nCount = __IntElement.Read();
                for (__IntElement k = 0; k < nCount; ++k){
                    __IntElement.Read();
                }
            }
        }
        for (__IntElement i = 0; i < iRow; ++i) {
            __DataClass pData = __DataClass.ReadMemory(reader, fileName);
            if (Contains(pData.ID()))
                throw new Exception(""文件"" + fileName + ""有重复项 ID : "" + pData.ID());
            m_dataArray.put(pData.ID(),pData);
        }
        reader.Close()
    }";
        }
        else if (program == PROGRAM.CPP)
        {
            str += @"
    public: void Initialize() {
        m_dataArray.clear();
        char * buffer = TableUtil.GetBuffer(fileName);
        TableReader reader = TableReader(buffer);
        __IntElement iRow = __IntElement.Read();
        __IntElement iColums = __IntElement.Read();
        __IntElement iCodeNum = __IntElement.Read();";
            Type[] types = CodeProvider.GetInstance().GetTypes();
            if (types != null && mCustomClasses.Count > 0)
            {
                foreach (Type type in types)
                {
                    if (!mCustomClasses.ContainsKey(type.Name))
                        continue;
                    string temp = @"
        __StringElement strClassMD5Code__Key = __StringElement.Read();
        if (!(strClassMD5Code__Key == __Key_MD5))
            throw new Exception(""文件"" + fileName + ""的自定义类[__Key]已被修改"");";
                    str += temp.Replace("__Key", type.Name);
                }
            }
            str += @"
        __StringElement strMD5Code = __StringElement.Read();
        if (!(strMD5Code == __TableClass_FILE_MD5_CODE))
            throw new Exception(""文件"" + fileName + ""版本验证失败"");
        for (__IntElement i = 0; i < iColums; ++i) {
            __IntElement index = __IntElement.Read();
            __IntElement.Read();
            if (index.equals(TableUtil.CLASS_VALUE)) {
                __StringElement.Read();
                __IntElement nCount = __IntElement.Read();
                for (__IntElement k = 0; k < nCount; ++k){
                    __IntElement.Read();
                }
            }
        }
        for (__IntElement i = 0; i < iRow; ++i) {
            __DataClass pData = __DataClass::ReadMemory(reader, fileName);
            if (Contains(pData.ID()))
                throw new Exception(""文件"" + fileName + ""有重复项 ID : "" + pData.ID());
            m_dataArray.put(pData.ID(),pData);
        }
        reader.Close();
    }";
        }
        else if (program == PROGRAM.PHP)
        {
            str += @"
    public function Initialize() {
        $this->m_dataArray = array();
        $buffer = TableUtil::GetBuffer($this->fileName);
        $reader = new ByteBuffer($buffer);
        $reader->order(ByteBuffer::LITTLE_ENDIAN);
        $iRow = __IntElement.Read();
        $iColums = __IntElement.Read();
        $iCodeNum = __IntElement.Read();";
            Type[] types = CodeProvider.GetInstance().GetTypes();
            if (types != null && mCustomClasses.Count > 0)
            {
                foreach (Type type in types)
                {
                    if (!mCustomClasses.ContainsKey(type.Name))
                        continue;
                    string temp = @"
        $strClassMD5Code__Key = __StringElement.Read();
        if ($strClassMD5Code__Key != __Key::MD5)
            throw new Exception('文件'.$this->fileName.'的自定义类[__Key]已被修改');";
                    str += temp.Replace("__Key", type.Name);
                }
            }
            str += @"
        $strMD5Code = __StringElement.Read();
        if ($strMD5Code != __TableClass::FILE_MD5_CODE)
            throw new Exception('文件'.$this->fileName.'版本验证失败');
        for ($i = 0; $i < $iColums; ++$i) {
            $index = __IntElement.Read();
            __IntElement.Read();
            if ($index == TableUtil::VARIABLE_CLASS)
            {
                __StringElement.Read();
                $nCount = __IntElement.Read();
                for ($k=0; $k < $nCount; ++$k){
                    __IntElement.Read();
                }
            }
        }
        for ($i = 0; $i < $iRow; ++$i) {
            $pData = __DataClass::ReadMemory($reader, $this->fileName);
            if ($this->Contains($pData->ID()))
                throw new Exception('文件' + $this->fileName + '有重复项 ID : ' + $pData->ID());
            else
                $this->m_dataArray[$pData->ID()] = $pData;
        }
    }";
        }
        return str;
    }
    /// <summary>
    /// 获得Table类的Contains函数
    /// </summary>
    private string GetTableContains(PROGRAM program)
    {
        string str = "";
        switch (program)
        {
            case PROGRAM.CS:
                str += @"
    public override __BoolElement Contains(__KeyElement ID) {
        return m_dataArray.ContainsKey(ID);
    }";
                break;
            case PROGRAM.JAVA:
                str += @"
    @Override
    public __BoolElement Contains(__KeyElement ID) {
        return m_dataArray.containsKey(ID);
    }";
                break;
            case PROGRAM.CPP:
                str += @"
    public: __BoolElement Contains(__KeyElement ID) {
        return (m_dataArray.find(ID) != m_dataArray.end());
    }";
                break;
            case PROGRAM.PHP:
                str += @"
    public function Contains($ID) {
        return array_key_exists($ID,$this->m_dataArray);
    }";
                break;
        }
        return str;
    }
    /// <summary>
    /// 获得Table类的GetElement函数
    /// </summary>
    private string GetTableGetElement(PROGRAM program)
    {
        string str = "";
        switch (program)
        {
            case PROGRAM.CS:
                str += @"
	public __DataClass GetElement(__KeyElement ID) {
		if (Contains(ID))
			return m_dataArray[ID];
        TableUtil.Warning(""__DataClass key is not exist "" + ID);
		return null;
	}";
                break;
            case PROGRAM.JAVA:
                str += @"
	public __DataClass GetElement(__KeyElement ID) {
		if (Contains(ID))
			return m_dataArray.get(ID);
        TableUtil.Warning(""__DataClass key is not exist "" + ID);
		return null;
	}";
                break;
            case PROGRAM.PHP:
                str += @"
	public function GetElement($ID) {
		if (Contains(ID))
			return $this->m_dataArray[$ID];
        TableUtil::Warning('__DataClass key is not exist '.$ID);
		return null;
	}";
                break;
        }
        return str;
    }
    /// <summary>
    /// 获得Table类的GetElement函数
    /// </summary>
    private string GetTableGetValue(PROGRAM program)
    {
        string str = "";
        switch (program)
        {
            case PROGRAM.CS:
                str += @"
	public override MT_DataBase GetValue(__KeyElement ID) {
        return GetElement(ID);
	}";
                break;
            case PROGRAM.JAVA:
                str += @"
    @Override
	public MT_DataBase GetValue(__KeyElement ID) {
		return GetElement(ID);
	}";
                break;
            case PROGRAM.PHP:
                str += @"
	public function GetValue($ID) {
		return $this->GetElement($ID);
	}";
                break;
        }
        return str;
    }
    /// <summary>
    /// 获得Table类的Count函数
    /// </summary>
    private string GetTableCount(PROGRAM program)
    {
        string str = "";
        switch (program)
        {
            case PROGRAM.CS:
                str += @"
	public override __IntElement Count() {
		return m_dataArray.Count;
	}";
                break;
            case PROGRAM.JAVA:
                str += @"
    @Override
	public __IntElement Count() {
		return m_dataArray.size();
	}";
                break;
            case PROGRAM.PHP:
                str += @"
	public function Count() {
		return count($this->m_dataArray);
	}";
                break;
        }
        return str;
    }
    /// <summary>
    /// 获得Table类的Datas函数
    /// </summary>
    private string GetTableDatas(PROGRAM program)
    {
        string str = "";
        switch (program)
        {
            case PROGRAM.CS:
                str += @"
	public Dictionary<__KeyElement,__DataClass> Datas() {
		return m_dataArray;
	}";
                break;
            case PROGRAM.JAVA:
                str += @"
	public final HashMap<__KeyElement,__DataClass> Datas() {
		return m_dataArray;
	}";
                break;
            case PROGRAM.PHP:
                str += @"
	public final function Datas() {
		return $this->m_dataArray;
	}";
                break;
        }
        return str;
    }
    /// <summary>
    /// 获得Table类的代码
    /// </summary>
    private string GetTableClass(PROGRAM program)
    {
        StringBuilder builder = new StringBuilder();
        switch (program)
        {
            case PROGRAM.CS:
                builder.Append(@"
public class __TableClass : MT_TableBase {
	const __StringElement FILE_MD5_CODE = ""__MD5Code"";
    private __StringElement fileName = """";
    private Dictionary<__KeyElement,__DataClass> m_dataArray = new Dictionary<__KeyElement,__DataClass>();");
                break;
            case PROGRAM.JAVA:
                builder.Append(@"
public class __TableClass extends MT_TableBase {
	final __StringElement FILE_MD5_CODE = ""__MD5Code"";
    private __StringElement fileName = """";
    private HashMap<__KeyElement,__DataClass> m_dataArray = new HashMap<__KeyElement,__DataClass>();");
                break;
            case PROGRAM.CPP:
                builder.Append(@"
#define __TableClass_FILE_MD5_CODE = ""__MD5Code"";
class __TableClass: public MT_TableBase {
    private: __StringElement fileName;
    private: map<__KeyElement,__DataClass> m_dataArray;");
                break;
            case PROGRAM.PHP:
                builder.Append(@"
class __TableClass {
	const FILE_MD5_CODE = '__MD5Code';
    private $fileName = '';
    private $m_dataArray = array();");
                break;
        }
        builder.Append(GetTableStructureFunc(program));
        builder.Append(GetTableInitialize(program));
        builder.Append(GetTableContains(program));
        builder.Append(GetTableGetElement(program));
        builder.Append(GetTableGetValue(program));
        builder.Append(GetTableCount(program));
        builder.Append(GetTableDatas(program));
        Element intElement = Util.INT_ELEMENT;
        Element strElement = Util.STRING_ELEMENT;
        Element boolElement = Util.BOOL_ELEMENT;
        builder = builder.Replace("KeyElement", mKeyElement);
        builder = builder.Replace("__IntElement.Read()", intElement.GetReadMemory_impl(program));
        builder = builder.Replace("__StringElement.Read()", strElement.GetReadMemory_impl(program));
        builder = builder.Replace("__BoolElement.Read()", boolElement.GetReadMemory_impl(program));
        builder = builder.Replace("__IntElement", intElement.GetVariable(program));
        builder = builder.Replace("__Int32Element", intElement.GetVariable(program));
        builder = builder.Replace("__StringElement", strElement.GetVariable(program));
        builder = builder.Replace("__BoolElement", boolElement.GetVariable(program));
        builder = builder.Replace("__DataClass", mStrDataClass);
        builder = builder.Replace("__TableClass", mStrTableClass);
        builder = builder.Replace("__MD5Code", mStrMD5Code);
        if (program == PROGRAM.CPP)
        {
            builder.Append(@"
};");
        }
        else
        {
            builder.Append(@"
}");
        }
        return builder.ToString();
    }
}
