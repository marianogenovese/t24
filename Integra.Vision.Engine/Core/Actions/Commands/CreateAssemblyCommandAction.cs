//-----------------------------------------------------------------------
// <copyright file="CreateAssemblyCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;

    /// <summary>
    /// Implements all the process of create a new assembly.
    /// </summary>
    internal sealed class CreateAssemblyCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                /*
                Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");
                Database.Repositories.Repository<Database.Models.Assembly> repo = new Database.Repositories.Repository<Database.Models.Assembly>(vc);
                Database.Models.Assembly assembly = new Database.Models.Assembly() { CreationDate = System.DateTime.Now, IsSystemObject = false, Type = ObjectTypeEnum.Assembly.ToString(), State = (int)UserDefinedObjectStateEnum.Stopped, Name = command["Name"].Value.ToString(), LocalPath = this.Arguments["LocalPath"].Value.ToString() };
                repo.Create(assembly);
                repo.Commit();

                repo.Dispose();
                vc.Dispose();
                */
                return new QueryCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }
    }
}
