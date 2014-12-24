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
        /// Status type
        /// </summary>
        Status = 133,

        /// <summary>
        /// Create assembly type.
        /// </summary>
        CreateAssembly = 150,

        /// <summary>
        /// Create adapter type.
        /// </summary>
        CreateAdapter = 151,

        /// <summary>
        /// Create source type.
        /// </summary>
        CreateSource = 152,

        /// <summary>
        /// Create stream type.
        /// </summary>
        CreateStream = 153,

        /// <summary>
        /// Create trigger type.
        /// </summary>
        CreateTrigger = 154,

        /// <summary>
        /// Create user type.
        /// </summary>
        CreateUser = 155,

        /// <summary>
        /// Create role type.
        /// </summary>
        CreateRole = 156,

        /// <summary>
        /// Drop assembly type.
        /// </summary>
        DropAssembly = 160,

        /// <summary>
        /// Drop adapter type.
        /// </summary>
        DropAdapter = 161,

        /// <summary>
        /// Drop source type.
        /// </summary>
        DropSource = 162,

        /// <summary>
        /// Drop stream type.
        /// </summary>
        DropStream = 163,

        /// <summary>
        /// Drop trigger type.
        /// </summary>
        DropTrigger = 164,

        /// <summary>
        /// Drop user type.
        /// </summary>
        DropUser = 165,

        /// <summary>
        /// Drop role type.
        /// </summary>
        DropRole = 166,

        /// <summary>
        /// Alter adapter type.
        /// </summary>
        AlterAdapter = 170,

        /// <summary>
        /// Alter source type.
        /// </summary>
        AlterSource = 171,

        /// <summary>
        /// Alter stream type.
        /// </summary>
        AlterStream = 172,

        /// <summary>
        /// Alter trigger type.
        /// </summary>
        AlterTrigger = 173,

        /// <summary>
        /// Alter user type.
        /// </summary>
        AlterUser = 174,

        /// <summary>
        /// Start assembly type.
        /// </summary>
        StartAssembly = 180,

        /// <summary>
        /// Start adapter type.
        /// </summary>
        StartAdapter = 181,

        /// <summary>
        /// Start source type.
        /// </summary>
        StartSource = 182,

        /// <summary>
        /// Start stream type.
        /// </summary>
        StartStream = 183,

        /// <summary>
        /// Start trigger type.
        /// </summary>
        StartTrigger = 184,

        /// <summary>
        /// Start user type.
        /// </summary>
        StartUser = 185,

        /// <summary>
        /// Start role type.
        /// </summary>
        StartRole = 186,

        /// <summary>
        /// Stop assembly type.
        /// </summary>
        StopAssembly = 180,

        /// <summary>
        /// Stop adapter type.
        /// </summary>
        StopAdapter = 181,

        /// <summary>
        /// Stop source type.
        /// </summary>
        StopSource = 182,

        /// <summary>
        /// Stop stream type.
        /// </summary>
        StopStream = 183,

        /// <summary>
        /// Stop trigger type.
        /// </summary>
        StopTrigger = 184,

        /// <summary>
        /// Stop user type.
        /// </summary>
        StopUser = 185,

        /// <summary>
        /// Stop role type.
        /// </summary>
        StopRole = 186,

        /// <summary>
        /// Grant type
        /// </summary>
        Grant = 190,

        /// <summary>
        /// Deny type
        /// </summary>
        Deny = 191,

        /// <summary>
        /// Revoke type
        /// </summary>
        Revoke = 192,

        /// <summary>
        /// Set trace adapter type.
        /// </summary>
        SetTraceAdapter = 200,

        /// <summary>
        /// Set trace source type.
        /// </summary>
        SetTraceSource = 201,

        /// <summary>
        /// Set trace stream type.
        /// </summary>
        SetTraceStream = 202,

        /// <summary>
        /// Set trace trigger type.
        /// </summary>
        SetTraceTrigger = 203,

        /// <summary>
        /// Set trace engine type.
        /// </summary>
        SetTraceEngine = 204,
        
        /// <summary>
        /// Set trace to specific object type
        /// </summary>
        SpecificObject = 205,

        /// <summary>
        /// System query type.
        /// </summary>
        SystemQuery = 210        
    }
}