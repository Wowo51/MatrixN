## `MatrixN` Code Guide

This guide provides a comprehensive overview of the `MatrixN` library, a generic and robust matrix manipulation tool written in C\#. It explains the library's design, how to use its features, and the internal workings of its core functionalities.

### 1\. Project Structure

The `MatrixN` solution is organized into two main projects:

  * **`MatrixN`**: The core library containing the `Matrix<T>` class and all related logic.
  * **`MatrixNTest`**: A unit testing project to ensure the correctness and robustness of the library.

The `Matrix<T>` class is implemented using `partial` classes, which splits its definition across several files for better organization:

  * **`Matrix.cs`**: Core class definition, constructors, and element accessors.
  * **`MatrixArithmetic.cs`**: Handles arithmetic operations like addition, subtraction, and multiplication.
  * **`MatrixCoreProperties.cs`**: Implements fundamental matrix properties like `IsSquare` and `Trace`.
  * **`MatrixDeterminant.cs`**: Provides functionality for calculating the determinant.
  * **`MatrixInverse.cs`**: Contains the logic for matrix inversion.
  * **`MatrixEquality.cs`**: Manages equality comparisons between matrices.
  * **`MatrixFactory.cs`**: A static class for creating common matrix types.

### 2\. Core Concepts

#### Generic Implementation

The library is built around the generic class `Matrix<T>`, where `T` is constrained to implement the `System.Numerics.INumber<T>` interface. This modern .NET feature allows the matrix to work seamlessly with any numeric type that supports basic arithmetic operations, including `int`, `long`, `float`, `double`, and `decimal`.

#### Internal Data Storage

A matrix is conceptually a two-dimensional grid, but for efficiency, `MatrixN` stores its data in a one-dimensional array (`_data`). Elements are arranged in row-major order. The index of an element at a given `(row, col)` is calculated by the internal `GetFlatIndex` method:

`index = row * _columns + col`

#### Error Handling: The "No-Throw" Philosophy

A key design principle of `MatrixN` is to avoid throwing exceptions for common operational failures, such as attempting an operation on matrices with incompatible dimensions. Instead, methods that can fail follow a `Try*` pattern. These methods return a `bool` indicating success or failure and provide the result through an `out` parameter. This approach promotes safer and more predictable code.

### 3\. How to Use the `Matrix<T>` Class

#### Initialization and Creation

There are several ways to create a `Matrix<T>` instance.

**a. Direct Instantiation**

Create a matrix of a specific size, initialized with default values (usually zero), using the main constructor.

```csharp
// Creates a 3x4 matrix of integers, all initialized to 0.
var matrix = new Matrix<int>(3, 4);
```

**b. Using `MatrixFactory`**

The `MatrixFactory` class provides static methods for creating common matrices.

  * **Zero Matrix**: A matrix where all elements are zero.

    ```csharp
    // Creates a 5x5 matrix of doubles, all initialized to 0.0.
    Matrix<double> zeroMatrix = MatrixFactory.CreateZero<double>(5, 5);
    ```

  * **Identity Matrix**: A square matrix with ones on the main diagonal and zeros elsewhere.

    ```csharp
    // Creates a 4x4 identity matrix of floats.
    Matrix<float> identityMatrix = MatrixFactory.CreateIdentity<float>(4);
    ```

#### Accessing and Modifying Elements

Direct element access via an indexer `matrix[r, c]` is not provided to enforce the "no-throw" policy. Instead, use the `TryGetElement` and `TrySetElement` methods.

  * **`TryGetElement(int row, int col, out T value)`**: Retrieves an element's value. Returns `false` if the indices are out of bounds.

    ```csharp
    matrix.TryGetElement(0, 1, out int value);
    Console.WriteLine(value); // Prints the element at (0, 1)
    ```

  * **`TrySetElement(int row, int col, T value)`**: Updates an element's value. Returns `false` if the indices are out of bounds.

    ```csharp
    // Sets the element at (2, 1) to 99.
    bool wasSet = matrix.TrySetElement(2, 1, 99);
    ```

### 4\. Matrix Operations

`MatrixN` supports a wide range of standard matrix operations.

#### Arithmetic Operations

Arithmetic operations can be performed using overloaded operators or the `Try*` methods for added safety.

  * **Addition**: `+` or `TryAdd()`
  * **Subtraction**: `-` or `TrySubtract()`
  * **Multiplication (Matrix-Matrix)**: `*` or `TryMultiply()`
  * **Multiplication (Matrix-Scalar)**: `*` or `ScalarMultiply()`

**How it Works**: The arithmetic operators are implemented for convenience but assume the operation is valid. The `Try*` methods first check for dimension compatibility before calling the corresponding operator. For example, `TryAdd` ensures both matrices have the same dimensions. Matrix multiplication is parallelized using `Parallel.For` to improve performance on larger matrices.

```csharp
Matrix<int> A = new Matrix<int>(2, 2);
// ... populate A ...
Matrix<int> B = new Matrix<int>(2, 2);
// ... populate B ...

// Using the Try* pattern (recommended)
if (Matrix<int>.TryAdd(A, B, out Matrix<int>? sum))
{
    Console.WriteLine("Sum:\n" + sum);
}
else
{
    Console.WriteLine("Matrices could not be added.");
}

// Using the operator (if you are certain dimensions are compatible)
Matrix<int> product = A * B;
```

#### Transposition

The `Transpose` method swaps the rows and columns of a matrix.

**How it Works**: A new matrix with inverted dimensions (`cols`, `rows`) is created. The method then iterates through the original matrix, and for each element at `(r, c)`, it places it into the new matrix at `(c, r)`. This operation is also parallelized.

```csharp
Matrix<double> original = new Matrix<double>(2, 3);
// ... populate original ...

Matrix<double> transposed = Matrix<double>.Transpose(original);
// transposed is now a 3x2 matrix.
```

#### Determinant

The determinant is a scalar value that can be computed from a square matrix.

**How it Works**: The `TryGetDeterminant(out T determinant)` method first checks if the matrix is square. It then calls a private recursive helper, `CalculateDeterminantRecursive`, which uses **cofactor expansion**. This method has base cases for 0x0, 1x1, and 2x2 matrices and recursively breaks down larger matrices into smaller ones until a base case is reached.

  * **Convention**: The determinant of a 0x0 matrix is 1.

<!-- end list -->

```csharp
Matrix<double> m = new Matrix<double>(3, 3);
// ... populate m ...

if (m.TryGetDeterminant(out double det))
{
    Console.WriteLine($"Determinant: {det}");
}
else
{
    Console.WriteLine("Determinant could not be calculated (matrix is not square).");
}
```

#### Inverse

The inverse of a square matrix `A`, denoted $A^{-1}$, is the matrix that, when multiplied by `A`, yields the identity matrix.

**How it Works**: The `TryGetInverse(out Matrix<T>? inverse)` method follows the formula:
$$A^{-1} = \frac{1}{\det(A)} \text{adj}(A)$$

1.  It first calculates the determinant using `TryGetDeterminant`. If the determinant is zero, the matrix is singular and has no inverse.
2.  It computes the **cofactor matrix**.
3.  It finds the **adjoint matrix** (adjugate) by transposing the cofactor matrix.
4.  Finally, it calculates the inverse by multiplying the adjoint matrix by the reciprocal of the determinant (`1 / det`).

<!-- end list -->

  * **Convention**: The inverse of a 0x0 matrix is an empty 0x0 matrix.

<!-- end list -->

```csharp
Matrix<float> invertibleMatrix = new Matrix<float>(2, 2);
// ... populate with invertible data ...

if (invertibleMatrix.TryGetInverse(out Matrix<float>? inverse))
{
    Console.WriteLine("Inverse matrix:\n" + inverse);
}
else
{
    Console.WriteLine("Matrix is not invertible.");
}
```

### 5\. Other Properties and Methods

  * **`IsSquare`**: A boolean property that returns `true` if the number of rows equals the number of columns.
  * **`TryGetTrace(out T trace)`**: Calculates the trace of a square matrix, which is the sum of the elements on the main diagonal.
  * **`Equals(Matrix<T> other)`**: Checks for value equality between two matrices. It returns `true` if both matrices have the same dimensions and all corresponding elements are equal.
  * **`ToString()`**: Returns a formatted string representation of the matrix, which is useful for debugging and display.

### 6\. Unit Testing

The `MatrixNTest` project provides a comprehensive suite of tests covering all aspects of the library's functionality, including:

  * Basic property and accessor tests.
  * Arithmetic operation correctness and dimension mismatch handling.
  * Determinant and inverse calculations for various matrix types.
  * Validation of mathematical properties like associativity and distributivity.
  * Edge cases, such as operations on 0x0 matrices.

These tests serve as an excellent resource for examples of how to use the library in practice.