// <copyright file="TextRedoUndo.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadSheetEngine;

namespace SpreadsheetEngineFixed
{
    public class TextRedoUndo : IUndoRedo
    {
        private CellClass cell;
        private string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextRedoUndo"/> class.
        /// </summary>
        /// <param name="cellInput">inputed cell.</param>
        /// <param name="textInput">inputed text.</param>
        public TextRedoUndo(CellClass cellInput, string textInput)
        {
            this.cell = cellInput;
            this.text = textInput;
        }

        /// <summary>
        /// Fixes cell with text it had previous.
        /// </summary>
        /// <param name="spreadSheet">sheet.</param>
        /// <returns>old text and old cell.</returns>
        public IUndoRedo Execute(SpreadsheetClass spreadSheet)
        {
            // Get cell name.
            string name = this.cell.ColumnIndex.ToString() + this.cell.RowIndex.ToString();

            // Get the cell.
            CellClass cell = spreadSheet.GetCell(name);

            // save the current text for future redo undo.
            string currentText = cell.Text;

            // set cells text to new text.
            cell.Text = this.text;

            // Return old cell and text.
            return new TextRedoUndo(cell, currentText);
        }
    }
}
