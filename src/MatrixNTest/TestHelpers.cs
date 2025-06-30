//Copyright Warren Harding 2025.
using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixN;

namespace MatrixN.Tests
{
    public static class TestHelpers
    {
        public static void AssertMatrixEquals<T>(Matrix<T> expected, Matrix<T> actual, string message = "", double doubleTolerance = -1.0, float floatTolerance = -1.0f, decimal decimalTolerance = -1.0M)
            where T : INumber<T>
        {
            Assert.IsNotNull(expected, $"Expected matrix cannot be null. {message}");
            Assert.IsNotNull(actual, $"Actual matrix cannot be null. {message}");
            Assert.AreEqual(expected.Rows, actual.Rows, $"Matrix row count mismatch. {message}");
            Assert.AreEqual(expected.Columns, actual.Columns, $"Matrix column count mismatch. {message}");
            for (int i = 0; i < expected.Rows; i++)
            {
                for (int j = 0; j < expected.Columns; j++)
                {
                    bool expectedGetResult = expected.TryGetElement(i, j, out T expectedValue);
                    bool actualGetResult = actual.TryGetElement(i, j, out T actualValue);
                    Assert.IsTrue(expectedGetResult, $"Failed to get expected element at ({i},{j}). {message}");
                    Assert.IsTrue(actualGetResult, $"Failed to get actual element at ({i},{j}). {message}");
                    if (typeof(T) == typeof(double))
                    {
                        double effectiveTolerance = (doubleTolerance >= 0) ? doubleTolerance : 1e-9; // Default if not provided
                        Assert.AreEqual(Convert.ToDouble(expectedValue), Convert.ToDouble(actualValue), effectiveTolerance, $"Element mismatch at ({i},{j}). {message}");
                    }
                    else if (typeof(T) == typeof(float))
                    {
                        float effectiveTolerance = (floatTolerance >= 0) ? floatTolerance : 1e-6f; // Default if not provided
                        Assert.AreEqual(Convert.ToSingle(expectedValue), Convert.ToSingle(actualValue), effectiveTolerance, $"Element mismatch at ({i},{j}). {message}");
                    }
                    else if (typeof(T) == typeof(decimal))
                    {
                        decimal effectiveTolerance = (decimalTolerance >= 0) ? decimalTolerance : 1e-15M; // Default if not provided
                        Assert.AreEqual(Convert.ToDecimal(expectedValue), Convert.ToDecimal(actualValue), effectiveTolerance, $"Element mismatch at ({i},{j}). {message}");
                    }
                    else
                    {
                        Assert.AreEqual(expectedValue, actualValue, $"Element mismatch at ({i},{j}). {message}");
                    }
                }
            }
        }
    }
}