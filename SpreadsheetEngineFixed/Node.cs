// <copyright file="Node.cs" company="Jillian Plahn">
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
    /// Base class for nodes.
    /// </summary>
    internal abstract class Node
    {
        protected string name;

        protected double opValue;

        /// <summary>
        /// Gets or sets and sets name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name == value)
                {
                    return;
                }
                else
                {
                    this.name = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets and sets operator value.
        /// </summary>
        public double OpValue
        {
            get
            {
                return this.opValue;
            }

            set
            {
                if (this.opValue == value)
                {
                    return;
                }
                else
                {
                    this.opValue = value;
                }
            }
        }
    }
}
