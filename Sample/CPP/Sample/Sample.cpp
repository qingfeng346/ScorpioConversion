// Sample.cpp : 定义控制台应用程序的入口点。
//
#include "Code/TableManager.h"
#include "Table/TableUtil.h"
#include <iostream>
#include <fstream>
#include <stdio.h>
#include <stdlib.h>
#include "String.h"
using namespace ScorpioProtoTest;
class TableDis : public ITableUtil
{
public:
	char * GetBuffer(const char * fileName) {
		String str = "E:/ScorpioConversion/Sample/CPP/Sample/Data/";
		str += fileName;
		str += ".data";
		std::ifstream file;
		file.open(str.GetBuffer());
		file.seekg(0, std::ios::end);
		std::streampos length = file.tellg();
		file.seekg(0, std::ios::beg);
		char * buffer = new char[length];
		file.read(buffer, length);
		file.close();
		return buffer;
	}
	void Warning(const char * message) {
		printf(message);
	}
};
int main()
{
	
	TableUtil::SetTableUtil(new TableDis());
	TableManager * tableManager = new TableManager();
	printf("%s\n", tableManager->GetTest()->GetElement(10000)->getTestString());
	system("pause");
	//printf()
    return 0;
}

