using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Integra.Vision.Language.Parsers;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;

namespace Integra.Space.LanguageUnitTests.Constants
{
    [TestClass]
    public class NullNodeTests
    {
        [TestMethod]
        public void ConstantNull()
        {
            ValuesParser parser = new ValuesParser("null");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<object> result = te.Compile<object>(plan);

            Assert.AreEqual<object>(null, result(), "El plan obtenido difiere del plan esperado.");
        }
    }
}
