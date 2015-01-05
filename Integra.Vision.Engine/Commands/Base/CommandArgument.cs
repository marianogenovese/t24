//-----------------------------------------------------------------------
// <copyright file="CommandArgument.cs" company="Integra.Vision.Engine">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    /// <summary>
    /// CommandArgument
    /// Doc goes here
    /// </summary>
    internal class CommandArgument : INamedElement
    {
        /// <summary>
        /// name
        /// Doc goes here
        /// </summary>
        private string name;

        /// <summary>
        /// value
        /// Doc goes here
        /// </summary>
        private object value;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandArgument"/> class
        /// </summary>
        /// <param name="name">name Doc goes here</param>
        /// <param name="value">value Doc goes here</param>
        public CommandArgument(string name, object value)
        {
            this.name = name;
            this.value = value;
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
        public object Value
        {
            get
            {
                return this.value;
            }
        }
    }
}
