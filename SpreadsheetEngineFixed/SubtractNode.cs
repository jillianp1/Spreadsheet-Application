// <copyright file="SubtractNode.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class SubtractNode : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtractNode"/> class.
        /// </summary>
        public SubtractNode()
            : base('-')
        {
        }

        /// <summary>
        /// Evaluates subtraction.
        /// </summary>
        /// <param name="left">left.</param>
        /// <param name="right">right.</param>
        /// <returns>double.</returns>
        public override double Evaluate(double left, double right)
        {
            return left - right;
        }
    }
}
