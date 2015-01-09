//-----------------------------------------------------------------------
// <copyright file="StartTriggerCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for start a trigger
    /// </summary>
    internal sealed class StartTriggerCommand : StartObjectCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartTriggerCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public StartTriggerCommand(PlanNode node) : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.StartTrigger;
            }
        }
    }
}
