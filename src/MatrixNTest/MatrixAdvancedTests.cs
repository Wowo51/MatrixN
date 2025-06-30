//Copyright Warren Harding 2025.
using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixN;

namespace MatrixN.Tests
{
    [TestClass]
    public sealed class MatrixAdvancedTests
    {
        [TestMethod]
        public void AddSubtractInverse_ReturnsOriginalMatrix()
        {
            Matrix<int> matrixA = TestGenerators.GenerateRandomIntMatrix(5, 5, -100, 100);
            Matrix<int> matrixB = TestGenerators.GenerateRandomIntMatrix(5, 5, -50, 50);
            Matrix<int>.TryAdd(matrixA, matrixB, out Matrix<int>? sumMatrix);
            Assert.IsNotNull(sumMatrix);
            Matrix<int>.TrySubtract(sumMatrix!, matrixB, out Matrix<int>? resultMatrix);
            Assert.IsNotNull(resultMatrix);
            TestHelpers.AssertMatrixEquals(matrixA, resultMatrix!, "Inverse add/subtract failed: result not equal to original matrixA.");
        }

        [TestMethod]
        public void TransposeInverse_ReturnsOriginalMatrix()
        {
            Matrix<double> originalMatrix = TestGenerators.GenerateRandomDoubleMatrix(4, 6, -10.0, 10.0);
            Matrix<double> transposedOnce = Matrix<double>.Transpose(originalMatrix);
            Matrix<double> transposedTwice = Matrix<double>.Transpose(transposedOnce);
            TestHelpers.AssertMatrixEquals(originalMatrix, transposedTwice, "Double transpose does not return original matrix.");
        }

        [TestMethod]
        public void LargeValueAddition_Long_HandlesCorrectly()
        {
            Matrix<long> matrixA = TestGenerators.CreateMatrixFrom2DArray(new long[, ] { { long.MaxValue - 100L, 100L }, { 1L, 2L } });
            Matrix<long> matrixB = TestGenerators.CreateMatrixFrom2DArray(new long[, ] { { 50L, 0L }, { 1L, 2L } });
            Matrix<long>.TryAdd(matrixA, matrixB, out Matrix<long>? resultMatrix);
            Assert.IsNotNull(resultMatrix);
            Matrix<long> expected = TestGenerators.CreateMatrixFrom2DArray(new long[, ] { { long.MaxValue - 50L, 100L }, { 2L, 4L } });
            TestHelpers.AssertMatrixEquals(expected, resultMatrix!, "Large value addition failed.");
        }

        [TestMethod]
        public void ZeroByZeroMatrixOperations_ReturnCorrectEmptyMatrices()
        {
            Matrix<int> zeroZeroA = TestGenerators.CreateMatrixFrom2DArray(new int[0, 0]);
            Matrix<int> zeroZeroB = TestGenerators.CreateMatrixFrom2DArray(new int[0, 0]);
            bool addSuccess = Matrix<int>.TryAdd(zeroZeroA, zeroZeroB, out Matrix<int>? addResult);
            Assert.IsTrue(addSuccess);
            Assert.IsNotNull(addResult);
            Assert.AreEqual(0, addResult.Rows);
            Assert.AreEqual(0, addResult.Columns);
            bool mulSuccess = Matrix<int>.TryMultiply(zeroZeroA, zeroZeroB, out Matrix<int>? mulResult);
            Assert.IsTrue(mulSuccess);
            Assert.IsNotNull(mulResult);
            Assert.AreEqual(0, mulResult.Rows);
            Assert.AreEqual(0, mulResult.Columns);
            Matrix<int> transposeResult = Matrix<int>.Transpose(zeroZeroA);
            Assert.IsNotNull(transposeResult);
            Assert.AreEqual(0, transposeResult.Rows);
            Assert.AreEqual(0, transposeResult.Columns);
            Matrix<int> scalarMulResult = Matrix<int>.ScalarMultiply(zeroZeroA, 5);
            Assert.IsNotNull(scalarMulResult);
            Assert.AreEqual(0, scalarMulResult.Rows);
            Assert.AreEqual(0, scalarMulResult.Columns);
        }
    }
}