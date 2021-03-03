using System;

namespace MatrixCalculator
{
    partial class Program
    {

        /// <summary>
        /// Selects the operation to be performed.
        /// </summary>
        /// <param name="wantsToExit">Boolean variable for detecting if the user wants to exit the program.</param>
        /// <returns>The index of the operation to be performed.</returns>
        static int Menu(ref bool wantsToExit)
        {
            int selectedOperation = 0;
            ConsoleKey keyPressed;
            do
            {
                // Outputting the visual clues.
                Console.Clear();
                Console.Write(Environment.NewLine + "\tWelcome to the ");
                WriteColour("Matrix Calculator", ConsoleColor.Green);
                Console.WriteLine("!" + Environment.NewLine);
                Console.WriteLine("\tSelect one of the following operations to be performed.");
                WriteColour("\t(i)", ConsoleColor.Yellow);
                Console.Write(" Use the ");
                WriteColour("UP", ConsoleColor.Cyan);
                Console.Write(" and ");
                WriteColour("DOWN", ConsoleColor.Cyan);
                Console.Write(" arrows to select the operation, press ");
                WriteColour("ENTER", ConsoleColor.Cyan);
                Console.WriteLine(" to confirm." + Environment.NewLine);
                HighlightOperation(selectedOperation, ConsoleColor.Yellow);

                keyPressed = Console.ReadKey().Key;
                // Moving up if the key pressed is the UP ARROW.
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedOperation = --selectedOperation < MIN_OPERATION_INDEX ? MAX_OPERATION_INDEX : selectedOperation;
                }
                // Moving down if the key pressed is the DOWN ARROW.
                if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedOperation = ++selectedOperation > MAX_OPERATION_INDEX ? MIN_OPERATION_INDEX : selectedOperation;
                }
                // Selecting the operation until pressing ENTER.
            } while (keyPressed != ConsoleKey.Enter);

            // If the operation selected is the last one, exit.
            if (selectedOperation == MAX_OPERATION_INDEX)
            {
                wantsToExit = true;
            }

            return selectedOperation;
        }

        /// <summary>
        /// Executes the selected command.
        /// </summary>
        /// <param name="command">The operation index of the command to be executed.</param>
        static void ExecuteCommand(int commandIndex)
        {
            Console.Clear();
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
            // Contains the name of the command to be executed (as a string).
            string executedCommand = operations[commandIndex];
            WriteLineColour(Environment.NewLine + "\t" + executedCommand.ToUpper(), ConsoleColor.Green);

            switch (executedCommand)
            {
                case CMD_TRACE:
                    GetMatrixTrace();
                    break;
                case CMD_TRANSPOSE:
                    TransposeMatrix();
                    break;
                case CMD_ADD:
                    AddMatrices();
                    break;
                case CMD_SUBTRACT:
                    SubtractMatrices();
                    break;
                case CMD_MULTIPLY_MATRICES:
                    MultiplyMatrices();
                    break;
                case CMD_MULTIPLY_BY_SCALAR:
                    MultiplyByScalar();
                    break;
                case CMD_DETERMINANT:
                    GetMatrixDeterminant();
                    break;
                case CMD_SOLVE_SLE:
                    SolveSystemOfEquations();
                    break;
                case CMD_EXIT:
                    return;
            }

            WriteColour("\t[>] ", ConsoleColor.Yellow);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

    }
}