#pragma once
#include <string>
#include <map>
using namespace::std;
#define __TableName_FILE_MD5_CODE = "__MD5";
class __TableName : public MT_TableBase {
    private: string fileName;
    private: map<int,__DataName*> m_dataArray;
    public: __TableName (string fileName) {
        this->fileName = fileName;
    }
    public: void Initialize() {
        m_dataArray.clear();
        ScorpioReader reader = ScorpioReader(TableUtil.GetBuffer(fileName));
        int iRow = reader.ReadInt32();                      //行数
        int iColums = reader.ReadInt32();                   //列数
        int iCodeNum = reader.ReadInt32();                  //自定义类数量       
        if (!(reader.ReadString() == __TableName_FILE_MD5_CODE))    //验证文件MD5(检测结构是否改变)
            throw new Exception("文件[" + fileName + "]版本验证失败");
        int i,j,index;
        for (i = 0; i < iColums; ++i) {
            index = reader.ReadInt32();                     //读取列类型
            reader.ReadInt32();                             //读取列是否是数组
            if (index == TableUtil.CLASS_VALUE) {           //如果列是自定义类
                reader.ReadString();                        //读取类名称
                for (j = 0; j < reader.ReadInt32(); ++j){   //自定义类字段个数(全部转成基础类型以后)
                    reader.ReadInt32();                     //取出字段类型
                }
            }
        }
        for (i = 0; i < iRow; ++i) {
            __DataName * pData = MT_Data_Active::Read(reader, fileName);
            if (Contains(pData->ID()))
                throw new Exception("文件" + fileName + "有重复项 ID : " + pData.ID());
            m_dataArray.put(pData->ID(),pData);
        }
        reader.Close();
    }
    public: bool Contains(int ID) {
        return (m_dataArray.find(ID) != m_dataArray.end());
    }
};