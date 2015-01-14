//-----------------------------------------------------------------------
// <copyright file="LoadAssemblyCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for load an assembly.
    /// </summary>
    internal sealed class LoadAssemblyCommand : StartObjectCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadAssemblyCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public LoadAssemblyCommand(PlanNode node)
            : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get 
            {
                return CommandTypeEnum.LoadAssembly;
            }
        }
    }
}
