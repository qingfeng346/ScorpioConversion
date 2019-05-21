"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ScorpioUtil_1 = require("../ScorpioProto/Commons/ScorpioUtil");
var Int3_1 = require("./Int3");
var DataTest = /** @class */ (function () {
    function DataTest() {
        this.m_IsInvalid = false;
        this._TestID = null;
        this._testEnum = null;
        this._TestDate = null;
    }
    /*   默认值() */
    DataTest.prototype.getTestID = function () { return this._TestID; };
    DataTest.prototype.ID = function () { return this._TestID; };
    /*   默认值() */
    DataTest.prototype.gettestEnum = function () { return this._testEnum; };
    /*   默认值() */
    DataTest.prototype.getTestDate = function () { return this._TestDate; };
    DataTest.prototype.GetData = function (key) {
        if ("TestID" == key)
            return this._TestID;
        if ("testEnum" == key)
            return this._testEnum;
        if ("TestDate" == key)
            return this._TestDate;
        return null;
    };
    DataTest.prototype.IsInvalid = function () { return this.m_IsInvalid; };
    DataTest.prototype.CheckInvalid = function () {
        return false;
    };
    DataTest.prototype.Set = function (value) {
        this._TestID = value._TestID;
        this._testEnum = value._testEnum;
        this._TestDate = value._TestDate;
    };
    DataTest.prototype.ToString = function () {
        return "{\n            TestID : " + this._TestID + ", \n            testEnum : " + this._testEnum + ", \n            TestDate : " + ScorpioUtil_1.ScorpioUtil.ToString(this._TestDate) + "\n            }";
    };
    DataTest.Read = function (fileName, reader) {
        var ret = new DataTest();
        ret._TestID = reader.ReadInt32();
        ret._testEnum = reader.ReadInt32();
        {
            var list = new Array();
            var number = reader.ReadInt32();
            for (var i = 0; i < number; ++i) {
                list.push(Int3_1.Int3.Read(fileName, reader));
            }
            ret._TestDate = list;
        }
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    };
    return DataTest;
}());
exports.DataTest = DataTest;
//# sourceMappingURL=DataTest.js.map