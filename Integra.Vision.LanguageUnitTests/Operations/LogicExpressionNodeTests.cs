using EQL_AritecTests.ASTNodes.Constants;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace EQL_Aritec.ASTNodes.Operations.Tests
{
    [TestClass()]
    public class LogicExpressionNodeTests
    {
        [TestMethod()]
        public void LogicExpressionTests()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("@event.Message.Body.#103.[\"Campo103.1\"] == @event.Message.Body.#103.[\"Campo103.1\"] and @event.Message.Body.#103.[\"Campo103.1\"] == @event.Message.Body.#103.[\"Campo103.1\"]");
            object result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Message.Body.#103.[\"Campo103.1\"] == @event.Message.Body.#103.[\"Campo103.1\"] or @event.Message.Body.#103.[\"Campo103.1\"] == @event.Message.Body.#103.[\"Campo103.1\"]");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("not (@event.Message.Body.#103.[\"Campo103.1\"] == @event.Message.Body.#103.[\"Campo103.1\"] and @event.Message.Body.#103.[\"Campo103.1\"] == @event.Message.Body.#103.[\"Campo103.1\"])");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("not (@event.Message.Body.#103.[\"Campo103.1\"] == @event.Message.Body.#103.[\"Campo103.1\"] or @event.Message.Body.#103.[\"Campo103.1\"] == @event.Message.Body.#103.[\"Campo103.1\"])");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("true and true");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("false and false");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("true and false");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("false and true");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("true or true");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("true or false");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("false or true");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("false or false");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("not(false or false)");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("not(true or true)");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("not true or false and true");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("not (true or false and true)");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");
        }
    }
}
