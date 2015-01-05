//-----------------------------------------------------------------------
// <copyright file="DenyPermissionCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Permission.Deny
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for deny permissions
    /// </summary>
    internal sealed class DenyPermissionCommand : PermissionCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new DenyPermissionArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new DenyPermissionDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="DenyPermissionCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public DenyPermissionCommand(PlanNode node) : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.Deny;
            }
        }

        /// <summary>
        /// Gets command argument enumerator
        /// </summary>
        protected override IArgumentEnumerator ArgumentEnumerator
        {
            get
            {
                return this.argumentEnumerator;
            }
        }

        /// <summary>
        /// Gets command dependency enumerator
        /// </summary>
        protected override IDependencyEnumerator DependencyEnumerator
        {
            get
            {
                return this.dependencyEnumerator;
            }
        }

        /// <summary>
        /// Contains deny permission logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // create repository
            Database.Repositories.Repository<Database.Models.UserDefinedObject> repoObject = new Database.Repositories.Repository<Database.Models.UserDefinedObject>(vc);
            string objectName = this.Arguments["SecureObjectName"].Value.ToString();

            if (this.Arguments["SecureObjectType"].Value.ToString().ToLower().Contains(ObjectTypeEnum.Role.ToString().ToLower()))
            {
                // create repository
                Database.Repositories.Repository<Database.Models.User> repoUser = new Database.Repositories.Repository<Database.Models.User>(vc);
                Database.Repositories.Repository<Database.Models.Role> repoRole = new Database.Repositories.Repository<Database.Models.Role>(vc);
                Database.Repositories.Repository<Database.Models.PermissionUser> repoPermission = new Database.Repositories.Repository<Database.Models.PermissionUser>(vc);
                Database.Repositories.Repository<Database.Models.RoleMember> repoRoleMember = new Database.Repositories.Repository<Database.Models.RoleMember>(vc);

                // get the user
                string userName = this.Arguments["AsignableObjectName"].Value.ToString();
                Database.Models.UserDefinedObject user = repoUser.Find(x => x.Name == userName);

                // get the role
                Database.Models.Role role = repoRole.Find(x => x.Name == objectName);

                if (repoRoleMember.Exists(x => x.RoleId == role.Id && x.UserId == user.Id))
                {
                    // update the user permission
                    Database.Models.PermissionUser permissionUser = repoPermission.Find(x => x.UserId == user.Id && x.UserDefinedObjectId == role.Id);
                    permissionUser.Type = false;
                    repoPermission.Update(permissionUser);
                    repoPermission.Commit();
                }
                else
                {
                    // create the role member
                    Database.Models.RoleMember roleMember = new Database.Models.RoleMember() { RoleId = role.Id, UserId = user.Id, CreationDate = System.DateTime.Now };
                    repoRoleMember.Create(roleMember);
                    repoRoleMember.Commit();

                    // create the user permission
                    Database.Models.PermissionUser permissionUser = new Database.Models.PermissionUser() { Type = false, UserId = user.Id, UserDefinedObjectId = role.Id, CreationDate = System.DateTime.Now };
                    repoPermission.Create(permissionUser);
                    repoPermission.Commit();
                }

                // close connections
                repoUser.Dispose();
                repoRole.Dispose();
                repoPermission.Dispose();
            }
            else
            {            
                Database.Models.UserDefinedObject userDefinedObject = repoObject.Find(x => x.Name == objectName);

                if (this.Arguments["AsignableObjectType"].Value.ToString().ToLower().Equals(ObjectTypeEnum.User.ToString().ToLower()))
                {
                    // create repository
                    Database.Repositories.Repository<Database.Models.PermissionUser> repoPermission = new Database.Repositories.Repository<Database.Models.PermissionUser>(vc);
                    Database.Repositories.Repository<Database.Models.User> repoUser = new Database.Repositories.Repository<Database.Models.User>(vc);

                    // get the user
                    string userName = this.Arguments["AsignableObjectName"].Value.ToString();
                    Database.Models.User user = repoUser.Find(x => x.Name == userName);

                    if (repoPermission.Exists(x => x.UserId == user.Id && x.UserDefinedObjectId == userDefinedObject.Id))
                    {
                        // update the user permission
                        Database.Models.PermissionUser permissionUser = repoPermission.Find(x => x.UserId == user.Id && x.UserDefinedObjectId == userDefinedObject.Id);
                        permissionUser.Type = false;
                        repoPermission.Update(permissionUser);
                        repoPermission.Commit();
                    }
                    else
                    {
                        // create the user permission
                        Database.Models.PermissionUser permissionUser = new Database.Models.PermissionUser() { Type = false, UserId = user.Id, UserDefinedObjectId = userDefinedObject.Id, CreationDate = System.DateTime.Now };
                        repoPermission.Create(permissionUser);
                        repoPermission.Commit();
                    }
                    
                    // close connections
                    repoUser.Dispose();
                    repoPermission.Dispose();
                }
                else if (this.Arguments["AsignableObjectType"].Value.ToString().ToLower().Equals(ObjectTypeEnum.Role.ToString().ToLower()))
                {
                    // create repository
                    Database.Repositories.Repository<Database.Models.PermissionRole> repoPermission = new Database.Repositories.Repository<Database.Models.PermissionRole>(vc);
                    Database.Repositories.Repository<Database.Models.Role> repoRole = new Database.Repositories.Repository<Database.Models.Role>(vc);

                    // get the role
                    string roleName = this.Arguments["AsignableObjectName"].Value.ToString();
                    Database.Models.Role role = repoRole.Find(x => x.Name == roleName);

                    if (repoPermission.Exists(x => x.RoleId == role.Id && x.UserDefinedObjectId == userDefinedObject.Id))
                    {
                        // update the role permission 
                        Database.Models.PermissionRole permissionRole = repoPermission.Find(x => x.RoleId == role.Id && x.UserDefinedObjectId == userDefinedObject.Id);
                        permissionRole.Type = false;
                        repoPermission.Update(permissionRole);
                        repoPermission.Commit();
                    }
                    else
                    {
                        // create the role permission 
                        Database.Models.PermissionRole permissionRole = new Database.Models.PermissionRole() { Type = false, RoleId = role.Id, UserDefinedObjectId = userDefinedObject.Id, CreationDate = System.DateTime.Now };
                        repoPermission.Create(permissionRole);
                        repoPermission.Commit();
                    }

                    // close connections
                    repoRole.Dispose();
                    repoPermission.Dispose();
                }
            }

            // close connections
            repoObject.Dispose();
            vc.Dispose();
        }
    }
}
