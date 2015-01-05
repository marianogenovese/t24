using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Integra.Vision.Engine.Commands.Alter.AlterAdapter;
using Integra.Vision.Engine.Commands.Create.CreateAdapter;
using Integra.Vision.Engine.Commands.Drop.DropAdapter;
using Integra.Vision.Engine.Commands.Alter.AlterSource;
using Integra.Vision.Engine.Commands.Alter.AlterStream;
using Integra.Vision.Engine.Commands.Alter.AlterTrigger;
using Integra.Vision.Engine.Commands.Alter.AlterUser;

namespace Integra.Vision.EngineUnitTests.Tests
{
    [TestClass]
    public class AlterCommandTest
    {
        [TestMethod]
        public void AlterAdapterTest()
        {
            string sentencia = "alter adapter adapterTest15 for output as string @p1 = \"cadena\" int @p2 = 1 reference assemblyTest";
            try
            {
                AlterAdapterCommand a = new AlterAdapterCommand(sentencia, null);
                a.Execute();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void AlterSourceTest()
        {
            string sentencia = "alter source sourceTest as from adapterTest where 1 == 2";
            try
            {
                AlterSourceCommand a = new AlterSourceCommand(sentencia, null);
                a.Execute();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void AlterStreamJoinTest()
        {
            string sentencia = "alter stream streamJoinTest as"
                                + " join sourceTest as s3"
                                + " with sourceTest as s4"
                                + " on s3.@event.Message.#0.#0 != s4.@event.Message.#0.#0"
                                + " apply window '00:00:01:00'"
                                + " where true != false"
                                + " select 1 as enteroJoin, \"hola\" as cadenaJoin, true as booleanoJoin, s4.@event.Message.#0.#0 as objeto";
            try
            {
                AlterStreamCommand a = new AlterStreamCommand(sentencia, null);
                a.Execute();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void AlterTriggerWithDurationTimeTest()
        {
            string sentencia1 = "alter trigger triggerTestWithDurationTime on streamJoinTest apply window '00:00:02:00' as "
                                + " if not @hasevents "
                                + " send @event to outputAdapterTest"
                                + " endif"
                                + " if @hasevents"
                                + " send @event to adapterTest"
                                + " endif";

            string sentencia2 = "alter trigger triggerTestWithDurationTime4 on streamJoinTest apply window '00:00:01:00' as "
                                + " if @hasevents "
                                + " send @event to outputAdapterTest"
                                + " endif"
                                + " send @event to adapterTest";

            try
            {
                AlterTriggerCommand a = new AlterTriggerCommand(sentencia1, null);
                a.Execute();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void AlterUserTest()
        {
            string sentencia = "alter user userTest with password = \"passTestAlter\", status = enable";

            try
            {
                AlterUserCommand a = new AlterUserCommand(sentencia, null);
                a.Execute();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
    }
}
