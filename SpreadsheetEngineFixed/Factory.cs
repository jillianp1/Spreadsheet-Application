// <copyright file="Factory.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System.Collections.Generic;
    using SpreadsheetEngineFixed;

    /// <summary>
    /// Factory class helps to make the method that creates the expression tree oblivious to different operators.
    /// You want the operators and any checks for operators to be in facotry class or operator node not in the expression tree.
    /// </summary>
    internal class Factory
    {
        /// <summary>
        /// Creates operator node. Holds the logic to create op node.
        /// Helps so that the expresion tree class only knows the operator node not the subclasses.
        /// </summary>
        /// <param name="op">operator.</param>
        /// <returns>Evaluates.</returns>
        public static BinaryOperatorNode CreateOperatorNode(char op)
        {
            switch (op)
            {
                case '+':
                    return new AddNode();
                case '-':
                    return new SubtractNode();
                case '*':
                    return new MultiplyNode();
                case '/':
                    return new DivideNode();
            }

            return null;
        }

        /// <summary>
        /// Used in compile to see if its an operator to be evaluated.
        /// </summary>
        /// <param name="op">operator.</param>
        /// <returns>true or false.</returns>
        public static bool CheckOperator(char op)
        {
            switch (op)
            {
                case '+':
                    return true;
                case '-':
                    return true;
                case '*':
                    return true;
                case '/':
                    return true;
            }

            return false;
        }
    }
}
