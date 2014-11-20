using System;
using System.Collections.Generic;
using System.Text;

public partial class TableManager
{
    public void CreateManagerCS()
    {
        PROGRAM program = PROGRAM.CS;
        var classes = mClasses;
        var spawnsClasses = mSpawnsClasses;
        StringBuilder builder = new StringBuilder();
        builder.Append(@"using System;
using System.Collections.Generic;
public class MT_TableManager {");
#region 生成Reset函数
        string resetStr = @"
    public void Reset()
    {";
        foreach (TableClass clazz in classes)
        {
            if (!clazz.IsCreate(program)) continue;
            resetStr += @"
        mTable__Filer = null;";
            resetStr = resetStr.Replace("__Filer", clazz.strFiler);
        }
        resetStr += Util.ReturnString;
        foreach (var pair in spawnsClasses)
        {
            SpawnsClass clazz = pair.Value;
            if (!clazz.IsCreate(program)) continue;
            resetStr += @"
        __spawnsNameArray.Clear();";
            resetStr = resetStr.Replace("__spawnsName", pair.Key);
        }
        resetStr += @"
    }";
        builder.Append(resetStr);
#endregion
#region 生成所有table
        foreach (TableClass clazz in classes)
        {
            if (!clazz.IsCreate(program)) continue;
            string str = @"
    private __TableClass mTable__Filer = null;
    public __TableClass Table__Filer {
        get {
            if (mTable__Filer == null) {
                mTable__Filer = new __TableClass(""__Filer"");
                mTable__Filer.Initialize();
            }
            return mTable__Filer;
        }
    }";
            str = str.Replace("__TableClass", clazz.strTableClass);
            str = str.Replace("__Filer", clazz.strFiler);
            builder.Append(str);
        }
#endregion
#region 生成模版table
        foreach (var pair in spawnsClasses)
        {
            SpawnsClass clazz = pair.Value;
            if (!clazz.IsCreate(program)) continue;
            string enumName = pair.Key;
            string classCode = @"
    public enum __enumName {";
            foreach (string value in clazz.list)
            {
                string enumElement = @"
        __enumTtitle,";
                classCode += enumElement.Replace("__enumTtitle", value);
            }
            classCode += @"
    }
    private Dictionary<string,__TableClass> __spawnsNameArray = new Dictionary<string,__TableClass>();
    public __TableClass getSpawns(__enumName index) {
        if (__spawnsNameArray.ContainsKey(index.ToString())) {
		    return __spawnsNameArray[index.ToString()];
	    }
	    __TableClass spawns = new __TableClass(index.ToString());
	    spawns.Initialize();
	    __spawnsNameArray.Add(index.ToString(), spawns);
	    return spawns;
    }
    public __TableClass getSpawns___enumName(string index)
    {
        string str = ""__enumName_"" + index;
        if (__spawnsNameArray.ContainsKey(str)) {
            return __spawnsNameArray[str];
        }
        __TableClass spawns = new __TableClass(str);
        spawns.Initialize();
        __spawnsNameArray.Add(str, spawns);
        return spawns;
    }";
            classCode = classCode.Replace("__TableClass", clazz.strTableClass);
            classCode = classCode.Replace("__enumName", enumName);
            classCode = classCode.Replace("__spawnsName", pair.Key);
            builder.Append(classCode);
        }
#endregion
#region 生成根据字符串获得模版表的函数
        builder.Append(@"
    public MT_TableBase getSpawns(string key, string index) {");
        foreach (var pair in spawnsClasses)
        {
            SpawnsClass clazz = pair.Value;
            if (!clazz.IsCreate(program)) continue;
            string str = @"
        if (key == ""__Spawns"") return getSpawns___Spawns(index);";
            builder.Append(str.Replace("__Spawns", pair.Key));
        }
        builder.Append(@"
        return null;
    }");
#endregion
#region 生成根据字符串获得table的函数
        builder.Append(@"
    public enum TABLE_ENUM {");
        foreach (TableClass clazz in classes)
        {
            if (!clazz.IsCreate(program)) continue;
            string str = @"
        __Table,";
            builder.Append(str.Replace("__Table", clazz.strFiler));
        }
        builder.Append(@"
    }");
        builder.Append(@"
    public MT_TableBase getTable(TABLE_ENUM key) {");
        foreach (TableClass clazz in classes)
        {
            if (!clazz.IsCreate(program)) continue;
            string str = @"
        if (key == TABLE_ENUM.__Table) return Table__Table;";
            builder.Append(str.Replace("__Table", clazz.strFiler));
        }
        builder.Append(@"
        return null;
    }");
        builder.Append(@"
    public MT_TableBase getTable(string key) {");
        foreach (TableClass clazz in classes)
        {
            if (!clazz.IsCreate(program)) continue;
            string str = @"
        if (key == ""__Table"") return Table__Table;";
            builder.Append(str.Replace("__Table", clazz.strFiler));
        }
        builder.Append(@"
        return null;
    }");
#endregion
        builder.Append(@"
}");
        FileUtil.CreateFile("MT_TableManager.cs", builder.ToString(), true, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
    }
    public void CreateManagerJAVA()
    {
        PROGRAM program = PROGRAM.JAVA;
        var classes = mClasses;
        var spawnsClasses = mSpawnsClasses;
        StringBuilder builder = new StringBuilder();
        builder.Append(@"package table;
import java.util.HashMap;
public class MT_TableManager {");
        foreach (TableClass clazz in classes)
        {
            if (!clazz.IsCreate(program)) continue;
            string str = @"
    private __TableClass mTable__Filer = null;
    public __TableClass Table__Filer() {
        if (mTable__Filer == null) {
            try {
                mTable__Filer = new __TableClass(""__Filer"");
                mTable__Filer.Initialize();
            } catch (Exception e) {
                TableUtil.Error(""Table__Filer is error : "",e);
            }
        }
        return mTable__Filer;
    }";
            str = str.Replace("__TableClass", clazz.strTableClass);
            str = str.Replace("__Filer", clazz.strFiler);
            builder.Append(str);
        }
        foreach (var pair in spawnsClasses)
        {
            SpawnsClass clazz = pair.Value;
            if (!clazz.IsCreate(program)) continue;
            string enumName = pair.Key;
            string classCode = @"
    public enum __enumName {";
            foreach (string value in clazz.list)
            {
                string enumElement = @"
        __enumTtitle,";
                classCode += enumElement.Replace("__enumTtitle", value);
            }
            classCode += @"
    }
    private HashMap<String,__TableClass> __spawnsNameArray = new HashMap<String,__TableClass>();
    public __TableClass getSpawns(__enumName index) {
        if (__spawnsNameArray.containsKey(index.toString())) {
		    return __spawnsNameArray.get(index.toString());
	    }
        try {
            __TableClass spawns = new __TableClass(index.toString());
	        spawns.Initialize();
	        __spawnsNameArray.put(index.toString(), spawns);
            return spawns;
        } catch (Exception e) {
            TableUtil.Error(""getSpawns(__enumName index) is error : "",e);
        }
        __spawnsNameArray.put(index.toString(), null);
	    return null;
    }
    public __TableClass getSpawns___enumName(String index) {
        String str = ""__enumName_"" + index;
        if (__spawnsNameArray.containsKey(str)) {
            return __spawnsNameArray.get(str);
        }
        try {
            __TableClass spawns = new __TableClass(str);
            spawns.Initialize();
            __spawnsNameArray.put(str, spawns);
            return spawns;
        } catch (Exception e) {
            TableUtil.Error(""getSpawns___enumName(String index) is error : "",e);
        }
        __spawnsNameArray.put(str, null);
	    return null;
    }";
            classCode = classCode.Replace("__TableClass", clazz.strTableClass);
            classCode = classCode.Replace("__enumName", enumName);
            classCode = classCode.Replace("__spawnsName", pair.Key);
            builder.Append(classCode);
        }
        builder.Append(@"
    public MT_TableBase getSpawns(String key, String index) {");
        foreach (var pair in spawnsClasses)
        {
            SpawnsClass clazz = pair.Value;
            if (!clazz.IsCreate(program)) continue;
            string str = @"
        if (key.equals(""__Spawns"")) return getSpawns___Spawns(index);";
            builder.Append(str.Replace("__Spawns", pair.Key));
        }
        builder.Append(@"
        return null;
    }");
        builder.Append(@"
}");
        FileUtil.CreateFile("MT_TableManager.java", builder.ToString(), false, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
    }
    public void CreateManagerPHP()
    {
        PROGRAM program = PROGRAM.PHP;
        var classes = mClasses;
        var spawnsClasses = mSpawnsClasses;
        StringBuilder builder = new StringBuilder();
        builder.Append(@"<?php");
        foreach (var pair in spawnsClasses)
        {
            string classCode = @"
class __enumName {";
            foreach (string value in pair.Value.list)
            {
                string enumElement = @"
    const __enumTtitle = ""__enumTtitle"";";
                classCode += enumElement.Replace("__enumTtitle", value);
            }
            classCode += @"
}";
            classCode = classCode.Replace("__enumName", pair.Key);
            builder.Append(classCode);
        }
        builder.Append(@"
class MT_TableManager {");
        foreach (TableClass clazz in classes)
        {
            if (!clazz.IsCreate(program)) continue;
            string str = @"
    private $mTable__Filer = null;
    public function Table__Filer() {
        if ($this->mTable__Filer == null) {
            $this->mTable__Filer = new __TableClass(""__Filer"");
            $this->mTable__Filer->Initialize();
        }
        return $this->mTable__Filer;
    }";
            str = str.Replace("__TableClass", clazz.strTableClass);
            str = str.Replace("__Filer", clazz.strFiler);
            builder.Append(str);
        }
        foreach (var pair in spawnsClasses)
        {
            SpawnsClass clazz = pair.Value;
            if (!clazz.IsCreate(program)) continue;
            string enumName = pair.Key;
            string classCode = @"
    private $__spawnsNameArray = array();
    public function getSpawns___spawnsName($index) {
        if (array_key_exists($this->__spawnsNameArray,$index)) {
		    return $this->__spawnsNameArray[$index];
	    }
	    $spawns = new __TableClass($index);
	    $spawns->Initialize();
	    $this->__spawnsNameArray[$index] = $spawns;
	    return $spawns;
    }";
            classCode = classCode.Replace("__TableClass", clazz.strTableClass);
            classCode = classCode.Replace("__enumName", enumName);
            classCode = classCode.Replace("__spawnsName", pair.Key);
            builder.Append(classCode);
        }
        builder.Append(@"
    public function getSpawns($key, $index) {");
        foreach (var pair in spawnsClasses)
        {
            SpawnsClass clazz = pair.Value;
            if (!clazz.IsCreate(program)) continue;
            string str = @"
        if ($key == ""__Spawns"") return $this->getSpawns___Spawns($index);";
            builder.Append(str.Replace("__Spawns", pair.Key));
        }
        builder.Append(@"
        return null;
    }");

        builder.Append(@"
}
?>");
        FileUtil.CreateFile("MT_TableManager.php", builder.ToString(), false, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
    }
}

