using EQL_AritecTests.ASTNodes.Constants;
using Integra.Vision.Language;
using Integra.Vision.Language.General;
using Integra.Vision.Language.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace EQL_Aritec.ASTNodes.QuerySections.Tests
{
    [TestClass()]
    internal sealed class FromNodeTests
    {
        [TestMethod()]
        public void InitTestFrom()
        {
            ParserTest parser = new ParserTest();
            ExpressionConstructor te = new ExpressionConstructor();

            PlanNode p = parser.callParserTest("from identificador");
            object result = te.GetExpressionCompiled(p, new EventObject());
        }
    }
}
