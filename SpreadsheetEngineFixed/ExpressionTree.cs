// <copyright file="ExpressionTree.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using SpreadsheetEngineFixed;

    public class ExpressionTree
    {
        private string InFixExpression;
        private string PostFixExpression;

        private Node root;

        private Dictionary<string, double> variables;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// Contructor to contruct tree from the specific expression.
        /// </summary>
        /// <param name="expression">expression.</param>
        public ExpressionTree(string expression)
        {
            expression = expression.Replace(" ", string.Empty);

            this.InFixExpression = expression;

            this.root = null;

            // Clear dictionary for new tree.
            this.variables = new Dictionary<string, double>();

            // Compile expression to root.
            this.BuildTree();
        }

        /// <summary>
        /// Set the specified variable within the ExpressionTree variables dictionary.
        /// </summary>
        /// <param name="variableName">Variable name. </param>
        /// <param name="variableValue">variable value. </param>
        public void SetVariable(string variableName, double variableValue)
        {
            this.variables[variableName] = variableValue;
        }

        /// <summary>
        /// This method evaluates the expression to a double value.
        /// </summary>
        /// <returns>root.</returns>
        public double Evaluate()
        {
            return this.Evaluate(this.root);
        }

        /// <summary>
        /// Method to convert expression from infix to postfix.
        /// https://www.geeksforgeeks.org/java-program-to-implement-shunting-yard-algorithm/.
        /// https://www.geeksforgeeks.org/program-to-convert-infix-notation-to-expression-tree/?ref=rp.
        /// </summary>
        /// <param name="expression">expression.</param>
        /// <returns>void.</returns>
        private void InfixToPost()
        {
            // List for post fix expression to make building tree easier.
            var postFixList = new List<string>();

            // Stack to hold chars
            Stack<string> stringStack = new Stack<string>();

            for (int i = 0; i < this.InFixExpression.Length; i++)
            {
                char c = this.InFixExpression[i];

                if (c == '(')
                {
                    // push in char stack
                    stringStack.Push(c.ToString());
                }

                // Push operand to node stack
                else if (char.IsDigit(c))
                {
                    // This helps if number is more than one digit
                    string builder = string.Empty;
                    while (char.IsDigit(c) && i < this.InFixExpression.Length)
                    {
                        // This is putting the whole number in builder
                        builder += c;
                        i++;
                        if (i < this.InFixExpression.Length)
                        {
                            c = this.InFixExpression[i];
                        }
                    }

                    postFixList.Add(builder);

                    // Go back to previous
                    i--;
                }
                else if (c == ')')
                {
                    string topStack = string.Empty;
                    while (stringStack.Count != 0 && (topStack = stringStack.Pop()) != "(")
                    {
                        postFixList.Add(topStack);
                    }

                    if (topStack != "(")
                    {
                        throw new ArgumentException("Uneven parethesis");
                    }
                }

                // if operator is found take action based on precedence
                else if (Factory.CheckOperator(c) == true)
                {
                    while (stringStack.Count != 0 && Factory.CheckOperator(stringStack.Peek()[0]) == true)
                    {
                        if (GetPrecedence(c) <= GetPrecedence(stringStack.Peek()[0]))
                        {
                            postFixList.Add(stringStack.Pop());
                        }
                        else
                        {
                            break;
                        }
                    }

                    stringStack.Push(c.ToString());
                }

                // else if its a variable
                else
                {
                    string variableBuild = string.Empty;
                    while (i < this.InFixExpression.Length && (Factory.CheckOperator(c) == false) && c != ')')
                    {
                        // If variable is more than one letter created string builder to hold more.
                        variableBuild += c;
                        i++;
                        if (i < this.InFixExpression.Length)
                        {
                            c = this.InFixExpression[i];
                        }
                    }

                    postFixList.Add(variableBuild);

                    if (!this.variables.ContainsKey(variableBuild))
                    {
                        this.variables.Add(variableBuild, 0);
                    }

                    i--;
                }
            }

            // pop remaining operators from stack and append them all.
            while (stringStack.Count != 0)
            {
                var topStack = stringStack.Pop();
                postFixList.Add(topStack);
            }

            this.PostFixExpression = string.Join(" ", postFixList);
        }

        /// <summary>
        /// Method to build tree using the postfix expression.
        /// </summary>
        private void BuildTree()
        {
            // stack to hold nodes.
            Node newNode;
            Stack<Node> nodeStack = new Stack<Node>();

            // Get the post fix expression
            this.InfixToPost();

            foreach (var c in this.PostFixExpression.Split(' '))
            {
                // check for constant
                double numCheck;
                if (double.TryParse(c, out numCheck))
                {
                    // CHANGE
                    newNode = new ConstantNode(double.Parse(c));
                }

                // check for operator
                else if (Factory.CheckOperator(c[0]) == true)
                {
                    newNode = Factory.CreateOperatorNode(c[0]);
                    ((BinaryOperatorNode)newNode).Right = nodeStack.Pop();
                    ((BinaryOperatorNode)newNode).Left = nodeStack.Pop();
                }

                // else its a varibale
                else
                {
                    newNode = new VariableNode(c);
                }

                nodeStack.Push(newNode);
            }

            // root points to operator at the top of stack
            try
            {
                this.root = nodeStack.Pop();
            }
            catch
            {
                throw new Exception("Top of node stack was not an operator.");
            }
        }

        /// <summary>
        /// Method to get precedence.
        /// https://www.geeksforgeeks.org/java-program-to-implement-shunting-yard-algorithm/.
        /// </summary>
        /// <param name="ch">character.</param>
        /// <returns>int.</returns>
        private static int GetPrecedence(char ch)
        {
            if (ch == '+' || ch == '-')
            {
                return 1;
            }
            else if (ch == '*' || ch == '/')
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Method will evaluate expression tree.
        /// </summary>
        /// <param name="node">node.</param>
        /// <returns>double.</returns>
        private double Evaluate(Node node)
        {
            this.BuildTree();

            // Evaluate node as a constant. Want to return the value.
            if (node is ConstantNode)
            {
                ConstantNode tempNode = (ConstantNode)node;
                return tempNode.OpValue;
            }

            // Evaluate as a variable node. Want to return the name.
            if (node is VariableNode)
            {
                VariableNode tempNode = (VariableNode)node;
                try
                {
                    return this.variables[tempNode.Name];
                }
                catch
                {
                    Console.WriteLine("Variable" + tempNode.Name + "has not been set by user. Default is 0.");
                }
            }

            // Evaluate as an operator node.
            if (node is BinaryOperatorNode)
            {
                BinaryOperatorNode tempNode = (BinaryOperatorNode)node;
                return tempNode.Evaluate(this.Evaluate(tempNode.Left), this.Evaluate(tempNode.Right));
            }

            return 0;
        }

        /// <summary>
        /// Method to retutn array of all variable names in expression.
        /// </summary>
        /// <returns>array.</returns>
        public string[] GetVariable()
        {
            return this.variables.Keys.ToArray();
        }
    }
}
