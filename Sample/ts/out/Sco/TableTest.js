"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TableUtil_1 = require("../ScorpioProto/Table/TableUtil");
var DataTest_1 = require("./DataTest");
var TableTest = /** @class */ (function () {
    function TableTest() {
        this.m_count = 0;
        this.m_dataArray = {};
    }
    TableTest.prototype.Initialize = function (fileName, reader) {
        var iRow = TableUtil_1.TableUtil.ReadHead(reader, fileName, "898169d7e1d0c2be013482d2c80052cc");
        for (var i = 0; i < iRow; ++i) {
            var pData = DataTest_1.DataTest.Read(fileName, reader);
            if (this.Contains(pData.ID())) {
                this.m_dataArray[pData.ID()].Set(pData);
            }
            else {
                this.m_dataArray[pData.ID()] = pData;
            }
        }
        this.m_count = Object.getOwnPropertyNames(this.m_dataArray).length;
        return this;
    };
    TableTest.prototype.GetValue = function (ID) {
        if (this.Contains(ID))
            return this.m_dataArray[ID];
        TableUtil_1.TableUtil.Warning("DataTest key is not exist " + ID);
        return null;
    };
    TableTest.prototype.Contains = function (ID) {
        return this.m_dataArray[ID] != null;
    };
    TableTest.prototype.Datas = function () {
        return this.m_dataArray;
    };
    TableTest.prototype.GetValueObject = function (ID) {
        return this.GetValue(ID);
    };
    TableTest.prototype.ContainsObject = function (ID) {
        return this.Contains(ID);
    };
    TableTest.prototype.GetDatas = function () {
        return this.Datas();
    };
    TableTest.prototype.Count = function () {
        return this.m_count;
    };
    return TableTest;
}());
exports.TableTest = TableTest;
//# sourceMappingURL=TableTest.js.map