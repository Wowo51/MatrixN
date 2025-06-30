//Copyright Warren Harding 2025.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixN;
using System.Numerics;

namespace MatrixN.Tests
{
    [TestClass]
    public sealed class MatrixTraceTests
    {
        private const double DoubleTolerance = 1e-9; // Tolerance for floating-point comparisons

        [TestMethod]
        public void TryGetTrace_2x2Matrix_ReturnsCorrectTrace()
        {
            Matrix<int> matrix = TestGenerators.CreateMatrixFrom2DArray(new int[,] { { 1, 2 }, { 3, 4 } });
            bool success = matrix.TryGetTrace(out int trace);
            Assert.IsTrue(success);
            Assert.AreEqual(5, trace);
        }

        [TestMethod]
        public void TryGetTrace_3x3Matrix_ReturnsCorrectTrace_Double()
        {
            Matrix<double> matrix = TestGenerators.CreateMatrixFrom2DArray(new double[,] { { 1.0, 2.0, 3.0 }, { 4.0, 5.0, 6.0 }, { 7.0, 8.0, 9.0 } });
            bool success = matrix.TryGetTrace(out double trace);
            Assert.IsTrue(success);
            Assert.AreEqual(15.0, trace, DoubleTolerance);
        }

        [TestMethod]
        public void TryGetTrace_OneByOneMatrix_ReturnsElement()
        {
            Matrix<long> matrix = TestGenerators.CreateMatrixFrom2DArray(new long[,] { { 7L } });
            bool success = matrix.TryGetTrace(out long trace);
            Assert.IsTrue(success);
            Assert.AreEqual(7L, trace);
        }

        [TestMethod]
        public void TryGetTrace_ZeroByZeroMatrix_ReturnsZero()
        {
            Matrix<int> matrix = new Matrix<int>(0, 0);
            bool success = matrix.TryGetTrace(out int trace);
            Assert.IsTrue(success);
            Assert.AreEqual(0, trace);
        }

        [TestMethod]
        public void TryGetTrace_NonSquareMatrix_ReturnsFalse()
        {
            Matrix<int> matrix = new Matrix<int>(2, 3);
            bool success = matrix.TryGetTrace(out int trace);
            Assert.IsFalse(success);
            Assert.AreEqual(0, trace); // Should return T.Zero for failure, per MatrixCoreProperties.cs
        }
    }
}