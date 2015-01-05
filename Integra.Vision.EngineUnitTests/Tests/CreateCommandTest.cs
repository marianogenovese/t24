using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Integra.Vision.Engine.Commands.Create;
using Integra.Vision.Engine.Commands;
using Integra.Vision.Engine.Commands.Create.CreateAdapter;
using Integra.Vision.Engine.Commands.Create.CreateSource;
using Integra.Vision.Engine.Commands.Create.CreateStream;
using Integra.Vision.Engine.Commands.Create.CreateTrigger;
using Integra.Vision.Engine.Commands.Create.CreateRole;
using Integra.Vision.Engine.Commands.Create.CreateUser;
using Integra.Vision.Engine.Commands.SystemViews;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;

namespace Integra.Vision.EngineUnitTests
{
    [TestClass]
    public class CreateCommandTest
    {
        [TestMethod]
        public void TestCreateAssembly()
        {
            string assemblyName = "assemblyTest", ruta = "alguna_ruta";
            string sentencia = string.Format("create assembly {0} from \"{1}\"", assemblyName, ruta);
            string sentenciaVista = string.Format("from System.Assemblies \n where Name = \"{0}\" \n select new(Id, Name, LocalPath, State, IsSystemObject, CreationDate)", assemblyName);
            try
            {
                CreateAssemblyCommand a = new CreateAssemblyCommand(sentencia, null);
                a.Execute();

                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                Assert.AreEqual(assemblyName, valueToEvaluate.GetType().GetProperty("Name").GetValue(valueToEvaluate));
                Assert.AreEqual(ruta, valueToEvaluate.GetType().GetProperty("LocalPath").GetValue(valueToEvaluate));
                Assert.AreEqual((int)UserDefinedObjectStateEnum.Stopped, valueToEvaluate.GetType().GetProperty("State").GetValue(valueToEvaluate));
                Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("IsSystemObject").GetValue(valueToEvaluate).ToString()));
                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestCreateAdapter()
        {
            string adapterName = "adapterTest", firstParamName = "p1", secondParamName = "p2", reference = "assemblyTest",
                firstParamType = ArgDataTypeEnum.String.ToString(), secondParamType = ArgDataTypeEnum.Int.ToString(), adapterType = AdapterTypeEnum.Input.ToString();
            object firstParamValue = "cadena", secondParamValue = 1;

            string sentencia = string.Format("create adapter {0} for {1} as {2} @{3} = \"{4}\" {5} @{6} = {7} reference {8}",
                adapterName, adapterType,
                firstParamType, firstParamName, firstParamValue,
                secondParamType, secondParamName, secondParamValue,
                reference);

            string sentenciaVista = string.Format("from System.Adapters \n where Name = \"{0}\" \n select new(Id, Reference, Type, Name, State, IsSystemObject, CreationDate)", adapterName);
            try
            {
                CreateAdapterCommand a = new CreateAdapterCommand(sentencia, null);
                a.Execute();

                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                Assert.AreEqual(adapterName, valueToEvaluate.GetType().GetProperty("Name").GetValue(valueToEvaluate));
                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Reference").GetValue(valueToEvaluate));
                Assert.AreEqual((int)AdapterTypeEnum.Input, valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate));
                Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("IsSystemObject").GetValue(valueToEvaluate).ToString()));
                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));

                var adapterId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);
                string adapterIdConverted = (string)adapterId.GetType().InvokeMember("ToString",
                            BindingFlags.DeclaredOnly |
                            BindingFlags.Public | BindingFlags.NonPublic |
                            BindingFlags.Instance | BindingFlags.InvokeMethod, null, adapterId, null);

                SystemQueriesCommand c = new SystemQueriesCommand("from System.Args \n where Id.ToString() = \"" + adapterIdConverted + "\" \n select new(Id, Type, Name, Value)", null);
                c.Execute();

                result = c.QueryResult;
                valueToEvaluate = result.GetValue(0);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                Assert.AreEqual((int)ArgDataTypeEnum.String, valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate));
                Assert.AreEqual(firstParamName, valueToEvaluate.GetType().GetProperty("Name").GetValue(valueToEvaluate));
                Assert.AreEqual(this.GetBytes(firstParamValue.ToString()).ToString(), valueToEvaluate.GetType().GetProperty("Value").GetValue(valueToEvaluate).ToString());

                valueToEvaluate = result.GetValue(1);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                Assert.AreEqual((int)ArgDataTypeEnum.Int, valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate));
                Assert.AreEqual(secondParamName, valueToEvaluate.GetType().GetProperty("Name").GetValue(valueToEvaluate));
                Assert.AreEqual(this.GetBytes(secondParamValue.ToString()).ToString(), valueToEvaluate.GetType().GetProperty("Value").GetValue(valueToEvaluate).ToString());

            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestCreateSource()
        {
            string sourceName = "sourceTest", adapterName = "adapterTest", where = "1 == 1";

            string sentencia = string.Format("create source {0} as from {1} where {2}",
                sourceName, adapterName, where);

            string sentenciaVista = string.Format("from System.Sources \n where Name=\"{0}\" \n select new(Id, Name, State, IsSystemObject, AdapterId, CreationDate)", sourceName);
            try
            {
                CreateSourceCommand a = new CreateSourceCommand(sentencia, null);
                a.Execute();

                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                Assert.AreEqual(sourceName, valueToEvaluate.GetType().GetProperty("Name").GetValue(valueToEvaluate));
                Assert.AreEqual((int)UserDefinedObjectStateEnum.Stopped, valueToEvaluate.GetType().GetProperty("State").GetValue(valueToEvaluate));
                Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("IsSystemObject").GetValue(valueToEvaluate).ToString()));
                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));
                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("AdapterId").GetValue(valueToEvaluate));

                var sourceId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);

                sentenciaVista = string.Format("from System.Conditions \n where Id.ToString()=\"{0}\" \n select new(Id, Type, Expression, IsOnCondition)", sourceId);
                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;
                valueToEvaluate = result.GetValue(0);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                Assert.AreEqual((int)ConditionTypeEnum.FilterCondition, valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate));
                Assert.AreEqual(where, valueToEvaluate.GetType().GetProperty("Expression").GetValue(valueToEvaluate));
                Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("IsOnCondition").GetValue(valueToEvaluate).ToString()));
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestCreateStreamSimple()
        {
            string streamName = "streamSimpleTest", from = "sourceTest", where = "true != false",
                                p1Value = "1", p1Alias = "entero", p2Value = "hola", p2Alias = "cadena", p3Value = "true", p3Alias = "booleano";

            string sentencia = string.Format("create stream {0} as"
                                + " from {1}"
                                + " where {2}"
                                + " select {3} as {4}, \"{5}\" as {6}, {7} as {8}",
                                streamName, from, where, p1Value, p1Alias, p2Value, p2Alias, p3Value, p3Alias);

            string sentenciaVista = string.Format("from System.Streams \n where Name=\"{0}\" \n select new(Id, DurationTime, UseJoin, Name, State, IsSystemObject, CreationDate)", streamName);
            try
            {
                CreateStreamCommand a = new CreateStreamCommand(sentencia, null);
                a.Execute();

                // verifico lo guardado en System.Streams
                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                Assert.AreEqual((double)0, valueToEvaluate.GetType().GetProperty("DurationTime").GetValue(valueToEvaluate));
                Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("UseJoin").GetValue(valueToEvaluate).ToString()));
                Assert.AreEqual(streamName, valueToEvaluate.GetType().GetProperty("Name").GetValue(valueToEvaluate));
                Assert.AreEqual((int)UserDefinedObjectStateEnum.Stopped, valueToEvaluate.GetType().GetProperty("State").GetValue(valueToEvaluate));
                Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("IsSystemObject").GetValue(valueToEvaluate).ToString()));
                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));

                // verifico lo guardado en System.Conditions
                var streamId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);
                sentenciaVista = string.Format("from System.Conditions \n where Id.ToString()=\"{0}\" \n select new(Id, Type, Expression, IsOnCondition)", streamId);

                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;
                valueToEvaluate = result.GetValue(0);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                Assert.AreEqual((int)ConditionTypeEnum.FilterCondition, valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate));
                Assert.AreEqual(where, valueToEvaluate.GetType().GetProperty("Expression").GetValue(valueToEvaluate));
                Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("IsOnCondition").GetValue(valueToEvaluate).ToString()));

                // verifico lo guardado en System.SourcesAsignedToStreams
                sentenciaVista = string.Format("from System.SourcesAsignedToStreams \n where StreamId.ToString()=\"{0}\" \n select new(StreamId, SourceId, Alias, IsWithSource)", streamId);

                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;
                valueToEvaluate = result.GetValue(0);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("StreamId").GetValue(valueToEvaluate));
                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("SourceId").GetValue(valueToEvaluate));
                Assert.AreEqual(from, valueToEvaluate.GetType().GetProperty("Alias").GetValue(valueToEvaluate));
                Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("IsWithSource").GetValue(valueToEvaluate).ToString()));

                // verifico lo guardado en System.PList
                sentenciaVista = string.Format("from System.PList \n where Id.ToString()=\"{0}\" \n select new(Id, Expression, Alias, Order)", streamId);

                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;

                for (int i = 0; i < result.Count<dynamic>(); i++)
                {
                    valueToEvaluate = result.GetValue(i);

                    if (valueToEvaluate.GetType().GetProperty("Order").GetValue(valueToEvaluate).Equals(0))
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                        Assert.AreEqual(int.Parse(p1Value), int.Parse(valueToEvaluate.GetType().GetProperty("Expression").GetValue(valueToEvaluate).ToString()));
                        Assert.AreEqual(p1Alias, valueToEvaluate.GetType().GetProperty("Alias").GetValue(valueToEvaluate));
                        Assert.AreEqual(0, valueToEvaluate.GetType().GetProperty("Order").GetValue(valueToEvaluate));
                    }
                    else if (valueToEvaluate.GetType().GetProperty("Order").GetValue(valueToEvaluate).Equals(1))
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                        Assert.AreEqual("\"" + p2Value + "\"", valueToEvaluate.GetType().GetProperty("Expression").GetValue(valueToEvaluate));
                        Assert.AreEqual(p2Alias, valueToEvaluate.GetType().GetProperty("Alias").GetValue(valueToEvaluate));
                    }
                    else if (valueToEvaluate.GetType().GetProperty("Order").GetValue(valueToEvaluate).Equals(2))
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                        Assert.AreEqual(bool.Parse(p3Value), bool.Parse(valueToEvaluate.GetType().GetProperty("Expression").GetValue(valueToEvaluate).ToString()));
                        Assert.AreEqual(p3Alias, valueToEvaluate.GetType().GetProperty("Alias").GetValue(valueToEvaluate));
                    }
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestCreateStreamJoin()
        {
            string streamName = "streamJoinTest14", joinValue = "sourceTest9", joinAlias = "s1", withValue = "sourceTest9", withAlias = "s2", on = "s1.@event.Message.#0.#0 == s2.@event.Message.#0.#0",
                    window = "00:00:01:00", where = "true != false",
                    p1Value = "1", p1Alias = "enteroJoin", p2Value = "hola", p2Alias = "cadenaJoin", p3Value = "true", p3Alias = "booleanoJoin",
                    p4Value = "s2.@event.Message.#0.#0", p4Alias = "objeto";

            string sentencia = string.Format("create stream {0} as"
                                + " join {1} as {2}"
                                + " with {3} as {4}"
                                + " on {5}"
                                + " apply window '{6}'"
                                + " where {7}"
                                + " select {8} as {9}, \"{10}\" as {11}, {12} as {13}, {14} as {15}",
                                streamName, joinValue, joinAlias, withValue, withAlias, on, window, where,
                                p1Value, p1Alias, p2Value, p2Alias, p3Value, p3Alias, p4Value, p4Alias);

            string sentenciaVista = string.Format("from System.Streams \n where Name=\"{0}\" \n select new(Id, DurationTime, UseJoin, Name, State, IsSystemObject, CreationDate)", streamName);
            try
            {
                CreateStreamCommand a = new CreateStreamCommand(sentencia, null);
                a.Execute();

                // verifico lo guardado en System.Streams
                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                Assert.AreEqual(TimeSpan.Parse(window).TotalMilliseconds, valueToEvaluate.GetType().GetProperty("DurationTime").GetValue(valueToEvaluate));
                Assert.IsTrue(bool.Parse(valueToEvaluate.GetType().GetProperty("UseJoin").GetValue(valueToEvaluate).ToString()));
                Assert.AreEqual(streamName, valueToEvaluate.GetType().GetProperty("Name").GetValue(valueToEvaluate));
                Assert.AreEqual((int)UserDefinedObjectStateEnum.Stopped, valueToEvaluate.GetType().GetProperty("State").GetValue(valueToEvaluate));
                Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("IsSystemObject").GetValue(valueToEvaluate).ToString()));
                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));

                // verifico lo guardado en System.Conditions
                var streamId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);
                sentenciaVista = string.Format("from System.Conditions \n where Id.ToString()=\"{0}\" \n select new(Id, Type, Expression, IsOnCondition)", streamId);

                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;

                for (int i = 0; i < result.Count<dynamic>(); i++)
                {
                    valueToEvaluate = result.GetValue(i);

                    if (bool.Parse(valueToEvaluate.GetType().GetProperty("IsOnCondition").GetValue(valueToEvaluate).ToString()))
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                        Assert.AreEqual((int)ConditionTypeEnum.CombinationCondition, valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate));
                        Assert.AreEqual(on, valueToEvaluate.GetType().GetProperty("Expression").GetValue(valueToEvaluate));
                    }
                    else
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                        Assert.AreEqual((int)ConditionTypeEnum.MergeResultCondition, valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate));
                        Assert.AreEqual(where, valueToEvaluate.GetType().GetProperty("Expression").GetValue(valueToEvaluate));
                    }
                }
                // verifico lo guardado en System.SourcesAsignedToStreams
                sentenciaVista = string.Format("from System.SourcesAsignedToStreams \n where StreamId.ToString()=\"{0}\" \n select new(StreamId, SourceId, Alias, IsWithSource)", streamId);

                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;

                for (int i = 0; i < result.Count<dynamic>(); i++)
                {
                    valueToEvaluate = result.GetValue(i);

                    if (bool.Parse(valueToEvaluate.GetType().GetProperty("IsWithSource").GetValue(valueToEvaluate).ToString()))
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("StreamId").GetValue(valueToEvaluate));
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("SourceId").GetValue(valueToEvaluate));
                        Assert.AreEqual(withAlias, valueToEvaluate.GetType().GetProperty("Alias").GetValue(valueToEvaluate));
                    }
                    else
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("StreamId").GetValue(valueToEvaluate));
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("SourceId").GetValue(valueToEvaluate));
                        Assert.AreEqual(joinAlias, valueToEvaluate.GetType().GetProperty("Alias").GetValue(valueToEvaluate));
                    }
                }

                // verifico lo guardado en System.PList
                sentenciaVista = string.Format("from System.PList \n where Id.ToString()=\"{0}\" \n select new(Id, Expression, Alias, Order)", streamId);

                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;

                for (int i = 0; i < result.Count<dynamic>(); i++)
                {
                    valueToEvaluate = result.GetValue(i);

                    if (valueToEvaluate.GetType().GetProperty("Order").GetValue(valueToEvaluate).Equals(0))
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                        Assert.AreEqual(int.Parse(p1Value), int.Parse(valueToEvaluate.GetType().GetProperty("Expression").GetValue(valueToEvaluate).ToString()));
                        Assert.AreEqual(p1Alias, valueToEvaluate.GetType().GetProperty("Alias").GetValue(valueToEvaluate));
                    }
                    else if (valueToEvaluate.GetType().GetProperty("Order").GetValue(valueToEvaluate).Equals(1))
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                        Assert.AreEqual("\"" + p2Value + "\"", valueToEvaluate.GetType().GetProperty("Expression").GetValue(valueToEvaluate));
                        Assert.AreEqual(p2Alias, valueToEvaluate.GetType().GetProperty("Alias").GetValue(valueToEvaluate));
                    }
                    else if (valueToEvaluate.GetType().GetProperty("Order").GetValue(valueToEvaluate).Equals(2))
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                        Assert.AreEqual(bool.Parse(p3Value), bool.Parse(valueToEvaluate.GetType().GetProperty("Expression").GetValue(valueToEvaluate).ToString()));
                        Assert.AreEqual(p3Alias, valueToEvaluate.GetType().GetProperty("Alias").GetValue(valueToEvaluate));
                    }
                    else if (valueToEvaluate.GetType().GetProperty("Order").GetValue(valueToEvaluate).Equals(3))
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                        Assert.AreEqual(p4Value, valueToEvaluate.GetType().GetProperty("Expression").GetValue(valueToEvaluate));
                        Assert.AreEqual(p4Alias, valueToEvaluate.GetType().GetProperty("Alias").GetValue(valueToEvaluate));
                    }
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestCreateTriggerSimple()
        {
            string triggerName = "triggerTestSimple5", streamName = "streamJoinTest14", outputAdapter = "outputAdapterTest";

            string sentencia = String.Format("create trigger {0} on {1} as send @event to {2}", triggerName, streamName, outputAdapter);

            string sentenciaVista = string.Format("from System.Triggers \n where Name=\"{0}\" \n select new(Id, DurationTime, StreamId, Name, State, IsSystemObject, CreationDate)", triggerName);

            try
            {
                CreateTriggerCommand a = new CreateTriggerCommand(sentencia, null);
                a.Execute();

                // verifico lo guardado en System.Triggers
                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                var triggerId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);

                Assert.IsNotNull(triggerId);
                Assert.AreEqual((double)0, valueToEvaluate.GetType().GetProperty("DurationTime").GetValue(valueToEvaluate));
                Assert.AreEqual(triggerName, valueToEvaluate.GetType().GetProperty("Name").GetValue(valueToEvaluate));
                Assert.AreEqual((int)UserDefinedObjectStateEnum.Stopped, valueToEvaluate.GetType().GetProperty("State").GetValue(valueToEvaluate));
                Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("IsSystemObject").GetValue(valueToEvaluate).ToString()));
                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));

                // verifico que haga referencia al stream indicado
                var streamId = valueToEvaluate.GetType().GetProperty("StreamId").GetValue(valueToEvaluate);
                sentenciaVista = string.Format("from System.Streams \n where Id.ToString()=\"{0}\" \n select new(Name)", streamId);

                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;
                valueToEvaluate = result.GetValue(0);

                Assert.AreEqual(streamName, valueToEvaluate.GetType().GetProperty("Name").GetValue(valueToEvaluate));

                // verifico lo guardado en System.Stmts
                sentenciaVista = string.Format("from System.Stmts \n where Id.ToString()=\"{0}\" \n select new(Id, Order, Type, AdapterId)", triggerId);

                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;
                valueToEvaluate = result.GetValue(0);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                Assert.AreEqual((int)0, valueToEvaluate.GetType().GetProperty("Order").GetValue(valueToEvaluate));
                Assert.AreEqual((int)StmtTypeEnum.SendAlways, valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate));

                // verifico que se haga referencia al adaptador indicado
                sentenciaVista = string.Format("from System.Adapters \n where Name=\"{0}\" \n select new(Id)", outputAdapter);

                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;
                valueToEvaluate = result.GetValue(0);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestCreateTriggerWithDurationTime()
        {
            string triggerName = "triggerTestWithDurationTime5", streamName = "streamJoinTest14", window = "00:00:01:00",
                                outputAdapter1 = "outputAdapterTest2", outputAdapter2 = "outputAdapterTest3", outputAdapter3 = "outputAdapterTest4";

            string sentencia1 = string.Format("create trigger {0} on {1} apply window '{2}' as "
                                + " if @hasevents "
                                + " send @event to {3}"
                                + " endif"
                                + " if not @hasevents"
                                + " send @event to {4}"
                                + " endif"
                                + " send @event to {5}",
                                triggerName, streamName, window, outputAdapter1, outputAdapter2, outputAdapter3);

            string sentenciaVista = string.Format("from System.Triggers \n where Name=\"{0}\" \n select new(Id, DurationTime, StreamId, Name, State, IsSystemObject, CreationDate)", triggerName);

            try
            {
                CreateTriggerCommand a = new CreateTriggerCommand(sentencia1, null);
                a.Execute();

                // verifico lo guardado en System.Triggers
                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                var triggerId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);

                Assert.IsNotNull(triggerId);
                Assert.AreEqual(TimeSpan.Parse("00:00:01:00").TotalMilliseconds, valueToEvaluate.GetType().GetProperty("DurationTime").GetValue(valueToEvaluate));
                Assert.AreEqual(triggerName, valueToEvaluate.GetType().GetProperty("Name").GetValue(valueToEvaluate));
                Assert.AreEqual((int)UserDefinedObjectStateEnum.Stopped, valueToEvaluate.GetType().GetProperty("State").GetValue(valueToEvaluate));
                Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("IsSystemObject").GetValue(valueToEvaluate).ToString()));
                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));

                // verifico que haga referencia al stream indicado
                var streamId = valueToEvaluate.GetType().GetProperty("StreamId").GetValue(valueToEvaluate);
                sentenciaVista = string.Format("from System.Streams \n where Id.ToString()=\"{0}\" \n select new(Name)", streamId);

                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;
                valueToEvaluate = result.GetValue(0);

                Assert.AreEqual(streamName, valueToEvaluate.GetType().GetProperty("Name").GetValue(valueToEvaluate));

                // verifico lo guardado en System.Stmts
                sentenciaVista = string.Format("from System.Stmts \n where Id.ToString()=\"{0}\" \n select new(Id, Order, Type, AdapterId)", triggerId);

                b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                result = b.QueryResult;

                for (int i = 0; i < result.Count<dynamic>(); i++)
                {
                    valueToEvaluate = result.GetValue(i);

                    if (valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate).Equals((int)StmtTypeEnum.SendAlways))
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                        Assert.AreEqual((int)2, valueToEvaluate.GetType().GetProperty("Order").GetValue(valueToEvaluate));

                        // verifico que se haga referencia al adaptador indicado
                        sentenciaVista = string.Format("from System.Adapters \n where Name=\"{0}\" \n select new(Id)", outputAdapter3);

                        b = new SystemQueriesCommand(sentenciaVista, null);
                        b.Execute();

                        var resultSystemAdapters = b.QueryResult;
                        valueToEvaluate = resultSystemAdapters.GetValue(0);

                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                    }
                    else if (valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate).Equals((int)StmtTypeEnum.SendIfHasEvents))
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                        Assert.AreEqual((int)0, valueToEvaluate.GetType().GetProperty("Order").GetValue(valueToEvaluate));

                        // verifico que se haga referencia al adaptador indicado
                        sentenciaVista = string.Format("from System.Adapters \n where Name=\"{0}\" \n select new(Id)", outputAdapter1);

                        b = new SystemQueriesCommand(sentenciaVista, null);
                        b.Execute();

                        var resultSystemAdapters = b.QueryResult;
                        valueToEvaluate = resultSystemAdapters.GetValue(0);

                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                    }
                    else if (valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate).Equals((int)StmtTypeEnum.SendIfNotHasEvents))
                    {
                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                        Assert.AreEqual((int)1, valueToEvaluate.GetType().GetProperty("Order").GetValue(valueToEvaluate));

                        // verifico que se haga referencia al adaptador indicado
                        sentenciaVista = string.Format("from System.Adapters \n where Name=\"{0}\" \n select new(Id)", outputAdapter2);

                        b = new SystemQueriesCommand(sentenciaVista, null);
                        b.Execute();

                        var resultSystemAdapters = b.QueryResult;
                        valueToEvaluate = resultSystemAdapters.GetValue(0);

                        Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                    }
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestCreateRole()
        {
            string roleName = "roleTest";

            string sentencia = String.Format("create role {0}", roleName);

            string sentenciaVista = String.Format("from System.Roles \n where Name=\"{0}\" \n select new(Id, IsServerRole, Name, CreationDate)", roleName);
            try
            {
                CreateRoleCommand a = new CreateRoleCommand(sentencia, null);
                a.Execute();

                // verifico lo guardado en System.Roles
                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("IsServerRole").GetValue(valueToEvaluate).ToString()));
                Assert.AreEqual(roleName, valueToEvaluate.GetType().GetProperty("Name").GetValue(valueToEvaluate));
                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestCreateUser()
        {
            string userName = "userTest", password = "passTest", status = UserStatusEnum.Enable.ToString();

            string sentencia = string.Format("create user {0} with password = \"{1}\", status = {2}", userName, password, status);

            string sentenciaVista = string.Format("from System.Users \n where Sid=\"{0}\" \n select new(Id, Sid, Password, Status, CreationDate)", userName);

            try
            {
                CreateUserCommand a = new CreateUserCommand(sentencia, null);
                a.Execute();

                // verifico lo guardado en System.Users
                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                Assert.AreEqual(userName, valueToEvaluate.GetType().GetProperty("Sid").GetValue(valueToEvaluate).ToString());
                Assert.AreEqual((int)UserStatusEnum.Enable, valueToEvaluate.GetType().GetProperty("Status").GetValue(valueToEvaluate));
                Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));

                MD5 md5Hash = MD5.Create();
                string passwordHash = this.GetMd5Hash(md5Hash, password);
                Assert.AreEqual(passwordHash, valueToEvaluate.GetType().GetProperty("Password").GetValue(valueToEvaluate));
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        /// <summary>
        /// Gets the byte array of a string
        /// </summary>
        /// <param name="str">string to convert</param>
        /// <returns>byte array</returns>
        public byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Get hash code from password
        /// </summary>
        /// <param name="md5Hash">Doc1 goes here</param>
        /// <param name="input">Doc2 goes here</param>
        /// <returns>Doc3 goes here</returns>
        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder stringBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return stringBuilder.ToString();
        }
    }
}
