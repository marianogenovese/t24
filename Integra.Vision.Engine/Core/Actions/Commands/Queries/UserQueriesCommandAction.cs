//-----------------------------------------------------------------------
// <copyright file="UserQueriesCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Event;
    using Integra.Vision.Language.Runtime;

    /// <summary>
    /// Implements all the process of the queries system views.
    /// </summary>
    internal sealed class UserQueriesCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                return new QueryCommandResult(this.GetObjects(command as UserQueriesCommand));
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains the logic to query data
        /// </summary>
        /// <param name="command">User query command</param>
        /// <returns>Array of objects.</returns>
        private IEnumerable GetObjects(UserQueriesCommand command)
        {
            // obtenemos la fuente de eventos
            string from = command.From;

            if (from.Equals(SR.SourceHttpType, StringComparison.InvariantCultureIgnoreCase))
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
                            listOfEvents.Add(new EventObject());
                        }
                    }
                }

                ExpressionConstructor constructor = new ExpressionConstructor();
                Func<EventObject, IDictionary<string, object>> select = constructor.CompileSelect(command.Select);
                Func<EventObject, bool> where = constructor.CompileWhere(command.Where);

                IDictionary<string, object> dic = listOfEvents.Where(where).Select(select).First();

                foreach (var tuple in dic)
                {
                    yield return new { Llave = tuple.Key, Valor = tuple.Value };
                }
            }
            else
            {
                yield break;
            }
        }
    }
}
