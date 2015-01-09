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
                // initialize context
                Integra.Vision.Engine.Database.Contexts.ViewsContext context = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

                this.SaveArguments(context, command as AlterUserCommand);
                return new QueryCommandResult();
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
        private void SaveArguments(ViewsContext vc, AlterUserCommand command)
        {
            // create repository
            Database.Repositories.Repository<Database.Models.User> repoUser = new Database.Repositories.Repository<Database.Models.User>(vc);

            // get the hash string
            MD5 md5Hash = MD5.Create();
            string hash = this.GetMd5Hash(md5Hash, command.Password);
            
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

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }

        /// <summary>
        /// Get hash code from password
        /// </summary>
        /// <param name="md5Hash">Doc1 goes here</param>
        /// <param name="input">Doc2 goes here</param>
        /// <returns>Doc3 goes here</returns>
        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder stringBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return stringBuilder.ToString();
        }
    }
}
