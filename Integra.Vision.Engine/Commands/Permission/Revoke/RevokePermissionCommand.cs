//-----------------------------------------------------------------------
// <copyright file="RevokePermissionCommand.cs" company="Intega.Vision.Engine">
//     Copyright (c) Intega.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Permission.Revoke
{
    /// <summary>
    /// Base class for revoke permissions
    /// </summary>
    internal sealed class RevokePermissionCommand : PermissionCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new RevokePermissionArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new RevokePermissionDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="RevokePermissionCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public RevokePermissionCommand(string commandText, ISecurityContext securityContext) : base(CommandTypeEnum.Revoke, commandText, securityContext)
        {
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
        /// Contains revoke permission logic
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
                Database.Repositories.Repository<Database.Models.RoleMember> repoRoleMember = new Database.Repositories.Repository<Database.Models.RoleMember>(vc);
                Database.Repositories.Repository<Database.Models.PermissionUser> repoPermission = new Database.Repositories.Repository<Database.Models.PermissionUser>(vc);

                // get the user
                string userName = this.Arguments["AsignableObjectName"].Value.ToString();
                Database.Models.UserDefinedObject user = repoUser.Find(x => x.Name == userName);

                // get the role
                Database.Models.Role role = repoRole.Find(x => x.Name == objectName);

                // delete the role member
                repoRoleMember.Delete(x => x.RoleId == role.Id && x.UserId == user.Id);
                repoRoleMember.Commit();

                // delete the user permission
                repoPermission.Delete(x => x.UserDefinedObjectId == role.Id && x.UserId == user.Id);
                repoPermission.Commit();

                // close connections
                repoUser.Dispose();
                repoRole.Dispose();
                repoRoleMember.Dispose();
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

                    // delete the user permission
                    repoPermission.Delete(x => x.UserDefinedObjectId == userDefinedObject.Id && x.UserId == user.Id);
                    repoPermission.Commit();

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

                    // delete the role permission 
                    repoPermission.Delete(x => x.UserDefinedObjectId == userDefinedObject.Id && x.RoleId == role.Id);
                    repoPermission.Commit();

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
