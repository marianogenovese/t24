using EQL_AritecTests.ASTNodes.Constants;
using Integra.Vision.Language;
using Integra.Vision.Language.General;
using Integra.Vision.Language.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EQL_Aritec.ASTNodes.Constants.Object.Tests
{
    [TestClass()]
    internal sealed class ObjectWithSuffixTests
    {
        [TestMethod()]
        public void InitTest()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("@event.Message.[\"Header\"].[\"MessageType\"]");
            object result = te.GetExpressionCompiled(plan, new EventObject());
            Assert.AreEqual<string>("0100", (string)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Message.Header.MessageType");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<string>("0100", (string)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Message.#0.#0");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<string>("0100", (string)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Message.#0.MessageType");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<string>("0100", (string)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Message.[\"Header\"].#0");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<string>("0100", (string)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Message.[\"Header\"].MessageType");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<string>("0100", (string)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Message.Header.[\"MessageType\"]");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<string>("0100", (string)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Message.Body.#103.[\"Campo103.1\"]");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<string>("Dato del 103.1", (string)result, "El plan obtenido difiere del plan esperado.");
        }
    }
}
