//Copyright Warren Harding 2025.
using System.Numerics;

namespace MatrixN
{
    public static class MatrixFactory
    {
        public static Matrix<T> CreateZero<T>(int rows, int columns) where T : INumber<T>
        {
            return new Matrix<T>(rows, columns);
        }

        public static Matrix<T> CreateIdentity<T>(int size) where T : INumber<T>
        {
            Matrix<T> identity = new Matrix<T>(size, size);
            for (int i = 0; i < size; i++)
            {
                // Replaced indexer access with TrySetElement to conform to the 'no throw' rule.
                // This operation is expected to succeed given valid matrix construction.
                identity.TrySetElement(i, i, T.One);
            }
            return identity;
        }
    }
}
