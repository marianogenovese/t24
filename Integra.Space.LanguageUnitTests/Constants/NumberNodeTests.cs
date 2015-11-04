using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Integra.Vision.Language.Parsers;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;

namespace Integra.Space.LanguageUnitTests.Constants
{
    [TestClass]
    public class NumberNodeTests
    {
        [TestMethod]
        public void ConstantIntegerValue()
        {
            ValuesParser parser = new ValuesParser("10");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(10, result(), "El plan obtenido difiere del plan esperado.");
        }
    }
}
