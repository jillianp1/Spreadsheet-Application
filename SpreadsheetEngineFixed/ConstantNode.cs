// <copyright file="ConstantNode.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Constant Node represents a constant numerical value.
    /// </summary>
    internal class ConstantNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// Contructor for constant node.
        /// </summary>
        /// <param name="num">number. </param>
        public ConstantNode(double num)
        {
            this.opValue = num;
        }
    }
}
