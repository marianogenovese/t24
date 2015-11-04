using EQL_AritecTests.ASTNodes.Constants;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace EQL_Aritec.ASTNodes.Constants.Tests.Constants
{
    [TestClass()]
    internal sealed class DateFunctionNodeTests
    {
        [TestMethod()]
        public void InitTest()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("hour('01/01/2014 10:11:12 am')");
            object result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<int>(10, (int)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("minute('01/01/2014 10:11:12 am')");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<int>(11, (int)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("second('01/01/2014 10:11:12 am')");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<int>(12, (int)result, "El plan obtenido difiere del plan esperado.");
        }
    }
}
