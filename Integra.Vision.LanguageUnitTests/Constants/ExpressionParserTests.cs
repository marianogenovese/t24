using Integra.Vision.Language;
using Integra.Vision.Language.Grammars;
using Irony.Interpreter;
using Irony.Parsing;
using System;
using System.Collections.Generic;

namespace EQL_AritecTests.ASTNodes.Constants
{
    internal sealed class ExpressionParserTests
    {
        public PlanNode callParserTest(string cadena)
        {
            try
            {
                ExpressionGrammar grammar = new ExpressionGrammar(true);
                LanguageData language = new LanguageData(grammar);
                Irony.Parsing.Parser parser = new Irony.Parsing.Parser(language);
                ParseTree parseTree = parser.Parse(cadena);

                if (parseTree.HasErrors())
                {
                    Console.WriteLine("La expresion es incorrecta");

                    foreach (var mensaje in parseTree.ParserMessages)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: " + mensaje.Message + " en la linea " + mensaje.Location.Line + " columna " + mensaje.Location.Column);
                    }

                    return null;
                }
                else
                {
                    ScriptApp app = new ScriptApp(new ExpressionLanguageRuntime());
                    PlanNode result = (PlanNode)app.Evaluate(parseTree);
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
