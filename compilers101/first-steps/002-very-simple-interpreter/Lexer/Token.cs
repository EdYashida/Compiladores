namespace VerySimpleInterpreter.Lexer
{
    public class Token
    {

        public ETokenType Type { get; set; }
        public int? Value{ get; set; }

        
        public Token(ETokenType type, Int32 value = 0)
        {
            Type = type;
            Value = value;
        }

        

        
    }
}