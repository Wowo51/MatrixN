//Copyright Warren Harding 2025.
using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixN;

namespace MatrixN.Tests
{
    [TestClass]
    public sealed class MatrixDeterminantTests
    {
        private const double DoubleTolerance = 1e-9; // Tolerance for floating-point comparisons

        [TestMethod]
        public void TryGetDeterminant_OneByOneMatrix_ReturnsElement()
        {
            Matrix<decimal> matrix = TestGenerators.CreateMatrixFrom2DArray(new decimal[,] { { 5m } });
            bool success = matrix.TryGetDeterminant(out decimal determinant);
            Assert.IsTrue(success);
            Assert.AreEqual(5m, determinant);
        }

        [TestMethod]
        public void TryGetDeterminant_TwoByTwoMatrix_ReturnsCorrectValue()
        {
            Matrix<int> matrix = TestGenerators.CreateMatrixFrom2DArray(new int[,] { { 2, 1 }, { 3, 4 } });
            bool success = matrix.TryGetDeterminant(out int determinant);
            Assert.IsTrue(success);
            Assert.AreEqual(5, determinant); // (2*4) - (1*3) = 8 - 3 = 5
        }

        [TestMethod]
        public void TryGetDeterminant_ThreeByThreeMatrix_ReturnsCorrectValue_Double()
        {
            Matrix<double> matrix = TestGenerators.CreateMatrixFrom2DArray(new double[,] { { 1.0, 2.0, 3.0 }, { 0.0, 1.0, 4.0 }, { 5.0, 6.0, 0.0 } });
            bool success = matrix.TryGetDeterminant(out double determinant);
            Assert.IsTrue(success);
            // 1*(0-24) - 2*(0-20) + 3*(0-5) = -24 + 40 - 15 = 1
            Assert.AreEqual(1.0, determinant, DoubleTolerance);
        }

        [TestMethod]
        public void TryGetDeterminant_ZeroDeterminantMatrix_ReturnsZero()
        {
            Matrix<int> matrix = TestGenerators.CreateMatrixFrom2DArray(new int[,] { { 1, 2 }, { 2, 4 } }); // Rows are linearly dependent
            bool success = matrix.TryGetDeterminant(out int determinant);
            Assert.IsTrue(success);
            Assert.AreEqual(0, determinant);
        }

        [TestMethod]
        public void TryGetDeterminant_IdentityMatrix_ReturnsOne()
        {
            Matrix<double> identity = MatrixFactory.CreateIdentity<double>(4);
            bool success = identity.TryGetDeterminant(out double determinant);
            Assert.IsTrue(success);
            Assert.AreEqual(1.0, determinant, DoubleTolerance);
        }

        [TestMethod]
        public void TryGetDeterminant_DiagonalMatrix_ReturnsProductOfDiagonal()
        {
            Matrix<int> matrix = TestGenerators.CreateMatrixFrom2DArray(new int[,] { { 2, 0, 0 }, { 0, 3, 0 }, { 0, 0, 4 } });
            bool success = matrix.TryGetDeterminant(out int determinant);
            Assert.IsTrue(success);
            Assert.AreEqual(24, determinant); // 2 * 3 * 4 = 24
        }
        
        [TestMethod]
        public void TryGetDeterminant_UpperTriangularMatrix_ReturnsProductOfDiagonal()
        {
            Matrix<double> matrix = TestGenerators.CreateMatrixFrom2DArray(new double[,] { { 1.0, 2.0, 3.0 }, { 0.0, 4.0, 5.0 }, { 0.0, 0.0, 6.0 } });
            bool success = matrix.TryGetDeterminant(out double determinant);
            Assert.IsTrue(success);
            Assert.AreEqual(24.0, determinant, DoubleTolerance); // 1 * 4 * 6 = 24
        }

        [TestMethod]
        public void TryGetDeterminant_LowerTriangularMatrix_ReturnsProductOfDiagonal()
        {
            Matrix<double> matrix = TestGenerators.CreateMatrixFrom2DArray(new double[,] { { 1.0, 0.0, 0.0 }, { 2.0, 3.0, 0.0 }, { 4.0, 5.0, 6.0 } });
            bool success = matrix.TryGetDeterminant(out double determinant);
            Assert.IsTrue(success);
            Assert.AreEqual(18.0, determinant, DoubleTolerance); // 1 * 3 * 6 = 18
        }

        [TestMethod]
        public void TryGetDeterminant_NonSquareMatrix_ReturnsFalse()
        {
            Matrix<int> matrix = new Matrix<int>(3, 2);
            bool success = matrix.TryGetDeterminant(out int determinant);
            Assert.IsFalse(success);
            Assert.AreEqual(0, determinant);
        }

        [TestMethod]
        public void TryGetDeterminant_ZeroByZeroMatrix_ReturnsOne()
        {
            Matrix<int> matrix = new Matrix<int>(0, 0);
            bool success = matrix.TryGetDeterminant(out int determinant);
            Assert.IsTrue(success);
            Assert.AreEqual(1, determinant); // Convention for 0x0 matrix determinant
        }
    }
}