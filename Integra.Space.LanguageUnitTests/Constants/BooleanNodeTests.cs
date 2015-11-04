using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Integra.Vision.Language.Parsers;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;

namespace Integra.Space.LanguageUnitTests.Constants
{
    [TestClass]
    public class BooleanNodeTests
    {
        [TestMethod]
        public void ConstantBooleanTrue()
        {
            ValuesParser parser = new ValuesParser("true");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<bool> result = te.Compile<bool>(plan);

            Assert.AreEqual<object>(true, result(), "El plan obtenido difiere del plan esperado.");
        }

        [TestMethod]
        public void ConstantBooleanFalse()
        {
            ValuesParser parser = new ValuesParser("false");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<bool> result = te.Compile<bool>(plan);

            Assert.AreEqual<object>(false, result(), "El plan obtenido difiere del plan esperado.");
        }
    }
}
