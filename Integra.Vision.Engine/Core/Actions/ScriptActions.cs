//-----------------------------------------------------------------------
// <copyright file="ScriptActions.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Models;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Class that contains the object script actions.
    /// </summary>
    internal sealed class ScriptActions
    {
        /// <summary>
        /// Actual context.
        /// </summary>
        private ObjectsContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptActions"/> class
        /// </summary>
        /// <param name="context">Current context.</param>
        public ScriptActions(ObjectsContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Save the script of the object.
        /// </summary>
        /// <param name="script">Script text.</param>
        /// <param name="objectId">Object id.</param>
        public void SaveScript(string script, System.Guid objectId)
        {
            Script scriptObject = new Script() { LastUpdate = System.DateTime.Now, ScriptText = script, UserDefinedObjectId = objectId };
            Repository<Script> repo = new Repository<Script>(this.context);
            repo.Create(scriptObject);
        }

        /// <summary>
        /// Update the script of the object.
        /// </summary>
        /// <param name="script">Script text.</param>
        /// <param name="objectId">Object id.</param>
        public void UpdateScript(string script, System.Guid objectId)
        {
            Repository<Script> repo = new Repository<Script>(this.context);

            Script scriptObject = repo.Find(x => x.UserDefinedObjectId == objectId);
            scriptObject.LastUpdate = System.DateTime.Now;
            scriptObject.ScriptText = script;

            repo.Update(scriptObject);
        }
    }
}
