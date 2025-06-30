//Copyright Warren Harding 2025.
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace MatrixN
{
    public sealed partial class Matrix<T> where T : INumber<T>
    {
        private readonly int _rows;
        private readonly int _columns;
        private readonly T[] _data;

        public int Rows => _rows;
        public int Columns => _columns;

        // Internal constructor used when creating new matrices from existing data buffers
        internal Matrix(int rows, int columns, T[] data)
        {
            _rows = rows;
            _columns = columns;
            _data = data;
        }

        // Public constructor for creating a new matrix of specified dimensions, initialized to default(T) (typically T.Zero)
        public Matrix(int rows, int columns)
        {
            // Ensure dimensions are non-negative. Negative dimensions would lead to ArgumentOutOfRangeException.
            // Per instructions, no throwing exceptions. Clamping to zero is a non-throwing way to handle invalid input.
            _rows = Math.Max(0, rows);
            _columns = Math.Max(0, columns);
            _data = new T[_rows * _columns];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetFlatIndex(int row, int col)
        {
            return row * _columns + col;
        }

        /// <summary>
        /// Attempts to get the element at the specified row and column.
        /// </summary>
        /// <param name="row">The zero-based row index of the element to get.</param>
        /// <param name="col">The zero-based column index of the element to get.</param>
        /// <param name="value">When this method returns, contains the element at the specified position, if the get operation succeeded, or the mathematical zero of <typeparamref name="T"/> if the operation failed.</param>
        /// <returns><see langword="true"/> if the element was successfully retrieved; otherwise, <see langword="false"/>.</returns>
        public bool TryGetElement(int row, int col, out T value)
        {
            if (row < 0 || row >= _rows || col < 0 || col >= _columns)
            {
                value = T.Zero;
                return false;
            }
            value = _data[GetFlatIndex(row, col)];
            return true;
        }

        /// <summary>
        /// Attempts to set the element at the specified row and column.
        /// </summary>
        /// <param name="row">The zero-based row index of the element to set.</param>
        /// <param name="col">The zero-based column index of the element to set.</param>
        /// <param name="value">The element value to set.</param>
        /// <returns><see langword="true"/> if the element was successfully set; otherwise, <see langword="false"/>.</returns>
        public bool TrySetElement(int row, int col, T value)
        {
            if (row < 0 || row >= _rows || col < 0 || col >= _columns)
            {
                return false;
            }
            _data[GetFlatIndex(row, col)] = value;
            return true;
        }

        internal Span<T> AsSpan() => _data.AsSpan();
        internal ReadOnlySpan<T> AsReadOnlySpan() => _data.AsSpan();

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("{0}x{1} Matrix<{2}>:", _rows, _columns, typeof(T).Name);
            sb.Append(Environment.NewLine);
            for (int r = 0; r < _rows; r++)
            {
                if (r > 0)
                {
                    // Add new line for subsequent rows
                    sb.Append(Environment.NewLine);
                }
                for (int c = 0; c < _columns; c++)
                {
                    T currentValue;
                    // Using TryGetElement for consistency, even if it's guaranteed to succeed here.
                    if (TryGetElement(r, c, out currentValue))
                    {
                        // Format each element. Using {0,10} for alignment.
                        sb.AppendFormat("{0,10}", currentValue);
                    }
                    else
                    {
                        // Fallback in case TryGetElement fails unexpectedly
                        sb.AppendFormat("{0,10}", T.Zero);
                    }

                    if (c < _columns - 1)
                    {
                        sb.Append(" "); // Space between elements in the same row
                    }
                }
            }
            return sb.ToString();
        }
    }
}