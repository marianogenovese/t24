using EQL_AritecTests.ASTNodes.Constants;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EQL_Aritec.ASTNodes.Constants.Tests
{
    [TestClass()]
    internal sealed class StringNodeTests
    {
        [TestMethod()]
        public void InitTest()
        {
            ParserTest parser = new ParserTest();
            PlanNode plan = parser.callParserTest("\"cadena\"");

            ExpressionConstructor te = new ExpressionConstructor();
            object result = te.GetConstantExpressionCompiled(plan);

            Assert.AreEqual<string>("cadena", (string)result, "El plan obtenido difiere del plan esperado.");
        }
    }
}
