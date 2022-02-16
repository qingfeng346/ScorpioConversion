
from Int2 import *
from Int3 import *
from TableTest import *
from DataTest import *
from TableTestCsv import *
from DataTestCsv import *
from TableSpawn import *
from DataSpawn import *
class TableManager:
    _tableTest = None
    def getTest(this):
        if this._tableTest == None:
            reader = this.GetReader("Test")
            this._tableTest = TableTest().Initialize("Test", reader)
            reader.Close()
        return this._tableTest

    _tableTestCsv = None
    def getTestCsv(this):
        if this._tableTestCsv == None:
            reader = this.GetReader("TestCsv")
            this._tableTestCsv = TableTestCsv().Initialize("TestCsv", reader)
            reader.Close()
        return this._tableTestCsv

    _tableTest1 = None
    def getSpawnTest1(this):
        if this._tableTest1 == None:
            reader = this.GetReader("Test1")
            this._tableTest1 = TableSpawn().Initialize("Test1", reader)
            reader.Close()
        return this._tableTest1

    _tableTest2 = None
    def getSpawnTest2(this):
        if this._tableTest2 == None:
            reader = this.GetReader("Test2")
            this._tableTest2 = TableSpawn().Initialize("Test2", reader)
            reader.Close()
        return this._tableTest2

    def getSpawn(this, name):
        if name == "Test1":
            return this.getSpawnTest1()
        if name == "Test2":
            return this.getSpawnTest2()
        return None

