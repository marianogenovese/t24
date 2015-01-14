//-----------------------------------------------------------------------
// <copyright file="CreateAssemblyCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

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
                using (ViewsContext context = new ViewsContext("EngineDatabase"))
                {
                    this.SaveArguments(context, command as CreateAssemblyCommand);

                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Save the command arguments.
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Create assembly command</param>
        private void SaveArguments(ViewsContext vc, CreateAssemblyCommand command)
        {
            Database.Repositories.Repository<Database.Models.Assembly> repo = new Database.Repositories.Repository<Database.Models.Assembly>(vc);
            Database.Models.Assembly assembly = new Database.Models.Assembly() { CreationDate = System.DateTime.Now, IsSystemObject = false, Type = ObjectTypeEnum.Assembly.ToString(), State = (int)UserDefinedObjectStateEnum.Stopped, Name = command.Name, LocalPath = command.LocalPath };
            repo.Create(assembly);

            // Guarda el script del objeto
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.SaveScript(command.Script, assembly.Id);

            // Guarda los cambios
            vc.SaveChanges();

            // Cierra la conexion
            vc.Dispose();
        }
    }
}
