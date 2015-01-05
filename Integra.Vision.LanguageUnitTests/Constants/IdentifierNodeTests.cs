using EQL_AritecTests.ASTNodes.Constants;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EQL_Aritec.ASTNodes.Constants.Tests
{
    [TestClass()]
    internal sealed class IdentifierNodeTests
    {
        [TestMethod()]
        public void InitTest()
        {
            ParserTest parser = new ParserTest();
            PlanNode plan = parser.callParserTest("identificador");

            ExpressionConstructor te = new ExpressionConstructor();
            object result = te.GetConstantExpressionCompiled(plan);

            Assert.AreEqual<string>("identificador", (string)result, "El plan obtenido difiere del plan esperado.");
        }
    }
}
