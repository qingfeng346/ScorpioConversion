"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ScorpioUtil = /** @class */ (function () {
    function ScorpioUtil() {
    }
    ScorpioUtil.ToString = function (list) {
        if (list == null || list.length == 0) {
            return "[]";
        }
        var length = list.length;
        var str = "[";
        for (var i = 0; i < length; ++i) {
            str += list[i];
            if (i != length - 1) {
                str += ",";
            }
        }
        str += "[";
        return str;
    };
    return ScorpioUtil;
}());
exports.ScorpioUtil = ScorpioUtil;
//# sourceMappingURL=ScorpioUtil.js.map