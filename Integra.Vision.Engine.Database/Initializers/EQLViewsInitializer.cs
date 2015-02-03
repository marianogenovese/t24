//-----------------------------------------------------------------------
// <copyright file="EQLViewsInitializer.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Initializers
{
    using System.Security.Cryptography;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Extensions;

    /// <summary>
    /// EQLDatabaseInitializer class
    /// </summary>
    internal sealed class EQLViewsInitializer : System.Data.Entity.CreateDatabaseIfNotExists<ObjectsContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EQLViewsInitializer"/> class
        /// </summary>
        /// <param name="context">doc goes here</param>
        protected override void Seed(ObjectsContext context)
        {
            var sqlFiles = System.IO.Directory.GetFiles(@"..\..\..\Integra.Vision.Engine.Database\bin\Debug\Scripts\Views", "*.sql");
            foreach (string file in sqlFiles)
            {
                context.Database.ExecuteSqlCommand(System.IO.File.ReadAllText(file));
            }

            MD5 md5Hash = MD5.Create();
            string hash = md5Hash.GetMd5Hash("vision");

            Models.User defaultUser = new Models.User()
            {
                Id = new System.Guid(),
                Name = "vision",
                Password = hash,
                CreationDate = System.DateTime.Now,
                IsSystemObject = true,
                SId = "vision",
                State = (int)UserDefinedObjectStateEnum.Started,
                Type = ObjectTypeEnum.User.ToString()
            };

            Repositories.Repository<Database.Models.User> userRepo = new Repositories.Repository<Models.User>(context);
            userRepo.Create(defaultUser);

            base.Seed(context);
        }
    }
}
