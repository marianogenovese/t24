using Integra.Vision.Engine.Commands.SystemViews;
using Integra.Vision.Engine.Commands.Trace;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integra.Vision.EngineUnitTests.Tests
{
    [TestClass]
    public class SetTraceCommandTest
    {
        [TestMethod]
        public void SetTraceTest()
        {
            string level = "2", objectName = "streamJoinTest14";

            string sentencia = string.Format("set trace level {0} to {1}", level, objectName);
                        
            string sentenciaVista = string.Format("from System.UserDefinedObjects \n where Name=\"{0}\" \n select new(Id)", objectName);
            try
            {
                SetTraceCommand a = new SetTraceCommand(sentencia, null);
                a.Execute();
                
                // obtengo el identificador del objeto a rastrear
                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                var objectId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);
                sentenciaVista = string.Format("from System.SetTrace \n where Id.ToString()=\"{0}\" \n select new(Id, Level, CreationDate)", objectId);

                // verifico lo guardado en System.SetTrace
                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;
                valueToEvaluate = result.GetValue(0);

                Assert.AreEqual(objectId, valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                Assert.AreEqual(level, valueToEvaluate.GetType().GetProperty("Level").GetValue(valueToEvaluate).ToString());
                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
    }
}
