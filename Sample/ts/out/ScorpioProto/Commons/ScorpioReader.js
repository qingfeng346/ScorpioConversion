"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
var buffer_1 = require("buffer");
var long_1 = __importDefault(require("long"));
var ScorpioReader = /** @class */ (function () {
    function ScorpioReader(buf) {
        this.offset = 0;
        this.offset = 0;
        this.buffer = buffer_1.Buffer.from(buf);
    }
    ScorpioReader.prototype.ReadBool = function () {
        return this.ReadInt8() == 1;
    };
    ScorpioReader.prototype.ReadInt8 = function () {
        var ret = this.buffer.readInt8(this.offset);
        this.offset += 1;
        return ret;
    };
    ScorpioReader.prototype.ReadInt16 = function () {
        var ret = this.buffer.readInt16LE(this.offset);
        this.offset += 2;
        return ret;
    };
    ScorpioReader.prototype.ReadInt32 = function () {
        var ret = this.buffer.readInt32LE(this.offset);
        this.offset += 4;
        return ret;
    };
    ScorpioReader.prototype.ReadInt64 = function () {
        var low = this.ReadInt32();
        var high = this.ReadInt32();
        return long_1.default.fromBits(low, high, false);
    };
    ScorpioReader.prototype.ReadFloat = function () {
        var ret = this.buffer.readFloatLE(this.offset);
        this.offset += 4;
        return ret;
    };
    ScorpioReader.prototype.ReadDouble = function () {
        var ret = this.buffer.readDoubleLE(this.offset);
        this.offset += 8;
        return ret;
    };
    ScorpioReader.prototype.ReadString = function () {
        var length = this.ReadInt32();
        var start = this.offset;
        var end = start + length;
        var ret = this.buffer.toString("utf8", start, end);
        this.offset += length;
        return ret;
    };
    ScorpioReader.prototype.ReadDateTime = function () {
        var time = this.ReadInt64();
        return new Date(time.toNumber());
    };
    return ScorpioReader;
}());
exports.ScorpioReader = ScorpioReader;
//# sourceMappingURL=ScorpioReader.js.map