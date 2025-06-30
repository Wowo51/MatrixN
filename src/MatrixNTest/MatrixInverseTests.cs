//Copyright Warren Harding 2025.
using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixN;

namespace MatrixN.Tests
{
    [TestClass]
    public sealed class MatrixInverseTests
    {
        private const double DoubleTolerance = 1e-9; // Tolerance for floating-point comparisons
        [TestMethod]
        public void TryGetInverse_TwoByTwoInvertibleMatrix_ReturnsCorrectInverse()
        {
            Matrix<double> matrix = TestGenerators.CreateMatrixFrom2DArray(new double[, ] { { 4.0, 7.0 }, { 2.0, 6.0 } });
            // Expected inverse: (1/10) * [[6, -7], [-2, 4]] = [[0.6, -0.7], [-0.2, 0.4]]
            Matrix<double> expectedInverse = TestGenerators.CreateMatrixFrom2DArray(new double[, ] { { 0.6, -0.7 }, { -0.2, 0.4 } });
            bool success = matrix.TryGetInverse(out Matrix<double>? actualInverse);
            Assert.IsTrue(success);
            Assert.IsNotNull(actualInverse);
            TestHelpers.AssertMatrixEquals(expectedInverse, actualInverse!, "2x2 inverse mismatch.", DoubleTolerance);
        }

        [TestMethod]
        public void TryGetInverse_IdentityMatrix_ReturnsSelf()
        {
            int size = 5;
            Matrix<double> identity = MatrixFactory.CreateIdentity<double>(size);
            bool success = identity.TryGetInverse(out Matrix<double>? inverse);
            Assert.IsTrue(success);
            Assert.IsNotNull(inverse);
            TestHelpers.AssertMatrixEquals(identity, inverse!, "Identity matrix inverse mismatch.");
        }

        [TestMethod]
        public void TryGetInverse_NonInvertibleMatrix_ReturnsFalse()
        {
            Matrix<long> matrix = TestGenerators.CreateMatrixFrom2DArray(new long[, ] { { 1L, 2L }, { 2L, 4L } }); // Determinant is 0
            bool success = matrix.TryGetInverse(out Matrix<long>? inverse);
            Assert.IsFalse(success);
            Assert.IsNull(inverse);
        }

        [TestMethod]
        public void TryGetInverse_NonSquareMatrix_ReturnsFalse()
        {
            Matrix<int> matrix = new Matrix<int>(3, 2);
            bool success = matrix.TryGetInverse(out Matrix<int>? inverse);
            Assert.IsFalse(success);
            Assert.IsNull(inverse);
        }

        [TestMethod]
        public void TryGetInverse_ZeroByZeroMatrix_ReturnsZeroByZeroInverse()
        {
            Matrix<int> matrix = new Matrix<int>(0, 0);
            bool success = matrix.TryGetInverse(out Matrix<int>? inverse);
            Assert.IsTrue(success);
            Assert.IsNotNull(inverse);
            Assert.AreEqual(0, inverse.Rows);
            Assert.AreEqual(0, inverse.Columns);
        }

        [TestMethod]
        public void TryGetInverse_InverseProperty_AInvA_IsIdentity_StressTest()
        {
            int size = 4; // Using a moderately sized matrix for stress testing
            Matrix<double> originalMatrix;
            double determinant;
            const int maxAttempts = 100;
            int attempts = 0;
            // Generate an invertible matrix by repeatedly trying random matrices
            do
            {
                originalMatrix = TestGenerators.GenerateRandomDoubleMatrix(size, size, -20.0, 20.0);
                originalMatrix.TryGetDeterminant(out determinant);
                attempts++;
            }
            while (Math.Abs(determinant) < DoubleTolerance && attempts < maxAttempts); // Determinant must not be too close to zero
            Assert.AreNotEqual(0.0, determinant, DoubleTolerance, "Failed to generate a sufficiently invertible random matrix.");
            bool inverseSuccess = originalMatrix.TryGetInverse(out Matrix<double>? inverseMatrix);
            Assert.IsTrue(inverseSuccess, "Inverse calculation failed for a seemingly invertible matrix.");
            Assert.IsNotNull(inverseMatrix);
            // Check A * A^-1 = I
            bool mul1Success = Matrix<double>.TryMultiply(originalMatrix, inverseMatrix!, out Matrix<double>? product1);
            Assert.IsTrue(mul1Success);
            Assert.IsNotNull(product1);
            Matrix<double> expectedIdentity1 = MatrixFactory.CreateIdentity<double>(size);
            // For floating point comparisons, iterate and use tolerance
            TestHelpers.AssertMatrixEquals(expectedIdentity1, product1!, "A * A^-1 should result in identity matrix.", DoubleTolerance);

            // Check A^-1 * A = I
            bool mul2Success = Matrix<double>.TryMultiply(inverseMatrix!, originalMatrix, out Matrix<double>? product2);
            Assert.IsTrue(mul2Success);
            Assert.IsNotNull(product2);
            Matrix<double> expectedIdentity2 = MatrixFactory.CreateIdentity<double>(size);
            TestHelpers.AssertMatrixEquals(expectedIdentity2, product2!, "A^-1 * A should result in identity matrix.", DoubleTolerance);
        }
    }
}