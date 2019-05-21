"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Int2_1 = require("./Int2");
var Int3 = /** @class */ (function () {
    function Int3() {
        this.m_IsInvalid = false;
        this._value1 = null;
        this._value2 = null;
    }
    /*   默认值() */
    Int3.prototype.getvalue1 = function () { return this._value1; };
    Int3.prototype.ID = function () { return this._value1; };
    /*   默认值() */
    Int3.prototype.getvalue2 = function () { return this._value2; };
    Int3.prototype.GetData = function (key) {
        if ("value1" == key)
            return this._value1;
        if ("value2" == key)
            return this._value2;
        return null;
    };
    Int3.prototype.IsInvalid = function () { return this.m_IsInvalid; };
    Int3.prototype.CheckInvalid = function () {
        return false;
    };
    Int3.prototype.Set = function (value) {
        this._value1 = value._value1;
        this._value2 = value._value2;
    };
    Int3.prototype.ToString = function () {
        return "{\n            value1 : " + this._value1 + ", \n            value2 : " + this._value2 + "\n            }";
    };
    Int3.Read = function (fileName, reader) {
        var ret = new Int3();
        ret._value1 = Int2_1.Int2.Read(fileName, reader);
        ret._value2 = reader.ReadInt32();
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    };
    return Int3;
}());
exports.Int3 = Int3;
//# sourceMappingURL=Int3.js.map