//-----------------------------------------------------------------------
// <copyright file="ReceiveCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Event;
    using Integra.Vision.Language;
    using Integra.Vision.Language.Runtime;

    /// <summary>
    /// Implements all the process of return the results to the client.
    /// </summary>
    internal sealed class ReceiveCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                return new QueryCommandResult(this.CreateEvent(command as ReceiveCommand));
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains the logic for return results to the client.
        /// </summary>
        /// <param name="receiveCommand">Receive command</param>
        /// <returns>Events result</returns>
        private System.Collections.IEnumerable CreateEvent(ReceiveCommand receiveCommand)
        {
            string onCondition = string.Empty;
            string whereCondition = string.Empty;
            string projectionScript = string.Empty;

            using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
            {
                // create repository
                Repository<Database.Models.UserDefinedObject> repoUserDefinedObject = new Repository<Database.Models.UserDefinedObject>(context);
                Repository<Database.Models.PList> repoProjection = new Repository<Database.Models.PList>(context);
                Repository<Database.Models.StreamCondition> repoStreamConditions = new Repository<Database.Models.StreamCondition>(context);

                // get the stream
                Database.Models.UserDefinedObject stream = repoUserDefinedObject.Find(x => x.Name == receiveCommand.StreamName);

                // get the stream projection
                IQueryable projection = repoProjection.Filter(x => x.StreamId == stream.Id);
                projectionScript = this.GenerateProjectionScript(projection);

                // get the stream conditions
                IQueryable conditions = repoStreamConditions.Filter(x => x.StreamId == stream.Id);

                foreach (Database.Models.StreamCondition condition in conditions)
                {
                    if (condition.IsOnCondition)
                    {
                        onCondition = condition.Expression;
                    }
                    else
                    {
                        whereCondition = condition.Expression;
                    }
                }
            }

            // load object
            if (onCondition.Equals(string.Empty))
            {
                IDictionary<string, object> dic = this.LoadObject("httpSource", whereCondition, projectionScript);
                foreach (var tuple in dic)
                {
                    yield return new { Llave = tuple.Key, Valor = tuple.Value };
                }
            }
            else
            {
                IDictionary<string, object> dic = this.LoadObject("httpSource", "httpSource", onCondition, whereCondition, projectionScript);
                foreach (var tuple in dic)
                {
                    yield return new { Llave = tuple.Key, Valor = tuple.Value };
                }
            }
        }

        /// <summary>
        /// Contains load stream logic.
        /// </summary>
        /// <param name="sourceFromName">Source name of the 'from' statement</param>
        /// <param name="sourceOnName">Source name of the 'on' statement</param>
        /// <param name="onCondition">On condition expression</param>
        /// <param name="whereCondition">Where condition expression</param>
        /// <param name="projection">Projection expression</param>
        /// <returns>Projection list</returns>
        private IDictionary<string, object> LoadObject(string sourceFromName, string sourceOnName, string onCondition, string whereCondition, string projection)
        {
            if (sourceFromName.Equals(Integra.Vision.Engine.SR.SourceHttpType, StringComparison.InvariantCultureIgnoreCase))
            {
                List<EventObject> listOfEvents = new List<EventObject>();

                if (System.Messaging.MessageQueue.Exists(@".\Private$\HttpSource"))
                {
                    using (System.Messaging.MessageQueue colaHttp = new System.Messaging.MessageQueue(@".\Private$\HttpSource"))
                    {
                        foreach (System.Messaging.Message message in colaHttp.GetAllMessages())
                        {
                            message.Formatter = new System.Messaging.BinaryMessageFormatter();
                            byte[] body = (byte[])message.Body;
                            using (MemoryStream ms = new MemoryStream(body))
                            {
                                BinaryFormatter formatter = new BinaryFormatter();
                                ms.Seek(0, SeekOrigin.Begin);
                                EventObject eve = (EventObject)formatter.Deserialize(ms);
                                listOfEvents.Add(eve);
                            }
                        }
                    }
                }

                ProjectionParser projectionParser = new ProjectionParser(projection);
                PlanNode projectionNode = projectionParser.Parse();

                ExpressionParser onConditionParser = new ExpressionParser(onCondition);
                PlanNode onConditionNode = onConditionParser.Parse();

                ExpressionParser whereConditionParser = new ExpressionParser(whereCondition);
                PlanNode whereConditionNode = whereConditionParser.Parse();

                ExpressionConstructor constructor = new ExpressionConstructor();
                Func<EventObject, EventObject, IDictionary<string, object>> select = constructor.CompileJoinSelect(projectionNode);
                Func<EventObject, EventObject, bool> on = constructor.CompileJoinWhere(onConditionNode);
                Func<EventObject, EventObject, bool> where = constructor.CompileJoinWhere(whereConditionNode);

                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Contains load stream logic.
        /// </summary>
        /// <param name="sourceName">Source name</param>
        /// <param name="whereCondition">Where condition expression</param>
        /// <param name="projection">Projection expression</param>
        /// <returns>Projection list</returns>
        private IDictionary<string, object> LoadObject(string sourceName, string whereCondition, string projection)
        {
            if (sourceName.Equals(Integra.Vision.Engine.SR.SourceHttpType, StringComparison.InvariantCultureIgnoreCase))
            {
                List<EventObject> listOfEvents = new List<EventObject>();
                using (System.Messaging.MessageQueue colaHttp = new System.Messaging.MessageQueue(@".\Private$\HttpSource"))
                {
                    foreach (System.Messaging.Message message in colaHttp.GetAllMessages())
                    {
                        message.Formatter = new System.Messaging.BinaryMessageFormatter();
                        byte[] body = (byte[])message.Body;
                        using (MemoryStream ms = new MemoryStream(body))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            ms.Seek(0, SeekOrigin.Begin);
                            EventObject eve = (EventObject)formatter.Deserialize(ms);
                            listOfEvents.Add(eve);
                        }
                    }
                }

                ProjectionParser projectionParser = new ProjectionParser(projection);
                PlanNode projectionNode = projectionParser.Parse();

                ExpressionParser whereConditionParser = new ExpressionParser(whereCondition);
                PlanNode whereConditionNode = whereConditionParser.Parse();

                ExpressionConstructor constructor = new ExpressionConstructor();
                Func<EventObject, IDictionary<string, object>> select = constructor.CompileSelect(projectionNode);
                Func<EventObject, bool> where = constructor.CompileWhere(whereConditionNode);

                return listOfEvents.Where(where).Select(select).First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Generate the projection script of the stream
        /// </summary>
        /// <param name="projection">Projection arguments</param>
        /// <returns>Projection script</returns>
        private string GenerateProjectionScript(IQueryable projection)
        {
            string script = "select ";
            bool isFirst = true;

            foreach (Database.Models.PList tupla in projection)
            {
                if (isFirst)
                {
                    script += tupla.Expression + " as " + tupla.Alias;
                    isFirst = false;
                }
                else
                {
                    script += ", " + tupla.Expression + " as " + tupla.Alias;
                }
            }

            return script;
        }
    }
}
