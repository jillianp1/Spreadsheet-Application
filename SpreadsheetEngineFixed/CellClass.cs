// <copyright file="CellClass.cs" company="Jillian Plahn">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace SpreadSheetEngine
{
    public abstract class CellClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler CellPropertyChanged = delegate { };

        private readonly int rowIndex;
        private readonly char columnIndex;
        protected string text = string.Empty;
        protected string value_;

        // Default background color is white
        private uint BGColor = 4294967295;

        /// <summary>
        /// Initializes a new instance of the <see cref="CellClass"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="row">row.</param>
        /// <param name="column">column.</param>
        public CellClass(int row, char column)
        {
            this.rowIndex = row;
            this.columnIndex = column;
        }

        /// <summary>
        /// Gets rowINdex getter returns field.
        /// </summary>
        public int RowIndex
        {
            get { return this.rowIndex; }
        }

        /// <summary>
        /// Gets column Index Getter.
        /// </summary>
        public char ColumnIndex
        {
            get { return this.columnIndex; }
        }

        /// <summary>
        /// Gets or sets text getter.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (this.text != value)
                {
                    this.text = value;

                    // Notify subscribers that property has changed
                    this.PropertyChanged("Text");
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Gets Value. Value should be a getter only and only the spreadhseet class is allowed to set.
        /// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/internal.
        /// </summary>
        public string Value
        {
            get
            {
                return this.value_;
            }

            internal set
            {
                if (value != this.value_)
                {
                    this.value_ = value;
                    this.PropertyChanged("Value");
                }
            }
        }

        /// <summary>
        /// Gets or sets background color.
        /// </summary>
        public uint Color
        {
            get
            {
                return this.BGColor;
            }

            set
            {
                if (this.BGColor == value)
                {
                    return;
                }
                else
                {
                    this.BGColor = value;
                    this.PropertyChanged("Background");
                }
            }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public void PropertyChanged(string property)
        {
            this.CellPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}