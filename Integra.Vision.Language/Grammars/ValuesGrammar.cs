//-----------------------------------------------------------------------
// <copyright file="ValuesGrammar.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Grammars
{
    using System;
    using System.Linq;
    using ASTNodes.Cast;
    using ASTNodes.QuerySections;
    using Integra.Vision.Language.ASTNodes.Constants;
    using Integra.Vision.Language.ASTNodes.Lists;
    using Integra.Vision.Language.ASTNodes.Objects.Event;
    using Integra.Vision.Language.ASTNodes.Objects.Object;
    using Integra.Vision.Language.ASTNodes.Operations;
    using Irony.Interpreter;
    using Irony.Parsing;

    /// <summary>
    /// EQLGrammar grammar for the commands and the predicates 
    /// </summary>
    [Language("ValueGrammar", "0.1", "")]
    internal sealed class ValuesGrammar : InterpretedLanguageGrammar
    {
        /// <summary>
        /// All values
        /// </summary>
        private NonTerminal values;

        /// <summary>
        /// Other values: string, boolean, null
        /// </summary>
        private NonTerminal otherValues;

        /// <summary>
        /// Numeric values
        /// </summary>
        private NonTerminal numericValues;

        /// <summary>
        /// Non constant values: objects
        /// </summary>
        private NonTerminal nonConstantValues;

        /// <summary>
        /// Projection values
        /// </summary>
        private NonTerminal projectionValue;

        /// <summary>
        /// Right parenthesis
        /// </summary>
        private KeyTerm parentesisDer;

        /// <summary>
        /// Left parenthesis
        /// </summary>
        private KeyTerm parentesisIz;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuesGrammar"/> class
        /// </summary>
        public ValuesGrammar()
            : base(false)
        {
            this.CreateGrammar();
        }

        /// <summary>
        /// Gets the nonterminal for all values
        /// </summary>
        public NonTerminal Values
        {
            get
            {
                return this.values;
            }
        }

        /// <summary>
        /// Gets the nonterminal for numeric values
        /// </summary>
        public NonTerminal NumericValues
        {
            get
            {
                return this.numericValues;
            }
        }

        /// <summary>
        /// Gets the nonterminal for non constant values
        /// </summary>
        public NonTerminal NonConstantValues
        {
            get
            {
                return this.nonConstantValues;
            }
        }

        /// <summary>
        /// Gets the nonterminal for other values
        /// </summary>
        public NonTerminal OtherValues
        {
            get
            {
                return this.otherValues;
            }
        }

        /// <summary>
        /// Gets the projection value
        /// </summary>
        public NonTerminal ProjectionValue
        {
            get
            {
                return this.projectionValue;
            }
        }

        /// <summary>
        /// Gets the right parenthesis
        /// </summary>
        public KeyTerm ParentesisDer
        {
            get
            {
                return this.parentesisDer;
            }
        }

        /// <summary>
        /// Gets the right parenthesis
        /// </summary>
        public KeyTerm ParentesisIz
        {
            get
            {
                return this.parentesisIz;
            }
        }

        /// <summary>
        /// Specify the language grammar
        /// </summary>
        public void CreateGrammar()
        {
            /* PROYECCIÓN */
            KeyTerm terminalKey = ToTerm("key", "key");

            /* FUNCIONES */
            KeyTerm terminalYear = ToTerm("year", "year");
            KeyTerm terminalMonth = ToTerm("month", "month");
            KeyTerm terminalDay = ToTerm("day", "day");
            KeyTerm terminalHour = ToTerm("hour", "hour");
            KeyTerm terminalMinute = ToTerm("minute", "minute");
            KeyTerm terminalSecond = ToTerm("second", "second");
            KeyTerm terminalMillisecond = ToTerm("millisecond", "millisecond");
            KeyTerm terminalCount = ToTerm("count", "count");
            KeyTerm terminalSum = ToTerm("sum", "sum");

            /* EVENTOS */
            KeyTerm terminalEvent = ToTerm("event", "event");
            KeyTerm terminalMessage = ToTerm("Message", "Message");
            KeyTerm terminalTimestamp = ToTerm("Timestamp", "Timestamp");
            KeyTerm terminalAgent = ToTerm("Agent", "Agent");
            KeyTerm terminalAdapter = ToTerm("Adapter", "Adapter");
            KeyTerm terminalName = ToTerm("Name", "Name");

            // Marcamos los terminales, definidos hasta el momento, como palabras reservadas
            this.MarkReservedWords(this.KeyTerms.Keys.ToArray());

            /* OPERADORES ARITMETICOS */
            KeyTerm terminalMenos = ToTerm("-", "menos");
            KeyTerm terminalMas = ToTerm("+", "mas");

            /* SIMBOLOS */
            KeyTerm terminalParentesisIz = ToTerm("(", "parentesisIzValuesGrammar");
            this.parentesisDer = this.ToTerm(")", "parentesisDerValuesGrammar");
            KeyTerm terminalCorcheteIz = ToTerm("[", "corcheteIz");
            KeyTerm terminalCorcheteDer = ToTerm("]", "corcheteDer");
            KeyTerm terminalLlaveIz = ToTerm("{", "llaveIz");
            KeyTerm terminalLlaveDer = ToTerm("}", "llaveDer");
            KeyTerm terminalPunto = ToTerm(".", "punto");
            KeyTerm terminalPorcentaje = ToTerm("%", "porcentaje");
            KeyTerm terminalNumeral = ToTerm("#", "numeral");
            KeyTerm terminalComillaSimple = ToTerm("'", "comillaSimple");
            KeyTerm terminalComa = ToTerm(",", "coma");
            KeyTerm terminalArroba = ToTerm("@", "arroba");
            KeyTerm terminalIgual = ToTerm("=", "igual");

            /* TIPOS PARA CASTEO */
            ConstantTerminal terminalType = new ConstantTerminal("contanteTipo");
            terminalType.Add("byte", typeof(byte));
            terminalType.Add("byte?", typeof(byte?));
            terminalType.Add("sbyte", typeof(sbyte));
            terminalType.Add("sbyte?", typeof(sbyte?));
            terminalType.Add("short", typeof(short));
            terminalType.Add("short?", typeof(short?));
            terminalType.Add("ushort", typeof(ushort));
            terminalType.Add("ushort?", typeof(ushort?));
            terminalType.Add("int", typeof(int));
            terminalType.Add("int?", typeof(int?));
            terminalType.Add("uint", typeof(uint));
            terminalType.Add("uint?", typeof(uint?));
            terminalType.Add("long", typeof(long));
            terminalType.Add("long?", typeof(long?));
            terminalType.Add("ulong", typeof(ulong));
            terminalType.Add("ulong?", typeof(ulong?));
            terminalType.Add("float", typeof(float));
            terminalType.Add("float?", typeof(float?));
            terminalType.Add("double", typeof(double));
            terminalType.Add("double?", typeof(double?));
            terminalType.Add("decimal", typeof(decimal));
            terminalType.Add("decimal?", typeof(decimal?));
            terminalType.Add("char", typeof(char));
            terminalType.Add("char?", typeof(char?));
            terminalType.Add("string", typeof(string));
            terminalType.Add("bool", typeof(bool));
            terminalType.Add("bool?", typeof(bool?));
            terminalType.Add("object", typeof(object));
            terminalType.Add("DateTime", typeof(DateTime));
            terminalType.Add("TimeSpan", typeof(TimeSpan));
            terminalType.AstConfig.NodeType = null;
            terminalType.AstConfig.DefaultNodeCreator = () => new TypeNode();

            /* COMENTARIOS */
            CommentTerminal comentarioLinea = new CommentTerminal("comentario_linea", "//", "\n", "\r\n");
            CommentTerminal comentarioBloque = new CommentTerminal("comentario_bloque", "/*", "*/");
            NonGrammarTerminals.Add(comentarioLinea);
            NonGrammarTerminals.Add(comentarioBloque);

            /* CONSTANTES E IDENTIFICADORES */
            Terminal terminalNumero = TerminalFactory.CreateCSharpNumber("numero");
            terminalNumero.AstConfig.NodeType = null;
            terminalNumero.AstConfig.DefaultNodeCreator = () => new NumberNode();
            Terminal terminalCadena = TerminalFactory.CreateCSharpString("cadena");
            terminalCadena.AstConfig.NodeType = null;
            terminalCadena.AstConfig.DefaultNodeCreator = () => new StringNode();
            ConstantTerminal terminalBool = new ConstantTerminal("constanteBool");
            terminalBool.Add("true", true);
            terminalBool.Add("false", false);
            terminalBool.AstConfig.NodeType = null;
            terminalBool.AstConfig.DefaultNodeCreator = () => new BooleanNode();
            Terminal terminalDateTimeValue = new QuotedValueLiteral("datetimeValue", "'", TypeCode.String);
            terminalDateTimeValue.AstConfig.NodeType = null;
            terminalDateTimeValue.AstConfig.DefaultNodeCreator = () => new DateTimeOrTimespanNode();
            ConstantTerminal terminalNull = new ConstantTerminal("constanteNull");
            terminalNull.Add("null", null);
            terminalNull.AstConfig.NodeType = null;
            terminalNull.AstConfig.DefaultNodeCreator = () => new NullValueNode();

            RegexBasedTerminal terminalId = new RegexBasedTerminal("[a-zA-Z]+([a-zA-Z]|[0-9]|[_])*");
            terminalId.AstConfig.NodeType = null;
            terminalId.AstConfig.DefaultNodeCreator = () => new IdentifierNode();

            /* PRECEDENCIA Y ASOCIATIVIDAD */
            /* this.AddBracePair();
            this.AddMarkPunctuation();*/

            /* NO TERMINALES */
            this.values = new NonTerminal("VALUES", typeof(ConstantValueNode));
            this.values.AstConfig.NodeType = null;
            this.values.AstConfig.DefaultNodeCreator = () => new ConstantValueNode();
            this.numericValues = new NonTerminal("NUMERIC_VALUES", typeof(ConstantValueNode));
            this.numericValues.AstConfig.NodeType = null;
            this.numericValues.AstConfig.DefaultNodeCreator = () => new ConstantValueNode();
            this.nonConstantValues = new NonTerminal("NON_CONSTANT_VALUES", typeof(ConstantValueNode));
            this.nonConstantValues.AstConfig.NodeType = null;
            this.nonConstantValues.AstConfig.DefaultNodeCreator = () => new ConstantValueNode();
            this.otherValues = new NonTerminal("OTHER_VALUES", typeof(ConstantValueNode));
            this.otherValues.AstConfig.NodeType = null;
            this.otherValues.AstConfig.DefaultNodeCreator = () => new ConstantValueNode();
            NonTerminal nt_DATETIME_TIMESPAN_VALUES = new NonTerminal("DATETIME_TIMESPAN_VALUES", typeof(ConstantValueNode));
            nt_DATETIME_TIMESPAN_VALUES.AstConfig.NodeType = null;
            nt_DATETIME_TIMESPAN_VALUES.AstConfig.DefaultNodeCreator = () => new ConstantValueNode();

            NonTerminal nt_EXPLICIT_CAST = new NonTerminal("EXPLICIT_CAST", typeof(ExplicitCast));
            nt_EXPLICIT_CAST.AstConfig.NodeType = null;
            nt_EXPLICIT_CAST.AstConfig.DefaultNodeCreator = () => new ExplicitCast();

            NonTerminal nt_PARAMETER_VALUES = new NonTerminal("PARAMETER_VALUES", typeof(ConstantValueNode));
            nt_PARAMETER_VALUES.AstConfig.NodeType = null;
            nt_PARAMETER_VALUES.AstConfig.DefaultNodeCreator = () => new ConstantValueNode();
            NonTerminal nt_VALUES_WITH_ALIAS = new NonTerminal("VALUES_WITH_ALIAS", typeof(ConstantValueWithAliasNode));
            nt_VALUES_WITH_ALIAS.AstConfig.NodeType = null;
            nt_VALUES_WITH_ALIAS.AstConfig.DefaultNodeCreator = () => new ConstantValueWithAliasNode();
            NonTerminal nt_LIST_OF_VALUES = new NonTerminal("LIST_OF_VALUES", typeof(PlanNodeListNode));
            nt_LIST_OF_VALUES.AstConfig.NodeType = null;
            nt_LIST_OF_VALUES.AstConfig.DefaultNodeCreator = () => new PlanNodeListNode();
            NonTerminal nt_DATE_FUNCTIONS = new NonTerminal("DATE_FUNCTIONS", typeof(DateFunctionNode));
            nt_DATE_FUNCTIONS.AstConfig.NodeType = null;
            nt_DATE_FUNCTIONS.AstConfig.DefaultNodeCreator = () => new DateFunctionNode();
            NonTerminal nt_EVENT = new NonTerminal("EVENT", typeof(EventNode));
            nt_EVENT.AstConfig.NodeType = null;
            nt_EVENT.AstConfig.DefaultNodeCreator = () => new EventNode();
            NonTerminal nt_GROUP_KEY = new NonTerminal("GROUP_KEY", typeof(GroupKey));
            nt_GROUP_KEY.AstConfig.NodeType = null;
            nt_GROUP_KEY.AstConfig.DefaultNodeCreator = () => new GroupKey();
            NonTerminal nt_GROUP_KEY_VALUE = new NonTerminal("GROUP_KEY_VALUE", typeof(GroupKeyValue));
            nt_GROUP_KEY_VALUE.AstConfig.NodeType = null;
            nt_GROUP_KEY_VALUE.AstConfig.DefaultNodeCreator = () => new GroupKeyValue();
            NonTerminal nt_EVENT_PROPERTIES = new NonTerminal("EVENT_VALUES", typeof(EventPropertiesNode));
            nt_EVENT_PROPERTIES.AstConfig.NodeType = null;
            nt_EVENT_PROPERTIES.AstConfig.DefaultNodeCreator = () => new EventPropertiesNode();
            NonTerminal nt_EVENT_WITH_SOURCE = new NonTerminal("EVENT_WITH_SOURCE", typeof(EventWithSource));
            nt_EVENT_WITH_SOURCE.AstConfig.NodeType = null;
            nt_EVENT_WITH_SOURCE.AstConfig.DefaultNodeCreator = () => new EventWithSource();
            NonTerminal nt_OBJECT_ID_OR_NUMBER = new NonTerminal("OBJECT_ID_OR_NUMBER", typeof(ObjectIdOrNumberNode));
            nt_OBJECT_ID_OR_NUMBER.AstConfig.NodeType = null;
            nt_OBJECT_ID_OR_NUMBER.AstConfig.DefaultNodeCreator = () => new ObjectIdOrNumberNode();
            NonTerminal nt_OBJECT = new NonTerminal("OBJECT", typeof(ObjectNode));
            nt_OBJECT.AstConfig.NodeType = null;
            nt_OBJECT.AstConfig.DefaultNodeCreator = () => new ObjectNode();
            NonTerminal nt_OBJECT_VALUE = new NonTerminal("OBJECT_VALUE", typeof(ObjectValueNode));
            nt_OBJECT_VALUE.AstConfig.NodeType = null;
            nt_OBJECT_VALUE.AstConfig.DefaultNodeCreator = () => new ObjectValueNode();
            NonTerminal nt_UNARY_ARITHMETIC_EXPRESSION = new NonTerminal("UNARY_ARITHMETIC_EXPRESSION", typeof(UnaryArithmeticExpressionNode));
            nt_UNARY_ARITHMETIC_EXPRESSION.AstConfig.NodeType = null;
            nt_UNARY_ARITHMETIC_EXPRESSION.AstConfig.DefaultNodeCreator = () => new UnaryArithmeticExpressionNode();
            NonTerminal nt_ARITHMETIC_EXPRESSION = new NonTerminal("ARITHMETIC_EXPRESSION", typeof(ArithmeticExpressionNode));
            nt_ARITHMETIC_EXPRESSION.AstConfig.NodeType = null;
            nt_ARITHMETIC_EXPRESSION.AstConfig.DefaultNodeCreator = () => new ArithmeticExpressionNode();

            this.projectionValue = new NonTerminal("PROJECTION_VALUES", typeof(ProjectionValue));
            this.projectionValue.AstConfig.NodeType = null;
            this.projectionValue.AstConfig.DefaultNodeCreator = () => new ProjectionValue();

            /* PROJECTION VALUES */
            this.projectionValue.Rule = terminalCount + terminalParentesisIz + this.parentesisDer
                                        | terminalSum + terminalParentesisIz + this.values + this.parentesisDer
                                        | nt_GROUP_KEY_VALUE
                                        | this.values
                                        | terminalId;
            /* **************************** */
            /* GROUP KEY */
            nt_GROUP_KEY.Rule = nt_GROUP_KEY + terminalPunto + terminalId
                                | terminalKey;

            nt_GROUP_KEY_VALUE.Rule = nt_GROUP_KEY;
            /* **************************** */
            /* CONSTANTES */
            this.values.Rule = this.numericValues
                                | this.nonConstantValues
                                | this.otherValues
                                | nt_EXPLICIT_CAST;

            nt_EXPLICIT_CAST.Rule = terminalParentesisIz + terminalType + this.parentesisDer + this.numericValues
                                    | terminalParentesisIz + terminalType + this.parentesisDer + this.nonConstantValues
                                    | terminalParentesisIz + terminalType + this.parentesisDer + this.otherValues;

            this.nonConstantValues.Rule = nt_EVENT_WITH_SOURCE
                                            | nt_OBJECT_VALUE
                                            | nt_EVENT_PROPERTIES;

            this.numericValues.Rule = terminalDateTimeValue
                                        | terminalNumero
                                        | nt_DATE_FUNCTIONS
                                        | nt_ARITHMETIC_EXPRESSION
                                        | nt_UNARY_ARITHMETIC_EXPRESSION;

            this.otherValues.Rule = terminalBool
                                | terminalNull
                                | terminalCadena;

            nt_DATETIME_TIMESPAN_VALUES.Rule = this.nonConstantValues
                                            | terminalDateTimeValue;

            /* **************************** */
            /* FUNCIONES DE FECHAS */
            nt_DATE_FUNCTIONS.Rule = terminalYear + terminalParentesisIz + nt_DATETIME_TIMESPAN_VALUES + this.parentesisDer
                                    | terminalMonth + terminalParentesisIz + nt_DATETIME_TIMESPAN_VALUES + this.parentesisDer
                                    | terminalDay + terminalParentesisIz + nt_DATETIME_TIMESPAN_VALUES + this.parentesisDer
                                    | terminalHour + terminalParentesisIz + nt_DATETIME_TIMESPAN_VALUES + this.parentesisDer
                                    | terminalMinute + terminalParentesisIz + nt_DATETIME_TIMESPAN_VALUES + this.parentesisDer
                                    | terminalSecond + terminalParentesisIz + nt_DATETIME_TIMESPAN_VALUES + this.parentesisDer
                                    | terminalMillisecond + terminalParentesisIz + nt_DATETIME_TIMESPAN_VALUES + this.parentesisDer;
            /* **************************** */
            /* EXPRESIONES ARITMETICAS */
            nt_ARITHMETIC_EXPRESSION.Rule = nt_ARITHMETIC_EXPRESSION + terminalMenos + nt_ARITHMETIC_EXPRESSION
                                            | this.numericValues;
            /* **************************** */
            /* OPERACION ARITMETICA UNARIA */
            nt_UNARY_ARITHMETIC_EXPRESSION.Rule = terminalMenos + terminalNumero
                                                    | terminalMas + terminalNumero;
            /* **************************** */
            /* VALORES DE LOS OBJETOS */
            nt_OBJECT_VALUE.Rule = nt_OBJECT;
            /* **************************** */
            /* OBJETOS */
            nt_OBJECT.Rule = nt_OBJECT + terminalPunto + nt_OBJECT_ID_OR_NUMBER
                                | nt_EVENT + terminalPunto + terminalMessage + terminalPunto + nt_OBJECT_ID_OR_NUMBER + terminalPunto + nt_OBJECT_ID_OR_NUMBER;
            /* **************************** */
            /* IDENTIFICADORES DE PARTES Y CAMPOS DE OBJETOS */
            nt_OBJECT_ID_OR_NUMBER.Rule = terminalNumeral + terminalNumero
                                        | terminalId
                                        | terminalCorcheteIz + terminalCadena + terminalCorcheteDer;
            /* **************************** */
            /* VALORES DEL EVENTO */
            nt_EVENT_PROPERTIES.Rule = nt_EVENT_PROPERTIES + terminalPunto + terminalId
                                        | nt_EVENT + terminalPunto + terminalAdapter
                                        | nt_EVENT + terminalPunto + terminalAgent;
            /* **************************** */
            /* EVENTO */
            nt_EVENT.Rule = terminalArroba + terminalEvent;
            /* **************************** */
            /* EVENTO CON FUENTE */
            nt_EVENT_WITH_SOURCE.Rule = terminalId + terminalPunto + nt_OBJECT_VALUE
                                            | terminalId + terminalPunto + nt_EVENT_PROPERTIES;
            /* **************************** */

            this.Root = this.values;

            this.LanguageFlags = Irony.Parsing.LanguageFlags.CreateAst;
        }
    }
}
