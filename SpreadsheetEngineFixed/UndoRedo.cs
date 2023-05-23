// <copyright file="UndoRedo.cs" company="Jillian Plahn">
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
    /// <summary>
    /// Undo redo interface.
    /// </summary>
    public interface IUndoRedo
    {
        IUndoRedo Execute(SpreadsheetClass spreadsheet);
    }

    /// <summary>
    /// Undo Redo class.
    /// </summary>
    public class UndoRedo
    {
        // Create stacks for undo redo commands
        private Stack<UndoRedoCollection> undoStack = new Stack<UndoRedoCollection>();
        private Stack<UndoRedoCollection> redoStack = new Stack<UndoRedoCollection>();

        /// <summary>
        /// Gets a value indicating whether to check if undo stack is empty. If its empty cant undo anything.
        /// </summary>
        public bool CheckEmptyUndo
        {
            get { return this.undoStack.Count != 0;  }
        }

        /// <summary>
        /// Gets a value indicating whether check if redo stack is empty.
        /// </summary>
        public bool CheckEmptyRedo
        {
            get { return this.redoStack.Count != 0; }
        }

        /// <summary>
        /// Gets the undo action user wants to do.
        /// </summary>
        public string GetUndo
        {
            get
            {
                // if there is something on undo stack
                if (this.CheckEmptyUndo)
                {
                    return this.undoStack.Peek().text;
                }

                // otherwise stack is empty
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the redo action the user wants to do.
        /// </summary>
        public string GetRedo
        {
            get
            {
                // If stack is not empty
                if (this.CheckEmptyRedo)
                {
                    return this.redoStack.Peek().text;
                }

                // Else the stack is emoty return empty string
                return string.Empty;
            }
        }

        /// <summary>
        /// Does an undo and then adds to redo stack for future redo.
        /// </summary>
        /// <param name="spreadsheet">spreadhseet.</param>
        public void Undo(SpreadsheetClass spreadsheet)
        {
            UndoRedoCollection commands = this.undoStack.Pop();

            this.redoStack.Push(commands.Undo(spreadsheet));
        }

        /// <summary>
        /// Does a redo and adds to undo stack for future.
        /// </summary>
        /// <param name="spreadsheet">sheet.</param>
        public void Redo(SpreadsheetClass spreadsheet)
        {
            UndoRedoCollection commands = this.redoStack.Pop();

            this.undoStack.Push(commands.Undo(spreadsheet));
        }

        /// <summary>
        /// Adds command to undo stack.
        /// </summary>
        /// <param name="undos">undo command.</param>
        public void AddUndo(UndoRedoCollection undos)
        {
            // add to undo stack
            this.undoStack.Push(undos);
            this.redoStack.Clear();
        }

        /// <summary>
        /// Helper function for load to clear stacks.
        /// </summary>
        public void clearStacks()
        {
            this.undoStack.Clear();
            this.redoStack.Clear();
        }
    }
}
