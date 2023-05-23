// <copyright file="SpreadsheetTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Data.Common;
using NUnit.Framework;
using SpreadSheetEngine;
using SpreadsheetEngineFixed;

namespace Spreadsheet_Jillian_Plahn.SpreadsheetEngineFixed.Tests
{
    public class SpreadsheetTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        // Test with one thing on stack.
        [Test]
        public void UndoTest()
        {
            // Spreadhseet
            SpreadsheetClass sheet = new SpreadsheetClass(5, 5);

            // Get cell and create cell text
            CellClass editCell = sheet.GetCell(1, 1 + 1);
            editCell.Text = "=4";

            // Create undo to be added to the undo stack
            IUndoRedo[] undos = new IUndoRedo[1];
            undos[0] = new TextRedoUndo(editCell, editCell.Text);

            // Add one thing to the undo stack
            sheet.undoRedo.AddUndo(new UndoRedoCollection("Text", undos));

            try
            {
                // Call the undo
                sheet.undoRedo.Undo(sheet);
            }
            catch
            {
                Assert.Fail("Expected no exceptions");
            }
        }

        [Test]

        // Test undo with 2 things on the stack.
        public void UndoTest2()
        {
            // Spreadhseet
            SpreadsheetClass sheet = new SpreadsheetClass(5, 5);

            // Get cell and create cell text
            CellClass editCell = sheet.GetCell(1, 1 + 1);
            editCell.Text = "=4";
            CellClass editCell2 = sheet.GetCell(1, 2 + 1);
            editCell2.Text = "=8";

            // Create undo to be added to the undo stack
            IUndoRedo[] undos = new IUndoRedo[1];
            undos[0] = new TextRedoUndo(editCell, editCell.Text);
            undos[0] = new TextRedoUndo(editCell2, editCell2.Text);

            // Add to the undo stack
            sheet.undoRedo.AddUndo(new UndoRedoCollection("Text", undos));

            try
            {
                // Call the undo
                sheet.undoRedo.Undo(sheet);
            }
            catch
            {
                Assert.Fail("Expected no exceptions");
            }
        }

        // Test with undo then redo.
        [Test]
        public void UndoRedoTest()
        {
            // Spreadhseet
            SpreadsheetClass sheet = new SpreadsheetClass(5, 5);

            // Get cell and create cell text
            CellClass editCell = sheet.GetCell(1, 1 + 1);
            editCell.Text = "=4";
            CellClass editCell2 = sheet.GetCell(1, 2 + 1);
            editCell2.Text = "=8";

            // Create undo to be added to the undo stack
            IUndoRedo[] undos = new IUndoRedo[1];
            undos[0] = new TextRedoUndo(editCell, editCell.Text);
            undos[0] = new TextRedoUndo(editCell2, editCell2.Text);

            // Add to the undo stack
            sheet.undoRedo.AddUndo(new UndoRedoCollection("Text", undos));

            try
            {
                sheet.undoRedo.Undo(sheet);
                sheet.undoRedo.Redo(sheet);
            }
            catch
            {
                Assert.Fail("Expected no error");
            }
        }

        // Test with multiple undo redo.
        [Test]
        public void UndoRedoTest2()
        {
            // Spreadhseet
            SpreadsheetClass sheet = new SpreadsheetClass(5, 5);

            // Get cell and create cell text
            CellClass editCell = sheet.GetCell(1, 1 + 1);
            editCell.Text = "=4";
            CellClass editCell2 = sheet.GetCell(1, 2 + 1);
            editCell2.Text = "=8";
            CellClass editCell3 = sheet.GetCell(1, 3 + 1);
            editCell3.Text = "=16";

            // Create undo to be added to the undo stack
            IUndoRedo[] undos = new IUndoRedo[1];
            undos[0] = new TextRedoUndo(editCell, editCell.Text);
            undos[0] = new TextRedoUndo(editCell2, editCell2.Text);
            undos[0] = new TextRedoUndo(editCell3, editCell3.Text);

            // Add to the undo stack
            sheet.undoRedo.AddUndo(new UndoRedoCollection("Text", undos));

            try
            {
                sheet.undoRedo.Undo(sheet);
                sheet.undoRedo.Redo(sheet);
                sheet.undoRedo.Undo(sheet);
            }
            catch
            {
                Assert.Fail("Expected no error");
            }
        }

        // For load we assume only valid XML files will be loaded.
        // Load method test.
        [Test]
        public void loadTest()
        {
            string path = @"C:\Users\jilli\OneDrive\Documents\demo.xml";

            FileStream SourceStream = File.Open(path, FileMode.Open);

            // Spreadhseet
            SpreadsheetClass sheet = new SpreadsheetClass(5, 5);

            try
            {
                sheet.Load(SourceStream);
            }
            catch
            {
                Assert.Fail("Expected no error");
            }
        }

        // Save test.
        [Test]
        public void saveTest()
        {
            string path = @"C:\Users\jilli\OneDrive\Documents\FRIDAY.xml";

            FileStream SourceStream = File.Open(path, FileMode.Open);

            // Spreadhseet
            SpreadsheetClass sheet = new SpreadsheetClass(5, 5);

            try
            {
                sheet.Save(SourceStream);
            }
            catch
            {
                Assert.Fail("Expected no error");
            }
        }

        // Self Reference test
        [Test]
        public void selfReferenceTest()
        {
            // Spreadsheet
            SpreadsheetClass sheet = new SpreadsheetClass(50, 26);

            // Cell
            CellClass editCell = sheet.GetCell(1, 1);

            // Set the cell text to reference itself
            editCell.Text = "=A2";

            // When cell is evaluated it should be set to an error of self reference.
            sheet.EvaluateCell(editCell);
            Assert.AreEqual("!(Self reference)", editCell.Value);
        }

        // Self Reference test 2
        [Test]
        public void selfReferenceTest2()
        {
            // Spreadsheet
            SpreadsheetClass sheet = new SpreadsheetClass(50, 26);

            // Cell
            CellClass editCell = sheet.GetCell(1, 1);

            // Set the cell text to reference itself
            editCell.Text = "=A2+5/10";

            // When cell is evaluated it should be set to an error of self reference.
            sheet.EvaluateCell(editCell);
            Assert.AreEqual("!(Self reference)", editCell.Value);
        }

        // Test for bad reference
        [Test]
        public void badReferenceTest()
        {
            // Spreadsheet
            SpreadsheetClass sheet = new SpreadsheetClass(50, 26);
            CellClass editCell = sheet.GetCell(1, 1);

            // Set the cell text to something that does not exist and is a bad ref.
            editCell.Text = "=cat";

            sheet.EvaluateCell(editCell);
            Assert.AreEqual("!(Bad reference)", editCell.Value);
        }

        // Test for bad reference 2
        [Test]
        public void badReferenceTest2()
        {
            // Spreadsheet
            SpreadsheetClass sheet = new SpreadsheetClass(50, 26);
            CellClass editCell = sheet.GetCell(1, 1);

            // Set the cell text to something that does not exist and is a bad ref.
            editCell.Text = "=6+Cell*7";

            sheet.EvaluateCell(editCell);
            Assert.AreEqual("!(Bad reference)", editCell.Value);
        }

        // Test for circular reference
        [Test]
        public void circularReference()
        {
            // Spreadsheet
            SpreadsheetClass sheet = new SpreadsheetClass(50, 26);

            // A2
            CellClass cell1 = sheet.GetCell(1, 1);
            cell1.Text = "=B2*2";

            // B2
            CellClass cell2 = sheet.GetCell(2, 1);
            cell2.Text = "=B3*3";

            // A3
            CellClass cell3 = sheet.GetCell(1, 2);
            cell3.Text = "=A2*5";

            // B3
            CellClass cell4 = sheet.GetCell(2, 2);
            cell4.Text = "=A3*4";

            sheet.EvaluateCell(cell1);
            Assert.AreEqual("!(Circular Reference!)", cell4.Value);
        }
    }
}