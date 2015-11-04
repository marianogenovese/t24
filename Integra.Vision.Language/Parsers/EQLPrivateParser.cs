﻿//-----------------------------------------------------------------------
// <copyright file="EQLPrivateParser.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language
{
    using System.Collections.Generic;
    using Exceptions;
    using Integra.Vision.Language.Grammars;
    using Irony.Parsing;

    /// <summary>
    /// Class that implements the logic to parse private commands
    /// </summary>
    internal sealed class EQLPrivateParser
    {
        /// <summary>
        /// Command text
        /// </summary>
        private string commandText;

        /// <summary>
        /// Initializes a new instance of the <see cref="EQLPrivateParser"/> class
        /// </summary>
        /// <param name="commandText">Command text</param>
        public EQLPrivateParser(string commandText)
        {
            this.commandText = commandText;
        }

        /// <summary>
        /// Implements the logic to parse private commands
        /// </summary>
        /// <returns>Execution plan</returns>
        public List<PlanNode> Parse()
        {
            List<PlanNode> nodes = null;

            try
            {
                PrivateGrammar grammar = new PrivateGrammar();
                LanguageData language = new LanguageData(grammar);
                Parser parser = new Parser(language);
                ParseTree parseTree = parser.Parse(this.commandText);
                if (parseTree.HasErrors())
                {
                    foreach (var parserMessage in parseTree.ParserMessages)
                    {
                        throw new SyntaxException(Resources.SR.SyntaxError(parserMessage.Message, parserMessage.Location.Line, parserMessage.Location.Column));
                    }
                }
                else
                {
                    Irony.Interpreter.ScriptApp app = new Irony.Interpreter.ScriptApp(new Integra.Vision.Language.Grammars.PrivateLanguageRuntime());
                    nodes = (List<PlanNode>)app.Evaluate(parseTree);
                }
            }
            catch (System.Exception e)
            {
                throw new ParseException(Resources.SR.InterpretationException, e);
            }

            return nodes;
        }
    }
}
