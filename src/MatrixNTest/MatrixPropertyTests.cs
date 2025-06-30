//Copyright Warren Harding 2025.
using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixN;

namespace MatrixN.Tests
{
    [TestClass]
    public sealed class MatrixPropertyTests
    {
        [TestMethod]
        public void MultiplyAssociativeProperty_Int_HoldsTrue()
        {
            // A (2x3), B (3x4), C (4x2)
            Matrix<int> matrixA = TestGenerators.GenerateRandomIntMatrix(2, 3, 1, 5);
            Matrix<int> matrixB = TestGenerators.GenerateRandomIntMatrix(3, 4, 1, 5);
            Matrix<int> matrixC = TestGenerators.GenerateRandomIntMatrix(4, 2, 1, 5);
            Matrix<int>.TryMultiply(matrixA, matrixB, out Matrix<int>? abMatrix);
            Assert.IsNotNull(abMatrix);
            Matrix<int>.TryMultiply(abMatrix!, matrixC, out Matrix<int>? lhs);
            Assert.IsNotNull(lhs);
            Matrix<int>.TryMultiply(matrixB, matrixC, out Matrix<int>? bcMatrix);
            Assert.IsNotNull(bcMatrix);
            Matrix<int>.TryMultiply(matrixA, bcMatrix!, out Matrix<int>? rhs);
            Assert.IsNotNull(rhs);
        }

        [TestMethod]
        public void MultiplyDistributiveProperty_Double_HoldsTrue()
        {
            // A (2x3), B (3x4), C (3x4)
            Matrix<double> matrixA = TestGenerators.GenerateRandomDoubleMatrix(2, 3, 0.1, 5.0);
            Matrix<double> matrixB = TestGenerators.GenerateRandomDoubleMatrix(3, 4, 0.1, 5.0);
            Matrix<double> matrixC = TestGenerators.GenerateRandomDoubleMatrix(3, 4, 0.1, 5.0);
            Matrix<double>.TryAdd(matrixB, matrixC, out Matrix<double>? bcSumMatrix);
            Assert.IsNotNull(bcSumMatrix);
            Matrix<double>.TryMultiply(matrixA, bcSumMatrix!, out Matrix<double>? lhs);
            Assert.IsNotNull(lhs);
            Matrix<double>.TryMultiply(matrixA, matrixB, out Matrix<double>? abProductMatrix);
            Assert.IsNotNull(abProductMatrix);
            Matrix<double>.TryMultiply(matrixA, matrixC, out Matrix<double>? acProductMatrix);
            Assert.IsNotNull(acProductMatrix);
            Matrix<double>.TryAdd(abProductMatrix!, acProductMatrix!, out Matrix<double>? rhs);
            Assert.IsNotNull(rhs);
            // Floating point comparisons need tolerance
            Assert.AreEqual(lhs!.Rows, rhs!.Rows); // '!' for null-forgiveness due to NotNullWhen(true)
            Assert.AreEqual(lhs.Columns, rhs.Columns); // '!' for null-forgiveness due to NotNullWhen(true)
            const double tolerance = 1e-9;
            for (int r = 0; r < lhs.Rows; r++)
            {
                for (int c = 0; c < lhs.Columns; c++)
                {
                    lhs.TryGetElement(r, c, out double lhsValue);
                    rhs.TryGetElement(r, c, out double rhsValue);
                    Assert.AreEqual(lhsValue, rhsValue, tolerance, $"Element mismatch at ({r},{c}) for distributive property.");
                }
            }
        }
    }
}