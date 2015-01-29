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
    using Integra.Vision.Event;

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
            if (receiveCommand.SourceName.Equals(SR.SourceHttpType, StringComparison.InvariantCultureIgnoreCase))
            {
                List<object> listOfEvents = new List<object>();
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
                            int value = int.Parse(eve.Message[1][1][1].Value.ToString());
                            listOfEvents.Add(new { Id = 123, Name = "O.O Falta la letra final" });
                        }
                    }
                }

                return listOfEvents.ToArray();
            }
            else
            {
                return null;
            }
        }
    }
}
