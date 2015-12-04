using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /*
    public class Grammar
    {
        private GrammarNode rootNode;

        public Grammar(GrammarNode rootNode)
        {
            this.rootNode = rootNode;
        }
    }

    public class GrammarNode
    {
        private readonly char[] alphabet;

        public GrammarNode(char[] alphabet)
        {
            this.alphabet = alphabet;
        }


    }

    public class test
    {
        public test()
        {
            const string englishAlphabet = "abcdefghijklmnopqrstuvwxyz";
            const string russianAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

            var identiferNameAlphabet = englishAlphabet
                                      + englishAlphabet.ToUpper()
                                      + russianAlphabet
                                      + russianAlphabet.ToUpper();

            var clauseName = new Lexeme(identiferNameAlphabet, "clause name");
            var openBracket = new Lexeme("(", "'('");
            var argumentName = new Lexeme(identiferNameAlphabet, "argument");
            var comma = new Lexeme(identiferNameAlphabet, "comma");

            var grammar = new Grammar();
            grammar
                .Decide()
                .Way(clauseName)
                    .Then(openBracket)
                    .Then(argumentName)
                    .OptionalLoop(comma)
                        .Then(argumentName)
                    
                )
                    
                    
                    ,
                new GrammarNode("CommentStart", "/*"),
                );


        }
    }
  */  
}
