export interface IScorpioReader {
    ReadBool():boolean;
    ReadInt8():number;
    ReadInt16():number;
    ReadInt32():number;
    ReadInt64():any;
    ReadFloat():number;
    ReadDouble():number;
    ReadString():string;
    ReadDateTime():Date;
}