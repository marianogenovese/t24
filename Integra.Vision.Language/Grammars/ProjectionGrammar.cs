//-----------------------------------------------------------------------
// <copyright file="ProjectionGrammar.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Grammars
{
    using Integra.Vision.Language.ASTNodes.Constants;
    using Integra.Vision.Language.ASTNodes.Lists;
    using Integra.Vision.Language.ASTNodes.QuerySections;
    using Irony.Interpreter;
    using Irony.Parsing;

    /// <summary>
    /// Projection grammar for the commands
    /// </summary>
    [Language("ProjectionGrammar", "0.1", "")]
    internal sealed class ProjectionGrammar : InterpretedLanguageGrammar
    {
        /// <summary>
        /// Expression grammar
        /// </summary>
        private ValuesGrammar valueGrammar;

        /// <summary>
        /// Expression grammar
        /// </summary>
        private NonTerminal projectionList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionGrammar"/> class
        /// </summary>
        public ProjectionGrammar()
            : base(false)
        {
            this.valueGrammar = new ValuesGrammar();
            this.CreateGrammar();
        }

        /// <summary>
        /// Gets the nonterminal for expression conditions
        /// </summary>
        public NonTerminal ProjectionList
        {
            get
            {
                return this.projectionList;
            }
        }

        /// <summary>
        /// Specify the language grammar
        /// </summary>
        public void CreateGrammar()
        {
            // reserved words
            KeyTerm terminalSelect = ToTerm("select", "select");
            KeyTerm terminalComa = ToTerm(",", "coma");
            KeyTerm terminalAs = ToTerm("as", "as");

            // terminals
            RegexBasedTerminal terminalId = new RegexBasedTerminal("[a-zA-Z]+([a-zA-Z]|[0-9]|[_])*");
            terminalId.AstConfig.NodeType = null;
            terminalId.AstConfig.DefaultNodeCreator = () => new IdentifierNode();

            /* PRECEDENCIA Y ASOCIATIVIDAD */
            this.AddBracePair();
            this.AddMarkPunctuation();

            // nonterminals
            NonTerminal nt_VALUES_WITH_ALIAS = new NonTerminal("VALUES_WITH_ALIAS", typeof(ConstantValueWithAliasNode));
            nt_VALUES_WITH_ALIAS.AstConfig.NodeType = null;
            nt_VALUES_WITH_ALIAS.AstConfig.DefaultNodeCreator = () => new ConstantValueWithAliasNode();
            NonTerminal nt_LIST_OF_VALUES = new NonTerminal("LIST_OF_VALUES", typeof(PlanNodeListNode));
            nt_LIST_OF_VALUES.AstConfig.NodeType = null;
            nt_LIST_OF_VALUES.AstConfig.DefaultNodeCreator = () => new PlanNodeListNode();
            this.projectionList = new NonTerminal("SELECT", typeof(SelectNode));
            this.projectionList.AstConfig.NodeType = null;
            this.projectionList.AstConfig.DefaultNodeCreator = () => new SelectNode();

            /* SELECT */
            this.projectionList.Rule = terminalSelect + nt_LIST_OF_VALUES;
            /* **************************** */
            /* LISTA DE VALORES */
            nt_LIST_OF_VALUES.Rule = nt_LIST_OF_VALUES + terminalComa + nt_VALUES_WITH_ALIAS
                                    | nt_VALUES_WITH_ALIAS;
            /* **************************** */
            /* VALORES CON ALIAS */
            nt_VALUES_WITH_ALIAS.Rule = this.valueGrammar.Values + terminalAs + terminalId;

            this.Root = this.projectionList;

            this.LanguageFlags = Irony.Parsing.LanguageFlags.CreateAst;
        }
    }
}
