//Copyright Warren Harding 2025.
using System;
using System.Numerics;
using MatrixN;

namespace MatrixN.Tests
{
    public static class TestGenerators
    {
        internal static readonly Random s_random = new Random();

        public static Matrix<int> GenerateRandomIntMatrix(int rows, int columns, int minValue, int maxValue)
        {
            Matrix<int> matrix = new Matrix<int>(rows, columns);
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    // Random.Next(minValue, maxValue) generates a random number within a specified range.
                    // The upper bound is exclusive, so use maxValue + 1.
                    matrix.TrySetElement(r, c, s_random.Next(minValue, maxValue + 1));
                }
            }
            return matrix;
        }

        public static Matrix<double> GenerateRandomDoubleMatrix(int rows, int columns, double minValue, double maxValue)
        {
            Matrix<double> matrix = new Matrix<double>(rows, columns);
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    // Generate random double between 0.0 and 1.0, then scale to desired range
                    double randomValue = minValue + (s_random.NextDouble() * (maxValue - minValue));
                    matrix.TrySetElement(r, c, randomValue);
                }
            }
            return matrix;
        }

        /// <summary>
        /// Creates a Matrix from a 2D array, useful for defining specific test matrices.
        /// </summary>
        public static Matrix<T> CreateMatrixFrom2DArray<T>(T[,] data) where T : INumber<T>
        {
            int rows = data.GetLength(0);
            int columns = data.GetLength(1);
            Matrix<T> matrix = new Matrix<T>(rows, columns);
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    matrix.TrySetElement(r, c, data[r, c]);
                }
            }
            return matrix;
        }
    }
}