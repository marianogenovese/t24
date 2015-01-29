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
        /// UserQuery
        /// Doc goes here
        /// </summary>
        UserQuery = CommandAccessLevelEnum.Public | CommandCategoryEnum.UserQueriesType,

        /// <summary>
        /// Boot
        /// Doc goes here
        /// </summary>
        Boot = CommandAccessLevelEnum.Private | CommandCategoryEnum.AdminType | AdminCommandTypeEnum.BootEngine,
        
        /// <summary>
        /// Publish
        /// Doc goes here
        /// </summary>
        Publish = CommandAccessLevelEnum.Public | CommandCategoryEnum.ActionType | ActionTypeEnum.Publish,

        /// <summary>
        /// Receive
        /// Doc goes here
        /// </summary>
        Receive = CommandAccessLevelEnum.Public | CommandCategoryEnum.ActionType | ActionTypeEnum.Receive
    }
}