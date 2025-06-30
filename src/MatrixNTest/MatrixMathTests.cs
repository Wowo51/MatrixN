//Copyright Warren Harding 2025.
using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixN;

namespace MatrixN.Tests
{
    [TestClass]
    public sealed class MatrixMathTests
    {
        [TestMethod]
        public void TryAdd_ValidMatrices_ReturnsCorrectSum()
        {
            Matrix<int> matrixA = new Matrix<int>(2, 2);
            matrixA.TrySetElement(0, 0, 1);
            matrixA.TrySetElement(0, 1, 2);
            matrixA.TrySetElement(1, 0, 3);
            matrixA.TrySetElement(1, 1, 4);
            Matrix<int> matrixB = new Matrix<int>(2, 2);
            matrixB.TrySetElement(0, 0, 5);
            matrixB.TrySetElement(0, 1, 6);
            matrixB.TrySetElement(1, 0, 7);
            matrixB.TrySetElement(1, 1, 8);
            bool success = Matrix<int>.TryAdd(matrixA, matrixB, out Matrix<int>? result);
            Assert.IsTrue(success);
            Assert.IsNotNull(result);
            Matrix<int> expected = new Matrix<int>(2, 2);
            expected.TrySetElement(0, 0, 6);
            expected.TrySetElement(0, 1, 8);
            expected.TrySetElement(1, 0, 10);
            expected.TrySetElement(1, 1, 12);
            TestHelpers.AssertMatrixEquals(expected, result!, "Addition sum mismatch.");
        }

        [TestMethod]
        public void TryAdd_MismatchDimensions_ReturnsFalse()
        {
            Matrix<int> matrixA = new Matrix<int>(2, 2);
            Matrix<int> matrixB = new Matrix<int>(2, 3); // Mismatched columns
            bool success = Matrix<int>.TryAdd(matrixA, matrixB, out Matrix<int>? result);
            Assert.IsFalse(success);
            Assert.IsNull(result);
            Matrix<int> matrixC = new Matrix<int>(3, 2); // Mismatched rows
            bool success2 = Matrix<int>.TryAdd(matrixA, matrixC, out Matrix<int>? result2);
            Assert.IsFalse(success2);
            Assert.IsNull(result2);
        }

        [TestMethod]
        public void TrySubtract_ValidMatrices_ReturnsCorrectDifference()
        {
            Matrix<double> matrixA = new Matrix<double>(2, 2);
            matrixA.TrySetElement(0, 0, 10.0);
            matrixA.TrySetElement(0, 1, 9.0);
            matrixA.TrySetElement(1, 0, 8.0);
            matrixA.TrySetElement(1, 1, 7.0);
            Matrix<double> matrixB = new Matrix<double>(2, 2);
            matrixB.TrySetElement(0, 0, 1.0);
            matrixB.TrySetElement(0, 1, 2.0);
            matrixB.TrySetElement(1, 0, 3.0);
            matrixB.TrySetElement(1, 1, 4.0);
            bool success = Matrix<double>.TrySubtract(matrixA, matrixB, out Matrix<double>? result);
            Assert.IsTrue(success);
            Assert.IsNotNull(result);
            Matrix<double> expected = new Matrix<double>(2, 2);
            expected.TrySetElement(0, 0, 9.0);
            expected.TrySetElement(0, 1, 7.0);
            expected.TrySetElement(1, 0, 5.0);
            expected.TrySetElement(1, 1, 3.0);
            TestHelpers.AssertMatrixEquals(expected, result!, "Subtraction difference mismatch.");
        }

        [TestMethod]
        public void TrySubtract_MismatchDimensions_ReturnsFalse()
        {
            Matrix<decimal> matrixA = new Matrix<decimal>(2, 2);
            Matrix<decimal> matrixB = new Matrix<decimal>(2, 1);
            bool success = Matrix<decimal>.TrySubtract(matrixA, matrixB, out Matrix<decimal>? result);
            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ScalarMultiply_ValidMatrixAndScalar_ReturnsCorrectResult()
        {
            Matrix<int> matrix = new Matrix<int>(2, 2);
            matrix.TrySetElement(0, 0, 1);
            matrix.TrySetElement(0, 1, 2);
            matrix.TrySetElement(1, 0, 3);
            matrix.TrySetElement(1, 1, 4);
            int scalar = 5;
            Matrix<int> result = Matrix<int>.ScalarMultiply(matrix, scalar);
            Matrix<int> expected = new Matrix<int>(2, 2);
            expected.TrySetElement(0, 0, 5);
            expected.TrySetElement(0, 1, 10);
            expected.TrySetElement(1, 0, 15);
            expected.TrySetElement(1, 1, 20);
            TestHelpers.AssertMatrixEquals(expected, result, "Scalar multiplication result mismatch.");
        }

        [TestMethod]
        public void TryMultiply_ValidMatrices_ReturnsCorrectProduct()
        {
            // Test 1: Standard multiplication
            Matrix<int> matrixA = new Matrix<int>(2, 3);
            matrixA.TrySetElement(0, 0, 1);
            matrixA.TrySetElement(0, 1, 2);
            matrixA.TrySetElement(0, 2, 3);
            matrixA.TrySetElement(1, 0, 4);
            matrixA.TrySetElement(1, 1, 5);
            matrixA.TrySetElement(1, 2, 6);
            Matrix<int> matrixB = new Matrix<int>(3, 2);
            matrixB.TrySetElement(0, 0, 7);
            matrixB.TrySetElement(0, 1, 8);
            matrixB.TrySetElement(1, 0, 9);
            matrixB.TrySetElement(1, 1, 1);
            matrixB.TrySetElement(2, 0, 2);
            matrixB.TrySetElement(2, 1, 3);
            bool success = Matrix<int>.TryMultiply(matrixA, matrixB, out Matrix<int>? result);
            Assert.IsTrue(success);
            Assert.IsNotNull(result);
            Matrix<int> expected = new Matrix<int>(2, 2);
            expected.TrySetElement(0, 0, 1 * 7 + 2 * 9 + 3 * 2); // 7 + 18 + 6 = 31
            expected.TrySetElement(0, 1, 1 * 8 + 2 * 1 + 3 * 3); // 8 + 2 + 9 = 19
            expected.TrySetElement(1, 0, 4 * 7 + 5 * 9 + 6 * 2); // 28 + 45 + 12 = 85
            expected.TrySetElement(1, 1, 4 * 8 + 5 * 1 + 6 * 3); // 32 + 5 + 18 = 55
            TestHelpers.AssertMatrixEquals(expected, result!, "Matrix multiplication product mismatch (Test 1).");
            // Test 2: Multiplication by Identity
            int size = 3;
            Matrix<float> identity = MatrixFactory.CreateIdentity<float>(size);
            Matrix<float> original = new Matrix<float>(size, size);
            original.TrySetElement(0, 0, 1.1f);
            original.TrySetElement(0, 1, 2.2f);
            original.TrySetElement(0, 2, 3.3f);
            original.TrySetElement(1, 0, 4.4f);
            original.TrySetElement(1, 1, 5.5f);
            original.TrySetElement(1, 2, 6.6f);
            original.TrySetElement(2, 0, 7.7f);
            original.TrySetElement(2, 1, 8.8f);
            original.TrySetElement(2, 2, 9.9f);
            bool identitySuccess = Matrix<float>.TryMultiply(original, identity, out Matrix<float>? resultIdentity);
            Assert.IsTrue(identitySuccess);
            Assert.IsNotNull(resultIdentity);
            TestHelpers.AssertMatrixEquals(original, resultIdentity!, "Multiplication by identity should return original matrix.");
            // Test 3: Multiplication by Zero matrix
            Matrix<int> zeroMatrix = MatrixFactory.CreateZero<int>(3, 3);
            Matrix<int> anyMatrix = new Matrix<int>(3, 3);
            anyMatrix.TrySetElement(0, 0, 100);
            bool zeroMultiplySuccess = Matrix<int>.TryMultiply(anyMatrix, zeroMatrix, out Matrix<int>? resultZero);
            Assert.IsTrue(zeroMultiplySuccess);
            Assert.IsNotNull(resultZero);
            TestHelpers.AssertMatrixEquals(zeroMatrix, resultZero!, "Multiplication by zero matrix should return zero matrix.");
        }

        [TestMethod]
        public void TryMultiply_MismatchDimensions_ReturnsFalse()
        {
            Matrix<long> matrixA = new Matrix<long>(2, 3);
            Matrix<long> matrixB = new Matrix<long>(2, 2); // Mismatch: A.Columns (3) != B.Rows (2)
            bool success = Matrix<long>.TryMultiply(matrixA, matrixB, out Matrix<long>? result);
            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Transpose_ValidMatrix_ReturnsCorrectTranspose()
        {
            Matrix<double> matrix = new Matrix<double>(2, 3);
            matrix.TrySetElement(0, 0, 1.1);
            matrix.TrySetElement(0, 1, 2.2);
            matrix.TrySetElement(0, 2, 3.3);
            matrix.TrySetElement(1, 0, 4.4);
            matrix.TrySetElement(1, 1, 5.5);
            matrix.TrySetElement(1, 2, 6.6);
            Matrix<double> transposedMatrix = Matrix<double>.Transpose(matrix);
            Assert.AreEqual(matrix.Columns, transposedMatrix.Rows);
            Assert.AreEqual(matrix.Rows, transposedMatrix.Columns);
            Matrix<double> expected = new Matrix<double>(3, 2);
            expected.TrySetElement(0, 0, 1.1);
            expected.TrySetElement(0, 1, 4.4);
            expected.TrySetElement(1, 0, 2.2);
            expected.TrySetElement(1, 1, 5.5);
            expected.TrySetElement(2, 0, 3.3);
            expected.TrySetElement(2, 1, 6.6);
            TestHelpers.AssertMatrixEquals(expected, transposedMatrix, "Transpose result mismatch (non-square).");
            // Test square matrix transpose
            Matrix<int> squareMatrix = new Matrix<int>(2, 2);
            squareMatrix.TrySetElement(0, 0, 1);
            squareMatrix.TrySetElement(0, 1, 2);
            squareMatrix.TrySetElement(1, 0, 3);
            squareMatrix.TrySetElement(1, 1, 4);
            Matrix<int> transposedSquare = Matrix<int>.Transpose(squareMatrix);
            Matrix<int> expectedSquare = new Matrix<int>(2, 2);
            expectedSquare.TrySetElement(0, 0, 1);
            expectedSquare.TrySetElement(0, 1, 3);
            expectedSquare.TrySetElement(1, 0, 2);
            expectedSquare.TrySetElement(1, 1, 4);
            TestHelpers.AssertMatrixEquals(expectedSquare, transposedSquare, "Transpose result mismatch (square).");
        }

        [TestMethod]
        public void Transpose_OneByOneMatrix_ReturnsSelf()
        {
            Matrix<int> matrix = new Matrix<int>(1, 1);
            matrix.TrySetElement(0, 0, 99);
            Matrix<int> transposedMatrix = Matrix<int>.Transpose(matrix);
            Matrix<int> expected = new Matrix<int>(1, 1);
            expected.TrySetElement(0, 0, 99);
            TestHelpers.AssertMatrixEquals(expected, transposedMatrix, "1x1 Transpose result mismatch.");
        }

        [TestMethod]
        public void TryMultiply_MatrixByColumnVector_ReturnsCorrectResult()
        {
            // Matrix A (2x3) * Column Vector V (3x1) = Result (2x1)
            Matrix<int> matrixA = TestGenerators.CreateMatrixFrom2DArray(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });
            Matrix<int> columnVector = TestGenerators.CreateMatrixFrom2DArray(new int[,] { { 7 }, { 8 }, { 9 } });

            bool success = Matrix<int>.TryMultiply(matrixA, columnVector, out Matrix<int>? result);
            Assert.IsTrue(success);
            Assert.IsNotNull(result);

            Matrix<int> expected = TestGenerators.CreateMatrixFrom2DArray(new int[,]
            {
                { 1 * 7 + 2 * 8 + 3 * 9 }, // 7 + 16 + 27 = 50
                { 4 * 7 + 5 * 8 + 6 * 9 }  // 28 + 40 + 54 = 122
            });
            TestHelpers.AssertMatrixEquals(expected, result!, "Matrix by column vector multiplication failed.");
        }

        [TestMethod]
        public void TryMultiply_RowVectorByMatrix_ReturnsCorrectResult()
        {
            // Row Vector R (1x3) * Matrix A (3x2) = Result (1x2)
            Matrix<int> rowVector = TestGenerators.CreateMatrixFrom2DArray(new int[,] { { 10, 11, 12 } });
            Matrix<int> matrixA = TestGenerators.CreateMatrixFrom2DArray(new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } });

            bool success = Matrix<int>.TryMultiply(rowVector, matrixA, out Matrix<int>? result);
            Assert.IsTrue(success);
            Assert.IsNotNull(result);

            Matrix<int> expected = TestGenerators.CreateMatrixFrom2DArray(new int[,]
            {
                { 10 * 1 + 11 * 3 + 12 * 5, 10 * 2 + 11 * 4 + 12 * 6 } // (10 + 33 + 60), (20 + 44 + 72) = (103), (136)
            });
            TestHelpers.AssertMatrixEquals(expected, result!, "Row vector by matrix multiplication failed.");
        }
    }
}