// <copyright file="SpreadsheetClass.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using SpreadsheetEngine;
using SpreadsheetEngineFixed;

namespace SpreadSheetEngine
{
    /// <summary>
    /// Creates cells in the spreadhseet. Stores 2D array of cells in Spreadsheet.
    /// </summary>
    public class SpreadsheetClass
    {
        // Cell property changed event handler
        // public event PropertyChangedEventHandler CellPropertyChanged = delegate { };

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetClass"/> class.
        /// Constructor for SpreadSheet class.
        /// </summary>
        /// <param name="numColumns" >Number of columns. </param>
        /// <param name="numRows" >Number of rows. </param>
        public SpreadsheetClass(int numRows, int numColumns)
        {
            // Initialize array
            this.cells = new CellClass[numRows, numColumns];

            // Initialize dictionary
            this.referenceDict = new Dictionary<string, HashSet<string>>();

            // Loop through each cell
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numColumns; j++)
                {
                    // Convert j column to char
                    char letterJ = (char)(j + 64);

                    // Create new cell
                    this.cells[i, j] = new BaseCell(i + 1, letterJ);

                    // Subscribe to cells property changed event
                    this.cells[i, j].CellPropertyChanged += this.PropertyChanged;
                }
            }

            this.rowCount = numRows;
            this.columnCount = numColumns;
        }

        /// <summary>
        /// 2D array of cells.
        /// </summary>
        private CellClass[,] cells;

        /// <summary>
        /// Column and row count properies.
        /// </summary>
        private int columnCount;
        private int rowCount;

        /// <summary>
        /// Gets or sets number of columns in spreadsheet.
        /// </summary>
        public int ColumnCount
        {
            get { return this.columnCount; }
            set { this.columnCount = value; }
        }

        /// <summary>
        /// Gets or sets number of rows in spreadhseet.
        /// </summary>
        public int RowCount
        {
            get { return this.rowCount; }
            set { this.rowCount = value; }
        }

        /// <summary>
        /// References are stored in this dictionary.
        /// </summary>
        private Dictionary<string, HashSet<string>> referenceDict;

        /// <summary>
        /// Undo redo initialization.
        /// </summary>
        public UndoRedo undoRedo = new UndoRedo();

        /// <summary>
        /// This will set the values of cells. Creates base cells.
        /// Inherits from the cell class.
        /// </summary>
        private class BaseCell : CellClass
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BaseCell"/> class.
            /// Constructor for base cell.
            /// </summary>
            /// <param name="row"> row param. </param>
            /// <param name="column">column parameter. </param>
            public BaseCell(int row, char column)
                : base(row, column)
            {
            }

            /// <summary>
            /// Sets new value.
            /// </summary>
            /// <param name="newValue">newValue.</param>
            public void SetValue(string newValue)
            {
                this.value_ = newValue;

                // Notify subscribers that this cell changed.
                this.PropertyChanged("Value");
            }
        }

        // Cell property changed event handler
        public event PropertyChangedEventHandler CellPropertyChanged = delegate { };

        /// <summary>
        /// This is the cellPropertyChanged event
        /// Property changed method this updates the cells value.
        /// It takes the value from the cell and copies it to the other cells value.
        /// </summary>
        /// <param name="sender"> Sender parameter. </param>
        /// <param name="e"> e parameter. </param>
        public void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // If text property changed
            if (e.PropertyName == "Text")
            {
                BaseCell baseCell = sender as BaseCell;
                string cellName = baseCell.ColumnIndex.ToString() + baseCell.RowIndex.ToString();

                // Delete reference.
                this.DeleteReference(cellName);

                // If cell is a formula create new expression tree.
                if (baseCell.Text != string.Empty && baseCell.Text[0] == '=' && baseCell.Text.Length > 1)
                {
                    // New expression tree and set references.
                    ExpressionTree tree = new ExpressionTree(baseCell.Text.Substring(1));
                    this.SetReference(cellName, tree.GetVariable());
                }

                this.EvaluateCell(sender as CellClass);
            }
            else if (e.PropertyName == "Background")
            {
                this.CellPropertyChanged(sender, new PropertyChangedEventArgs("Background"));
            }
        }

        /// <summary>
        /// GetCell method that returns the cell at the given column and row indexes.
        /// </summary>
        /// <param name="rowIndex"> Index of row. </param>
        /// <param name="columnIndex">Index of column. </param>
        /// <returns> Returns a cell. </returns>
        public CellClass GetCell(int rowIndex, int columnIndex)
        {
            if (this.cells[rowIndex, columnIndex] == null)
            {
                return null;
            }
            else
            {
                return this.cells[rowIndex, columnIndex];
            }
        }

        /// <summary>
        /// Get Cell function that given a location finds the row and index then calls other get cell method.
        /// </summary>
        /// <param name="name">location.</param>
        /// <returns>cell.</returns>
        public CellClass GetCell(string name)
        {
            char column = name[0];
            int row;
            CellClass cell;

            // If first letter in column is not a char invalid.
            if (!char.IsLetter(column))
            {
                return null;
            }

            // If the rest of string is not integer return null.
            if (!int.TryParse(name.Substring(1), out row))
            {
                return null;
            }

            // Set the cell to the result of get cell sending it the row and column.
            cell = this.GetCell(row - 1, column - 64);
            return cell;
        }

        /// <summary>
        /// Method to properly evaluate the formula.
        /// </summary>
        /// <param name="cell">cell.</param>
        public void EvaluateCell(CellClass cell)
        {
            BaseCell evaluateCell = cell as BaseCell;
            string cellName = evaluateCell.ColumnIndex.ToString() + evaluateCell.RowIndex.ToString();

            // If there is a formula.
            if (evaluateCell.Text.Length > 1 && evaluateCell.Text[0] == '=')
            {
                double varValue;

                // Getting rid of the =
                string text = evaluateCell.Text.Substring(1);

                // Create expression tree
                ExpressionTree evaluateTree = new ExpressionTree(text);

                // Get variables into an array.
                string[] variables = evaluateTree.GetVariable();

                foreach (string name in variables)
                {
                    // If cell its referencing does not exist
                    if (this.GetCell(name) == null)
                    {
                        evaluateCell.SetValue("!(Bad reference)");
                        this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                        return;
                    }

                    // Here we know it is a valid cell so get the cell.
                    CellClass varCell = this.GetCell(name);

                    if (name == cellName)
                    {
                        evaluateCell.SetValue("!(Self reference)");
                        this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                        return;
                    }

                    // If value is a number
                    else if (double.TryParse(varCell.Value, out varValue))
                    {
                        evaluateTree.SetVariable(name, varValue);
                    }

                    // Else if refrencing a cell that DOES exist but has no numerical value or no value at all set value to 0.
                    else
                    {
                        evaluateCell.SetValue("0");
                        this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                    }
                }

                // Check each varibale in variable list for circular refrence.
                foreach (string name in variables)
                {
                    string cellNames = evaluateCell.ColumnIndex.ToString() + evaluateCell.RowIndex.ToString();
                    if (this.CircularReference(name, cellNames) == true)
                    {
                        evaluateCell.SetValue("!(Circular Reference!)");
                        this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                        return;
                    }
                }

                // All variabel are good and can evaluate tree.
                evaluateCell.SetValue(evaluateTree.Evaluate().ToString());
                this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
            }

            // if its not a formula just test the value.

            // If text is empty.
            else if (string.IsNullOrWhiteSpace(evaluateCell.Text))
            {
                evaluateCell.SetValue(string.Empty);
                this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
            }
            else
            {
                evaluateCell.SetValue(evaluateCell.Text);
                this.CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
            }

            // Last have to update dependency and references
            if (this.referenceDict.ContainsKey(cellName))
            {
                foreach (string cells in this.referenceDict[cellName])
                {
                    this.EvaluateCell(this.GetCell(cells));
                }
            }
        }

        /// <summary>
        /// Finds if there is a circular reference.
        /// </summary>
        /// <param name="variableName">variable name.</param>
        /// <param name="currentCell">current cell.</param>
        /// <returns>true or false whether or not there is a circular reference.</returns>
        public bool CircularReference(string variableName, string currentCell)
        {
            if (variableName == currentCell)
            {
                return true;
            }

            // If current cell is not in the reference dictionary then there is no circular reference.
            if (this.referenceDict.ContainsKey(currentCell) == false)
            {
                return false;
            }

            foreach (string cell in this.referenceDict[currentCell])
            {
                // Call circular reference function on all cells in the dictionary.
                if (this.CircularReference(variableName, cell))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // At this point there is no circular reference found.
            return false;
        }

        /// <summary>
        /// Method to set references.
        /// </summary>
        private void SetReference(string cellName, string[] variables)
        {
            // Go through each varibale in the cell
            foreach (string variableName in variables)
            {
                // If variable name isnt already in dictionary add it.
                if (!this.referenceDict.ContainsKey(variableName))
                {
                    this.referenceDict[variableName] = new HashSet<string>();
                }

                // Add reference for variable.
                this.referenceDict[variableName].Add(cellName);
            }
        }

        /// <summary>
        /// Method to delete a reference.
        /// </summary>
        /// <param name="cellName">cell name.</param>
        private void DeleteReference(string cellName)
        {
            List<string> referenceList = new List<string>();

            foreach (string key in this.referenceDict.Keys)
            {
                if (this.referenceDict[key].Contains(cellName))
                {
                    // Add key to the list.
                    referenceList.Add(key);
                }
            }

            // Go through the list and remove from the reference dictionary
            foreach (string item in referenceList)
            {
                HashSet<string> set = this.referenceDict[item];
                if (set.Contains(cellName))
                {
                    set.Remove(cellName);
                }
            }
        }

        /// <summary>
        /// Will save file to an XML file.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter?redirectedfrom=MSDN&view=net-7.0.
        /// </summary>
        /// <param name="saveFile">file to save.</param>
        public void Save(Stream saveFile)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(saveFile, settings);

            using (writer)
            {
                // Begin writing the document.
                writer.WriteStartDocument();

                // Creates spreadsheet element as main element.
                writer.WriteStartElement("Spreadsheet");

                foreach (var cell in this.cells)
                {
                    // if the cell text has been changed and the background color isnt set to default.
                    if (cell.Text != string.Empty | cell.Color != 4294967295)
                    {
                        // cell element.
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("column", cell.ColumnIndex.ToString());
                        writer.WriteElementString("row", cell.RowIndex.ToString());
                        writer.WriteElementString("text", cell.Text);
                        writer.WriteElementString("color", cell.Color.ToString());
                        writer.WriteEndElement();
                    }
                }

                // End spreadshett element.
                writer.WriteEndElement();

                // End the document.
                writer.WriteEndDocument();

                // Close writer to save.
                writer.Close();
            }
        }

        /// <summary>
        /// Loads an xml file.
        /// Referenced the following:
        /// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.read?view=net-8.0.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlreader?redirectedfrom=MSDN&view=net-7.0.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.xml.linq.xdocument?redirectedfrom=MSDN&view=net-7.0.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlnode?redirectedfrom=MSDN&view=net-7.0.
        /// https://www.codeproject.com/Articles/169598/Parse-XML-Documents-by-XMLDocument-and-XDocument.
        /// </summary>
        /// <param name="loadFile">file to load.</param>
        public void Load(Stream loadFile)
        {
            // Need to clear all spreadsheet data before loading file data.
            foreach (var cell in this.cells)
            {
                cell.Text = string.Empty;
                cell.Color = 4294967295;
            }

            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader reader = XmlReader.Create(loadFile, settings);

            string text = string.Empty;

           // CellClass cells = null;
            char letter;
            int column = 0;
            int row = 0;
            uint color = 4294967295;

            while (reader.Read())
            {
                if (reader.Name == "cell")
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "column":
                                    // when the column is read convert to an int.
                                    reader.Read();
                                    letter = char.Parse(reader.Value);
                                    column = letter - 64;
                                    break;
                                case "row":
                                    // when you get row convert row to an int.
                                    reader.Read();
                                    row = Convert.ToInt32(reader.Value) - 1;
                                    break;
                                case "color":
                                    reader.Read();
                                    color = Convert.ToUInt32(reader.Value);
                                    break;
                                case "text":
                                    reader.Read();
                                    text = reader.Value;
                                    break;
                            }
                        }
                        else if (reader.Name == "cell")
                        {
                            break;
                        }
                    }

                    // Sets loaded bckground color.
                    if (color != 4294967295)
                    {
                        this.cells[row, column].Color = color;
                    }

                    if (text == null)
                    {
                        break;
                    }
                    else
                    {
                        this.cells[row, column].Text = text;
                    }
                }
            }

            // Close the reader.
            reader.Close();

            // Have to clear undo redo stacks when done.
            this.undoRedo.clearStacks();
        }
    }
}