//-----------------------------------------------------------------------
// <copyright file="EQLViewsInitializer.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Initializers
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Models;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// EQLDatabaseInitializer class
    /// </summary>
    internal sealed class EQLViewsInitializer : System.Data.Entity.CreateDatabaseIfNotExists<ViewsContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EQLViewsInitializer"/> class
        /// </summary>
        /// <param name="context">doc goes here</param>
        protected override void Seed(ViewsContext context)
        {
            var sqlFiles = System.IO.Directory.GetFiles(@"..\..\..\Integra.Vision.Engine.Database\bin\Debug\Scripts\Views", "*.sql");
            foreach (string file in sqlFiles)
            {
                context.Database.ExecuteSqlCommand(System.IO.File.ReadAllText(file));
            }

            base.Seed(context);
        }
    }
}
