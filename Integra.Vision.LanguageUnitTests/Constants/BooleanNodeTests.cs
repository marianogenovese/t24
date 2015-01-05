using EQL_AritecTests.ASTNodes.Constants;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Integra.Vision.LanguageUnitTests.Constants.Tests
{
    [TestClass()]
    internal sealed class BooleanNodeTests
    {
        [TestMethod()]
        public void InitTest()
        {
            ParserTest parser = new ParserTest();
            PlanNode plan = parser.callParserTest("true");

            ExpressionConstructor te = new ExpressionConstructor();
            object result = te.GetConstantExpressionCompiled(plan);

            Assert.AreEqual<object>(true, (bool)result, "El plan obtenido difiere del plan esperado.");
        }
    }
}
