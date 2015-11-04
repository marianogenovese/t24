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

            // ConnectRole
            Models.Role connectRole = new Models.Role()
            {
                Id = new System.Guid(),
                Name = "ConnectRole",
                CreationDate = System.DateTime.Now,
                IsServerRole = true,
                IsSystemObject = true,
                State = (int)UserDefinedObjectStateEnum.Started,
                Type = ObjectTypeEnum.Role.ToString()
            };

            // SystemAdminRole
            Models.Role systemAdminRole = new Models.Role()
            {
                Id = new System.Guid(),
                Name = "SystemAdminRole",
                CreationDate = System.DateTime.Now,
                IsServerRole = true,
                IsSystemObject = true,
                State = (int)UserDefinedObjectStateEnum.Started,
                Type = ObjectTypeEnum.Role.ToString()
            };

            // AdapterAdminRole 
            Models.Role adapterAdminRole = new Models.Role()
            {
                Id = new System.Guid(),
                Name = "AdapterAdminRole",
                CreationDate = System.DateTime.Now,
                IsServerRole = true,
                IsSystemObject = true,
                State = (int)UserDefinedObjectStateEnum.Started,
                Type = ObjectTypeEnum.Role.ToString()
            };

            // SourceAdminRole 
            Models.Role sourceAdminRole = new Models.Role()
            {
                Id = new System.Guid(),
                Name = "SourceAdminRole",
                CreationDate = System.DateTime.Now,
                IsServerRole = true,
                IsSystemObject = true,
                State = (int)UserDefinedObjectStateEnum.Started,
                Type = ObjectTypeEnum.Role.ToString()
            };

            // StreamAdminRole 
            Models.Role streamAdminRole = new Models.Role()
            {
                Id = new System.Guid(),
                Name = "StreamAdminRole",
                CreationDate = System.DateTime.Now,
                IsServerRole = true,
                IsSystemObject = true,
                State = (int)UserDefinedObjectStateEnum.Started,
                Type = ObjectTypeEnum.Role.ToString()
            };

            // TriggerAdminRole 
            Models.Role triggerAdminRole = new Models.Role()
            {
                Id = new System.Guid(),
                Name = "TriggerAdminRole",
                CreationDate = System.DateTime.Now,
                IsServerRole = true,
                IsSystemObject = true,
                State = (int)UserDefinedObjectStateEnum.Started,
                Type = ObjectTypeEnum.Role.ToString()
            };

            // SystemReaderRole
            Models.Role systemReaderRole = new Models.Role()
            {
                Id = new System.Guid(),
                Name = "SystemReaderRole",
                CreationDate = System.DateTime.Now,
                IsServerRole = true,
                IsSystemObject = true,
                State = (int)UserDefinedObjectStateEnum.Started,
                Type = ObjectTypeEnum.Role.ToString()
            };

            // AdapterReaderRole 
            Models.Role adapterReaderRole = new Models.Role()
            {
                Id = new System.Guid(),
                Name = "AdapterReaderRole",
                CreationDate = System.DateTime.Now,
                IsServerRole = true,
                IsSystemObject = true,
                State = (int)UserDefinedObjectStateEnum.Started,
                Type = ObjectTypeEnum.Role.ToString()
            };

            // SourceReaderRole 
            Models.Role sourceReaderRole = new Models.Role()
            {
                Id = new System.Guid(),
                Name = "SourceReaderRole",
                CreationDate = System.DateTime.Now,
                IsServerRole = true,
                IsSystemObject = true,
                State = (int)UserDefinedObjectStateEnum.Started,
                Type = ObjectTypeEnum.Role.ToString()
            };

            // StreamReaderRole 
            Models.Role streamReaderRole = new Models.Role()
            {
                Id = new System.Guid(),
                Name = "StreamReaderRole",
                CreationDate = System.DateTime.Now,
                IsServerRole = true,
                IsSystemObject = true,
                State = (int)UserDefinedObjectStateEnum.Started,
                Type = ObjectTypeEnum.Role.ToString()
            };

            // TriggerReaderRole
            Models.Role triggerReaderRole = new Models.Role()
            {
                Id = new System.Guid(),
                Name = "TriggerReaderRole",
                CreationDate = System.DateTime.Now,
                IsServerRole = true,
                IsSystemObject = true,
                State = (int)UserDefinedObjectStateEnum.Started,
                Type = ObjectTypeEnum.Role.ToString()
            };

            Repositories.Repository<Models.Role> repoRole = new Repositories.Repository<Models.Role>(context);
            repoRole.Create(connectRole);
            repoRole.Create(systemAdminRole);
            repoRole.Create(adapterAdminRole);
            repoRole.Create(sourceAdminRole);
            repoRole.Create(streamAdminRole);
            repoRole.Create(triggerAdminRole);
            repoRole.Create(systemReaderRole);
            repoRole.Create(adapterReaderRole);
            repoRole.Create(sourceReaderRole);
            repoRole.Create(streamReaderRole);
            repoRole.Create(triggerReaderRole);

            base.Seed(context);
        }
    }
}
