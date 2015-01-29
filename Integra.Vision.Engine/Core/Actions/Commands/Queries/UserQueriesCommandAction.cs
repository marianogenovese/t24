//-----------------------------------------------------------------------
// <copyright file="UserQueriesCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections;
    using System.Linq;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Models.SystemViews;
    using Integra.Vision.Engine.Database.Repositories;
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
            ExpressionConstructor constructor = new ExpressionConstructor();
            var select = constructor.CompileSelect(command.Select);
            var where = constructor.CompileWhere(command.Where);

            return null;
        }
    }
}
