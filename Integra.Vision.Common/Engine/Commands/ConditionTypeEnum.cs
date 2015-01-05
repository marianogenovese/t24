//-----------------------------------------------------------------------
// <copyright file="ConditionTypeEnum.cs" company="Integra.Vision.Common">
//     Copyright (c) Integra.Vision.Common. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    /// <summary>
    /// Condition type
    /// </summary>
    internal enum ConditionTypeEnum
    {
        /// <summary>
        /// Filter condition type
        /// </summary>
        FilterCondition = 0,

        /// <summary>
        /// Condition of combination type
        /// </summary>
        CombinationCondition = 1,

        /// <summary>
        /// Condition merge result type
        /// </summary>
        MergeResultCondition = 2
    }
}
