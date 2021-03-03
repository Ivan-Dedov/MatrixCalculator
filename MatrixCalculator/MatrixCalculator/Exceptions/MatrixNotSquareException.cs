using System;

namespace MatrixCalculator
{
    class MatrixNotSquareException : Exception
    {
        public MatrixNotSquareException() { }

        public MatrixNotSquareException(string message) : base(message) { }

        public MatrixNotSquareException(string message, Exception inner) : base(message, inner) { }
    }
}