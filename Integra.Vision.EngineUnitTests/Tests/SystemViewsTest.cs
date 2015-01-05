using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Integra.Vision.Engine.Commands.SystemViews;

namespace Integra.Vision.EngineUnitTests.Tests
{
    [TestClass]
    public class SystemViewsTest
    {
        [TestMethod]
        public void TestSystemViews()
        {
            string sentencia = "from System_Assemblies \n where LocalPath = \"alguna_ruta\" \n select new (Id, Name)";
            try
            {
                SystemQueriesCommand a = new SystemQueriesCommand(sentencia, null);
                a.Execute();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
    }
}
