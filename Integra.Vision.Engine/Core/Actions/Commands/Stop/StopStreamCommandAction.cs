//-----------------------------------------------------------------------
// <copyright file="StopStreamCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Implements all the process of stop a stream.
    /// </summary>
    internal sealed class StopStreamCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
                {
                    this.StopObject(context, command as StopStreamCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains stop object logic
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Stop stream command</param>
        private void StopObject(ObjectsContext vc, StopStreamCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.UserDefinedObject> repoUserDefinedObject = new Database.Repositories.Repository<Database.Models.UserDefinedObject>(vc);

            // get the adapter
            Database.Models.UserDefinedObject adapter = repoUserDefinedObject.Find(x => x.Name == command.Name);

            // update the adapter
            adapter.State = (int)UserDefinedObjectStateEnum.Stopped;

            // save changes
            vc.SaveChanges();

            // close connection
            vc.Dispose();
        }
    }
}
