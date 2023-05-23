// <copyright file="AddNode.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SpreadsheetEngine;

    internal class AddNode : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddNode"/> class.
        /// </summary>
        public AddNode()
            : base('+')
        {
        }

        /// <summary>
        /// Override the function to evaluate left plus right.
        /// </summary>
        /// <param name="left">left.</param>
        /// <param name="right">right.</param>
        /// <returns>double.</returns>
        public override double Evaluate(double left, double right)
        {
            return left + right;
        }
    }
}
