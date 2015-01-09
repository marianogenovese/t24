//-----------------------------------------------------------------------
// <copyright file="StopSourceCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for stop an source.
    /// </summary>
    internal sealed class StopSourceCommand : StopObjectCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StopSourceCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public StopSourceCommand(PlanNode node) : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get 
            {
                return CommandTypeEnum.StopSource;
            }
        }
    }
}
