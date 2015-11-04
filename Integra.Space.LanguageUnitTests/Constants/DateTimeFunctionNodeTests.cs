using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Integra.Vision.Language.Parsers;
using Integra.Vision.Language;
using Integra.Vision.Language.Runtime;

namespace Integra.Space.LanguageUnitTests.Constants
{
    [TestClass]
    public class DateTimeFunctionNodeTests
    {
        /// <summary>
        /// Obtiene el segundo de la cadena especificada
        /// </summary>
        [TestMethod]
        public void YearFunction()
        {
            ValuesParser parser = new ValuesParser("year('02/01/2014 10:11:12 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(2014, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el segundo de la cadena especificada
        /// </summary>
        [TestMethod]
        public void MonthFunction()
        {
            ValuesParser parser = new ValuesParser("month('02/01/2014 10:11:12 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(1, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el segundo de la cadena especificada
        /// </summary>
        [TestMethod]
        public void DayFunction()
        {
            ValuesParser parser = new ValuesParser("day('02/01/2014 10:11:12 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(2, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene la hora de la cadena especificada
        /// </summary>
        [TestMethod]
        public void HourFunction()
        {
            ValuesParser parser = new ValuesParser("hour('01/01/2014 10:11:12 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(10, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el minuto de la cadena especificada
        /// </summary>
        [TestMethod]
        public void MinuteFunction()
        {
            ValuesParser parser = new ValuesParser("minute('01/01/2014 10:11:12 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(11, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el segundo de la cadena especificada
        /// </summary>
        [TestMethod]
        public void SecondFunction()
        {
            ValuesParser parser = new ValuesParser("second('01/01/2014 10:11:12 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(12, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el segundo de la cadena especificada
        /// </summary>
        [TestMethod]
        public void MillisecondFunction()
        {
            ValuesParser parser = new ValuesParser("millisecond('01/01/2014 10:11:12.1 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(100, result(), "El plan obtenido difiere del plan esperado.");
        }
    }
}
