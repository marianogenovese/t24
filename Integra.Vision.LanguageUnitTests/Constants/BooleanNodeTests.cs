//-----------------------------------------------------------------------
// <copyright file="BooleanNodeTests.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.LanguageUnitTests.Constants.Tests
{
    using System;
    using Integra.Vision.Language;
    using Integra.Vision.Language.Parsers;
    using Integra.Vision.Language.Runtime;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Boolean node test class
    /// </summary>
    [TestClass]
    internal sealed class BooleanNodeTests
    {
        /// <summary>
        /// Test of boolean constant
        /// </summary>
        [TestMethod]
        public void InitTest()
        {
            ValuesParser parser = new ValuesParser("true");
            PlanNode plan = parser.Parse();
            
            ObservableConstructor te = new ObservableConstructor();
            Func<bool> result = te.Compile<bool>(plan);

            Assert.AreEqual<object>(true, result(), "El plan obtenido difiere del plan esperado.");
        }
    }
}
