//Copyright Warren Harding 2025.
using System.Numerics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MatrixN
{
    // This partial class contains matrix arithmetic operations and related static methods.
    public sealed partial class Matrix<T> where T : INumber<T>
    {
        #region Operators
        // Operators assume compatible dimensions when called directly or are implicitly guarded by Try* methods.
        // No ArgumentException throws, adhering to the "Do not use C# throw statements" principle.

        /// <summary>
        /// Adds two matrices element-wise.
        /// </summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>A new matrix representing the sum of the two matrices.</returns>
        /// <remarks>This operator assumes the input matrices have compatible dimensions. Use <see cref="TryAdd"/> for dimension validation.</remarks>
        public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right)
        {
            int rows = left.Rows;
            int columns = left.Columns;
            T[] newBuffer = new T[rows * columns];
            for (int i = 0; i < newBuffer.Length; i++)
            {
                newBuffer[i] = left._data[i] + right._data[i];
            }
            return new Matrix<T>(rows, columns, newBuffer);
        }

        /// <summary>
        /// Subtracts one matrix from another element-wise.
        /// </summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>A new matrix representing the difference of the two matrices.</returns>
        /// <remarks>This operator assumes the input matrices have compatible dimensions. Use <see cref="TrySubtract"/> for dimension validation.</remarks>
        public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right)
        {
            int rows = left.Rows;
            int columns = left.Columns;
            T[] newBuffer = new T[rows * columns];
            for (int i = 0; i < newBuffer.Length; i++)
            {
                newBuffer[i] = left._data[i] - right._data[i];
            }
            return new Matrix<T>(rows, columns, newBuffer);
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        /// <param name="matrix">The matrix to multiply.</param>
        /// <param name="scalar">The scalar value.</param>
        /// <returns>A new matrix with each element multiplied by the scalar.</returns>
        public static Matrix<T> operator *(Matrix<T> matrix, T scalar)
        {
            int rows = matrix.Rows;
            int columns = matrix.Columns;
            T[] newBuffer = new T[rows * columns];
            for (int i = 0; i < newBuffer.Length; i++)
            {
                newBuffer[i] = matrix._data[i] * scalar;
            }
            return new Matrix<T>(rows, columns, newBuffer);
        }

        /// <summary>
        /// Multiplies a scalar value by a matrix.
        /// </summary>
        /// <param name="scalar">The scalar value.</param>
        /// <param name="matrix">The matrix to multiply.</param>
        /// <returns>A new matrix with each element multiplied by the scalar.</returns>
        public static Matrix<T> operator *(T scalar, Matrix<T> matrix)
        {
            return matrix * scalar;
        }

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="left">The left-hand side matrix.</param>
        /// <param name="right">The right-hand side matrix.</param>
        /// <returns>A new matrix representing the product of the two matrices.</returns>
        /// <remarks>This operator assumes the input matrices have compatible dimensions (left.Columns == right.Rows). Use <see cref="TryMultiply"/> for dimension validation.</remarks>
        public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right)
        {
            int newRows = left.Rows;
            int newColumns = right.Columns;
            int commonDim = left.Columns; // This must equal right.Rows for valid multiplication
            T[] newBuffer = new T[newRows * newColumns];

            Parallel.For(0, newRows, rowIndex =>
            {
                for (int colIndex = 0; colIndex < newColumns; colIndex++)
                {
                    T sum = T.Zero;
                    for (int k = 0; k < commonDim; k++)
                    {
                        T leftVal;
                        T rightVal;
                        // It's safe to assume TryGetElement will succeed here as indices are within bounds.
                        left.TryGetElement(rowIndex, k, out leftVal);
                        right.TryGetElement(k, colIndex, out rightVal);

                        sum += leftVal * rightVal;
                    }
                    newBuffer[rowIndex * newColumns + colIndex] = sum;
                }
            });
            return new Matrix<T>(newRows, newColumns, newBuffer);
        }
        #endregion

        /// <summary>
        /// Transposes the given matrix, swapping rows and columns.
        /// </summary>
        /// <param name="matrix">The matrix to transpose.</param>
        /// <returns>A new matrix representing the transpose of the input matrix.</returns>
        public static Matrix<T> Transpose(Matrix<T> matrix)
        {
            Matrix<T> result = new Matrix<T>(matrix.Columns, matrix.Rows);
            Parallel.For(0, matrix.Rows, i =>
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    T elementValue;
                    // It's safe to assume TryGetElement/TrySetElement will succeed here as indices are within bounds.
                    if (matrix.TryGetElement(i, j, out elementValue))
                    {
                        result.TrySetElement(j, i, elementValue);
                    }
                }
            });
            return result;
        }

        // Static methods providing a safe, fallible interface for matrix operations.
        // These methods perform dimension checks and return success/failure.

        /// <summary>
        /// Attempts to add two matrices.
        /// </summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <param name="result">When this method returns, contains the sum of the two matrices if successful; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the matrices were successfully added; otherwise, <see langword="false"/> (e.g., incompatible dimensions).</returns>
        public static bool TryAdd(Matrix<T> left, Matrix<T> right, [NotNullWhen(true)] out Matrix<T>? result)
        {
            if (left.Rows != right.Rows || left.Columns != right.Columns)
            {
                result = null;
                return false;
            }
            result = left + right; // Dimensions are compatible, so operator call will not fail conceptually.
            return true;
        }

        /// <summary>
        /// Attempts to subtract one matrix from another.
        /// </summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <param name="result">When this method returns, contains the difference of the two matrices if successful; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the matrices were successfully subtracted; otherwise, <see langword="false"/> (e.g., incompatible dimensions).</returns>
        public static bool TrySubtract(Matrix<T> left, Matrix<T> right, [NotNullWhen(true)] out Matrix<T>? result)
        {
            if (left.Rows != right.Rows || left.Columns != right.Columns)
            {
                result = null;
                return false;
            }
            result = left - right; // Dimensions are compatible, so operator call will not fail conceptually.
            return true;
        }

        /// <summary>
        /// Attempts to multiply two matrices.
        /// </summary>
        /// <param name="left">The left-hand side matrix.</param>
        /// <param name="right">The right-hand side matrix.</param>
        /// <param name="result">When this method returns, contains the product of the two matrices if successful; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the matrices were successfully multiplied; otherwise, <see langword="false"/> (e.g., incompatible dimensions).</returns>
        public static bool TryMultiply(Matrix<T> left, Matrix<T> right, [NotNullWhen(true)] out Matrix<T>? result)
        {
            if (left.Columns != right.Rows)
            {
                result = null;
                return false;
            }
            result = left * right; // Dimensions are compatible, so operator call will not fail conceptually.
            return true;
        }

        /// <summary>
        /// Multiplies a matrix by a scalar value.
        /// </summary>
        /// <param name="matrix">The matrix to multiply.</param>
        /// <param name="scalar">The scalar value.</param>
        /// <returns>A new matrix with each element multiplied by the scalar.</returns>
        /// <remarks>Scalar multiplication always succeeds, so no `Try*` version is typically needed.</remarks>
        public static Matrix<T> ScalarMultiply(Matrix<T> matrix, T scalar)
        {
            return matrix * scalar;
        }
    }
}