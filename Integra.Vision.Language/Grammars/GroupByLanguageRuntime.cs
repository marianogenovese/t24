//-----------------------------------------------------------------------
// <copyright file="GroupByLanguageRuntime.cs" company="Integra.Vision.Language">
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
    internal class GroupByLanguageRuntime : LanguageRuntime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupByLanguageRuntime"/> class
        /// </summary>
        public GroupByLanguageRuntime()
            : base(new LanguageData(new GroupByGrammar()))
        { 
        }
    }
}
