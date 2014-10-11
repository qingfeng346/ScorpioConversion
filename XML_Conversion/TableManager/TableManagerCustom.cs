using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public partial class TableManager
{
    public void CreateCustomCS()
    {
        PROGRAM program = PROGRAM.CS;
        string strStream = @"using System;
using System.IO;
using System.Collections.Generic;
#pragma warning disable 0219";
        Type[] types = CodeProvider.GetInstance().GetTypes();
        if (types != null)
        {
            foreach (Type type in types)
            {
                FieldInfo[] fieldInfos = type.GetFields();
                List<Variable> variables = new List<Variable>();
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    Variable variable = new Variable();
                    variable.strFieldType = fieldInfo.FieldType.Name;
                    variable.strFieldName = fieldInfo.Name;
                    variable.strFieldNote = "";
                    variable.bArray = false;
                    variables.Add(variable);
                }
                string strData = Util.GetDataClass(PROGRAM.CS, type.Name, variables, false, false, false);
                FileUtil.CreateFile(string.Format("{0}.cs", type.Name), strStream + strData, true, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
            }
        }
    }
    public void CreateCustomJAVA()
    {
        PROGRAM program = PROGRAM.JAVA;
        Type[] types = CodeProvider.GetInstance().GetTypes();
        if (types != null && types.Length > 0)
        {
            string strStream = @"package table;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.HashMap;
@SuppressWarnings(""unused"")";
            foreach (Type type in types)
            {
                FieldInfo[] fieldInfos = type.GetFields();
                List<Variable> variables = new List<Variable>();
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    Variable variable = new Variable();
                    variable.strFieldType = fieldInfo.FieldType.Name;
                    variable.strFieldName = fieldInfo.Name;
                    variable.strFieldNote = "";
                    variable.bArray = false;
                    variables.Add(variable);
                }
                string strData = Util.GetDataClass(PROGRAM.JAVA, type.Name, variables, false, false, false);
                FileUtil.CreateFile(string.Format("{0}.java", type.Name), strStream + strData, false, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
            }
        }
    }
    public void CreateCustomPHP()
    {
        PROGRAM program = PROGRAM.PHP;
        Type[] types = CodeProvider.GetInstance().GetTypes();
        if (types != null && types.Length > 0)
        {
            string strStream = @"<?php
require_once 'TableUtil.php';";
            foreach (Type type in types)
            {
                FieldInfo[] fieldInfos = type.GetFields();
                List<Variable> variables = new List<Variable>();
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    Variable variable = new Variable();
                    variable.strFieldType = fieldInfo.FieldType.Name;
                    variable.strFieldName = fieldInfo.Name;
                    variable.strFieldNote = "";
                    variable.bArray = false;
                    variables.Add(variable);
                }
                string strData = Util.GetDataClass(PROGRAM.PHP, type.Name, variables, false, false, false);
                strStream += strData;
            }
            strStream += @"
?>";
            FileUtil.CreateFile("Custom.php", strStream, false, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
        }
    }
}

