//-----------------------------------------------------------------------
// <copyright file="CreateUserCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateUser
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for create users
    /// </summary>
    internal class CreateUserCommand : CreateObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new CreateUserArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new CreateUserDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public CreateUserCommand(PlanNode node)
            : base(node)
        {
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
        /// Save user arguments
        /// </summary>
        public virtual void SaveArguments()
        {
            // initialize context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // create repository
            Database.Repositories.Repository<Database.Models.User> repoUser = new Database.Repositories.Repository<Database.Models.User>(vc);

            int status = -1;
            if (this.Arguments["Status"].Value.ToString().ToLower().Equals(UserStatusEnum.Enable.ToString().ToLower()))
            {
                status = (int)UserStatusEnum.Enable;
            }
            else if (this.Arguments["Status"].Value.ToString().ToLower().Equals(UserStatusEnum.Disable.ToString().ToLower()))
            {
                status = (int)UserStatusEnum.Disable;
            }

            MD5 md5Hash = MD5.Create();
            string hash = this.GetMd5Hash(md5Hash, this.Arguments["Password"].Value.ToString());

            // create role
            Database.Models.User user = new Database.Models.User() { CreationDate = DateTime.Now, IsSystemObject = false, Name = this.Arguments["Name"].Value.ToString(), State = status, Password = hash, Type = ObjectTypeEnum.User.ToString(), SId = this.Arguments["Name"].Value.ToString() };
            repoUser.Create(user);
            repoUser.Commit();

            // close connections
            repoUser.Dispose();
            vc.Dispose();
        }

        /// <summary>
        /// Contains create user logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // save arguments
            this.SaveArguments();
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
