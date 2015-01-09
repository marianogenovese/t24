//-----------------------------------------------------------------------
// <copyright file="StopAdapterCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for stop an adapter.
    /// </summary>
    internal sealed class StopAdapterCommand : StopObjectCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StopAdapterCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public StopAdapterCommand(PlanNode node)
            : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get 
            {
                return CommandTypeEnum.StopAdapter;
            }
        }
    }
}
