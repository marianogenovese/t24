using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Integra.Vision.Language;
using System.Collections.Generic;
using System.Linq;
using Integra.Messaging;

namespace Integra.Vision.LanguageUnitTests
{
    [TestClass]
    public class LangugeTests
    {

        [TestMethod]
        public void CountCommandsTest()
        {
            string sentencia = "create assembly assemblyTest from \"alguna_ruta\""
                                + " alter adapter adapterTest15 for output as string @p1 = \"cadena\" int @p2 = 1 reference assemblyTest"
                                + " drop user userTest"
                                + " grant stream streamJoinTest to user userTest"
                                + " deny stream streamJoinTest to user userTest"
                                + " revoke stream streamJoinTest to user userTest"
                                + " set trace level 2 to adapter"
                                + " start adapter adapterTest"
                                + " stop adapter adapterTest"
                                + " from System_Assemblies \n where LocalPath = \"alguna_ruta\" \n select new (Id, Name)";

            int cantCommands = 10;

            try
            {
                Integra.Vision.Language.Grammars.EQLGrammar grammar = new Integra.Vision.Language.Grammars.EQLGrammar();
                Irony.Parsing.LanguageData language = new Irony.Parsing.LanguageData(grammar);
                Irony.Parsing.Parser parser = new Irony.Parsing.Parser(language);
                Irony.Parsing.ParseTree parseTree = parser.Parse(sentencia);
                if (parseTree.HasErrors())
                {
                    string errors = "";
                    foreach (var parserMessage in parseTree.ParserMessages)
                    {
                        errors += parserMessage.Message + parserMessage.Location.Line + parserMessage.Location.Column;
                    }
                    Assert.Fail("Error de parseo: " + errors);
                }
                else
                {
                    Irony.Interpreter.ScriptApp app = new Irony.Interpreter.ScriptApp(new Integra.Vision.Language.Grammars.EQLLanguageRuntime());
                    List<PlanNode> planNodeList = new List<PlanNode>();
                    planNodeList = (List<PlanNode>)app.Evaluate(parseTree);

                    if(cantCommands != planNodeList.Count)
                    {
                        Assert.Fail("La cantidad de nodos devueltos no coincide con la cantidad esperada");
                    }
                }
            }
            catch (System.Exception e)
            {
                Assert.Fail("Error: " + e.ToString());
            }
        }
    }
}
