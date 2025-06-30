//Copyright Warren Harding 2025.
using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixN;

namespace MatrixN.Tests
{
    [TestClass]
    public sealed class MatrixTests
    {
        [TestMethod]
        public void Constructor_ValidDimensions_InitializesCorrectly()
        {
            int rows = 3;
            int columns = 4;
            Matrix<int> matrix = new Matrix<int>(rows, columns);

            Assert.AreEqual(rows, matrix.Rows, "Rows should match constructor argument.");
            Assert.AreEqual(columns, matrix.Columns, "Columns should match constructor argument.");

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    bool success = matrix.TryGetElement(r, c, out int value);
                    Assert.IsTrue(success, $"Element at ({r},{c}) should be retrievable.");
                    Assert.AreEqual(0, value, $"Newly created matrix elements should be initialized to zero.");
                }
            }
        }

        [TestMethod]
        public void TryGetElement_ValidIndex_ReturnsCorrectValue()
        {
            Matrix<int> matrix = new Matrix<int>(2, 2);
            matrix.TrySetElement(0, 0, 10);
            matrix.TrySetElement(1, 1, 20);

            bool success00 = matrix.TryGetElement(0, 0, out int value00);
            Assert.IsTrue(success00);
            Assert.AreEqual(10, value00);

            bool success11 = matrix.TryGetElement(1, 1, out int value11);
            Assert.IsTrue(success11);
            Assert.AreEqual(20, value11);
        }

        [TestMethod]
        public void TryGetElement_InvalidIndex_ReturnsFalseAndZero()
        {
            Matrix<double> matrix = new Matrix<double>(2, 2);

            bool successNegativeRow = matrix.TryGetElement(-1, 0, out double valueNegRow);
            Assert.IsFalse(successNegativeRow);
            Assert.AreEqual(0.0, valueNegRow); // T.Zero for double

            bool successOutOfBoundRow = matrix.TryGetElement(2, 0, out double valueOOBRow);
            Assert.IsFalse(successOutOfBoundRow);
            Assert.AreEqual(0.0, valueOOBRow);

            bool successNegativeCol = matrix.TryGetElement(0, -1, out double valueNegCol);
            Assert.IsFalse(successNegativeCol);
            Assert.AreEqual(0.0, valueNegCol);

            bool successOutOfBoundCol = matrix.TryGetElement(0, 2, out double valueOOBCol);
            Assert.IsFalse(successOutOfBoundCol);
            Assert.AreEqual(0.0, valueOOBCol);
        }

        [TestMethod]
        public void TrySetElement_ValidIndex_SetsValueCorrectly()
        {
            Matrix<float> matrix = new Matrix<float>(2, 2);
            float setValue = 12.3f;

            bool success = matrix.TrySetElement(1, 0, setValue);
            Assert.IsTrue(success);

            matrix.TryGetElement(1, 0, out float retrievedValue);
            Assert.AreEqual(setValue, retrievedValue);
            
            // Verify other elements remain zero
            matrix.TryGetElement(0, 0, out float val00);
            Assert.AreEqual(0.0f, val00);
        }

        [TestMethod]
        public void TrySetElement_InvalidIndex_ReturnsFalse()
        {
            Matrix<decimal> matrix = new Matrix<decimal>(2, 2);
            decimal setValue = 100M;

            Assert.IsFalse(matrix.TrySetElement(-1, 0, setValue));
            Assert.IsFalse(matrix.TrySetElement(2, 0, setValue));
            Assert.IsFalse(matrix.TrySetElement(0, -1, setValue));
            Assert.IsFalse(matrix.TrySetElement(0, 2, setValue));

            // Ensure no elements were set
            matrix.TryGetElement(0, 0, out decimal val00);
            Assert.AreEqual(0M, val00);
        }

        [TestMethod]
        public void ToString_FormatsCorrectly()
        {
            Matrix<int> matrix = new Matrix<int>(2, 3);
            matrix.TrySetElement(0, 0, 1);
            matrix.TrySetElement(0, 1, 2);
            matrix.TrySetElement(0, 2, 3);
            matrix.TrySetElement(1, 0, 4);
            matrix.TrySetElement(1, 1, 5);
            matrix.TrySetElement(1, 2, 6);

            string expectedString = 
                "2x3 Matrix<Int32>:\r\n" +
                "         1          2          3\r\n" +
                "         4          5          6";
            
            string actualString = matrix.ToString();
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void ToString_LargeValues_FormatsCorrectly()
        {
            Matrix<long> matrix = new Matrix<long>(2, 2);
            matrix.TrySetElement(0, 0, 1234567890L);
            matrix.TrySetElement(0, 1, -1L);
            matrix.TrySetElement(1, 0, 123L);
            matrix.TrySetElement(1, 1, 0L);

            string expectedString = 
                "2x2 Matrix<Int64>:\r\n" +
                "1234567890         -1\r\n" +
                "       123          0";
            
            string actualString = matrix.ToString();
            Assert.AreEqual(expectedString, actualString);
        }
    }
}