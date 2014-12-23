//-----------------------------------------------------------------------
// <copyright file="StmtTypeEnum.cs" company="Integra.Vision.Common">
//     Copyright (c) Integra.Vision.Common. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    /// <summary>
    /// Statement Type enumerator
    /// </summary>
    internal enum StmtTypeEnum
    {
        /// <summary>
        /// SendAlways type
        /// </summary>
        SendAlways = 0,

        /// <summary>
        /// SendIfHasEvents type
        /// </summary>
        SendIfHasEvents = 1,

        /// <summary>
        /// SendIfNotHasEvents type
        /// </summary>
        SendIfNotHasEvents = 2
    }
}
