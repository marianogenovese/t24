using EQL_AritecTests.ASTNodes.Constants;
using Integra.Vision.Language;
using Integra.Vision.Language.ASTNodes.Operations;
using Integra.Vision.Language.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace EQL_Aritec.ASTNodes.Operations.Tests
{
    [TestClass()]
    public class ComparativeExpressionNodeTests
    {
        [TestMethod()]
        public void IsNumericTypeTestTrue()
        {
            ComparativeExpressionNode comp = new ComparativeExpressionNode();

            decimal a = 1;
            bool result = comp.IsNumericType(a.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");

            float b = 1;
            result = comp.IsNumericType(b.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");

            double c = 1.1;
            result = comp.IsNumericType(c.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");

            int d = 1;
            result = comp.IsNumericType(d.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");

            Int16 e = 1;
            result = comp.IsNumericType(e.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");

            Int32 f = 1;
            result = comp.IsNumericType(f.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");
            
            Int64 g = 1;
            result = comp.IsNumericType(g.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");
            
            uint h = 1;
            result = comp.IsNumericType(h.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");
            
            long i = 1;
            result = comp.IsNumericType(i.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");
            
            ulong j = 1;
            result = comp.IsNumericType(j.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");
            
            short k = 1;
            result = comp.IsNumericType(k.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");
            
            ushort l = 1;
            result = comp.IsNumericType(l.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");
            
            byte m = 1;
            result = comp.IsNumericType(m.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");
            
            sbyte n = 1;
            result = comp.IsNumericType(n.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");

            object o = 1;
            result = comp.IsNumericType(o.GetType());
            Assert.IsTrue(result, "No tomó el numero como válido");
        }

        [TestMethod()]
        public void IsNumericTypeTestFalse()
        {
            ComparativeExpressionNode comp = new ComparativeExpressionNode();

            string a = "cadena";
            bool result = comp.IsNumericType(a.GetType());
            Assert.IsFalse(result, "Tomó una cadena como numero válido");

            bool b = true;
            result = comp.IsNumericType(b.GetType());
            Assert.IsFalse(result, "Tomó un booleano como numero válido");

            DateTime d;
            DateTime.TryParseExact("01/01/2014", "dd/MM/yyyy", null, DateTimeStyles.None, out d);
            result = comp.IsNumericType(d.GetType());
            Assert.IsFalse(result, "Tomó un datetime como numero válido");

            object e = "cadena";
            result = comp.IsNumericType(e.GetType());
            Assert.IsFalse(result, "Tomó un object cadena como numero válido");

            object f = true;
            result = comp.IsNumericType(f.GetType());
            Assert.IsFalse(result, "Tomó un object booleano como numero válido");

            DateTime g;
            DateTime.TryParseExact("01/01/2014", "dd/MM/yyyy", null, DateTimeStyles.None, out g);
            object h = g;
            result = comp.IsNumericType(h.GetType());
            Assert.IsFalse(result, "Tomó un object datetime como numero válido");
        }

        [TestMethod()]
        public void InitTestEqualBoolean()
        {
            ParserTest parser = new ParserTest();
            PlanNode plan = parser.callParserTest("true == true");

            ExpressionConstructor te = new ExpressionConstructor();
            object result = te.GetConstantExpressionCompiled(plan);

            Assert.AreEqual<bool>(true, (bool)result, "El plan obtenido difiere del plan esperado.");
        }

        [TestMethod()]
        public void InitTestEqualNumber()
        {
            ParserTest parser = new ParserTest();
            PlanNode plan = parser.callParserTest("1 == 1");
            
            ExpressionConstructor te = new ExpressionConstructor();
            object result = te.GetConstantExpressionCompiled(plan);

            Assert.AreEqual<bool>(true, (bool)result, "El plan obtenido difiere del plan esperado.");
        }

        [TestMethod()]
        public void InitTestEqualDatetime()
        {
            ParserTest parser = new ParserTest();
            PlanNode plan = parser.callParserTest("'01/01/2014' == '01/01/2014'");
            
            ExpressionConstructor te = new ExpressionConstructor();
            object result = te.GetConstantExpressionCompiled(plan);

            Assert.AreEqual<bool>(true, (bool)result, "El plan obtenido difiere del plan esperado.");
        }

        [TestMethod()]
        public void InitTestEqualString()
        {
            ParserTest parser = new ParserTest();
            PlanNode plan = parser.callParserTest("\"a\" == \"a\"");

            ExpressionConstructor te = new ExpressionConstructor();
            object result = te.GetConstantExpressionCompiled(plan);

            Assert.AreEqual<bool>(true, (bool)result, "El plan obtenido difiere del plan esperado.");
        }

        [TestMethod()]
        public void InitTestEqualNull()
        {
            ParserTest parser = new ParserTest();
            PlanNode plan = parser.callParserTest("null == null");

            ExpressionConstructor te = new ExpressionConstructor();
            object result = te.GetConstantExpressionCompiled(plan);

            Assert.AreEqual<bool>(true, (bool)result, "El plan obtenido difiere del plan esperado.");
        }

        [TestMethod()]
        public void InitTestEqualObject()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("@event.Message.Body.#103.[\"Campo103.1\"] == @event.Message.Body.#103.[\"Campo103.1\"]");
            object result = te.GetConstantExpressionCompiled(plan);
            Assert.AreEqual<bool>(true, (bool)result, "El plan obtenido difiere del plan esperado.");

            PlanNode plan2 = parser.callParserTest("@event.Message.Header.MessageType == @event.Message.Header.MessageType");
            object result2 = te.GetConstantExpressionCompiled(plan2);
            Assert.AreEqual<bool>(true, (bool)result2, "El plan obtenido difiere del plan esperado.");

            PlanNode plan3 = parser.callParserTest("@event.Message.Header.MessageType == \"0100\"");
            object result3 = te.GetConstantExpressionCompiled(plan3);
            Assert.AreEqual<bool>(true, (bool)result3, "El plan obtenido difiere del plan esperado.");

            PlanNode plan6 = parser.callParserTest("\"0100\" == @event.Message.Header.MessageType");
            Object result6 = te.GetConstantExpressionCompiled(plan6);
            Assert.AreEqual<bool>(true, (bool)result6, "El plan obtenido difiere del plan esperado.");

            PlanNode plan4 = parser.callParserTest("@event.Message.Body.TransactionAmount == 0m");
            object result4 = te.GetConstantExpressionCompiled(plan4);
            Assert.AreEqual<bool>(true, (bool)result4, "El plan obtenido difiere del plan esperado.");

            PlanNode plan5 = parser.callParserTest("0m == @event.Message.Body.TransactionAmount");
            object result5 = te.GetConstantExpressionCompiled(plan5);
            Assert.AreEqual<bool>(true, (bool)result5, "El plan obtenido difiere del plan esperado.");
        }

        [TestMethod()]
        public void InitTestNot()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("not false");
            object result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("not true");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("not @event.Message.Body.#103.[\"Campo103.1\"] == @event.Message.Body.#103.[\"Campo103.1\"]");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("not @event.Message.Body.#103.[\"Campo103.1\"] != @event.Message.Body.#103.[\"Campo103.1\"]");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");
        }

        [TestMethod()]
        public void InitTestLessThan()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("1 < 2");
            object result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("'02/03/2015' < '01/01/2014'");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("1-1 < 1");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("10.21 < 10.22");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("hour('02/03/2015') - hour('01/01/2014') < 1000");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");
        }

        [TestMethod()]
        public void InitTestLike()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("\"cadena\" like \"%ena\"");
            object result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("\"cadena\" like \"%ena2\"");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("\"cadena\" like \"cad%\"");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("\"cadena\" like \"c3ad%\"");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Message.#1.#2 like \"99999%\"");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Message.#1.#2 like \"%663\"");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("@event.Message.#1.#2 like \"%4161%\"");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");
        }

        [TestMethod()]
        public void InitTestGreaterThan()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("1 > 2");
            object result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("'02/03/2015' > '01/01/2014'");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("1-1 > 1");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("11.21 > 10.22");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("1000 > hour('02/03/2015') - hour('01/01/2014')");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");
        }

        [TestMethod()]
        public void InitTestGreaterThanOrEqual()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("1 >= 2");
            object result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("'02/03/2015' >= '01/01/2014'");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("'01/01/2014' >= '01/01/2014'");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("1-1 >= 1");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("11.21 >= 10.22");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("11.21 >= 11.21");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("1000 >= hour('02/03/2015') - hour('01/01/2014')");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("1 >= hour('02/01/2014') - hour('01/01/2014')");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");
        }

        [TestMethod()]
        public void InitTestLessThanOrEqual()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("1 <= 2");
            object result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("2 <= 2");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("'02/03/2015' <= '01/01/2014'");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsFalse((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("'01/01/2014' <= '01/01/2014'");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("1-1 <= 1");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("10.22 <= 11.21 ");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("11.21 <= 11.21");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("hour('02/03/2015') - hour('01/01/2014') <= 1000");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("1 <= hour('02/01/2014 12:11:10') - hour('01/01/2014 11:11:10')");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");

            plan = parser.callParserTest("0 <= hour('02/01/2014') - hour('01/01/2014')");
            result = te.GetConstantExpressionCompiled(plan);
            Assert.IsTrue((bool)result, "El plan obtenido difiere del plan esperado.");
        }
    }
}
