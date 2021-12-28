﻿using System;

public interface IReader {
    bool ReadBool();
    sbyte ReadInt8();
    byte ReadUInt8();
    short ReadInt16();
    ushort ReadUInt16();
    int ReadInt32();
    uint ReadUInt32();
    long ReadInt64();
    ulong ReadUInt64();
    float ReadFloat();
    double ReadDouble();
    string ReadString();
    string ReadL10N();
    DateTime ReadDateTime();
    byte[] ReadBytes();
}
