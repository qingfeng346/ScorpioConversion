import sys
sys.path.append('Example/src')
sys.path.append('Scorpio.Conversion.Runtime')
from TableManager import *
from DefaultReader import *
def GetReader(this, fileName):
    return DefaultReader(open("../" + fileName + ".data", "rb"))
TableManager.GetReader = GetReader
def main():
    tttt = TableManager()
    for key,value in tttt.getTest().Datas().items():
        print(value)
main()
