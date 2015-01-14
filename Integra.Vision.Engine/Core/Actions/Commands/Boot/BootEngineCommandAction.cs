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
            using (ViewsContext context = new ViewsContext("EngineDatabase"))
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
        private Tuple<string, ObjectTypeEnum>[] Boot(ViewsContext context, BootEngineCommand command)
        {
            // obtengo los objetos script
            Repository<UserDefinedObject> repoUserDefinedObject = new Repository<UserDefinedObject>(context);
            List<Tuple<string, ObjectTypeEnum>> userDefinedObjectList = new List<Tuple<string, ObjectTypeEnum>>();

            string type1 = ObjectTypeEnum.Assembly.ToString();
            string type2 = ObjectTypeEnum.Adapter.ToString();
            string type3 = ObjectTypeEnum.Source.ToString();
            string type4 = ObjectTypeEnum.Stream.ToString();
            string type5 = ObjectTypeEnum.Trigger.ToString();
            int state1 = (int)UserDefinedObjectStateEnum.Started;
            int state2 = (int)UserDefinedObjectStateEnum.StoppedByError;

            IQueryable result = repoUserDefinedObject.Filter(x => (x.Type == type1 || x.Type == type2 || x.Type == type3 || x.Type == type4 || x.Type == type5) && (x.State == state1 || x.State == state2));

            foreach (UserDefinedObject userDefinedObject in result)
            {
                if (userDefinedObject.Type.Equals(type1))
                {
                    userDefinedObjectList.Add(new Tuple<string, ObjectTypeEnum>(userDefinedObject.Name, ObjectTypeEnum.Assembly));
                }
                else if (userDefinedObject.Type.Equals(type2))
                {
                    userDefinedObjectList.Add(new Tuple<string, ObjectTypeEnum>(userDefinedObject.Name, ObjectTypeEnum.Adapter));
                }
                else if (userDefinedObject.Type.Equals(type3))
                {
                    userDefinedObjectList.Add(new Tuple<string, ObjectTypeEnum>(userDefinedObject.Name, ObjectTypeEnum.Source));
                }
                else if (userDefinedObject.Type.Equals(type4))
                {
                    userDefinedObjectList.Add(new Tuple<string, ObjectTypeEnum>(userDefinedObject.Name, ObjectTypeEnum.Stream));
                }
                else if (userDefinedObject.Type.Equals(type2))
                {
                    userDefinedObjectList.Add(new Tuple<string, ObjectTypeEnum>(userDefinedObject.Name, ObjectTypeEnum.Trigger));
                }
            }
            
            // ordena los objetos de forma ascedente, por eso es importante el valor que tienen en el ObjectTypeEnum
            userDefinedObjectList = userDefinedObjectList.OrderBy(x => x.Item2).ToList();

            return userDefinedObjectList.ToArray();
        }
    }
}
