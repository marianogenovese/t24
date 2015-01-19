//-----------------------------------------------------------------------
// <copyright file="LoadAssemblyCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Reflection;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process of load an assembly.
    /// </summary>
    internal sealed class LoadAssemblyCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ViewsContext context = new ViewsContext())
                {
                    this.GetAssembliesToLoad(context, command as LoadAssemblyCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Gets and loads the assemblies.
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="command">Load assembly command</param>
        private void GetAssembliesToLoad(ViewsContext context, LoadAssemblyCommand command)
        {
            // creamos el repositorio de assemblies
            Repository<Database.Models.Assembly> repoAssembly = new Repository<Database.Models.Assembly>(context);

            // obtenemos el assembly
            Database.Models.Assembly assembly = repoAssembly.Find(x => x.Name == command.Name);

            // el nombre del assembly debe ser igual al nombre de la clase a cargar junto con su respectivo namespace
            this.LoadAssembly(assembly.LocalPath, assembly.Name);
        }

        /// <summary>
        /// Contains load assembly logic.
        /// </summary>
        /// <param name="localPath">Assembly path</param>
        /// <param name="assemblyClass">Class to load</param>
        private void LoadAssembly(string localPath, string assemblyClass)
        {
            /*Assembly assemblyExtension = Assembly.LoadFile(localPath);
            Type extensionType = assemblyExtension.GetType(assemblyClass);
            object objectInstance = Activator.CreateInstance(extensionType);
            object finalObject = (object)objectInstance;*/
        }
    }
}
