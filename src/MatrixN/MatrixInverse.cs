//Copyright Warren Harding 2025.
using System;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MatrixN
{
    // This partial class contains methods for calculating the inverse of a matrix.
    public sealed partial class Matrix<T> where T : INumber<T>
    {
        /// <summary>
        /// Computes the cofactor of an element at a given position.
        /// Cofactor C_ij = (-1)^(i+j) * det(M_ij) where M_ij is the minor matrix formed by removing row i and column j.
        /// </summary>
        /// <param name="row">The row index of the element.</param>
        /// <param name="col">The column index of the element.</param>
        /// <returns>The cofactor value.</returns>
        private T GetCofactor(int row, int col)
        {
            Matrix<T> minor = GetSubmatrix(row, col);
            T minorDeterminant;
            // The minor matrix should always be square, so TryGetDeterminant should succeed.
            if (!minor.TryGetDeterminant(out minorDeterminant))
            {
                return T.Zero; // Should theoretically not happen for valid submatrices.
            }

            T cofactor = minorDeterminant;
            if ((row + col) % 2 == 1) // If sum of indices (i+j) is odd, negate the cofactor.
            {
                cofactor = T.Zero - cofactor;
            }
            return cofactor;
        }

        /// <summary>
        /// Computes the cofactor matrix for the current matrix.
        /// Each element (i,j) in the cofactor matrix is the cofactor of the element (i,j) in the original matrix.
        /// </summary>
        /// <returns>A new Matrix<T> representing the cofactor matrix.</returns>
        private Matrix<T> GetCofactorMatrix()
        {
            Matrix<T> cofactorMatrix = new Matrix<T>(_rows, _columns);
            Parallel.For(0, _rows, i =>
            {
                for (int j = 0; j < _columns; j++)
                {
                    T cofactor = GetCofactor(i, j);
                    // TrySetElement should succeed as indices are within bounds.
                    cofactorMatrix.TrySetElement(i, j, cofactor);
                }
            });
            return cofactorMatrix;
        }

        /// <summary>
        /// Computes the adjoint (or adjugate) matrix.
        /// The adjoint matrix is the transpose of the cofactor matrix.
        /// </summary>
        /// <returns>A new Matrix<T> representing the adjoint matrix.</returns>
        private Matrix<T> GetAdjointMatrix()
        {
            Matrix<T> cofactorMatrix = GetCofactorMatrix();
            // Reuse the static Transpose method located in MatrixArithmetic.cs.
            return Matrix<T>.Transpose(cofactorMatrix);
        }

        /// <summary>
        /// Attempts to calculate the inverse of the matrix.
        /// Only applicable to square matrices with a non-zero determinant.
        /// </summary>
        /// <param name="inverse">When this method returns, contains the inverse of the matrix if calculation succeeds; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the inverse was successfully calculated; otherwise, <see langword="false"/> (e.g., matrix is not square or determinant is zero).</returns>
        public bool TryGetInverse([NotNullWhen(true)] out Matrix<T>? inverse)
        {
            inverse = null;

            if (!IsSquare)
            {
                return false;
            }

            // Inverse of a 0x0 matrix is considered itself (identity equivalent).
            if (_rows == 0)
            {
                inverse = new Matrix<T>(0, 0);
                return true;
            }

            T determinant;
            // TryGetDeterminant should succeed for a square matrix.
            if (!TryGetDeterminant(out determinant))
            {
                return false;
            }

            // Check if determinant is effectively zero.
            // For integer types, it's a direct zero check. For floating-point types (e.g., float, double),
            // a direct equality check `T.Zero.Equals(determinant)` is not robust due to precision issues.
            // However, without methods to check for floating-point types or access to 'epsilon' values
            // provided by System.Numerics.INumber<T> or allowed custom implementations,
            // direct equality is the only compliant way to check for zero.
            if (T.Zero.Equals(determinant))
            {
                return false; // Inverse does not exist if determinant is zero.
            }

            // Special case for 1x1 matrix: [a]^-1 = [1/a]
            if (_rows == 1)
            {
                T value;
                if (TryGetElement(0, 0, out value))
                {
                    if (T.Zero.Equals(value)) // Check for division by zero for the 1x1 case.
                    {
                        return false;
                    }
                    inverse = new Matrix<T>(1, 1);
                    inverse.TrySetElement(0, 0, T.One / value);
                    return true;
                }
                return false;
            }

            // Calculate adjoint matrix.
            Matrix<T> adjoint = GetAdjointMatrix();

            // Inverse = (1 / determinant) * adjoint.
            T invDeterminant = T.One / determinant;
            inverse = Matrix<T>.ScalarMultiply(adjoint, invDeterminant);

            return true;
        }
    }
}