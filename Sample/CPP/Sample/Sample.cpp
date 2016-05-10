// Sample.cpp : 定义控制台应用程序的入口点。
//
#include "Code/TableManager.h"
#include "Code/Msg_C2G_Test.h"
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
	Msg_C2G_Test * message = new Msg_C2G_Test();
	message->setValue1(100);
	message->setValue2("1234567890");
	char * buf = message->Serialize();
	delete message;
	Msg_C2G_Test * msg = Msg_C2G_Test::Deserialize(buf);
	printf("%d\n", msg->getValue1());
	printf("%s\n", msg->getValue2());
	printf("%d\n", msg->HasSign(3));
	system("pause");
	//printf()
    return 0;
}

