using System;

namespace MatrixCalculator
{
    class MatricesOfIncompatibleSizesException : Exception
    {
        public MatricesOfIncompatibleSizesException() { }

        public MatricesOfIncompatibleSizesException(string message) : base(message) { }

        public MatricesOfIncompatibleSizesException(string message, Exception inner) : base(message, inner) { }
    }
}