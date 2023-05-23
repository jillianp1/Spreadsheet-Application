// <copyright file="MultiplyNode.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class MultiplyNode : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplyNode"/> class.
        /// </summary>
        public MultiplyNode()
            : base('*')
        {
        }

        /// <summary>
        /// Overrides evaluate method. Multiplies left and right.
        /// </summary>
        /// <param name="left">left.</param>
        /// <param name="right">right.</param>
        /// <returns>double after multiplying.</returns>
        public override double Evaluate(double left, double right)
        {
            return left * right;
        }
    }
}
