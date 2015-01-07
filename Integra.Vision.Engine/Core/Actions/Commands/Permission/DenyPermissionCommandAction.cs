﻿//-----------------------------------------------------------------------
// <copyright file="DenyPermissionCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process of deny permission to an object.
    /// </summary>
    internal sealed class DenyPermissionCommandAction : ExecutionCommandAction
    {
        /// <summary>
        /// Create adapter command
        /// </summary>
        private DenyPermissionCommand command;

        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            this.command = command as DenyPermissionCommand;

            try
            {
                this.SaveArguments();
                return new QueryCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }
        
        /// <summary>
        /// Contains deny permission logic
        /// </summary>
        private void SaveArguments()
        {
            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // create repository
            Database.Repositories.Repository<Database.Models.UserDefinedObject> repoObject = new Database.Repositories.Repository<Database.Models.UserDefinedObject>(vc);
            string objectName = this.command.SecureObjectName;

            if (this.command.SecureObjectType.Equals(ObjectTypeEnum.Role))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.User> repoUser = new Database.Repositories.Repository<Database.Models.User>(vc);
                Database.Repositories.Repository<Database.Models.Role> repoRole = new Database.Repositories.Repository<Database.Models.Role>(vc);
                Database.Repositories.Repository<Database.Models.PermissionUser> repoPermission = new Database.Repositories.Repository<Database.Models.PermissionUser>(vc);
                Database.Repositories.Repository<Database.Models.RoleMember> repoRoleMember = new Database.Repositories.Repository<Database.Models.RoleMember>(vc);

                // get the user
                string userName = this.command.AssignableObjectName;
                Database.Models.UserDefinedObject user = repoUser.Find(x => x.Name == userName);

                // get the role
                Database.Models.Role role = repoRole.Find(x => x.Name == objectName);

                if (repoRoleMember.Exists(x => x.RoleId == role.Id && x.UserId == user.Id))
                {
                    // update the user permission
                    Database.Models.PermissionUser permissionUser = repoPermission.Find(x => x.UserId == user.Id && x.UserDefinedObjectId == role.Id);
                    permissionUser.Type = false;
                    repoPermission.Update(permissionUser);
                }
                else
                {
                    // create the role member
                    Database.Models.RoleMember roleMember = new Database.Models.RoleMember() { RoleId = role.Id, UserId = user.Id, CreationDate = System.DateTime.Now };
                    repoRoleMember.Create(roleMember);

                    // create the user permission
                    Database.Models.PermissionUser permissionUser = new Database.Models.PermissionUser() { Type = false, UserId = user.Id, UserDefinedObjectId = role.Id, CreationDate = System.DateTime.Now };
                    repoPermission.Create(permissionUser);
                }
            }
            else
            {
                Database.Models.UserDefinedObject userDefinedObject = repoObject.Find(x => x.Name == objectName);

                if (this.command.AsignableObjectType.Equals(ObjectTypeEnum.User))
                {
                    // create repository
                    Database.Repositories.Repository<Database.Models.PermissionUser> repoPermission = new Database.Repositories.Repository<Database.Models.PermissionUser>(vc);
                    Database.Repositories.Repository<Database.Models.User> repoUser = new Database.Repositories.Repository<Database.Models.User>(vc);

                    // get the user
                    string userName = this.command.AssignableObjectName;
                    Database.Models.User user = repoUser.Find(x => x.Name == userName);

                    if (repoPermission.Exists(x => x.UserId == user.Id && x.UserDefinedObjectId == userDefinedObject.Id))
                    {
                        // update the user permission
                        Database.Models.PermissionUser permissionUser = repoPermission.Find(x => x.UserId == user.Id && x.UserDefinedObjectId == userDefinedObject.Id);
                        permissionUser.Type = false;
                        repoPermission.Update(permissionUser);
                    }
                    else
                    {
                        // create the user permission
                        Database.Models.PermissionUser permissionUser = new Database.Models.PermissionUser() { Type = false, UserId = user.Id, UserDefinedObjectId = userDefinedObject.Id, CreationDate = System.DateTime.Now };
                        repoPermission.Create(permissionUser);
                    }

                    // close connections
                    repoUser.Dispose();
                    repoPermission.Dispose();
                }
                else if (this.command.AsignableObjectType.Equals(ObjectTypeEnum.Role))
                {
                    // create repository
                    Database.Repositories.Repository<Database.Models.PermissionRole> repoPermission = new Database.Repositories.Repository<Database.Models.PermissionRole>(vc);
                    Database.Repositories.Repository<Database.Models.Role> repoRole = new Database.Repositories.Repository<Database.Models.Role>(vc);

                    // get the role
                    string roleName = this.command.AssignableObjectName;
                    Database.Models.Role role = repoRole.Find(x => x.Name == roleName);

                    if (repoPermission.Exists(x => x.RoleId == role.Id && x.UserDefinedObjectId == userDefinedObject.Id))
                    {
                        // update the role permission 
                        Database.Models.PermissionRole permissionRole = repoPermission.Find(x => x.RoleId == role.Id && x.UserDefinedObjectId == userDefinedObject.Id);
                        permissionRole.Type = false;
                        repoPermission.Update(permissionRole);
                    }
                    else
                    {
                        // create the role permission 
                        Database.Models.PermissionRole permissionRole = new Database.Models.PermissionRole() { Type = false, RoleId = role.Id, UserDefinedObjectId = userDefinedObject.Id, CreationDate = System.DateTime.Now };
                        repoPermission.Create(permissionRole);
                    }
                }
            }

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }
    }
}
