// <copyright file="Form1.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadSheetEngine;
using SpreadsheetEngineFixed;

namespace Spreadsheet_Jillian_Plahn
{
    /// <summary>
    /// Form class creates spreadsheet object.
    /// </summary>
    public partial class Form1 : Form
    {
        // Spreadsheet object
        private SpreadsheetClass spreadsheet;

        // Undo redo class
        public UndoRedo undoRedo = new UndoRedo();

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// Form will initialize all components and crate intitial spreadsheet.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();

            // Initialize spreadhseet with 26 columns and 50 rows
            this.spreadsheet = new SpreadsheetClass(50, 26);
        }

        /// <summary>
        /// This method will load the form and calls the initialize method for the spreadsheet.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.InitializeDataGrid();
        }

        /// <summary>
        /// Cell property changed event. When a cells value changes it is updated in the DataGridView.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void Spreadsheet_CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Cell to update:
            CellClass updateCell = sender as CellClass;

            // Convert column
            int columnNum = updateCell.ColumnIndex - 64;

            if (e.PropertyName == "Value")
            {
                if (columnNum == 0)
                {
                    this.dataGridView1.Rows[updateCell.RowIndex - 1].Cells[columnNum].Value = updateCell.Value;
                }
                else
                {
                    // Update the cell.
                    // Set row property and value property.
                    this.dataGridView1.Rows[updateCell.RowIndex - 1].Cells[columnNum - 1].Value = updateCell.Value;
                }
            }

            if (e.PropertyName == "Background")
            {
                if (updateCell != null)
                {
                    this.dataGridView1.Rows[updateCell.RowIndex - 1].Cells[columnNum].Style.BackColor = Color.FromArgb((int)updateCell.Color);
                }
            }
        }

        /// <summary>
        /// Creates 50 rows and columns A-Z.
        /// </summary>
        public void InitializeDataGrid()
        {
            // Clear any columns already made
            this.dataGridView1.Columns.Clear();

            // update menu so user dosnt try to redo or undo without editing.
            this.updateMenu();

            // Create columns A to Z with code
            char alphaHeader = 'A';
            for (int i = 0; i < 26; i++, alphaHeader++)
            {
                this.dataGridView1.Columns.Add(alphaHeader.ToString(), alphaHeader.ToString());
            }

            // Clear any rows made
            this.dataGridView1.Rows.Clear();

            // Create rows 1-50
            for (int i = 1; i <= 50; i++)
            {
                // Add the rows
                this.dataGridView1.Rows.Add();

                // Add the row headers
                this.dataGridView1.Rows[i - 1].HeaderCell.Value = i.ToString();
            }

            // Subscribe to CellPropertyChanged event
            this.spreadsheet.CellPropertyChanged += this.Spreadsheet_CellPropertyChanged;

            this.dataGridView1.CellBeginEdit += this.dataGridView1_CellBeginEdit;
            this.dataGridView1.CellEndEdit += this.dataGridView1_CellEndEdit;
        }

        /// <summary>
        /// Occurs when edit mode starts for a selected cell.
        /// CellBeginEdit.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Finding cell were editing.
            int row = e.RowIndex;
            int column = e.ColumnIndex;

            // Getting cell from spreadhseet.
            CellClass editCell = this.spreadsheet.GetCell(row, column + 1);

            // Setting value of cell to spreadseet cells text.
            this.dataGridView1.Rows[row].Cells[column].Value = editCell.Text;
        }

        /// <summary>
        /// Method called when cell is done beign edited.
        /// CellEndEdit.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string text;

            // Get cell being edited.
            int row = e.RowIndex;
            int column = e.ColumnIndex;
            CellClass editCell = this.spreadsheet.GetCell(row, column + 1);
            IUndoRedo[] undos = new IUndoRedo[1];

            // Text to be added and replaced in undo stack.
            undos[0] = new TextRedoUndo(editCell, editCell.Text);

            // If the value is not null:
            if (this.dataGridView1.Rows[row].Cells[column].Value != null)
            {
                text = this.dataGridView1.Rows[row].Cells[column].Value.ToString();
            }
            else
            {
                text = string.Empty;
            }

            // Set the text of cell:
            editCell.Text = text;

            // add to the undo array.
            this.undoRedo.AddUndo(new UndoRedoCollection("Text", undos));

            // Set the value of spreadsheet cell.
            this.dataGridView1.Rows[row].Cells[column].Value = editCell.Value;

            // Update menu options.
            this.updateMenu();
        }

        /// <summary>
        /// When user clicks change background color.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.datagridviewcellstyle.backcolor?redirectedfrom=MSDN&view=windowsdesktop-7.0#System_Windows_Forms_DataGridViewCellStyle_BackColor.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.colordialog?redirectedfrom=MSDN&view=windowsdesktop-7.0.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.drawing.color.fromargb?redirectedfrom=MSDN&view=net-7.0#System_Drawing_Color_FromArgb_System_Int32_.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void changeBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uint newColor = 0;
            ColorDialog colorDialog = new ColorDialog();

            // need a list of commands for when I implement undo and redo.
            List<IUndoRedo> undo = new List<IUndoRedo>();

            // If user selects OK:
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // Sets new color picked by user to color in dialog
                // But converts to number
                newColor = (uint)colorDialog.Color.ToArgb();

                // Update all cells user has selected
                foreach (DataGridViewCell cell in this.dataGridView1.SelectedCells)
                {
                    // get cell.
                    CellClass spreadsheetCell = this.spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex);

                    // add old color to undo command list
                    undo.Add(new BackgroundColor(spreadsheetCell.Color, spreadsheetCell));

                    // Update color
                    spreadsheetCell.Color = newColor;
                }

                // Add to undo list
                this.undoRedo.AddUndo(new UndoRedoCollection("Background", undo));

                // Update the menu.
                this.updateMenu();
            }
        }

        /// <summary>
        /// Have to update menu when undo or redo option is not allowed.
        /// </summary>
        private void updateMenu()
        {
            ToolStripMenuItem menuItems = this.menuStrip1.Items[2] as ToolStripMenuItem;
            foreach (ToolStripMenuItem item in menuItems.DropDownItems)
            {
                if (item.Text.Contains("Undo"))
                {
                    item.Enabled = this.undoRedo.CheckEmptyUndo;
                }
                else if (item.Text.Contains("Redo"))
                {
                    item.Enabled = this.undoRedo.CheckEmptyRedo;
                }
            }
        }

        /// <summary>
        /// Undo action when button clicked.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void undoCellTextChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Call undo function.
            this.undoRedo.Undo(this.spreadsheet);

            // Update the menu options.
            this.updateMenu();
        }

        /// <summary>
        /// Redo action when button clicked.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void redoCellTextChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Call redo function.
            this.undoRedo.Redo(this.spreadsheet);

            // Update the menu options.
            this.updateMenu();
        }

        /// <summary>
        /// When user clicks save.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create save file dialog.
            var saveFileDialog = new SaveFileDialog();

            // Set the default extension to be an xml.
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.AddExtension = true;

            // If user selects ok.
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Create file to save.
                Stream savedFile = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                this.spreadsheet.Save(savedFile);

                // Close file.
                savedFile.Dispose();
            }
        }

        /// <summary>
        /// When user clicks load.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var loadFileDialog = new OpenFileDialog();

            // Set the default extension to be an xml.
            loadFileDialog.DefaultExt = "xml";
            loadFileDialog.AddExtension = true;

            if (loadFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream loadFile = new FileStream(loadFileDialog.FileName, FileMode.Open, FileAccess.Read);

                // Load into the spreadsheet.
                this.spreadsheet.Load(loadFile);

                loadFile.Dispose();
            }
        }
    }
}
