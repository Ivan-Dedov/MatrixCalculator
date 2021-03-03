using System;

namespace MatrixCalculator
{
    partial class Program
    {

        /// <summary>
        /// Writes the message in a specified colour.
        /// </summary>
        /// <param name="message">The string to be written.</param>
        /// <param name="colour">The selected colour.</param>
        static void WriteColour(string message, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Console.Write(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes the message in a specified colour, inserts a linebreak.
        /// </summary>
        /// <param name="message">The string to be written.</param>
        /// <param name="colour">The selected colour.</param>
        static void WriteLineColour(string message, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Highlights the selected operation (sets the background colour).
        /// </summary>
        /// <param name="operationIndex">The index of the operation to be highlighted.</param>
        /// <param name="colour">The colour to choose for highlighting.</param>
        static void HighlightOperation(int operationIndex, ConsoleColor colour)
        {
            // Array of strings containg the names of the operations (see Constants.cs).
            string[] operations = new string[]
            {
                CMD_TRACE,
                CMD_TRANSPOSE,
                CMD_ADD,
                CMD_SUBTRACT,
                CMD_MULTIPLY_MATRICES,
                CMD_MULTIPLY_BY_SCALAR,
                CMD_DETERMINANT,
                CMD_SOLVE_SLE,
                CMD_EXIT,
            };

            for (int i = MIN_OPERATION_INDEX; i < operationIndex; i++)
            {
                Console.WriteLine("\t" + operations[i]);
            }

            Console.BackgroundColor = colour;
            WriteLineColour("\t" + operations[operationIndex], ConsoleColor.Black);
            Console.ResetColor();

            for (int i = operationIndex + 1; i <= MAX_OPERATION_INDEX; i++)
            {
                Console.WriteLine("\t" + operations[i]);
            }
        }

        /// <summary>
        /// Highlights the selected method.
        /// </summary>
        /// <param name="methodIndex">The index of the method to be highlighted.</param>
        /// <param name="colour">The colour used to highlight the method.</param>
        static void HighlightMethod(int methodIndex, ConsoleColor colour)
        {
            // Array of strings containg the names of the operations (see Constants.cs).
            string[] methods = new string[]
            {
                MTHD_CONSOLE,
                MTHD_FILE,
                MTHD_RANDOM_INT,
                MTHD_RANDOM_DOUBLE,
            };

            for (int i = MIN_METHOD_INDEX; i < methodIndex; i++)
            {
                Console.WriteLine("\t" + methods[i]);
            }

            Console.BackgroundColor = colour;
            WriteLineColour("\t" + methods[methodIndex], ConsoleColor.Black);
            Console.ResetColor();

            for (int i = methodIndex + 1; i <= MAX_METHOD_INDEX; i++)
            {
                Console.WriteLine("\t" + methods[i]);
            }
        }

        /// <summary>
        /// Checks if the matrix size is correct (if inputs are integers, etc.).
        /// </summary>
        /// <param name="input">String array of user inputs.</param>
        /// <returns>true, if the input can be interpreted as a matrix size; false, otherwise.</returns>
        static bool IsMatrixSizeCorrect(string[] input)
        {
            // Checking if input contains excatly two values (length, width).
            if (input.Length != 2)
            {
                return false;
            }
            // Checking if the two values can be interpreted as positive integers.
            for (int i = 0; i < input.Length; i++)
            {
                if ((!int.TryParse(input[i], out int s)) || (s <= 0) || (s > MAX_MATRIX_SIZE))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if the row of the matrix is correct.
        /// </summary>
        /// <param name="input">String array containing the elements in one row of the matrix.</param>
        /// <returns>true, if all elements of the row can be interpreted as doubles; false, otherwise.</returns>
        static bool IsRowCorrect(string[] input, int numberOfColumns)
        {
            // Checking that the row contains the correct number of elements.
            if (input.Length != numberOfColumns)
            {
                return false;
            }
            // Checking if every element can be parsed into the double datatype.
            foreach (string number in input)
            {
                if (!double.TryParse(number, out double a) || (Math.Abs(a) >= UNREACHABLE_ELEMENT_VALUE))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if the system of equations has rows, where 0*N = B, where B is not 0.
        /// </summary>
        /// <param name="matrix">The matrix to be checked (in canonical form).</param>
        /// <returns>true, if the system, represented by the matrix cannot be solved; false, otherwise.</returns>
        static bool ContainsUnsolvableRows(Matrix matrix)
        {
            for (int i = matrix.NumberOfRows() - 1; i >= 0; i--)
            {
                // Looking for the first non-zero element in the row.
                int j = 0;
                while ((j < matrix.NumberOfColumns() - 1) && (matrix.Elements[i, j] == 0))
                {
                    j++;
                }

                // If there are no non-zero elements in the row and the element in the last row is not 0.
                if ((j == matrix.NumberOfColumns() - 1) && (matrix.Elements[i, matrix.NumberOfColumns() - 1] != 0))
                {
                    return true;
                }

                // If the last row is solveable, every row is.
                if (j < matrix.NumberOfColumns() - 1)
                {
                    return false;
                }
            }
            return false;
        }

    }
}