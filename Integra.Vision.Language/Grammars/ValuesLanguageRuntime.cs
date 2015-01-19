//-----------------------------------------------------------------------
// <copyright file="ValuesLanguageRuntime.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Grammars
{
    using Irony.Interpreter;
    using Irony.Parsing;

    /// <summary>
    /// ValueLanguageRuntime class
    /// </summary>
    internal class ValuesLanguageRuntime : LanguageRuntime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValuesLanguageRuntime"/> class
        /// </summary>
        public ValuesLanguageRuntime()
            : base(new LanguageData(new ValuesGrammar()))
        { 
        }
    }
}
