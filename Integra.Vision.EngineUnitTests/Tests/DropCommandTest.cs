using Integra.Vision.Engine.Commands.Drop.DropAdapter;
using Integra.Vision.Engine.Commands.Drop.DropAssembly;
using Integra.Vision.Engine.Commands.Drop.DropRole;
using Integra.Vision.Engine.Commands.Drop.DropSource;
using Integra.Vision.Engine.Commands.Drop.DropStream;
using Integra.Vision.Engine.Commands.Drop.DropTrigger;
using Integra.Vision.Engine.Commands.Drop.DropUser;
using Integra.Vision.Engine.Commands.SystemViews;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integra.Vision.EngineUnitTests
{
    [TestClass]
    public class DropCommandTest
    {
        [TestMethod]
        public void TestDropUser()
        {
            string objectName = "userTest1";
            string sentencia = string.Format("drop user {0}", objectName);
            string sentenciaVista = string.Format("from System.Users \n where Sid=\"{0}\" \n select new(Id)", objectName);
            try
            {
                DropUserCommand a = new DropUserCommand(sentencia, null);
                a.Execute();

                // verifico si fue eliminado el objeto
                if(!this.wasRemoved(sentenciaVista))
                {
                    Assert.Fail("No eliminó el objeto");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestDropRole()
        {
            string objectName = "roleTest";
            string sentencia = string.Format("drop role {0}", objectName);
            string sentenciaVista = string.Format("from System.Roles \n where Name=\"{0}\" \n select new(Id)", objectName);
            try
            {
                DropRoleCommand a = new DropRoleCommand(sentencia, null);
                a.Execute();

                // verifico si fue eliminado el objeto
                if (!this.wasRemoved(sentenciaVista))
                {
                    Assert.Fail("No eliminó el objeto");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestDropTrigger()
        {
            string objectName = "TriggerTestWithDurationTime1";
            string sentencia = string.Format("drop trigger {0}", objectName);
            string sentenciaVista = string.Format("from System.Triggers \n where Name=\"{0}\" \n select new(Id)", objectName);
            try
            {
                DropTriggerCommand a = new DropTriggerCommand(sentencia, null);
                a.Execute();

                // verifico si fue eliminado el objeto
                if (!this.wasRemoved(sentenciaVista))
                {
                    Assert.Fail("No eliminó el objeto");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestDropStream()
        {
            string objectName = "streamJoinTest10";
            string sentencia = string.Format("drop stream {0}", objectName);
            string sentenciaVista = string.Format("from System.Streams \n where Name=\"{0}\" \n select new(Id)", objectName);
            try
            {
                DropStreamCommand a = new DropStreamCommand(sentencia, null);
                a.Execute();

                // verifico si fue eliminado el objeto
                if (!this.wasRemoved(sentenciaVista))
                {
                    Assert.Fail("No eliminó el objeto");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestDropSource()
        {
            string objectName = "sourceTest";
            string sentencia = string.Format("drop source {0}", objectName);
            string sentenciaVista = string.Format("from System.Sources \n where Name=\"{0}\" \n select new(Id)", objectName);
            try
            {
                DropSourceCommand a = new DropSourceCommand(sentencia, null);
                a.Execute();

                // verifico si fue eliminado el objeto
                if (!this.wasRemoved(sentenciaVista))
                {
                    Assert.Fail("No eliminó el objeto");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestDropAdapter()
        {
            string objectName = "outputAdapterTest";
            string sentencia = string.Format("drop adapter {0}", objectName);
            string sentenciaVista = string.Format("from System.Adapters \n where Name=\"{0}\" \n select new(Id)", objectName);
            try
            {
                DropAdapterCommand a = new DropAdapterCommand(sentencia, null);
                a.Execute();

                // verifico si fue eliminado el objeto
                if (!this.wasRemoved(sentenciaVista))
                {
                    Assert.Fail("No eliminó el objeto");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestDropAssembly()
        {
            string objectName = "assemblyTest";
            string sentencia = string.Format("drop assembly {0}", objectName);
            string sentenciaVista = string.Format("from System.Assemblies \n where Name=\"{0}\" \n select new(Id)", objectName);
            try
            {
                DropAssemblyCommand a = new DropAssemblyCommand(sentencia, null);
                a.Execute();

                // verifico si fue eliminado el objeto
                if (!this.wasRemoved(sentenciaVista))
                {
                    Assert.Fail("No eliminó el objeto");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        /// <summary>
        /// Checks whether the object was removed
        /// </summary>
        /// <param name="sentencia">View query</param>
        /// <returns></returns>
        private bool wasRemoved(string sentencia)
        {
            // obtengo el objeto eliminado
            SystemQueriesCommand b = new SystemQueriesCommand(sentencia, null);
            b.Execute();

            dynamic[] result = b.QueryResult;
            int objectCount = result.Count<dynamic>();

            // se verifica si existe mas de un objeto
            if (objectCount != 0)
            {
                // si hay algun objeto con el nombre especificado, la prueba falla
                return false;
            }

            // si no existe algun objeto con el nombre especificado, la prueba es exitosa
            return true;
        }
    }
}
