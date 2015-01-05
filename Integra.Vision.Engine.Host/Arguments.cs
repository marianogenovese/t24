//-----------------------------------------------------------------------
// <copyright file="Arguments.cs" company="CompanyName">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    
    /// <summary>
    /// Implements argument parsing in format key=value, that is passed in console mode of engine hosting
    /// </summary>
    internal sealed class Arguments : IEnumerable<Argument>
    {
        /// <summary>
        /// Key/Value regex parser, used for decompose name and value of an argument
        /// </summary>
        private readonly Regex nameParser = new Regex(@"^(?<flag>-|/)(?<name>[^:=]+)((?<sep>[:=])(?<value>.*))?$", RegexOptions.IgnoreCase | RegexOptions.Compiled); // private readonly Regex nameParser = new Regex(@"^(?<f>--|-|/)(?<name>[^:=]+)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);        

        // private readonly Regex nameParser = new Regex(@"(?<name>[^=]+)=?((?<quoted>\""?)(?<value>(?(quoted)[^\""]+|[^,]+))\""?,?)*", RegexOptions.IgnoreCase | RegexOptions.Compiled);        
        
        /// <summary>
        /// Internal collection of arguments
        /// </summary>
        private HashSet<Argument> argumentSet = new HashSet<Argument>();

        /// <summary>
        /// Gets the value associated with the specific argument name
        /// </summary>
        /// <param name="name">Name of the argument</param>
        /// <returns>Value of the argument</returns>
        public string this[string name]
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Add a new argument to the set of arguments
        /// </summary>
        /// <param name="name">Name associated to the argument</param>
        /// <param name="description">Description associated to the argument</param>
        /// <param name="action">Action associated to the argument</param>
        public void Add(string name, string description, Action<Arguments, string> action)
        {
            this.argumentSet.Add(new Argument(name, description, action));
        }

        /// <summary>
        /// Indicates whether the specified arguments are valid
        /// </summary>
        /// <param name="args">The arguments associated</param>
        /// <returns>true if the args parameter is valid; otherwise, false.</returns>
        public bool Parse(IEnumerable<string> args)
        {
            if (args == null || args.Count() == 0)
            {
                return false;
            }

            Dictionary<string, string> argValues = new Dictionary<string, string>();

            // Obtiene los nombres de los argumentos
            foreach (string arg in args)
            {
                string name = this.GetName(arg);

                if (name == null)
                {
                    argValues.Clear();
                    argValues = null;
                    return false;
                }

                string value = this.GetValue(arg);

                argValues.Add(name, value);
            }

            // Si todos los nombres de los argumentos se encuentran definidos, los configura
            foreach (KeyValuePair<string, string> arg in argValues)
            {
                var parameter = this.argumentSet.FirstOrDefault(a => a.Names.Contains(arg.Key));

                if (parameter == null)
                {
                    return false;
                }

                parameter.Action(this, arg.Value);
            }

            argValues.Clear();
            argValues = null;

            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a Arguments
        /// </summary>
        /// <returns>A Enumerator object for the Arguments</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.argumentSet.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a Arguments
        /// </summary>
        /// <returns>A Enumerator object for the Arguments</returns>
        public IEnumerator<Argument> GetEnumerator()
        {
            return this.argumentSet.GetEnumerator() as IEnumerator<Argument>;
        }

        /// <summary>
        /// Get the name of an string argument
        /// </summary>
        /// <param name="argument">string of the argument name</param>
        /// <returns>argument name if the argument parameter match with the format; otherwise, null.</returns>
        private string GetName(string argument)
        {
            Match m = this.nameParser.Match(argument);
            if (!m.Success)
            {
                return null;
            }

            return m.Groups["name"].Value;
        }

        /// <summary>
        /// Get the name of an string argument
        /// </summary>
        /// <param name="argument">string of the argument name</param>
        /// <returns>argument name if the argument parameter match with the format; otherwise, null.</returns>
        private string GetValue(string argument)
        {
            Match m = this.nameParser.Match(argument);
            if (!m.Success)
            {
                return null;
            }

            return m.Groups["value"].Value;
        }
    }
}
