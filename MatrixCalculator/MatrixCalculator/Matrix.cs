using System;

namespace MatrixCalculator
{
    class Matrix
    {
        
        /// <summary>
        /// Contains the elements of the matrix.
        /// </summary>
        public double[,] Elements { get; set; }

        /// <summary>
        /// Creates a 1x1 matrix.
        /// </summary>
        public Matrix()
        {
            Elements = new double[1, 1];
        }

        /// <summary>
        /// Creates a rectangular matrix with the given size.
        /// </summary>
        /// <param name="numberOfRows">Number of horizontal rows in the matrix.</param>
        /// <param name="numberOfColumns">Number of vertical columns in the matrix.</param>
        public Matrix(int numberOfRows, int numberOfColumns)
        {
            Elements = new double[numberOfRows, numberOfColumns];
        }

        /// <summary>
        /// Creates a matrix from a given 2-dimensional array.
        /// </summary>
        /// <param name="matrix">2D array of numbers.</param>
        public Matrix(double[,] matrix)
        {
            Elements = matrix;
        }

        /// <summary>
        /// Overrides the + operator for adding two matrices.
        /// </summary>
        /// <param name="matrix1">First matrix.</param>
        /// <param name="matrix2">Second matrix.</param>
        /// <returns>Sum of the two matrices.</returns>
        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            // Checking that both matrices have the same number of rows.
            if (matrix1.NumberOfRows() != matrix2.NumberOfRows())
            {
                throw new MatricesOfIncompatibleSizesException("Cannot add matrices of different sizes.");
            }
            // Checking that both matrices have the asme number of columns.
            if (matrix1.NumberOfColumns() != matrix2.NumberOfColumns())
            {
                throw new MatricesOfIncompatibleSizesException("Cannot add matrices of different sizes.");
            }

            // Adding two matrices.
            Matrix matrix3 = new Matrix(matrix1.NumberOfRows(), matrix1.NumberOfColumns());
            for (int i = 0; i < matrix1.NumberOfRows(); i++)
            {
                for (int j = 0; j < matrix1.NumberOfColumns(); j++)
                {
                    matrix3.Elements[i, j] = matrix1.Elements[i, j] + matrix2.Elements[i, j];
                }
            }
            return matrix3;
        }

        /// <summary>
        /// Overrides the - operator for subtracting one matrix from another.
        /// </summary>
        /// <param name="matrix1">First matrix.</param>
        /// <param name="matrix2">Second matrix.</param>
        /// <returns>Difference of two matrices.</returns>
        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            // Checking that both matrices have the same number of rows.
            if (matrix1.NumberOfRows() != matrix2.NumberOfRows())
            {
                throw new MatricesOfIncompatibleSizesException("Cannot subtract matrices of different sizes.");
            }
            // Checking that both matrices have the asme number of columns.
            if (matrix1.NumberOfColumns() != matrix2.NumberOfColumns())
            {
                throw new MatricesOfIncompatibleSizesException("Cannot subtract matrices of different sizes.");
            }

            // Subtracting one matrix from another.
            Matrix matrix3 = new Matrix(matrix1.NumberOfRows(), matrix1.NumberOfColumns());
            for (int i = 0; i < matrix1.NumberOfRows(); i++)
            {
                for (int j = 0; j < matrix1.NumberOfColumns(); j++)
                {
                    matrix3.Elements[i, j] = matrix1.Elements[i, j] - matrix2.Elements[i, j];
                }
            }
            return matrix3;
        }

        /// <summary>
        /// Overrides the * operator for multiplying a matrix by a scalar.
        /// </summary>
        /// <param name="matrix">Matrix.</param>
        /// <param name="scalar">Scalar.</param>
        /// <returns>Matrix, multiplied by the scalar.</returns>
        public static Matrix operator *(Matrix matrix, double scalar)
        {
            for (int i = 0; i < matrix.NumberOfRows(); i++)
            {
                for (int j = 0; j < matrix.NumberOfColumns(); j++)
                {
                    matrix.Elements[i, j] *= scalar;
                }
            }
            return matrix;
        }

        /// <summary>
        /// Overrides the * operator for multiplying two matrices.
        /// </summary>
        /// <param name="matrix1">First matrix.</param>
        /// <param name="matrix2">Second matrix.</param>
        /// <returns>Product of two matrices.</returns>
        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            // Checking so that matrix1 has the same number of columns as matrix2 has rows.
            if (matrix1.NumberOfColumns() != matrix2.NumberOfRows())
            {
                throw new MatricesOfIncompatibleSizesException("Cannot multiply matrices with such dimensions.");
            }

            // Multiplying two matrices.
            Matrix matrix3 = new Matrix(matrix1.NumberOfRows(), matrix2.NumberOfColumns());
            for (int i = 0; i < matrix1.NumberOfRows(); i++)
            {
                for (int j = 0; j < matrix2.NumberOfColumns(); j++)
                {
                    double resultingElementValue = 0;
                    for (int k = 0; k < matrix1.NumberOfColumns(); k++)
                    {
                        resultingElementValue += matrix1.Elements[i, k] * matrix2.Elements[k, j];
                    }
                    matrix3.Elements[i, j] = resultingElementValue;
                }
            }
            return matrix3;
        }

        /// <summary>
        /// Converts a matrix into a two-dimensional array.
        /// </summary>
        /// <returns>A 2D array representing that matrix.</returns>
        public double[,] ToArray()
        {
            double[,] outMatrix = new double[NumberOfRows(), NumberOfColumns()];
            for (int i = 0; i < NumberOfRows(); i++)
            {
                for (int j = 0; j < NumberOfColumns(); j++)
                {
                    outMatrix[i, j] = Elements[i, j];
                }
            }
            return outMatrix;
        }

        /// <summary>
        /// Writes the matrix into the console.
        /// </summary>
        public void Write()
        {
            for (int i = 0; i < NumberOfRows(); i++)
            {
                Console.Write("\t");
                for (int j = 0; j < NumberOfColumns(); j++)
                {
                    Console.Write($"{Math.Round(Elements[i, j], 3), -13}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Returns the number of horizontal rows in the given matrix.
        /// </summary>
        /// <returns>The number of rows in the matrix.</returns>
        public int NumberOfRows()
        {
            return Elements.GetLength(0);
        }

        /// <summary>
        /// Returns the number of vertical columns in the given matrix.
        /// </summary>
        /// <returns>The number of columns in the matrix.</returns>
        public int NumberOfColumns()
        {
            return Elements.GetLength(1);
        }

        /// <summary>
        /// Checks if the matrix has the same number of rows as columns (i.e. if it is a square matrix).
        /// </summary>
        /// <returns>true, if the matrix is square; false, otherwise.</returns>
        public bool IsSquare()
        {
            return NumberOfColumns() == NumberOfRows();
        }

        /// <summary>
        /// Swaps two rows of the matrix.
        /// </summary>
        /// <param name="rowIndex1">First row (numbering starts with 0).</param>
        /// <param name="rowIndex2">Second row (numbering starts with 0).</param>
        public void SwapRows(int rowIndex1, int rowIndex2)
        {
            Matrix newMatrix = new Matrix(NumberOfRows(), NumberOfColumns());
            for (int i = 0; i < NumberOfRows(); i++)
            {
                for (int j = 0; j < NumberOfColumns(); j++)
                {
                    newMatrix.Elements[i, j] = Elements[i, j];
                }
            }
            for (int j = 0; j < NumberOfColumns(); j++)
            {
                newMatrix.Elements[rowIndex1, j] = Elements[rowIndex2, j];
                newMatrix.Elements[rowIndex2, j] = Elements[rowIndex1, j];
            }
            Elements = newMatrix.ToArray();
        }

        /// <summary>
        /// Fills the matrix with random integers within range [lowerBound; upperBound] (inclusive).
        /// </summary>
        /// <param name="lowerBound">The smallest number possible to be generated.</param>
        /// <param name="upperBound">The largest number possible to be generated.</param>
        public void FillRandomInt(int lowerBound, int upperBound)
        {
            for (int i = 0; i < NumberOfRows(); i++)
            {
                for (int j = 0; j < NumberOfColumns(); j++)
                {
                    Random rand = new Random();
                    Elements[i, j] = rand.Next(lowerBound, upperBound + 1);
                }
            }
        }

        /// <summary>
        /// Fills the matrix with random double values within range [lowerBound; upperBound] (inclusive).
        /// </summary>
        /// <param name="lowerBound">The smallest number possible to be generated.</param>
        /// <param name="upperBound">The largest number possible to be generated.</param>
        public void FillRandomDouble(double lowerBound, double upperBound)
        {
            for (int i = 0; i < NumberOfRows(); i++)
            {
                for (int j = 0; j < NumberOfColumns(); j++)
                {
                    Random rand = new Random();
                    Elements[i, j] = rand.NextDouble() * (upperBound - lowerBound) + lowerBound;
                }
            }
        }

        /// <summary>
        /// Gets the trace of the given matrix.
        /// </summary>
        /// <returns>The sum of the elements on the main diagonal of the matrix.</returns>
        public double Trace()
        {
            double trace = 0;
            if (!IsSquare())
            {
                throw new MatrixNotSquareException("Non-square matrices do not have a trace.");
            }
            for (int i = 0; i < NumberOfRows(); i++)
            {
                trace += Elements[i, i];
            }
            return trace;
        }

        /// <summary>
        /// Transposes the given matrix.
        /// </summary>
        public void Transpose()
        {
            Matrix matrixT = new Matrix(NumberOfColumns(), NumberOfRows());
            for (int i = 0; i < NumberOfColumns(); i++)
            {
                for (int j = 0; j < NumberOfRows(); j++)
                {
                    matrixT.Elements[i, j] = Elements[j, i];
                }
            }
            Elements = matrixT.ToArray();
        }

        /// <summary>
        /// Stage 1 of the Gaussian method (from the lecture #2 in linear algebra).
        /// </summary>
        /// <param name="currentRow">Number of row of the current element.</param>
        /// <param name="currentColumn">Number of column of the current element.</param>
        private void GaussianMethodStage1(int currentRow, int currentColumn, ref double extraCoefficient)
        {
            if (Elements[currentRow, currentColumn] == 0)
            {
                GaussianMethodStage2(currentColumn, currentColumn, ref extraCoefficient);
            }
            else
            {
                for (int row = currentRow + 1; row < NumberOfRows(); row++)
                {
                    double coefficient;
                    coefficient = (-1) * Elements[row, currentColumn] / Elements[currentRow, currentColumn];
                    for (int column = currentColumn; column < NumberOfColumns(); column++)
                    {
                        Elements[row, column] += coefficient * Elements[currentRow, column];
                    }
                }
                if ((++currentRow < NumberOfRows()) && (++currentColumn < NumberOfColumns()))
                {
                    GaussianMethodStage1(currentRow, currentColumn, ref extraCoefficient);
                }
            }
        }

        /// <summary>
        /// Stage 2 of the Gaussian method (from the lecture #2 in linear algebra).
        /// </summary>
        /// <param name="currentRow">Number of row of the current element.</param>
        /// <param name="currentColumn">Number of row of the current element.</param>
        private void GaussianMethodStage2(int currentRow, int currentColumn, ref double extraCoefficient)
        {
            if (Elements[currentRow, currentColumn] == 0)
            {
                bool containsNonZeroElements = false;
                int rowOfFirstNonZeroElement = 0;
                for (int row = currentRow + 1; row < NumberOfRows() && (!containsNonZeroElements); row++)
                {
                    if (Elements[row, currentColumn] != 0)
                    {
                        rowOfFirstNonZeroElement = row;
                        containsNonZeroElements = true;
                    }
                }
                if (!containsNonZeroElements)
                {
                    GaussianMethodStage3(currentRow, currentColumn, ref extraCoefficient);
                }
                else
                {
                    SwapRows(currentRow, rowOfFirstNonZeroElement);
                    extraCoefficient *= -1;
                    GaussianMethodStage1(++currentRow, ++currentColumn, ref extraCoefficient);
                }
            }
        }

        /// <summary>
        /// Stage 3 of the Gaussian method (from the lecture #2 in linear algebra).
        /// </summary>
        /// <param name="currentRow">Number of row of the current element.</param>
        /// <param name="currentColumn">Number of column of the current element.</param>
        private void GaussianMethodStage3(int currentRow, int currentColumn, ref double extraCoefficient)
        {
            if ((++currentRow < NumberOfRows()) && (++currentColumn < NumberOfColumns()))
            {
                GaussianMethodStage1(currentRow, currentColumn, ref extraCoefficient);
            }
        }

        /// <summary>
        /// Uses the Gaussian method to make the current matrix into an upper triangular matrix.
        /// </summary>
        public void ToUpperTriangularMatrix(ref double extraCoefficient)
        {
            GaussianMethodStage1(0, 0, ref extraCoefficient);
        }

        /// <summary>
        /// Uses the Gaussian method to turn the given matrix to a staircase form.
        /// </summary>
        public void ToStaircaseMatrix()
        {
            double dummyCoefficient = 0;
            ToUpperTriangularMatrix(ref dummyCoefficient);
            for (int i = 0; i < NumberOfRows(); i++)
            {
                int firstNonZeroElementIndex = 0;
                while ((firstNonZeroElementIndex < NumberOfColumns() - 1) && (Elements[i, firstNonZeroElementIndex] == 0))
                {
                    firstNonZeroElementIndex++;
                }
                if (firstNonZeroElementIndex < NumberOfColumns() - 1)
                {
                    double coefficient = 1 / Elements[i, firstNonZeroElementIndex];
                    for (int j = firstNonZeroElementIndex; j < NumberOfColumns(); j++)
                    {
                        Elements[i, j] *= coefficient;
                    }
                }
                else
                {
                    // If we encountered a row of zeroes, we can stop checking further, since
                    // there cannot be numbers below that.
                    return;
                }
            }
        }

        /// <summary>
        /// Turns the matrix into its canonical form.
        /// </summary>
        public void ToCanonicalForm()
        {
            ToStaircaseMatrix();

            // Searching for the non-zero row at the bottom of the matrix.
            int nonZeroRowIndex = NumberOfRows() - 1;
            bool foundNonZeroRow = false;
            for (int row = NumberOfRows() - 1; (row >= 0) && (!foundNonZeroRow); row--)
            {
                int firstNonZeroElement = NumberOfColumns() - 1;
                while ((firstNonZeroElement > 0) && (Elements[row, firstNonZeroElement] == 0))
                {
                    firstNonZeroElement--;
                }
                if (firstNonZeroElement != 0)
                {
                    nonZeroRowIndex = row;
                    foundNonZeroRow = true;
                }
            }

            for (int row = nonZeroRowIndex; row > 0; row--)
            {
                int nonZeroColumnIndex = 0;
                while ((nonZeroColumnIndex < NumberOfColumns() - 1) && (Elements[row, nonZeroColumnIndex] == 0))
                {
                    nonZeroColumnIndex++;
                }
                for (int k = 1; k <= row; k++)
                {
                    double coefficient = Elements[row - k, nonZeroColumnIndex];
                    for (int column = NumberOfColumns() - 1; column > 0; column--)
                    {

                        Elements[row - k, column] -= Elements[row, column] * coefficient;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the determinant of the given matrix.
        /// </summary>
        /// <returns>The value of the determinant of the matrix.</returns>
        public double Determinant()
        {
            if (!IsSquare())
            {
                throw new MatrixNotSquareException("Cannot get a determinant of a non-square matrix.");
            }
            Matrix dummyMatrix = new Matrix(Elements);
            double det = 1;
            double extraCoefficient = 1;
            dummyMatrix.ToUpperTriangularMatrix(ref extraCoefficient);
            for (int i = 0; i < dummyMatrix.NumberOfRows(); i++)
            {
                det *= dummyMatrix.Elements[i, i];
            }
            det *= 1 / extraCoefficient;
            return det;
        }
    }
}