//-----------------------------------------------------------------------
// <copyright file="CommandTypeEnum.cs" company="Integra.Vision.Engine">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;

    /// <summary>
    /// CommandType
    /// Doc goes here
    /// </summary>
    [Flags]
    internal enum CommandTypeEnum : long
    {
        /// <summary>
        /// CreateAdapter
        /// Doc goes here
        /// </summary>
        CreateAdapter = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Create | ObjectTypeEnum.Adapter,

        /// <summary>
        /// AlterAdapter
        /// Doc goes here
        /// </summary>
        AlterAdapter = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Alter | ObjectTypeEnum.Adapter,
        
        /// <summary>
        /// DropAdapter
        /// Doc goes here
        /// </summary>
        DropAdapter = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Drop | ObjectTypeEnum.Adapter,

        /// <summary>
        /// CreateSource
        /// Doc goes here
        /// </summary>
        CreateSource = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Create | ObjectTypeEnum.Source,

        /// <summary>
        /// AlterSource
        /// Doc goes here
        /// </summary>
        AlterSource = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Alter | ObjectTypeEnum.Source,

        /// <summary>
        /// DropSource
        /// Doc goes here
        /// </summary>
        DropSource = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Drop | ObjectTypeEnum.Source,

        /// <summary>
        /// CreateStream
        /// Doc goes here
        /// </summary>
        CreateStream = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Create | ObjectTypeEnum.Stream,

        /// <summary>
        /// AlterStream
        /// Doc goes here
        /// </summary>
        AlterStream = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Alter | ObjectTypeEnum.Stream,
        
        /// <summary>
        /// DropStream
        /// Doc goes here
        /// </summary>
        DropStream = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Drop | ObjectTypeEnum.Stream,

        /// <summary>
        /// CreateTrigger
        /// Doc goes here
        /// </summary>
        CreateTrigger = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Create | ObjectTypeEnum.Trigger,

        /// <summary>
        /// AlterTrigger
        /// Doc goes here
        /// </summary>
        AlterTrigger = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Alter | ObjectTypeEnum.Trigger,

        /// <summary>
        /// DropTrigger
        /// Doc goes here
        /// </summary>
        DropTrigger = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Drop | ObjectTypeEnum.Trigger,

        /// <summary>
        /// CreateAssembly
        /// Doc goes here
        /// </summary>
        CreateAssembly = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Create | ObjectTypeEnum.Assembly,

        /// <summary>
        /// AlterAssembly
        /// Doc goes here
        /// </summary>
        AlterAssembly = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Alter | ObjectTypeEnum.Assembly,

        /// <summary>
        /// DropAssembly
        /// Doc goes here
        /// </summary>
        DropAssembly = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Drop | ObjectTypeEnum.Assembly,

        /// <summary>
        /// CreateRole
        /// Doc goes here
        /// </summary>
        CreateRole = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Create | ObjectTypeEnum.Role,

        /// <summary>
        /// DropRole
        /// Doc goes here
        /// </summary>
        DropRole = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Drop | ObjectTypeEnum.Role,

        /// <summary>
        /// CreateUser
        /// Doc goes here
        /// </summary>
        CreateUser = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Create | ObjectTypeEnum.User,

        /// <summary>
        /// AlterUser
        /// Doc goes here
        /// </summary>
        AlterUser = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Alter | ObjectTypeEnum.User,

        /// <summary>
        /// DropUser
        /// Doc goes here
        /// </summary>
        DropUser = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserDefinedObjectType | UserDefinedObjectActionEnum.Drop | ObjectTypeEnum.User,

        /// <summary>
        /// StartAdapter
        /// Doc goes here
        /// </summary>
        StartAdapter = CommandAccessLevelEnum.Public | CommandCategoryEnum.StartStopType | StartStopActionEnum.Start | ObjectTypeEnum.Adapter,

        /// <summary>
        /// StopAdapter
        /// Doc goes here
        /// </summary>
        StopAdapter = CommandAccessLevelEnum.Public | CommandCategoryEnum.StartStopType | StartStopActionEnum.Stop | ObjectTypeEnum.Adapter,

        /// <summary>
        /// StartSource
        /// Doc goes here
        /// </summary>
        StartSource = CommandAccessLevelEnum.Public | CommandCategoryEnum.StartStopType | StartStopActionEnum.Start | ObjectTypeEnum.Source,

        /// <summary>
        /// StopSource
        /// Doc goes here
        /// </summary>
        StopSource = CommandAccessLevelEnum.Public | CommandCategoryEnum.StartStopType | StartStopActionEnum.Stop | ObjectTypeEnum.Source,

        /// <summary>
        /// StartStream
        /// Doc goes here
        /// </summary>
        StartStream = CommandAccessLevelEnum.Public | CommandCategoryEnum.StartStopType | StartStopActionEnum.Start | ObjectTypeEnum.Stream,

        /// <summary>
        /// StopSource
        /// Doc goes here
        /// </summary>
        StopStream = CommandAccessLevelEnum.Public | CommandCategoryEnum.StartStopType | StartStopActionEnum.Stop | ObjectTypeEnum.Stream,

        /// <summary>
        /// StartTrigger
        /// Doc goes here
        /// </summary>
        StartTrigger = CommandAccessLevelEnum.Public | CommandCategoryEnum.StartStopType | StartStopActionEnum.Start | ObjectTypeEnum.Trigger,

        /// <summary>
        /// StopTrigger
        /// Doc goes here
        /// </summary>
        StopTrigger = CommandAccessLevelEnum.Public | CommandCategoryEnum.StartStopType | StartStopActionEnum.Stop | ObjectTypeEnum.Trigger,

        /// <summary>
        /// SetTraceAdapter
        /// Doc goes here
        /// </summary>
        SetTraceAdapter = CommandAccessLevelEnum.Public | CommandCategoryEnum.SetType | SetTypeEnum.Trace | ObjectTypeEnum.Adapter,

        /// <summary>
        /// SetTraceSource
        /// Doc goes here
        /// </summary>
        SetTraceSource = CommandAccessLevelEnum.Public | CommandCategoryEnum.SetType | SetTypeEnum.Trace | ObjectTypeEnum.Source,

        /// <summary>
        /// SetTraceStream
        /// Doc goes here
        /// </summary>
        SetTraceStream = CommandAccessLevelEnum.Public | CommandCategoryEnum.SetType | SetTypeEnum.Trace | ObjectTypeEnum.Stream,

        /// <summary>
        /// SetTraceTrigger
        /// Doc goes here
        /// </summary>
        SetTraceTrigger = CommandAccessLevelEnum.Public | CommandCategoryEnum.SetType | SetTypeEnum.Trace | ObjectTypeEnum.Trigger,

        /// <summary>
        /// SetTraceEngine
        /// Doc goes here
        /// </summary>
        SetTraceEngine = CommandAccessLevelEnum.Public | CommandCategoryEnum.SetType | SetTypeEnum.Trace | ObjectTypeEnum.Engine,

        /// <summary>
        /// SetTraceObject
        /// Doc goes here
        /// </summary>
        SetTraceObject = CommandAccessLevelEnum.Public | CommandCategoryEnum.SetType | SetTypeEnum.Trace | ObjectTypeEnum.SpecificObject,

        /// <summary>
        /// Grant
        /// Doc goes here
        /// </summary>
        Grant = CommandAccessLevelEnum.Public | CommandCategoryEnum.PermissionType | PermissionTypeEnum.Grant,

        /// <summary>
        /// Deny
        /// Doc goes here
        /// </summary>
        Deny = CommandAccessLevelEnum.Public | CommandCategoryEnum.PermissionType | PermissionTypeEnum.Deny,

        /// <summary>
        /// Revoke
        /// Doc goes here
        /// </summary>
        Revoke = CommandAccessLevelEnum.Public | CommandCategoryEnum.PermissionType | PermissionTypeEnum.Revoke,

        /// <summary>
        /// SystemQuery
        /// Doc goes here
        /// </summary>
        SystemQuery = CommandAccessLevelEnum.Public | CommandCategoryEnum.SystemQueriesType,

        /// <summary>
        /// Boot
        /// Doc goes here
        /// </summary>
        Boot = CommandAccessLevelEnum.Private | CommandCategoryEnum.AdminType | AdminCommandTypeEnum.BootEngine,

        /// <summary>
        /// LoadAssembly
        /// Doc goes here
        /// </summary>
        LoadAssembly = CommandAccessLevelEnum.Private | CommandCategoryEnum.AdminType | AdminCommandTypeEnum.LoadAssembly
    }
}