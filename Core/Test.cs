using System.CodeDom;

namespace Core
{
    public class Test
    {
        private readonly Grammar tree;

        public GrammarNode Tree => tree.GrammarTree;

        public Test()
        {
            const string englishAlphabet = "abcdefghijklmnopqrstuvwxyz";
            const string russianAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

            var identiferNameAlphabet = englishAlphabet
                                      + englishAlphabet.ToUpper()
                                      + russianAlphabet
                                      + russianAlphabet.ToUpper();

            var identiferName = new Lexeme(identiferNameAlphabet, "clause name");
            var openBracket   = new Lexeme("(");
            var closeBracket  = new Lexeme(")");
            var semicolon     = new Lexeme(";");
            var questionMark  = new Lexeme("?");
            var comma         = new Lexeme(",");
            var colon         = new Lexeme(":");

            var notOperator       = new Lexeme("not", constant: true);
            var conditionOperator = new Lexeme(new[] {"or", "and", ","});

            var commentBeginning  = new Lexeme("/*", constant: true);
            var commentEnd        = new Lexeme("*/", constant: true);

            var commentText = new Lexeme("*");

            tree = new Grammar()
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
                        .DetermineEnd().
                DetermineEnd();
        }
    }
}