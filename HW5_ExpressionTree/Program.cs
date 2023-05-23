using SpreadsheetEngine;
using System;

namespace ExpressionTreeDemo
{
    internal class Program
    {
        private ExpressionTree userExpressionTree;

        /// <summary>
        /// Method to print main menu and ask for user input.
        /// </summary>
        public void Menu()
        {
            bool run = true;
            string userExpression = "A1+B1+C1";
            string userInput;
            string userVariableName;
            string userVariableValue;
            double userVarValue;

            while (run)
            {
                // Print menu to screen.
                Console.WriteLine("Menu (current expression = " + userExpression + ")" + "\n");
                Console.WriteLine("1 - Enter a new expression\n");
                Console.WriteLine("2 - Set a variable value\n");
                Console.WriteLine("3 - Evaluate tree\n");
                Console.WriteLine("4 - Quit\n");

                // Read in user input.
                userInput = Console.ReadLine();

                if (userInput == "1")
                {
                    // Get expression from user and create tree.
                    Console.WriteLine("Enter an expression: \n");
                    userExpression = Console.ReadLine();

                    userExpressionTree = new ExpressionTree(userExpression);
                }
                else if (userInput == "2")
                {
                    // Get varibale name and value from user.
                    Console.WriteLine("Enter a variable name: \n");
                    userVariableName = Console.ReadLine();

                    Console.WriteLine("Enter variable value: \n");
                    userVariableValue = Console.ReadLine();

                    // Variable value has to be a double.
                    userVarValue = Convert.ToDouble(userVariableValue);

                    userExpressionTree.SetVariable(userVariableName, userVarValue);
                }
                else if (userInput == "3")
                {
                    // Evaluate tree.
                    Console.WriteLine(userExpressionTree.Evaluate());
                }
                else if (userInput == "4")
                {
                    // Quit the program.
                    Console.WriteLine("Program exited\n");
                    run = false;
                    Console.ReadLine();
                }
            }
        }
        public static void Main(string[] args)
        {
            Program newProgram = new Program();
            newProgram.Menu();
        }
    }
}
