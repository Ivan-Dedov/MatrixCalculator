using System;

namespace MatrixCalculator
{
    partial class Program
    {

        // Operation/command indices.
        const int MIN_OPERATION_INDEX = 0;
        const int MAX_OPERATION_INDEX = 8;

        // Method indeces.
        const int MIN_METHOD_INDEX = 0;
        const int MAX_METHOD_INDEX = 3;

        // The absolute value of the element value that cannot be entered into the matrix.
        const int UNREACHABLE_ELEMENT_VALUE = 10000;

        // The maximum number of rows or columns in the matrix, it is a little bit arbitrary,
        // just for the output's sake. So that it doesn't look like a pile of trash, you know.
        // The size doesn't affect the determinant calculation since I'm using the Gaussian method,
        // unlike some fools who use Cramer, hehe.
        const int MAX_MATRIX_SIZE = 12;

        // Command strings.
        const string CMD_TRACE = "Get the trace of the matrix";
        const string CMD_TRANSPOSE = "Transpose the matrix";
        const string CMD_ADD = "Add two matrices";
        const string CMD_SUBTRACT = "Subtract one matrix from another";
        const string CMD_MULTIPLY_MATRICES = "Multiply two matrices";
        const string CMD_MULTIPLY_BY_SCALAR = "Multiply the matrix by a scalar";
        const string CMD_DETERMINANT = "Get the determinant of the matrix";
        const string CMD_SOLVE_SLE = "Solve a system of equations";
        const string CMD_EXIT = "Exit";
        
        // Matrix input method strings.
        const string MTHD_CONSOLE = "Using the console";
        const string MTHD_FILE = "From a text file";
        const string MTHD_RANDOM_INT = "With random integers from a range";
        const string MTHD_RANDOM_DOUBLE = "With random doubles from a range";

    }
}