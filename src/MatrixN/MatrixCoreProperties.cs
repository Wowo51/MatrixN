//Copyright Warren Harding 2025.
using System.Numerics;

namespace MatrixN
{
    // This partial class contains core properties and simple computations.
    public sealed partial class Matrix<T> where T : INumber<T>
    {
        /// <summary>
        /// Gets a value indicating whether the matrix is square (number of rows equals number of columns).
        /// </summary>
        public bool IsSquare => _rows == _columns;

        /// <summary>
        /// Attempts to calculate the trace of the matrix (sum of the elements on the main diagonal).
        /// Only applicable to square matrices.
        /// </summary>
        /// <param name="trace">When this method returns, contains the trace of the matrix if it's square and calculation succeeds; otherwise, the mathematical zero of <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if the trace was successfully calculated; otherwise, <see langword="false"/> (e.g., matrix is not square).</returns>
        public bool TryGetTrace(out T trace)
        {
            if (!IsSquare)
            {
                trace = T.Zero;
                return false;
            }

            // A 0x0 matrix has a trace of zero. If determinant is 1, trace is 0.
            // https://math.stackexchange.com/questions/100067/what-is-the-determinant-of-a-0-times-0-matrix
            // https://en.wikipedia.org/wiki/Empty_matrix
            if (_rows == 0)
            {
                trace = T.Zero;
                return true;
            }

            T currentTrace = T.Zero;
            for (int i = 0; i < _rows; i++)
            {
                T value;
                // TryGetElement should always succeed for valid diagonal indices.
                if (TryGetElement(i, i, out value))
                {
                    currentTrace += value;
                }
                else
                {
                    // This scenario should only be reached if TryGetElement has an unforeseen issue,
                    // but we must still adhere to the 'no throw' principle.
                    trace = T.Zero;
                    return false;
                }
            }

            trace = currentTrace;
            return true;
        }
    }
}