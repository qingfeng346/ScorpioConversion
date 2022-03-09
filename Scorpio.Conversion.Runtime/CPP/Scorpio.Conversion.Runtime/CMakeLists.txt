# 项目特定的逻辑。
#
cmake_minimum_required (VERSION 3.8)

project ("Scorpio.Conversion.Runtime")

# 将源代码添加到此项目的可执行文件。
# add_executable (Scorpio.Conversion.Runtime "ConversionUtil.cpp" "ConversionUtil.h" "DefaultReader.cpp" "DefaultReader.h" "IReader.cpp" "IReader.h" "IData.h" "ITable.h" "main.cpp")

add_library(Scorpio.Conversion.Runtime STATIC "ConversionUtil.cpp" "ConversionUtil.h" "DefaultReader.cpp" "DefaultReader.h" "IReader.cpp" "IReader.h" "IData.h" "ITable.h")

# TODO: 如有需要，请添加测试并安装目标。
