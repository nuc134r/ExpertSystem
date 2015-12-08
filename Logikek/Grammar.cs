using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Logikek
{
    public class Grammar
    {
        private static Core.Grammar _grammar;

        public static Core.Grammar Get()
        {
            if (_grammar == null) CreateGrammar();
            return _grammar;
        }

        private static void CreateGrammar()
        {
            const string englishAlphabet = "abcdefghijklmnopqrstuvwxyz";
            const string russianAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

            const string identiferNameAlphabet = englishAlphabet + russianAlphabet;

            var identiferName = new GrammarToken(identiferNameAlphabet, "clause name");
            var openBracket = new GrammarToken("(");
            var closeBracket = new GrammarToken(")");
            var semicolon = new GrammarToken(";");
            var questionMark = new GrammarToken("?");
            var comma = new GrammarToken(",");
            var colon = new GrammarToken(":");

            var notOperator = new GrammarToken("not", fixedWord: true);
            var conditionOperator = new GrammarToken(new[] { "or", "and", "," });

            var commentBeginning = new GrammarToken("/*", fixedWord: true);
            var commentEnd = new GrammarToken("*/", fixedWord: true);

            var commentText = new GrammarToken("*");

            _grammar = new Core.Grammar()
                .Determine()
                    /* Commentary */
                    .Way()
                        .Then(commentBeginning)
                        .Optional()
                            .Loop()
                                .Then(commentText)
                            .LoopEnd()
                        .OptionalEnd()
                        .Then(commentEnd)
                    .WayEnd()
                    /* Clause (rule, fact or query) */
                    .Way()
                        .Then(identiferName)
                        .Then(openBracket)
                        .Then(identiferName)
                        .Optional()
                            .Loop()
                                .Then(comma)
                                .Then(identiferName)
                            .LoopEnd()
                        .OptionalEnd()
                        .Then(closeBracket)
                        .Determine()
                            /* Fact */
                            .Way()
                                .Then(semicolon)
                            .WayEnd()
                            /* Query */
                            .Way()
                                .Then(questionMark)
                            .WayEnd()
                            /* Rule */
                            .Way()
                                .Then(colon)
                                .Optional()
                                    .Then(notOperator)
                                .OptionalEnd()
                                .Then(identiferName)
                                .Then(openBracket)
                                .Then(identiferName)
                                .Optional()
                                    .Loop()
                                        .Then(comma)
                                        .Then(identiferName)
                                    .LoopEnd()
                                .OptionalEnd()
                                .Then(closeBracket)
                                .Optional()
                                    .Loop()
                                        .Then(conditionOperator)
                                        .Optional()
                                            .Then(notOperator)
                                        .OptionalEnd()
                                        .Then(identiferName)
                                        .Then(openBracket)
                                        .Then(identiferName)
                                        .Optional()
                                            .Loop()
                                                .Then(comma)
                                                .Then(identiferName)
                                            .LoopEnd()
                                        .OptionalEnd()
                                        .Then(closeBracket)
                                    .LoopEnd()
                                .OptionalEnd()
                                .Then(semicolon)
                            .WayEnd()
                        .DetermineEnd()
                .DetermineEnd();
        }
    }
}
