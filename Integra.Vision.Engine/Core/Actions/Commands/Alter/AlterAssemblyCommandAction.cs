//-----------------------------------------------------------------------
// <copyright file="AlterAssemblyCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Implements all the process of alter an assembly.
    /// </summary>
    internal sealed class AlterAssemblyCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                ViewsContext context = new ViewsContext("EngineDatabase");

                this.UpdateObjects(context, command as AlterAssemblyCommand);
                return new QueryCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains alter assembly logic.
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Alter assembly command</param>
        private void UpdateObjects(ViewsContext vc, AlterAssemblyCommand command)
        {   
            Database.Repositories.Repository<Database.Models.Assembly> repo = new Database.Repositories.Repository<Database.Models.Assembly>(vc);

            // delete the object
            Database.Models.Assembly assembly = repo.Find(x => x.Name == command.Name);

            // update the assembly arguments
            assembly.CreationDate = System.DateTime.Now;
            assembly.IsSystemObject = false;
            assembly.Type = ObjectTypeEnum.Assembly.ToString();
            assembly.State = (int)UserDefinedObjectStateEnum.Stopped;
            assembly.LocalPath = command.LocalPath;

            // update the object
            repo.Update(assembly);

            // Guarda los cambios
            vc.SaveChanges();

            // Cierra la conexion
            vc.Dispose();
        }
    }
}
