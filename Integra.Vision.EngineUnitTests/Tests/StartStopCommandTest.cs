using Integra.Vision.Engine.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integra.Vision.EngineUnitTests.Tests
{
    [TestClass]
    public class StartStopCommandTest
    {
        [TestMethod]
        public void StartTest()
        {
            string objectName = "streamJoinTest14", objectType = "stream";
            string sentencia = string.Format("start {0} {1}", objectType, objectName);
            string sentenciaVista = string.Format("from System.UserDefinedObjects \n where Name=\"{0}\" \n select new(Id)", objectName);
            try
            {
                StartObjectCommand a = new StartObjectCommand(sentencia, null);
                a.Execute();

                // obtengo el identificador del objeto a iniciar
                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                var objectId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);
                sentenciaVista = string.Format("from System.UserDefinedObjects \n where Id.ToString()=\"{0}\" \n select new(State)", objectId);

                // verifico el cambio de estado del objeto
                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;
                valueToEvaluate = result.GetValue(0);

                Assert.AreEqual((int)UserDefinedObjectStateEnum.Started, valueToEvaluate.GetType().GetProperty("State").GetValue(valueToEvaluate));
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void StopTest()
        {
            string objectName = "streamJoinTest14", objectType = "stream";
            string sentencia = string.Format("stop {0} {1}", objectType, objectName);
            string sentenciaVista = string.Format("from System.UserDefinedObjects \n where Name=\"{0}\" \n select new(Id)", objectName);
            try
            {
                StopObjectCommand a = new StopObjectCommand(sentencia, null);
                a.Execute();

                // obtengo el identificador del objeto a detener
                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                var objectId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);
                sentenciaVista = string.Format("from System.UserDefinedObjects \n where Id.ToString()=\"{0}\" \n select new(State)", objectId);

                // verifico el cambio de estado del objeto
                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;
                valueToEvaluate = result.GetValue(0);

                Assert.AreEqual((int)UserDefinedObjectStateEnum.Stopped, valueToEvaluate.GetType().GetProperty("State").GetValue(valueToEvaluate));
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
    }
}
