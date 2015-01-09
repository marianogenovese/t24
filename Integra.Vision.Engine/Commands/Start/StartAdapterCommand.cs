//-----------------------------------------------------------------------
// <copyright file="StartAdapterCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for start an adapter
    /// </summary>
    internal sealed class StartAdapterCommand : StartObjectCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartAdapterCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public StartAdapterCommand(PlanNode node) : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get 
            {
                return CommandTypeEnum.StartAdapter;
            }
        }
    }
}
