//Copyright Warren Harding 2025.
using System;
using System.Numerics;
using System.Threading.Tasks; // Added for consistency with other partial classes, though not directly used here.

namespace MatrixN
{
    // This partial class contains methods for calculating the determinant of a matrix.
    public sealed partial class Matrix<T> where T : INumber<T>
    {
        /// <summary>
        /// Computes the submatrix (minor) by excluding a specified row and column.
        /// </summary>
        /// <param name="excludeRow">The zero-based row index to exclude.</param>
        /// <param name="excludeCol">The zero-based column index to exclude.</param>
        /// <returns>A new Matrix<T> that is the submatrix.</returns>
        /// <remarks>The resulting submatrix will have dimensions (Rows-1) x (Columns-1).</remarks>
        private Matrix<T> GetSubmatrix(int excludeRow, int excludeCol)
        {
            Matrix<T> submatrix = new Matrix<T>(_rows - 1, _columns - 1);
            int destRow = 0;
            for (int srcRow = 0; srcRow < _rows; srcRow++)
            {
                if (srcRow == excludeRow)
                {
                    continue;
                }

                int destCol = 0;
                for (int srcCol = 0; srcCol < _columns; srcCol++)
                {
                    if (srcCol == excludeCol)
                    {
                        continue;
                    }

                    T val;
                    // TryGetElement should succeed for valid source indices.
                    if (TryGetElement(srcRow, srcCol, out val))
                    {
                        // TrySetElement should succeed for valid destination indices.
                        submatrix.TrySetElement(destRow, destCol, val);
                    }
                    destCol++;
                }
                destRow++;
            }
            return submatrix;
        }

        /// <summary>
        /// Recursively calculates the determinant of a matrix using cofactor expansion.
        /// </summary>
        /// <param name="matrix">The matrix for which to calculate the determinant.</param>
        /// <returns>The determinant of the matrix. Returns T.Zero if the matrix is not square (should be guarded by TryGetDeterminant).</returns>
        private static T CalculateDeterminantRecursive(Matrix<T> matrix)
        {
            if (!matrix.IsSquare)
            {
                // This state should be prevented by the public TryGetDeterminant method.
                return T.Zero;
            }

            int n = matrix.Rows;

            if (n == 0) // Determinant of an empty matrix is 1.
            {
                return T.One;
            }

            if (n == 1)
            {
                T value;
                matrix.TryGetElement(0, 0, out value);
                return value;
            }

            if (n == 2)
            {
                // det([[a, b], [c, d]]) = ad - bc
                T a, b, c, d;
                matrix.TryGetElement(0, 0, out a);
                matrix.TryGetElement(0, 1, out b);
                matrix.TryGetElement(1, 0, out c);
                matrix.TryGetElement(1, 1, out d);
                return (a * d) - (b * c);
            }

            // For n > 2, use cofactor expansion along the first row
            T determinant = T.Zero;
            for (int col = 0; col < n; col++)
            {
                T element;
                // Get the element at (0, col) for cofactor expansion.
                if (!matrix.TryGetElement(0, col, out element))
                {
                    // Should theoretically not happen for valid matrices.
                    return T.Zero;
                }

                Matrix<T> minor = matrix.GetSubmatrix(0, col);
                T minorDeterminant = CalculateDeterminantRecursive(minor);

                T term = element * minorDeterminant;

                // Apply alternating signs: (-1)^(0+col) = (-1)^col
                if (col % 2 == 1) // If column index is odd (1, 3, 5...)
                {
                    term = T.Zero - term; // Equivalent to -term
                }
                determinant += term;
            }
            return determinant;
        }

        /// <summary>
        /// Attempts to calculate the determinant of this matrix.
        /// Only applicable to square matrices.
        /// </summary>
        /// <param name="determinant">When this method returns, contains the determinant of the matrix if calculation succeeds; otherwise, the mathematical zero of <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if the determinant was successfully calculated; otherwise, <see langword="false"/> (e.g., matrix is not square).</returns>
        public bool TryGetDeterminant(out T determinant)
        {
            if (!IsSquare)
            {
                determinant = T.Zero;
                return false;
            }

            // Handle 0x0 matrix specifically as per common mathematical convention for empty product/determinant.
            if (_rows == 0)
            {
                determinant = T.One;
                return true;
            }

            // Call the recursive helper method to calculate the determinant.
            determinant = CalculateDeterminantRecursive(this);
            return true;
        }
    }
}