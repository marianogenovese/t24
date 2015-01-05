using EQL_AritecTests.ASTNodes.Constants;
using Integra.Vision.Language;
using Integra.Vision.Language.General;
using Integra.Vision.Language.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
namespace EQL_Aritec.ASTNodes.QuerySections.Tests
{
    [TestClass()]
    internal sealed class SelectNodeTests
    {
        [TestMethod()]
        public void InitTestSelect()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode plan = parser.callParserTest("select 1 as numero");
            Dictionary<string, object> dic = te.GetSelectValues(plan, new EventObject());

            foreach (var result in dic)
            {
                var key = result.Key;
                var value = result.Value;

                if (key.Equals("numero"))
                {
                    Assert.AreEqual(1, value);
                }
                else
                {
                    Assert.Fail("Error con la proyeccion");
                }
            }

            plan = parser.callParserTest("select 1 as numero, \"hola\" as cadena, true as booleano, @event.Message.[\"Header\"].[\"MessageType\"] as objeto");
            dic = te.GetSelectValues(plan, new EventObject());

            foreach (var result in dic)
            {
                var key = result.Key;
                var value = result.Value;

                if (key.Equals("numero"))
                {
                    Assert.AreEqual(1, value);
                }
                else if (key.Equals("cadena"))
                {
                    Assert.AreEqual("hola", value);
                }
                else if (key.Equals("booleano"))
                {
                    Assert.AreEqual(true, value);
                }
                else if (key.Equals("objeto"))
                {
                    Assert.AreEqual("0100", value);
                }
                else
                {
                    Assert.Fail("Error con la proyeccion");
                }
            }
        }
    }
}
