using System;
using System.IO;

namespace MatrixCalculator
{
    partial class Program
    {

        /// <summary>
        /// Reads the matrix using the specified method.
        /// </summary>
        /// <param name="commandName">The name of the command that invoked the method.</param>
        /// <returns>The input matrix.</returns>
        static Matrix InputMatrix(string commandName)
        {
            int method = ChooseMethodOfInput(commandName);
            switch (method)
            {
                case 0:
                    return InputMatrixFromConsole(commandName);
                case 1:
                    return InputMatrixFromFile(commandName);
                case 2:
                    return InputMatrixWithRandomIntegers(commandName);
                case 3:
                    return InputMatrixWithRandomDoubles(commandName);
                default:
                    return new Matrix();
            }
        }

        /// <summary>
        /// Selects the method which to use to input the elements of the matrix.
        /// </summary>
        /// <param name="commandName">The name of the command that invoked the method.</param>
        /// <returns>An index of the chosen method.</returns>
        static int ChooseMethodOfInput(string commandName)
        {
            int selectedMethod = 0;
            ConsoleKey keyPressed;
            do
            {
                // Outputting the visual clues.
                Console.Clear();
                WriteLineColour(Environment.NewLine + "\t" + commandName.ToUpper() + Environment.NewLine, ConsoleColor.Green);
                HighlightMethod(selectedMethod, ConsoleColor.Yellow);

                keyPressed = Console.ReadKey().Key;
                // Moving up if the key pressed is the UP ARROW.
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedMethod = --selectedMethod < MIN_METHOD_INDEX ? MAX_METHOD_INDEX : selectedMethod;
                }
                // Moving down if the key pressed is the DOWN ARROW.
                if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedMethod = ++selectedMethod > MAX_METHOD_INDEX ? MIN_METHOD_INDEX : selectedMethod;
                }
                // Selecting the operation until pressing ENTER.
            } while (keyPressed != ConsoleKey.Enter);

            return selectedMethod;
        }

        /// <summary>
        /// Allows the user to input the matrix from the console.
        /// </summary>
        /// <param name="commandName">The name of the command that invoked this method.</param>
        /// <returns>The matrix that the user has input.</returns>
        static Matrix InputMatrixFromConsole(string commandName)
        {
            Console.Clear();
            WriteLineColour(Environment.NewLine + "\t" + commandName.ToUpper(), ConsoleColor.Green);
            if (commandName == CMD_SOLVE_SLE)
            {
                OutputInstructionsForSLE();
            }
            WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
            Console.WriteLine($"Input the size of your matrix (cannot be greater than {MAX_MATRIX_SIZE}).");
            WriteColour("\tFormat: ", ConsoleColor.DarkGray);
            Console.WriteLine("two positive integers, separated by a space.");
            Console.Write("\tInput: ");
            string[] userInput;
            do
            {
                userInput = Console.ReadLine().TrimEnd().Split(' ');
                if (IsMatrixSizeCorrect(userInput))
                {
                    Console.WriteLine();
                    int numberOfRows = int.Parse(userInput[0]);
                    int numberOfColumns = int.Parse(userInput[1]);
                    double[,] matrixArray = new double[numberOfRows, numberOfColumns];

                    WriteColour("\t[>] ", ConsoleColor.Yellow);
                    Console.WriteLine($"Input the elements of the matrix (their absolute values CANNOT be equal to or greater than {UNREACHABLE_ELEMENT_VALUE}).");
                    WriteColour("\tFormat: ", ConsoleColor.DarkGray);
                    Console.WriteLine("each row starts from the new line, elements inside one row are separated by spaces.");

                    for (int i = 0; i < numberOfRows; i++)
                    {
                        string[] row;
                        do
                        {
                            WriteColour($"\tRow #{i + 1}: ", ConsoleColor.DarkGray);
                            // Reading a row.
                            row = Console.ReadLine().Split(' ');
                            // If it is correct, parse the elements into the matrix.
                            if (IsRowCorrect(row, numberOfColumns))
                            {
                                for (int j = 0; j < numberOfColumns; j++)
                                {
                                    matrixArray[i, j] = double.Parse(row[j]);
                                }
                            }
                            // If not, output the error message and ask for input again.
                            else
                            {
                                WriteLineColour(Environment.NewLine + "\t[!] Incorrect elements in the array!", ConsoleColor.Red);
                            }
                        } while (!IsRowCorrect(row, numberOfColumns));
                    }
                    // When everything is correct, return the resulting matrix.
                    return new Matrix(matrixArray);
                }
                else
                {
                    WriteLineColour(Environment.NewLine + "\t[!] Matrix size is incorrect!", ConsoleColor.Red);
                    Console.Write("\tPlease, input the size again: ");
                }

            } while (!IsMatrixSizeCorrect(userInput));
            return new Matrix();
        }

        /// <summary>
        /// Fills the matrix using the elements from a given file.
        /// </summary>
        /// <param name="commandName">The name of the command that invoked this method.</param>
        /// <returns>A matrix from the specified file.</returns>
        static Matrix InputMatrixFromFile(string commandName)
        {
            Console.Clear();
            WriteLineColour(Environment.NewLine + "\t" + commandName.ToUpper(), ConsoleColor.Green);
            string path;
            do
            {
                try
                {
                    if (commandName == CMD_SOLVE_SLE)
                    {
                        OutputInstructionsForSLE();
                    }
                    WriteColour(Environment.NewLine + "\t[!] ", ConsoleColor.Red);
                    Console.WriteLine("Note that the file should contain ONLY the elements of the matrix, " +
                        "separated by spaces. Nothing more." + Environment.NewLine +
                        $"\tElement values CANNOT be equal to or greater than {UNREACHABLE_ELEMENT_VALUE}!");
                    WriteColour("\t[>] ", ConsoleColor.Yellow);
                    Console.Write("Input the absolute path to your file: ");
                    path = Console.ReadLine();

                    StreamReader initialFileStreamReader = new StreamReader(path);
                    int rowCount = 0;
                    int columnCount = 0;
                    string[] row;
                    // If we're going through the first row, then there is nothing to compare the previous column count to.
                    bool isOnFirstRow = true;

                    // Counting the number of rows and columns in the file.
                    while (!initialFileStreamReader.EndOfStream)
                    {
                        rowCount++;
                        row = initialFileStreamReader.ReadLine().Split(' ');
                        // If some row has a different number of columns, throw an exception.
                        if ((columnCount != row.Length) && !isOnFirstRow)
                        {
                            throw new Exception("File contains an incorrect matrix.");
                        }
                        columnCount = row.Length;
                        isOnFirstRow = false;
                    }
                    initialFileStreamReader.Close();
                    if ((rowCount > MAX_MATRIX_SIZE) || (columnCount > MAX_MATRIX_SIZE))
                    {
                        throw new Exception("Matrix is too large!");
                    }
                    Matrix m = new Matrix(rowCount, columnCount);

                    // Re-read the file and parse all the elements inside the matrix.
                    StreamReader fileStreamReader = new StreamReader(path);
                    for (int i = 0; i < rowCount; i++)
                    {
                        row = fileStreamReader.ReadLine().Split(' ');
                        for (int j = 0; j < columnCount; j++)
                        {
                            m.Elements[i, j] = double.Parse(row[j]);
                        }
                    }
                    fileStreamReader.Close();
                    return m;
                }
                catch (Exception ex)
                {
                    WriteLineColour(Environment.NewLine + "\t[!] " + ex.Message, ConsoleColor.Red);
                }
            } while (true);
        }

        /// <summary>
        /// Fills a matrix with random integers.
        /// </summary>
        /// <param name="commandName">The name of the command that invoked this method.</param>
        /// <returns>A matrix with user-specified size with random integer elements.</returns>
        static Matrix InputMatrixWithRandomIntegers(string commandName)
        {
            int lowerBound, upperBound;
            Console.Clear();
            WriteLineColour(Environment.NewLine + "\t" + commandName.ToUpper(), ConsoleColor.Green);
            if (commandName == CMD_SOLVE_SLE)
            {
                OutputInstructionsForSLE();
            }
            WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
            Console.WriteLine($"Input the size of your matrix (cannot be greater than {MAX_MATRIX_SIZE}).");
            WriteColour("\tFormat: ", ConsoleColor.DarkGray);
            Console.WriteLine("two positive integers, separated by a space.");
            Console.Write("\tInput: ");
            string[] userInput;
            do
            {
                // Getting user's input.
                userInput = Console.ReadLine().TrimEnd().Split(' ');
                // If it's correct, parsing the size.
                if (IsMatrixSizeCorrect(userInput))
                {
                    Console.WriteLine();
                    int numberOfRows = int.Parse(userInput[0]);
                    int numberOfColumns = int.Parse(userInput[1]);
                    Matrix matrix = new Matrix(numberOfRows, numberOfColumns);

                    // Getting the lower bound.
                    WriteColour("\t[>] ", ConsoleColor.Yellow);
                    Console.Write($"Input the lower bound of the integers for your matrix from range [{-UNREACHABLE_ELEMENT_VALUE + 1}; {UNREACHABLE_ELEMENT_VALUE - 2}]: ");
                    string userInputBound = Console.ReadLine();
                    while ((userInputBound.Split(' ').Length != 1) || (!int.TryParse(userInputBound, out lowerBound))
                        || (lowerBound >= UNREACHABLE_ELEMENT_VALUE - 1) || (lowerBound <= -UNREACHABLE_ELEMENT_VALUE))
                    {
                        WriteLineColour(Environment.NewLine + "\t[!] Incorrect lower bound!", ConsoleColor.Red);
                        Console.Write("\tPlease, input the lower bound again: ");
                        userInputBound = Console.ReadLine();
                    }

                    // Getting the upper bound.
                    WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
                    Console.Write($"Input the upper bound of the integers for your matrix from range [{lowerBound + 1}; {UNREACHABLE_ELEMENT_VALUE - 1}]: ");
                    userInputBound = Console.ReadLine();
                    while ((userInputBound.Split(' ').Length != 1) || (!int.TryParse(userInputBound, out upperBound))
                        || (upperBound <= lowerBound) || (Math.Abs(upperBound) >= UNREACHABLE_ELEMENT_VALUE))
                    {
                        WriteLineColour(Environment.NewLine + "\t[!] Incorrect upper bound!", ConsoleColor.Red);
                        Console.Write("\tPlease, input the upper bound again: ");
                        userInputBound = Console.ReadLine();
                    }
                    matrix.FillRandomInt(lowerBound, upperBound);
                    return matrix;
                }
                else
                {
                    WriteLineColour(Environment.NewLine + "\t[!] Matrix size is incorrect!", ConsoleColor.Red);
                    Console.Write("\tPlease, input the size again: ");
                }
            } while (!IsMatrixSizeCorrect(userInput));
            return new Matrix();
        }

        /// <summary>
        /// Fills a matrix with random doubles.
        /// </summary>
        /// <param name="commandName">The name of the command that invoked this method.</param>
        /// <returns>The matrix with the user-specified size with random real elements.</returns>
        static Matrix InputMatrixWithRandomDoubles(string commandName)
        {
            double lowerBound, upperBound;
            Console.Clear();
            WriteLineColour(Environment.NewLine + "\t" + commandName.ToUpper(), ConsoleColor.Green);
            if (commandName == CMD_SOLVE_SLE)
            {
                OutputInstructionsForSLE();
            }
            WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
            Console.WriteLine($"Input the size of your matrix (cannot be greater than {MAX_MATRIX_SIZE}).");
            WriteColour("\tFormat: ", ConsoleColor.DarkGray);
            Console.WriteLine("two positive integers, separated by a space.");
            Console.Write("\tInput: ");
            string[] userInput;
            do
            {
                // Getting user's input.
                userInput = Console.ReadLine().TrimEnd().Split(' ');
                // If it's correct, parsing the size.
                if (IsMatrixSizeCorrect(userInput))
                {
                    Console.WriteLine();
                    int numberOfRows = int.Parse(userInput[0]);
                    int numberOfColumns = int.Parse(userInput[1]);
                    Matrix matrix = new Matrix(numberOfRows, numberOfColumns);

                    // Getting the lower bound.
                    WriteColour("\t[>] ", ConsoleColor.Yellow);
                    Console.Write($"Input the lower bound of the doubles for your matrix from range ({-UNREACHABLE_ELEMENT_VALUE}; {UNREACHABLE_ELEMENT_VALUE}): ");
                    string userInputBound = Console.ReadLine();
                    while ((userInputBound.Split(' ').Length != 1) || (!double.TryParse(userInputBound, out lowerBound))
                        || (Math.Abs(lowerBound) >= UNREACHABLE_ELEMENT_VALUE))
                    {
                        WriteLineColour(Environment.NewLine + "\t[!] Incorrect lower bound!", ConsoleColor.Red);
                        Console.Write("\tPlease, input the lower bound again: ");
                        userInputBound = Console.ReadLine();
                    }

                    // Getting the upper bound.
                    WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
                    Console.Write($"Input the upper bound of the integers for your matrix from range ({lowerBound}; {UNREACHABLE_ELEMENT_VALUE}): ");
                    userInputBound = Console.ReadLine();
                    while ((userInputBound.Split(' ').Length != 1) || (!double.TryParse(userInputBound, out upperBound))
                        || (upperBound <= lowerBound) || (Math.Abs(upperBound) >= UNREACHABLE_ELEMENT_VALUE))
                    {
                        WriteLineColour(Environment.NewLine + "\t[!] Incorrect upper bound!", ConsoleColor.Red);
                        Console.Write("\tPlease, input the upper bound again: ");
                        userInputBound = Console.ReadLine();
                    }
                    matrix.FillRandomDouble(lowerBound, upperBound);
                    return matrix;
                }
                else
                {
                    WriteLineColour(Environment.NewLine + "\t[!] Matrix size is incorrect!", ConsoleColor.Red);
                    Console.Write("\tPlease, input the size again: ");
                }
            } while (!IsMatrixSizeCorrect(userInput));
            return new Matrix();
        }

        /// <summary>
        /// Output instructions for solving SLE.
        /// </summary>
        private static void OutputInstructionsForSLE()
        {
            WriteColour(Environment.NewLine + "\t[!] ", ConsoleColor.Red);
            Console.WriteLine("Note that the size of the matrix and the matrix itself HAVE TO include the column without variables!");
            WriteLineColour("\tExample:", ConsoleColor.DarkGray);
            Console.WriteLine("\tSize: 2 3");
            Console.WriteLine("\t1 2 3" + Environment.NewLine + "\t4 5 6");
            WriteLineColour("\tis equivalent to:", ConsoleColor.DarkGray);
            Console.WriteLine("\t1 2 | 3" + Environment.NewLine + "\t4 5 | 6");
            WriteLineColour("\ti.e.", ConsoleColor.DarkGray);
            Console.WriteLine("\t1*x1 + 2*x2 = 3" + Environment.NewLine + "\t4*x1 + 5*x2 = 6");
        }

        /// <summary>
        /// Executes the trace command.
        /// </summary>
        static void GetMatrixTrace()
        {
            try
            {
                Matrix inputMatrix = InputMatrix(CMD_TRACE);
                double trace = inputMatrix.Trace();
                WriteLineColour(Environment.NewLine + "\tYour matrix:", ConsoleColor.DarkGray);
                inputMatrix.Write();
                WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
                Console.Write($"The trace of the matrix is ");
                WriteLineColour(trace.ToString(), ConsoleColor.Cyan);
            }
            catch (Exception ex)
            {
                WriteLineColour(Environment.NewLine + $"\t[!] Error: {ex.Message}", ConsoleColor.Red);
            }
        }

        /// <summary>
        /// Executes the transpose command.
        /// </summary>
        static void TransposeMatrix()
        {
            Matrix inputMatrix = InputMatrix(CMD_TRANSPOSE);
            WriteLineColour(Environment.NewLine + "\tYour matrix:", ConsoleColor.DarkGray);
            inputMatrix.Write();
            WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
            Console.WriteLine("Here is the transposed matrix:");
            inputMatrix.Transpose();
            inputMatrix.Write();
        }

        /// <summary>
        /// Executes the addition command.
        /// </summary>
        static void AddMatrices()
        {
            try
            {
                Matrix inputMatrix1 = InputMatrix(CMD_ADD);
                Matrix inputMatrix2 = InputMatrix(CMD_ADD);
                WriteLineColour(Environment.NewLine + "\tYour first matrix:", ConsoleColor.DarkGray);
                inputMatrix1.Write();
                WriteLineColour(Environment.NewLine + "\tYour second matrix:", ConsoleColor.DarkGray);
                inputMatrix2.Write();
                Matrix sum = inputMatrix1 + inputMatrix2;
                WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
                Console.WriteLine("The sum of the matrices:");
                sum.Write();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                WriteLineColour(Environment.NewLine + $"\t[!] Error: {ex.Message}", ConsoleColor.Red);
            }
        }

        /// <summary>
        /// Executes the subtraction command.
        /// </summary>
        static void SubtractMatrices()
        {
            try
            {
                Matrix inputMatrix1 = InputMatrix(CMD_SUBTRACT);
                Matrix inputMatrix2 = InputMatrix(CMD_SUBTRACT);
                WriteLineColour(Environment.NewLine + "\tYour first matrix:", ConsoleColor.DarkGray);
                inputMatrix1.Write();
                WriteLineColour(Environment.NewLine + "\tYour second matrix:", ConsoleColor.DarkGray);
                inputMatrix2.Write();
                Matrix difference = inputMatrix1 - inputMatrix2;
                WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
                Console.WriteLine("The difference of the matrices:");
                difference.Write();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                WriteLineColour(Environment.NewLine + $"\t[!] Error: {ex.Message}", ConsoleColor.Red);
            }
        }

        /// <summary>
        /// Executes the multiplication by matrix command.
        /// </summary>
        static void MultiplyMatrices()
        {
            try
            {
                Matrix inputMatrix1 = InputMatrix(CMD_MULTIPLY_MATRICES);
                Matrix inputMatrix2 = InputMatrix(CMD_MULTIPLY_MATRICES);
                WriteLineColour(Environment.NewLine + "\tYour first matrix:", ConsoleColor.DarkGray);
                inputMatrix1.Write();
                WriteLineColour(Environment.NewLine + "\tYour second matrix:", ConsoleColor.DarkGray);
                inputMatrix2.Write();
                Matrix product = inputMatrix1 * inputMatrix2;
                WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
                Console.WriteLine("The product of the matrices:");
                product.Write();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                WriteLineColour(Environment.NewLine + $"\t[!] Error: {ex.Message}", ConsoleColor.Red);
            }
        }

        /// <summary>
        /// Executes the multiplication by scalar command.
        /// </summary>
        static void MultiplyByScalar()
        {
            try
            {
                Matrix inputMatrix = InputMatrix(CMD_MULTIPLY_BY_SCALAR);
                WriteLineColour(Environment.NewLine + "\tYour first matrix:", ConsoleColor.DarkGray);
                inputMatrix.Write();

                WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
                Console.Write("Input the real number by which to multiply the matrix: ");
                double scalar;
                while (!double.TryParse(Console.ReadLine(), out scalar))
                {
                    WriteLineColour(Environment.NewLine + "\t[!] Incorrect input.", ConsoleColor.Red);
                    Console.Write("\tInput the number again: ");
                }

                Matrix product = inputMatrix * scalar;
                WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
                Console.WriteLine("The product of the matrix by the scalar:");
                product.Write();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                WriteLineColour(Environment.NewLine + $"\t[!] Error: {ex.Message}", ConsoleColor.Red);
            }
        }

        /// <summary>
        /// Executes the determinant command.
        /// </summary>
        static void GetMatrixDeterminant()
        {
            try
            {
                Matrix inputMatrix = InputMatrix(CMD_DETERMINANT);
                Matrix inputMatrixCopy = new Matrix(inputMatrix.ToArray());
                double determinant = inputMatrix.Determinant();
                WriteLineColour(Environment.NewLine + "\tYour matrix:", ConsoleColor.DarkGray);
                inputMatrixCopy.Write();
                WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
                Console.Write($"The determinant of the matrix is ");
                WriteLineColour(determinant.ToString(), ConsoleColor.Cyan);
            }
            catch (Exception ex)
            {
                WriteLineColour(Environment.NewLine + $"\t[!] Error: {ex.Message}", ConsoleColor.Red);
            }
        }

        /// <summary>
        /// Executes the method for solving a system of algebraic equations.
        /// </summary>
        static void SolveSystemOfEquations()
        {
            try
            {
                Matrix inputMatrix = InputMatrix(CMD_SOLVE_SLE);

                // If there are more variables than rows, matrix is indeterminate.
                if ((inputMatrix.NumberOfRows() + 1) != inputMatrix.NumberOfColumns())
                {
                    throw new MatricesOfIncompatibleSizesException("The system with such parameters is indeterminate!");
                }

                WriteLineColour(Environment.NewLine + "\tYour matrix:", ConsoleColor.DarkGray);
                inputMatrix.Write();
                inputMatrix.ToCanonicalForm();

                // If there is a row with zeroes and a non-zero element on the right, matrix is unsolveable.
                if (ContainsUnsolvableRows(inputMatrix))
                {
                    throw new Exception("The system does not have solutions!");
                }

                // Matrix is canonical, therefore, solveable.
                WriteColour(Environment.NewLine + "\t[>] ", ConsoleColor.Yellow);
                Console.WriteLine("The solution:");
                for (int i = 0; i < inputMatrix.NumberOfRows(); i++)
                {
                    WriteColour($"\tx{i + 1}", ConsoleColor.DarkGray);
                    Console.WriteLine($" = {Math.Round(inputMatrix.Elements[i, inputMatrix.NumberOfColumns() - 1], 10)}");
                }
            }
            catch (Exception ex)
            {
                WriteLineColour(Environment.NewLine + $"\t[!] Error: {ex.Message}", ConsoleColor.Red);
            }
        }

    }
}