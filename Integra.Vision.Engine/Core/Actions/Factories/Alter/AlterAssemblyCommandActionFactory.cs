//-----------------------------------------------------------------------
// <copyright file="AlterAssemblyCommandActionFactory.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements a action filter factory for create new action pipeline filters.
    /// This factory creates action filters for handle Alter Assembly Command
    /// </summary>
    internal sealed class AlterAssemblyCommandActionFactory : CommandActionFactory
    {
        /// <inheritdoc />
        public override CommandAction Create()
        {
            return new AlterAssemblyCommandAction();
        }
    }
}
