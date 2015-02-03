//-----------------------------------------------------------------------
// <copyright file="DbUserAuthenticator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Security.Cryptography;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Models;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Engine.Extensions;

    /// <summary>
    /// Implements a user authenticator which use the register users in a database.
    /// </summary>
    internal sealed class DbUserAuthenticator : IUserAuthenticator
    {
        /// <inheritdoc />
        public bool Validate(string user, string password)
        {
            using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
            {
                MD5 md5Hash = MD5.Create();
                string hash = md5Hash.GetMd5Hash(password);

                Repository<User> repoUser = new Repository<User>(context);
                bool exists = repoUser.Exists(x => x.Name == user && x.Password == hash);

                if (exists)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
