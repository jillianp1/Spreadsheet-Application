// <copyright file="DivideNode.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngineFixed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SpreadsheetEngine;

    internal class DivideNode : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DivideNode"/> class.
        /// </summary>
        public DivideNode()
            : base('/')
        {
        }

        /// <summary>
        /// Overrides evaluate method and divides.
        /// </summary>
        /// <param name="left">left.</param>
        /// <param name="right">right.</param>
        /// <returns>double.</returns>
        public override double Evaluate(double left, double right)
        {
            return left / right;
        }
    }
}
