﻿//-----------------------------------------------------------------------
// <copyright file="RevokePermissionCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// Implements all the process of revoke permission to an object.
    /// </summary>
    internal sealed class RevokePermissionCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ViewsContext context = new ViewsContext("EngineDatabase"))
                {
                    this.SaveArguments(context, command as RevokePermissionCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }
        
        /// <summary>
        /// Contains revoke permission logic
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Revoke command</param>
        private void SaveArguments(ViewsContext vc, RevokePermissionCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.UserDefinedObject> repoObject = new Database.Repositories.Repository<Database.Models.UserDefinedObject>(vc);
            string objectName = command.SecureObjectName;

            if (command.SecureObjectType.Equals(ObjectTypeEnum.Role))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.User> repoUser = new Database.Repositories.Repository<Database.Models.User>(vc);
                Database.Repositories.Repository<Database.Models.Role> repoRole = new Database.Repositories.Repository<Database.Models.Role>(vc);
                Database.Repositories.Repository<Database.Models.RoleMember> repoRoleMember = new Database.Repositories.Repository<Database.Models.RoleMember>(vc);
                Database.Repositories.Repository<Database.Models.PermissionUser> repoPermission = new Database.Repositories.Repository<Database.Models.PermissionUser>(vc);

                // get the user
                string userName = command.AsignableObjectName;
                Database.Models.UserDefinedObject user = repoUser.Find(x => x.Name == userName);

                // get the role
                Database.Models.Role role = repoRole.Find(x => x.Name == objectName);

                // delete the role member
                repoRoleMember.Delete(x => x.RoleId == role.Id && x.UserId == user.Id);

                // delete the user permission
                repoPermission.Delete(x => x.UserDefinedObjectId == role.Id && x.UserId == user.Id);
            }
            else
            {
                Database.Models.UserDefinedObject userDefinedObject = repoObject.Find(x => x.Name == objectName);

                if (command.AssignableObjectType.Equals(ObjectTypeEnum.User))
                {
                    // create repository
                    Database.Repositories.Repository<Database.Models.PermissionUser> repoPermission = new Database.Repositories.Repository<Database.Models.PermissionUser>(vc);
                    Database.Repositories.Repository<Database.Models.User> repoUser = new Database.Repositories.Repository<Database.Models.User>(vc);

                    // get the user
                    string userName = command.AsignableObjectName;
                    Database.Models.User user = repoUser.Find(x => x.Name == userName);

                    // delete the user permission
                    repoPermission.Delete(x => x.UserDefinedObjectId == userDefinedObject.Id && x.UserId == user.Id);
                }
                else if (command.AssignableObjectType.Equals(ObjectTypeEnum.Role))
                {
                    // create repository
                    Database.Repositories.Repository<Database.Models.PermissionRole> repoPermission = new Database.Repositories.Repository<Database.Models.PermissionRole>(vc);
                    Database.Repositories.Repository<Database.Models.Role> repoRole = new Database.Repositories.Repository<Database.Models.Role>(vc);

                    // get the role
                    string roleName = command.AsignableObjectName;
                    Database.Models.Role role = repoRole.Find(x => x.Name == roleName);

                    // delete the role permission 
                    repoPermission.Delete(x => x.UserDefinedObjectId == userDefinedObject.Id && x.RoleId == role.Id);
                }
            }

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }
    }
}