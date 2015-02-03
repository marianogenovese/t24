//-----------------------------------------------------------------------
// <copyright file="AlterUserCommandAction.cs" company="Ingetra.Vision.Engine">
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
    internal sealed class AlterUserCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
                {
                    this.SaveArguments(context, command as AlterUserCommand);
                    return new OkCommandResult();
                }
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains alter user logic.
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Alter user command</param>
        private void SaveArguments(ObjectsContext vc, AlterUserCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.User> repoUser = new Database.Repositories.Repository<Database.Models.User>(vc);

            // get the hash string
            MD5 md5Hash = MD5.Create();
            string hash = md5Hash.GetMd5Hash(command.Password);
            
            // get the user
            Database.Models.User user = repoUser.Find(x => x.Name == command.Name);

            // update the user arguments
            user.CreationDate = DateTime.Now;
            user.IsSystemObject = false;
            user.State = (int)command.Status;
            user.Password = hash;
            user.Type = ObjectTypeEnum.User.ToString();
            user.SId = command.Name;
            
            // update the user
            repoUser.Update(user);

            // update the object script
            ScriptActions scriptActions = new ScriptActions(vc);
            scriptActions.UpdateScript(command.Script, user.Id);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }
    }
}
