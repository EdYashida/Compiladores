namespace VerySimpleInterpreter.Lexer
{
    public class BasicLexer
    {
                
        public Int32 Line{get;protected set;}   //divide arquivo em linhas e colunas
        public Int32 Column{get;protected set;}
                
        //public string Filename {get; protected set;} //especifica arquivo

        public SymbolTable SymbolTable {get; protected set;}        //tabela de simbolos

        
        private char? _peek;        //verifica proximo token sem consumi-lo
        private StreamReader _reader;            //le

        public BasicLexer(StreamReader filename, SymbolTable? st = null)  //construtor
        {
             if (st == null)
                st = new SymbolTable();
            SymbolTable = st;                              //se parametro st for nulo, cria nova tabela de simbolos
                                           
            
 
            this._reader=filename;           //Filename serve apenas para parassar arquivo pro leitor, 
                                             //pode-se fazer o leitor receber direto

            Column = Line = 1; //inicializa leitura na primeira posicao do arquivo
        }

        public Token GetNextToken() //consome proximo token
        {
            if (_reader.EndOfStream)                //se chegar no fim do arquivo
                return new Token(ETokenType.EOF);      //retorna sinalizando isso

            while (_peek == null || _peek== ' ' ||  _peek== '\t' || _peek== '\r')    //continua lendo ate econtrar um token "valido"
            {
                _peek = NextChar();
            }
            
            //possiveis caracteres de serem encontrados ao olhar o proximo
            
            switch (_peek) 
            {
                case '+': _peek = null; return new Token(ETokenType.SUM);
                case '-': _peek = null; return new Token(ETokenType.SUB);
                case '*': _peek = null; return new Token(ETokenType.MULT);
                case '/': _peek = null; return new Token(ETokenType.DIV);
                case '(': _peek = null; return new Token(ETokenType.OE);
                case ')': _peek = null; return new Token(ETokenType.CE);
                case '=': _peek = null; return new Token(ETokenType.AT);
                case '\n':
                    _peek = null; 
                    Column = 1;
                    Line++;
                    return new Token(ETokenType.EOL);                

            }

            if (_peek == '$')  //$[a-z]+
            {
                var varName = "";                
                do {
                    _peek = NextChar();
                    if (Char.IsLetter(_peek.Value))
                        varName += _peek;      //concatena proximo caractere ao nome da variavel
                } while (Char.IsLetter(_peek.Value));  //roda enquanto proximo caractere for letra
                var key = SymbolTable.Put(varName);     //associa token com tabela de simbolos e passa chave de acesso
                return new Token(ETokenType.VAR, key);
            }

            if (_peek == 'r')  //'read'
            {
                if (testSufix("ead"))  //veririfica oque sucede o 'r'
                    return new Token(ETokenType.INPUT);
            }

            if (_peek == 'w')  //'write'
            {
                if (testSufix("rite"))  //veririfica oque sucede o 'w'
                    return new Token(ETokenType.OUTPUT);
            }

            
            if (Char.IsDigit(_peek.Value))  //[0-9]+
            {
                var value = 0;                
                do {

                    value = value * 10 + GetValue(_peek);
                    _peek = NextChar(); //passa pro proximo caractere só depois do primeiro ter seu valor avaliado
                } while (Char.IsDigit(_peek.Value));        //enquanto proximo caractere for numerico

                return new Token(ETokenType.NUM, value);
            }        
            
            return new Token(ETokenType.ERR); //se nao tiver sido nenhum outro token
        }

        public char NextChar()  //le proximo caractere
        {
            Column++;   //atualiza posicao de leitura na linha atual
            Char c = '\0';
            if (!_reader.EndOfStream)  //se for diferente do fim do arquivo
                c = (Char) _reader.Read(); //caracactere c se torna oque foi lido
            return c;
        }

        private bool testSufix(String suffix){  //verifica se "veracidade" do token checando o que sucede a letra que o inicia
            var res = true;
            suffix.ToCharArray().ToList().ForEach(c => { //itera sobre cada caractere da sequencia
                _peek = NextChar();
                if (_peek != c){  //se caractere lido for diferente do atual lido
                    res = false;
                    return;  //sai do loop
                }
            });
            _peek = null;
            return res;  //o sufixo está corretor ou não?
        }

        private int GetValue(Char? c){
            if (c == '0') return 0;
            if (c == '1') return 1;
            if (c == '2') return 2;
            if (c == '3') return 3;
            if (c == '4') return 4;
            if (c == '5') return 5;
            if (c == '6') return 6;
            if (c == '7') return 7;
            if (c == '8') return 8;
            if (c == '9') return 9;

            return 0;
        }

    }
}