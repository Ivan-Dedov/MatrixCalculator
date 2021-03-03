using System;

namespace MatrixCalculator
{
    partial class Program
    {

        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            // I'm sorry that sometimes the determinant function returns not
            // what it should due to the double datatype messing up (like -0,9999999991 instead of -1).
            // Can't do anything about it... :[
            // And also, some methods are almost 60 lines, not 40. So, please... try not to be an idiot.
            // 60 lines isn't that much for a single method (because of text output). Don't be a bastard.
            // Thank you.

            bool wantsToExit = false;
            do
            {
                ExecuteCommand(Menu(ref wantsToExit));
            } while (!wantsToExit);
        }

    }
}