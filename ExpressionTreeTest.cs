using SpreadsheetEngine;

namespace Spreadsheet_Jillian_Plahn.ExpressionTreeDemo.Tests
{
    // Assuming only valid expressions are entered.
    [TestFixture]
    public class ExpressionTreeTests
    {
        /// <summary>
        /// Normal test cases.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [Test]
        // Addition
        [TestCase("10+2", ExpectedResult = 12.0)]
        // Subtraction
        [TestCase("12-3", ExpectedResult = 9.0)]
        // Multiplication
        [TestCase("6*5", ExpectedResult = 30.0)]
        // Division
        [TestCase("30/5", ExpectedResult = 6.0)]
        // Negative result
        [TestCase("5-8", ExpectedResult = -3)]
        // Spaces.
        [TestCase("5 + 2 + 4", ExpectedResult = 11)]
        // Parethesis and different operators
        [TestCase("200/(5*5)", ExpectedResult = 8.0)]
        // Different operators
        [TestCase("100/10*10", ExpectedResult = 100.0)]
        // Mixing operators and adding precedence
        [TestCase("20/(7-2)", ExpectedResult = 4)]
        // Operators with differetn precedecne
        [TestCase("(25/5)/1", ExpectedResult = 5)]
        // Extra parenthesis
        [TestCase("((((5+2) + (6+3))))", ExpectedResult = 16)]


        public double TestNormalCasesEvaluate(string expression)
        {
            ExpressionTree exp = new ExpressionTree(expression);
            return exp.Evaluate();
        }


        /// <summary>
        /// Normal test case to test SetVariable and Evaluate method in ExpressionTree class.
        /// </summary>
        [Test]
        public void TestExpressionsWithVariableValue1()
        {
            ExpressionTree exp = new ExpressionTree("A4+7");
            exp.SetVariable("A4", 20);
            Assert.AreEqual(27, exp.Evaluate());
        }

        /// <summary>
        /// Normal test case for variables.
        /// </summary>
        [Test]
        public void TestExpressionsWithVariableValue2()
        {
            ExpressionTree exp = new ExpressionTree("B3+A2+2");
            exp.SetVariable("B3", 5);
            exp.SetVariable("A2", 6);
            Assert.AreEqual(13, exp.Evaluate());
        }

        /// <summary>
        /// Test for having an extra parethesis.
        /// </summary>
        /// <param name="expression"></param>
        [Test]
        [TestCase("((10+5) + (2+2)))")]
       
        public void TestInvalidExpression(string expression)
        {
            try
            {
                ExpressionTree exp = new ExpressionTree(expression);
                Assert.Fail("An exception was not thrown for invlaid input");
            }
            catch (ArgumentException ex)
            {

            }
        }

        /// <summary>
        /// Testing multiple operators and variables.
        /// </summary>
        [Test]
        public void TestExpressionWithVariableValue3() 
        {
            ExpressionTree exp = new ExpressionTree("B5*(A3+8)");
            exp.SetVariable("B5", 4);
            exp.SetVariable("A3", 6);
            Assert.AreEqual(56, exp.Evaluate());
        }

        /// <summary>
        /// Testing parethesis and operators.
        /// </summary>
        [Test]
        public void TestExpressionWithVariableValue4()
        {
            ExpressionTree exp = new ExpressionTree("(A1+C3)*(B4+D3)");
            exp.SetVariable("A1", 4);
            exp.SetVariable("C3", 5);
            exp.SetVariable("B4", 6);
            exp.SetVariable("D3", 7);
            Assert.AreEqual(117, exp.Evaluate());
        }

        /// <summary>
        /// Testing parenthesis with divide and variables.
        /// </summary>
        [Test]
        public void TestExpressionWithVariableValue5()
        {
            ExpressionTree exp = new ExpressionTree("100/(A1*B3)");
            exp.SetVariable("A1", 4);
            exp.SetVariable("B3", 5);
            Assert.AreEqual(5, exp.Evaluate());
        }

       
    }
}