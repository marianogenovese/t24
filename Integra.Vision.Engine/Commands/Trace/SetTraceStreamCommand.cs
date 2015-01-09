//-----------------------------------------------------------------------
// <copyright file="SetTraceStreamCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Class for trace a stream.
    /// </summary>
    internal sealed class SetTraceStreamCommand : SetTraceCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetTraceStreamCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public SetTraceStreamCommand(PlanNode node)
            : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get 
            {
                return CommandTypeEnum.SetTraceStream;
            }
        }

        /// <summary>
        /// Gets the object name or object family
        /// </summary>
        public ObjectTypeEnum FamilyName
        {
            get
            {
                return (ObjectTypeEnum)this.Arguments["FamilyName"].Value;
            }
        }
    }
}
