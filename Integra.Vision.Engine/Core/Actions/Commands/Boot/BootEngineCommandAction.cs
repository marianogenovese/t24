//-----------------------------------------------------------------------
// <copyright file="BootEngineCommandAction.cs" company="Ingetra.Vision.Engine">
//     Copyright (c) Ingetra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Models;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process to boot the engine.
    /// </summary>
    internal sealed class BootEngineCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            // initialize context
            using (ObjectsContext context = new ObjectsContext("EngineDatabase"))
            {
                return new BootCommandResult(this.Boot(context, command as BootEngineCommand));
            }
        }

        /// <summary>
        /// Contains the Boot Engine logic.
        /// </summary>
        /// <param name="context">Current context.</param>
        /// <param name="command">Boot engine command.</param>
        /// <returns>List of scripts.</returns>
        private Tuple<string, ObjectTypeEnum>[] Boot(ObjectsContext context, BootEngineCommand command)
        {
            // obtengo los objetos script
            Repository<UserDefinedObject> repoUserDefinedObject = new Repository<UserDefinedObject>(context);
            List<Tuple<string, ObjectTypeEnum>> userDefinedObjectList = new List<Tuple<string, ObjectTypeEnum>>();

            string type3 = ObjectTypeEnum.Source.ToString();
            int state1 = (int)UserDefinedObjectStateEnum.Started;
            int state2 = (int)UserDefinedObjectStateEnum.StoppedByError;

            IQueryable result = repoUserDefinedObject.Filter(x => (x.Type == type3) && (x.State == state1 || x.State == state2));

            foreach (UserDefinedObject userDefinedObject in result)
            {
                if (userDefinedObject.Type.Equals(type3))
                {
                    userDefinedObjectList.Add(new Tuple<string, ObjectTypeEnum>(userDefinedObject.Name, ObjectTypeEnum.Source));
                }
            }
            
            // ordena los objetos de forma ascedente, por eso es importante el valor que tienen en el ObjectTypeEnum
            userDefinedObjectList = userDefinedObjectList.OrderBy(x => x.Item2).ToList();

            return userDefinedObjectList.ToArray();
        }
    }
}
