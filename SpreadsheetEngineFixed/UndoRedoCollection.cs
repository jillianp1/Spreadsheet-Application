// <copyright file="UndoRedoCollection.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using SpreadSheetEngine;

namespace SpreadsheetEngineFixed
{
    public class UndoRedoCollection
    {
        private IUndoRedo[] objects;

        public string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoCollection"/> class.
        /// Constructor for undo redo collection.
        /// </summary>
        public UndoRedoCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoCollection"/> class.
        /// </summary>
        /// <param name="textInput">inputed text.</param>
        /// <param name="commandsInput">commands.</param>
        public UndoRedoCollection(string textInput, IUndoRedo[] commandsInput)
        {
            this.text = textInput;
            this.objects = commandsInput;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoCollection"/> class.
        /// </summary>
        /// <param name="textInput">text.</param>
        /// <param name="commandsInput">commands.</param>
        public UndoRedoCollection(string textInput, List<IUndoRedo> commandsInput)
        {
            this.text = textInput;
            this.objects = commandsInput.ToArray();
        }

        /// <summary>
        /// Creates an undo collects the commands into a list.
        /// </summary>
        /// <param name="spreadsheet">spreadsheet.</param>
        /// <returns>commands.</returns>
        public UndoRedoCollection Undo(SpreadsheetClass spreadsheet)
        {
            List<IUndoRedo> listCommands = new List<IUndoRedo>();

            // execute each coomand in the list
            foreach (IUndoRedo command in this.objects)
            {
                listCommands.Add(command.Execute(spreadsheet));
            }

            return new UndoRedoCollection(this.text, listCommands.ToArray());
        }
    }
}
