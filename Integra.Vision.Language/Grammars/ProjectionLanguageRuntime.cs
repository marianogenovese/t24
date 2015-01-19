//-----------------------------------------------------------------------
// <copyright file="ProjectionLanguageRuntime.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Grammars
{
    using Irony.Interpreter;
    using Irony.Parsing;

    /// <summary>
    /// ProjectionLanguageRuntime class
    /// </summary>
    internal class ProjectionLanguageRuntime : LanguageRuntime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionLanguageRuntime"/> class
        /// </summary>
        public ProjectionLanguageRuntime()
            : base(new LanguageData(new ProjectionGrammar()))
        { 
        }
    }
}
