//-----------------------------------------------------------------------
// <copyright file="CreateUserCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Extensions;

    /// <summary>
    /// Implements all the process of create a new user.
    /// </summary>
    internal sealed class CreateUserCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
                {
                    this.SaveArguments(context, command as CreateUserCommand);
                }

                return new OkCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Save user arguments
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Create user command</param>
        private void SaveArguments(ObjectsContext vc, CreateUserCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.User> repoUser = new Database.Repositories.Repository<Database.Models.User>(vc);

            MD5 md5Hash = MD5.Create();
            string hash = md5Hash.GetMd5Hash(command.Password);

            // create role
            Database.Models.User user = new Database.Models.User() { CreationDate = DateTime.Now, IsSystemObject = false, Name = command.Name, State = (int)command.Status, Password = hash, Type = ObjectTypeEnum.User.ToString(), SId = command.Name };
            repoUser.Create(user);

            // save the object script
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.SaveScript(command.Script, user.Id);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }
    }
}
