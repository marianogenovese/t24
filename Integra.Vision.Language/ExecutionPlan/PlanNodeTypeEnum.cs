//-----------------------------------------------------------------------
// <copyright file="PlanNodeTypeEnum.cs" company="Integra.Vision.Common">
//     Copyright (c) Integra.Vision.Common. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language
{
    /// <summary>
    /// PlanType enumerable
    /// </summary>
    internal enum PlanNodeTypeEnum : uint
    {
        /// <summary>
        /// Constant type
        /// </summary>
        Constant = 10,

        /// <summary>
        /// Cast type
        /// </summary>
        Cast = 21,

        /// <summary>
        /// Equal type
        /// </summary>
        Equal = 31,

        /// <summary>
        /// NotEqual type
        /// </summary>
        NotEqual = 32,

        /// <summary>
        /// LessThan type
        /// </summary>
        LessThan = 33,

        /// <summary>
        /// LessThanOrEqual type
        /// </summary>
        LessThanOrEqual = 34,

        /// <summary>
        /// GreaterThan type
        /// </summary>
        GreaterThan = 35,

        /// <summary>
        /// GreaterThanOrEqual type
        /// </summary>
        GreaterThanOrEqual = 36,

        /// <summary>
        /// Not type
        /// </summary>
        Not = 37,

        /// <summary>
        /// Like type
        /// </summary>
        Like = 38,

        /// <summary>
        /// Or type
        /// </summary>
        Or = 50,

        /// <summary>
        /// And type
        /// </summary>
        And = 51,

        /// <summary>
        /// Event type
        /// </summary>
        Event = 60,

        /// <summary>
        /// Timestamp type
        /// </summary>
        Timestamp = 61,

        /// <summary>
        /// Name type
        /// </summary>
        Name = 62,

        /// <summary>
        /// Adapter type
        /// </summary>
        Adapter = 63,

        /// <summary>
        /// Agent type
        /// </summary>
        Agent = 64,

        /// <summary>
        /// Source type
        /// </summary>
        Source = 70,

        /// <summary>
        /// ObjectPart type
        /// </summary>
        ObjectPart = 71,

        /// <summary>
        /// ObjectField type
        /// </summary>
        ObjectField = 72,

        /// <summary>
        /// ObjectValue type
        /// </summary>
        ObjectValue = 73,

        /// <summary>
        /// ObjectMessage type
        /// </summary>
        ObjectMessage = 74,

        /// <summary>
        /// ObjectWithPrefix type
        /// </summary>
        ObjectWithPrefix = 75,

        /// <summary>
        /// Negate type
        /// </summary>
        Negate = 90,

        /// <summary>
        /// Subtract type
        /// </summary>
        Subtract = 91,

        /// <summary>
        /// Property type
        /// </summary>
        Property = 100,

        /// <summary>
        /// TupleProjection type
        /// </summary>
        TupleProjection = 109,

        /// <summary>
        /// Select type
        /// </summary>
        Select = 110,

        /// <summary>
        /// From type
        /// </summary>
        From = 111,

        /// <summary>
        /// Where type
        /// </summary>
        Where = 112,

        /// <summary>
        /// On type
        /// </summary>
        On = 113,

        /// <summary>
        /// Join type
        /// </summary>
        Join = 114,

        /// <summary>
        /// With type
        /// </summary>
        With = 115,

        /// <summary>
        /// ApplyWindow type
        /// </summary>
        ApplyWindow = 116,

        /// <summary>
        /// ValueWithAlias type
        /// </summary>
        ValueWithAlias = 120,

        /// <summary>
        /// If type
        /// </summary>
        IfHasEvents = 130,

        /// <summary>
        /// Send type
        /// </summary>
        Send = 131,

        /// <summary>
        /// If not type
        /// </summary>
        IfNotHasEvents = 132,

        /// <summary>
        /// status type
        /// </summary>
        Status = 133
    }
}