//-----------------------------------------------------------------------
// <copyright file="StartStreamCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Implements all the process of start a stream.
    /// </summary>
    internal sealed class StartStreamCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ViewsContext context = new ViewsContext("EngineDatabase"))
                {
                    this.StartObject(context, command as StartStreamCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains start object logic
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Start stream command</param>
        private void StartObject(ViewsContext vc, StartStreamCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.UserDefinedObject> repoUserDefinedObject = new Database.Repositories.Repository<Database.Models.UserDefinedObject>(vc);

            // get the adapter
            Database.Models.UserDefinedObject adapter = repoUserDefinedObject.Find(x => x.Name == command.Name);

            // update the adapter
            adapter.State = (int)UserDefinedObjectStateEnum.Started;

            // save changes
            vc.SaveChanges();

            // close connection
            vc.Dispose();
        }
    }
}
