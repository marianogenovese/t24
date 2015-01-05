using EQL_AritecTests.ASTNodes.Constants;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EQL_Aritec.ASTNodes.Constants.Tests
{
    [TestClass()]
    internal sealed class DateTimeNodeTests
    {
        [TestMethod()]
        public void InitTest()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("'01/01/2014'");
            DateTime parsedDate;
            DateTime.TryParse("01/01/2014", out parsedDate);
            object result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<DateTime>(parsedDate, (DateTime)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("'00:00:00:00'");
            TimeSpan parsedDate2;
            TimeSpan.TryParse("00:00:00:00", out parsedDate2);
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<TimeSpan>(parsedDate2, (TimeSpan)result, "El plan obtenido difiere del plan esperado.");
        }
    }
}
