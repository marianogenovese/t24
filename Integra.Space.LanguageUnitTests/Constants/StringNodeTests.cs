using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Integra.Vision.Language.Parsers;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;

namespace Integra.Space.LanguageUnitTests.Constants
{
    [TestClass]
    public class StringNodeTests
    {
        [TestMethod]
        public void StringConstant()
        {
            ValuesParser parser = new ValuesParser("\"hello world! :D\"");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<string> result = te.Compile<string>(plan);

            Assert.AreEqual<string>("hello world! :D", result(), "El plan obtenido difiere del plan esperado.");
        }
    }
}
