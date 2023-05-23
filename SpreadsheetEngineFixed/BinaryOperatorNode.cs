// <copyright file="BinaryOperatorNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal abstract class BinaryOperatorNode : Node
    {
        private char operate;

        private Node left;
        private Node right;

        private char value;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
        /// </summary>
        /// <param name="op">Inputed operator.</param>
        /// <param name="left">Left node.</param>
        /// <param name="right">Right node.</param>
        public BinaryOperatorNode(char op, Node left, Node right)
        {
            this.operate = op;
            this.left = left;
            this.right = right;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
        /// </summary>
        /// <param name="valueInput">Inputed value. </param>
        public BinaryOperatorNode(char valueInput)
        {
            this.value = valueInput;
        }

        /// <summary>
        /// Gets operator.
        /// </summary>
        public char Operator
        {
            get { return this.operate; }
        }

        /// <summary>
        /// Gets or sets left node.
        /// </summary>
        public Node Left
        {
            get { return this.left; }
            set { this.left = value; }
        }

        /// <summary>
        /// Gets or sets right node.
        /// </summary>
        public Node Right
        {
            get { return this.right; }
            set { this.right = value; }
        }

        /// <summary>
        /// Evaluates the expression and returns double.
        /// </summary>
        /// <param name="left">left double.</param>
        /// <param name="right">right double.</param>
        /// <returns>double.</returns>
        public abstract double Evaluate(double left, double right);
    }
}
