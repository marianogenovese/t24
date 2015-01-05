using EQL_AritecTests.ASTNodes.Constants;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace EQL_Aritec.ASTNodes.Operations.Tests
{
    [TestClass()]
    internal sealed class ArithmeticExpressionNodeTests
    {
        [TestMethod()]
        public void InitTest()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("1 - 1");
            object result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual(0, result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("'02/03/2015' - '01/01/2014'");
            result = te.GetConstantExpressionCompiled(plan);
            TimeSpan r = (TimeSpan)result;
            Assert.AreEqual<int>(425, r.Days, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("-1 - -1");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual(0, result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("1 - -1");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual(2, result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("hour('02/01/2014') - hour('01/01/2014')");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual(0, result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("hour('02/01/2014 12:11:10') - hour('01/01/2014 11:11:10')");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual(1, result, "El plan obtenido difiere del plan esperado.");
        }
    }
}
