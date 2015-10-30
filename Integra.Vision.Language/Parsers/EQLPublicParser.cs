//-----------------------------------------------------------------------
// <copyright file="EQLPublicParser.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Integra.Vision.Language.Grammars;
    using Irony.Parsing;

    /// <summary>
    /// Class that implements the logic to parse commands
    /// </summary>
    internal sealed class EQLPublicParser
    {
        /// <summary>
        /// Command text
        /// </summary>
        private string commandText;

        /// <summary>
        /// Initializes a new instance of the <see cref="EQLPublicParser"/> class
        /// </summary>
        /// <param name="commandText">Command text</param>
        public EQLPublicParser(string commandText)
        {
            this.commandText = commandText;
        }

        /// <summary>
        /// Implements the logic to parse commands
        /// </summary>
        /// <returns>Execution plan</returns>
        public List<PlanNode> Parse()
        {
            List<PlanNode> nodes = null;

            try
            {
                EQLGrammar grammar = new EQLGrammar();
                LanguageData language = new LanguageData(grammar);
                Parser parser = new Parser(language);
                ParseTree parseTree = parser.Parse(this.commandText);
                if (parseTree.HasErrors())
                {
                    foreach (var parserMessage in parseTree.ParserMessages)
                    {
                        throw new Exception(Resources.SR.SyntaxError(parserMessage.Message, parserMessage.Location.Line, parserMessage.Location.Column));
                    }
                }
                else
                {
                    Irony.Interpreter.ScriptApp app = new Irony.Interpreter.ScriptApp(new Integra.Vision.Language.Grammars.EQLLanguageRuntime());
                    nodes = (List<PlanNode>)app.Evaluate(parseTree);
                }
            }
            catch (System.Exception e)
            {
                throw new Exception(Resources.SR.InterpretationException, e);
            }

            return nodes;
        }
    }
}
