//Copyright Warren Harding 2025.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixN;
using System;
using System.Numerics;

namespace MatrixN.Tests
{
    [TestClass]
    public sealed class MatrixPropertyTestsEx
    {
        private const double DoubleTolerance = 1e-9;
        private const decimal DecimalTolerance = 1e-15m;

        [TestMethod]
        public void AddCommutativeProperty_Int_HoldsTrue()
        {
            // A + B = B + A
            Matrix<int> matrixA = TestGenerators.GenerateRandomIntMatrix(5, 5, -100, 100);
            Matrix<int> matrixB = TestGenerators.GenerateRandomIntMatrix(5, 5, -100, 100);

            bool successAB = Matrix<int>.TryAdd(matrixA, matrixB, out Matrix<int>? sumAB);
            Assert.IsTrue(successAB);
            Assert.IsNotNull(sumAB);

            bool successBA = Matrix<int>.TryAdd(matrixB, matrixA, out Matrix<int>? sumBA);
            Assert.IsTrue(successBA);
            Assert.IsNotNull(sumBA);

            TestHelpers.AssertMatrixEquals(sumAB!, sumBA!, "Matrix addition should be commutative.");
        }

        [TestMethod]
        public void ScalarDistributiveProperty_Double_HoldsTrue()
        {
            // c * (A + B) = c * A + c * B
            Matrix<double> matrixA = TestGenerators.GenerateRandomDoubleMatrix(3, 4, -10.0, 10.0);
            Matrix<double> matrixB = TestGenerators.GenerateRandomDoubleMatrix(3, 4, -10.0, 10.0);
            double scalarC = TestGenerators.s_random.NextDouble() * 20.0 - 10.0; // Random scalar between -10 and 10

            // Calculate LHS: c * (A + B)
            bool sum_AB_Success = Matrix<double>.TryAdd(matrixA, matrixB, out Matrix<double>? sumAB);
            Assert.IsTrue(sum_AB_Success);
            Assert.IsNotNull(sumAB);
            Matrix<double> lhs = Matrix<double>.ScalarMultiply(sumAB!, scalarC);
            Assert.IsNotNull(lhs);

            // Calculate RHS: c * A + c * B
            Matrix<double> cA = Matrix<double>.ScalarMultiply(matrixA, scalarC);
            Matrix<double> cB = Matrix<double>.ScalarMultiply(matrixB, scalarC);
            bool sum_cA_cB_Success = Matrix<double>.TryAdd(cA, cB, out Matrix<double>? rhs);
            Assert.IsTrue(sum_cA_cB_Success);
            Assert.IsNotNull(rhs);

            Assert.AreEqual(lhs.Rows, rhs.Rows);
            Assert.AreEqual(lhs.Columns, rhs.Columns);
            for (int r = 0; r < lhs.Rows; r++)
            {
                for (int c = 0; c < lhs.Columns; c++)
                {
                    lhs.TryGetElement(r, c, out double lhsValue);
                    rhs.TryGetElement(r, c, out double rhsValue);
                    Assert.AreEqual(lhsValue, rhsValue, DoubleTolerance, $"Element mismatch at ({r},{c}) for scalar distributive property.");
                }
            }
        }

        [TestMethod]
        public void ScalarAssociativeProperty_Decimal_HoldsTrue()
        {
            // (c * d) * A = c * (d * A)
            Matrix<decimal> matrixA = TestGenerators.CreateMatrixFrom2DArray(new decimal[,] { { 1.1m, 2.2m, 3.3m }, { 4.4m, 5.5m, 6.6m } });
            decimal scalarC = 2.5m;
            decimal scalarD = 3.0m;

            // Calculate LHS: (c * d) * A
            decimal productCD = scalarC * scalarD;
            Matrix<decimal> lhs = Matrix<decimal>.ScalarMultiply(matrixA, productCD);
            Assert.IsNotNull(lhs);

            // Calculate RHS: c * (d * A)
            Matrix<decimal> dA = Matrix<decimal>.ScalarMultiply(matrixA, scalarD);
            Matrix<decimal> rhs = Matrix<decimal>.ScalarMultiply(dA, scalarC);
            Assert.IsNotNull(rhs);

            Assert.AreEqual(lhs.Rows, rhs.Rows);
            Assert.AreEqual(lhs.Columns, rhs.Columns);
            for (int r = 0; r < lhs.Rows; r++)
            {
                for (int c = 0; c < lhs.Columns; c++)
                {
                    lhs.TryGetElement(r, c, out decimal lhsValue);
                    rhs.TryGetElement(r, c, out decimal rhsValue);
                    Assert.AreEqual(lhsValue, rhsValue, DecimalTolerance, $"Element mismatch at ({r},{c}) for scalar associative property.");
                }
            }
        }
    }
}