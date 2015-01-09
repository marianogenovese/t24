//-----------------------------------------------------------------------
// <copyright file="StopTriggerCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for stop an stream.
    /// </summary>
    internal sealed class StopTriggerCommand : StopObjectCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StopTriggerCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public StopTriggerCommand(PlanNode node) : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get 
            {
                return CommandTypeEnum.StopTrigger;
            }
        }
    }
}
