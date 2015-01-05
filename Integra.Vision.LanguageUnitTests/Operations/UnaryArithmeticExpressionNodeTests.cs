using EQL_AritecTests.ASTNodes.Constants;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EQL_Aritec.ASTNodes.Operations.Tests
{
    [TestClass()]
    internal sealed class UnaryArithmeticExpressionNodeTests
    {
        [TestMethod()]
        public void InitTestNegate()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("-1");
            object result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<int>(-1, (int)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("-10.21");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<object>(-10.21, result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("-1m");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<object>(-1m, result, "El plan obtenido difiere del plan esperado.");
        }
    }
}
