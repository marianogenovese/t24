//-----------------------------------------------------------------------
// <copyright file="CommandDependency.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{    
    /// <summary>
    /// CommandDependency
    /// Doc goes here
    /// </summary>
    internal class CommandDependency : INamedElement
    {
        /// <summary>
        /// Doc goes here
        /// </summary>
        private string name;

        /// <summary>
        /// Doc goes here
        /// </summary>
        private ObjectTypeEnum objectType;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDependency"/> class
        /// </summary>
        /// <param name="name">name doc goes here</param>
        /// <param name="objectType">objectType doc goes here</param>
        public CommandDependency(string name, ObjectTypeEnum objectType)
        {
            this.name = name;
            this.objectType = objectType;
        }

        /// <summary>
        /// Gets
        /// Doc goes here
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets
        /// Doc goes here
        /// </summary>
        public ObjectTypeEnum ObjectType
        {
            get
            {
                return this.objectType;
            }
        }
    }
}
