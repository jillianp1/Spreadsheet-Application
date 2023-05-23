// <copyright file="BackgroundColor.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngineFixed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SpreadSheetEngine;

    /// <summary>
    /// Background color class.
    /// </summary>
    public class BackgroundColor : IUndoRedo
    {
        private CellClass cell;
        private uint color;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundColor"/> class.
        /// </summary>
        /// <param name="colorInput">inputed color.</param>
        /// <param name="cellInput">inputed cell.</param>
        public BackgroundColor(uint colorInput, CellClass cellInput)
        {
            this.color = colorInput;
            this.cell = cellInput;
        }

        /// <summary>
        /// sets color of cell.
        /// </summary>
        /// <param name="spreadsheet">sheet.</param>
        /// <returns>cell and color.</returns>
        public IUndoRedo Execute(SpreadsheetClass spreadsheet)
        {
            // Get cell name
            string name = this.cell.ColumnIndex.ToString() + this.cell.RowIndex.ToString();

            // save current color.
            uint currentColor = this.cell.Color;

            // set background color
            this.cell.Color = this.color;

            return new BackgroundColor(currentColor, this.cell);
        }
    }
}
