"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Int2 = /** @class */ (function () {
    function Int2() {
        this.m_IsInvalid = false;
        this._value1 = null;
        this._value2 = null;
    }
    /*   默认值() */
    Int2.prototype.getvalue1 = function () { return this._value1; };
    Int2.prototype.ID = function () { return this._value1; };
    /*   默认值() */
    Int2.prototype.getvalue2 = function () { return this._value2; };
    Int2.prototype.GetData = function (key) {
        if ("value1" == key)
            return this._value1;
        if ("value2" == key)
            return this._value2;
        return null;
    };
    Int2.prototype.IsInvalid = function () { return this.m_IsInvalid; };
    Int2.prototype.CheckInvalid = function () {
        return false;
    };
    Int2.prototype.Set = function (value) {
        this._value1 = value._value1;
        this._value2 = value._value2;
    };
    Int2.prototype.ToString = function () {
        return "{\n            value1 : " + this._value1 + ", \n            value2 : " + this._value2 + "\n            }";
    };
    Int2.Read = function (fileName, reader) {
        var ret = new Int2();
        ret._value1 = reader.ReadInt32();
        ret._value2 = reader.ReadInt32();
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    };
    return Int2;
}());
exports.Int2 = Int2;
//# sourceMappingURL=Int2.js.map