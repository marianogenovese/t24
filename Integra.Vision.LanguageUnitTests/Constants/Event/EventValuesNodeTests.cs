using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQL_Aritec.ASTNodes.Constants.Event;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EQL_AritecTests.ASTNodes.Constants;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;
namespace EQL_Aritec.ASTNodes.Constants.Event.Tests
{
    [TestClass()]
    internal sealed class EventValuesNodeTests
    {
        [TestMethod()]
        public void InitTest()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("@event.agent.Name");
            object result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<string>("Anonimo", (string)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Agent.MachineName");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<string>("Anonimo", (string)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Agent.timestamp");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsInstanceOfType(result, typeof(DateTime), "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.adapter.Timestamp");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsInstanceOfType(result, typeof(DateTime), "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Adapter.name");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<string>("Anonimo", (string)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Adapter.Timestamp");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsInstanceOfType(result, typeof(DateTime), "El plan obtenido difiere del plan esperado.");
        }
    }
}
