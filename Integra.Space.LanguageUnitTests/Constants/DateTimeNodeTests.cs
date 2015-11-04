using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Integra.Vision.Language.Parsers;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;

namespace Integra.Space.LanguageUnitTests.Constants
{
    [TestClass]
    public class DateTimeNodeTests
    {
        [TestMethod]
        public void ConstantDateTimeValue()
        {
            ValuesParser parser = new ValuesParser("'01/01/2014'");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<DateTime> result = te.Compile<DateTime>(plan);

            DateTime parsedDate;
            DateTime.TryParse("01/01/2014", out parsedDate);

            Assert.AreEqual<DateTime>(parsedDate, result(), "El plan obtenido difiere del plan esperado.");
        }

        [TestMethod]
        public void ConstantTimeSpanValue()
        {
            ValuesParser parser = new ValuesParser("'00:00:00:01'");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<TimeSpan> result = te.Compile<TimeSpan>(plan);

            TimeSpan parsedDate;
            TimeSpan.TryParse("00:00:00:01", out parsedDate);

            Assert.AreEqual<TimeSpan>(parsedDate, result(), "El plan obtenido difiere del plan esperado.");
        }
    }
}
