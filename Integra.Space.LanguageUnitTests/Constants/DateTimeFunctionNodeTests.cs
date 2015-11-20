﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            ExpressionParser parser = new ExpressionParser("year('02/01/2014 10:11:12 am')");
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
            ExpressionParser parser = new ExpressionParser("month('02/01/2014 10:11:12 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(1, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el día de la cadena especificada
        /// </summary>
        [TestMethod]
        public void DayFunction1()
        {
            ExpressionParser parser = new ExpressionParser("day('02/01/2014 10:11:12 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(2, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el día de la cadena especificada
        /// </summary>
        [TestMethod]
        public void DayFunction2()
        {
            ExpressionParser parser = new ExpressionParser("day('1.02:00:30')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(1, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene la hora de la cadena especificada
        /// </summary>
        [TestMethod]
        public void HourFunction1()
        {
            ExpressionParser parser = new ExpressionParser("hour('01/01/2014 10:11:12 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(10, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene la hora de la cadena especificada
        /// </summary>
        [TestMethod]
        public void HourFunction2()
        {
            ExpressionParser parser = new ExpressionParser("hour('10:11:12 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(10, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene la hora de la cadena especificada
        /// </summary>
        [TestMethod]
        public void HourFunction3()
        {
            ExpressionParser parser = new ExpressionParser("hour('1.02:00:30')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(2, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el minuto de la cadena especificada
        /// </summary>
        [TestMethod]
        public void MinuteFunction1()
        {
            ExpressionParser parser = new ExpressionParser("minute('01/01/2014 10:11:12 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(11, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el minuto de la cadena especificada
        /// </summary>
        [TestMethod]
        public void MinuteFunction2()
        {
            ExpressionParser parser = new ExpressionParser("minute('1.02:10:30')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(10, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el segundo de la cadena especificada
        /// </summary>
        [TestMethod]
        public void SecondFunction1()
        {
            ExpressionParser parser = new ExpressionParser("second('01/01/2014 10:11:12 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(12, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el segundo de la cadena especificada
        /// </summary>
        [TestMethod]
        public void SecondFunction2()
        {
            ExpressionParser parser = new ExpressionParser("second('10:11:12 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(12, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el segundo de la cadena especificada
        /// </summary>
        [TestMethod]
        public void SecondFunction3()
        {
            ExpressionParser parser = new ExpressionParser("second('1.02:10:30')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(30, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el segundo de la cadena especificada
        /// </summary>
        [TestMethod]
        public void MillisecondFunction1()
        {
            ExpressionParser parser = new ExpressionParser("millisecond('01/01/2014 10:11:12.1 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(100, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el segundo de la cadena especificada
        /// </summary>
        [TestMethod]
        public void MillisecondFunction2()
        {
            ExpressionParser parser = new ExpressionParser("millisecond('10:11:12.1 am')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(100, result(), "El plan obtenido difiere del plan esperado.");
        }

        /// <summary>
        /// Obtiene el segundo de la cadena especificada
        /// </summary>
        [TestMethod]
        public void MillisecondFunction3()
        {
            ExpressionParser parser = new ExpressionParser("millisecond('1.02:10:30.6')");
            PlanNode plan = parser.Parse();

            ObservableConstructor te = new ObservableConstructor();
            Func<int> result = te.Compile<int>(plan);

            Assert.AreEqual<int>(600, result(), "El plan obtenido difiere del plan esperado.");
        }
    }
}
