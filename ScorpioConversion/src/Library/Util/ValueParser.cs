using System;
using System.Collections.Generic;
using System.Text;
using Scorpio;
using Scorpio.Compiler;
using Scorpio.Exception;
public class ValueParser : ScriptParser
{
    public ValueParser(Script script, List<Token> listTokens, string strBreviary) : base(script, listTokens, strBreviary) { }
    public IValue GetObject()
    {
        Token token = ReadToken();
        switch (token.Type)
        {
            case TokenType.Minus:
                token = ReadToken();
                if (token.Type == TokenType.Number) {
                    return new ValueString("-" + token.Lexeme.ToString());
                } else {
                    throw new Exception("负号支持数字");
                }
            case TokenType.Null:
            case TokenType.Identifier:
            case TokenType.Boolean:
            case TokenType.Number:
            case TokenType.String:
            case TokenType.SimpleString:
                return new ValueString(token.Lexeme.ToString());
            case TokenType.LeftBracket:
                UndoToken();
                return GetArray();
            default:
                throw new Exception("不支持的关键字 " + token.Type);
        }
    }
    private ValueList GetArray()
    {
        ReadLeftBracket();
        Token token = PeekToken();
        ValueList ret = new ValueList();
        while (token.Type != TokenType.RightBracket) {
            if (PeekToken().Type == TokenType.RightBracket)
                break;
            ret.values.Add(GetObject());
            token = PeekToken();
            if (token.Type == TokenType.Comma) {
                ReadComma();
            } else if (token.Type == TokenType.RightBracket) {
                break;
            } else {
                throw new ParserException("Comma ',' or right parenthesis ']' expected in array object.", token);
            }
        }
        ReadRightBracket();
        return ret;
    }
}

