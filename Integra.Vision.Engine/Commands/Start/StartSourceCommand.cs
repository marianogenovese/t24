//-----------------------------------------------------------------------
// <copyright file="StartSourceCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for start a source
    /// </summary>
    internal sealed class StartSourceCommand : StartObjectCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartSourceCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public StartSourceCommand(PlanNode node) : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.StartSource;
            }
        }
    }
}
