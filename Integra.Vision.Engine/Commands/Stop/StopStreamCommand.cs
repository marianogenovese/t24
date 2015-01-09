//-----------------------------------------------------------------------
// <copyright file="StopStreamCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for stop an stream.
    /// </summary>
    internal sealed class StopStreamCommand : StopObjectCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StopStreamCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public StopStreamCommand(PlanNode node) : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get 
            {
                return CommandTypeEnum.StopStream;
            }
        }
    }
}
