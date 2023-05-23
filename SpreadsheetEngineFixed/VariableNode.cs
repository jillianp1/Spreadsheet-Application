// <copyright file="VariableNode.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Variable node represents a variable.
    /// </summary>
    internal class VariableNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// Constructor for variable node.
        /// </summary>
        /// <param name="varName">Variable name. </param>
        public VariableNode(string varName)
        {
            this.name = varName;
        }
    }
}
