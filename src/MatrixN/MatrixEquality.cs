//Copyright Warren Harding 2025.
using System;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace MatrixN
{
    // This partial class contains methods for comparing Matrix<T> objects for equality.
    public sealed partial class Matrix<T> : IEquatable<Matrix<T>> where T : INumber<T>
    {
        /// <summary>
        /// Determines whether the current matrix and the specified matrix are equal.
        /// </summary>
        /// <param name="other">The other matrix to compare with.</param>
        /// <returns><see langword="true"/> if the matrices are equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals([NotNullWhen(true)] Matrix<T>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (Rows != other.Rows || Columns != other.Columns)
            {
                return false;
            }

            // Compare underlying data buffer element by element.
            for (int i = 0; i < _data.Length; i++)
            {
                if (!this._data[i].Equals(other._data[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether the current object and the specified object are equal.
        /// </summary>
        /// <param name="obj">The other object to compare with.</param>
        /// <returns><see langword="true"/> if the objects are equal and both are matrices with the same dimensions and elements; otherwise, <see langword="false"/>.</returns>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return Equals(obj as Matrix<T>);
        }

        /// <summary>
        /// Returns the hash code for the current matrix.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Rows);
            hash.Add(Columns);
            // Iterating over the data to include content in the hash code.
            // This ensures matrices with the same content have the same hash code.
            for (int i = 0; i < _data.Length; i++)
            {
                hash.Add(_data[i]);
            }
            return hash.ToHashCode();
        }

        /// <summary>
        /// Compares two matrices for equality.
        /// </summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns><see langword="true"/> if the matrices are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Matrix<T>? left, Matrix<T>? right)
        {
            if (left is null)
            {
                return right is null;
            }
            // Uses the instance Equals method to handle comparison logic.
            // This implicitly handles the case where 'left' is not null but 'right' is null.
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two matrices for inequality.
        /// </summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns><see langword="true"/> if the matrices are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Matrix<T>? left, Matrix<T>? right)
        {
            return !(left == right);
        }
    }
}