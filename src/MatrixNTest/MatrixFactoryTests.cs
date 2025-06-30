//Copyright Warren Harding 2025.
using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixN;

namespace MatrixN.Tests
{
    [TestClass]
    public sealed class MatrixFactoryTests
    {
        [TestMethod]
        public void CreateZero_ValidDimensions_ReturnsMatrixOfZeros()
        {
            int rows = 3;
            int columns = 5;
            Matrix<int> zeroMatrix = MatrixFactory.CreateZero<int>(rows, columns);

            Assert.AreEqual(rows, zeroMatrix.Rows);
            Assert.AreEqual(columns, zeroMatrix.Columns);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    zeroMatrix.TryGetElement(r, c, out int value);
                    Assert.AreEqual(0, value, $"Element at ({r},{c}) should be zero.");
                }
            }
        }

        [TestMethod]
        public void CreateIdentity_ValidSize_ReturnsIdentityMatrix()
        {
            int size = 4;
            Matrix<double> identityMatrix = MatrixFactory.CreateIdentity<double>(size);

            Assert.AreEqual(size, identityMatrix.Rows);
            Assert.AreEqual(size, identityMatrix.Columns);

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    identityMatrix.TryGetElement(r, c, out double value);
                    if (r == c)
                    {
                        Assert.AreEqual(1.0, value, $"Diagonal element at ({r},{c}) should be one.");
                    }
                    else
                    {
                        Assert.AreEqual(0.0, value, $"Non-diagonal element at ({r},{c}) should be zero.");
                    }
                }
            }
        }

        [TestMethod]
        public void CreateIdentity_SizeOne_ReturnsOneByOneIdentityMatrix()
        {
            int size = 1;
            Matrix<long> identityMatrix = MatrixFactory.CreateIdentity<long>(size);

            Assert.AreEqual(size, identityMatrix.Rows);
            Assert.AreEqual(size, identityMatrix.Columns);

            identityMatrix.TryGetElement(0, 0, out long value);
            Assert.AreEqual(1L, value);
        }

        [TestMethod]
        public void CreateZero_ZeroDimensions_ReturnsEmptyMatrix()
        {
            Matrix<int> matrix = MatrixFactory.CreateZero<int>(0, 0);
            Assert.AreEqual(0, matrix.Rows);
            Assert.AreEqual(0, matrix.Columns);
        }

        [TestMethod]
        public void CreateIdentity_ZeroSize_ReturnsEmptyMatrix()
        {
            Matrix<int> matrix = MatrixFactory.CreateIdentity<int>(0);
            Assert.AreEqual(0, matrix.Rows);
            Assert.AreEqual(0, matrix.Columns);
        }
    }
}