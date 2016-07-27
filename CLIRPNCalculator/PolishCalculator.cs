using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CLIRPNCalculator
{
    public class Functions
    {
        public static decimal Addition(decimal firstValue, decimal secondValue)
        {
            return firstValue + secondValue;
        }

        public static decimal Subtraction(decimal firstValue, decimal secondValue)
        {
            return firstValue - secondValue;
        }

        public static decimal Division(decimal firstValue, decimal secondValue)
        {
            return firstValue / secondValue;
        }

        public static decimal Multiplication(decimal firstValue, decimal secondValue)
        {
            return firstValue * secondValue;
        }
    }

    enum Command
    {
        Clear,
        Quit,
        History,
        Help,
        Version,
        Unknown,
        Empty,
        Equation,
        ClearMemory
    }

    class PolishCalculator
    {
        static Assembly _app;
        static Stack<decimal> _stack;
        static readonly string[] _validOperators = { "+", "/", "-", "*" };
        const string _prompt = "> ";

        static void Main(string[] args)
        {
            _stack = new Stack<decimal>();
            var expression = string.Empty;

            try
            {
                //Get the current application.
                _app = Assembly.GetExecutingAssembly();

                //Greeting
                Console.Title = "Reverse Polish Calculator";
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Calculator ready.  Type '?' for help.");

                //Console loop
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(_prompt);
                    Console.ForegroundColor = ConsoleColor.White;
                    var input = Console.ReadLine().Trim().ToLower();
                    ProcessInput(input);

                    if (_stack.Count > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(_stack.Peek());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Main(args);
            }
        }

        private static void ProcessInput(string input)
        {
            var command = ParseInput(input);

            switch (command)
            {
                case Command.Clear:
                    _stack.Pop();
                    return;
                case Command.ClearMemory:
                    _stack.Clear();
                    Console.Clear();
                    return;
                case Command.Empty:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a value or equation.");
                    return;
                case Command.Version:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("{0} version {1}", _app.GetName().Name, _app.GetName().Version);
                    return;
                case Command.Help:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    var help = System.IO.File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "help.txt"));
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(help);
                    return;
                case Command.Quit:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Environment.Exit(0);
                    return;
                case Command.History:
                    PrintStack(_stack);
                    return;
                case Command.Equation:
                    //Inline equation processing
                    if (input.Length > 1)
                    {
                        string[] expressionTokens = input.Split(' ');

                        foreach (var expressionToken in expressionTokens)
                        {
                            Calculate(expressionToken);
                        }
                    }
                    //One input per line processing
                    else
                        Calculate(input);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} is an unknown command.", input);
                    break;
            }
        }

        private static Command ParseInput(string input)
        {
            var command = Command.Unknown;
            var digit = decimal.Zero;

            if (string.Equals(input, "c"))
                command = Command.Clear;
            else if (string.Equals(input, "ce"))
                command = Command.ClearMemory;
            else if (string.Equals(input, "v"))
                command = Command.Version;
            else if (string.Equals(input, "?"))
                command = Command.Help;
            else if (string.Equals(input, "q"))
                command = Command.Quit;
            else if (string.Equals(input, "h"))
                command = Command.History;
            else if (input.Length == 0)
                command = Command.Empty;
            else if ((decimal.TryParse(input.Split(' ')[0], out digit)) || 
                _validOperators.Any(s =>s.Equals(input, StringComparison.InvariantCultureIgnoreCase)))
                command = Command.Equation;

            return command;
        }

        static void Calculate(string input)
        {
            var digit = decimal.Zero;

            //If the user input is a number then push onto the stack
            if (decimal.TryParse(input, out digit))
            {
                _stack.Push(digit);
            }
            else
            {
                //Operator processing
                if (_stack.Count > 1)
                {
                    switch (input)
                    {
                        case "*":
                                _stack.Push(Functions.Multiplication(_stack.Pop(), _stack.Pop()));
                                break;
                        case "+":
                                _stack.Push(Functions.Addition(_stack.Pop(), _stack.Pop()));
                                break;
                        case "/":
                            {
                                digit = _stack.Pop();
                                try
                                {
                                    _stack.Push(Functions.Division(_stack.Pop(), digit));
                                }
                                catch (DivideByZeroException e)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(e.Message);
                                }

                                break;
                            }
                        case "-":
                                digit = _stack.Pop();
                                _stack.Push(Functions.Subtraction(_stack.Pop(), digit));
                                break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("{0} is not a valid operand or operator.", input);
                            break;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Syntax . An operator must have at least two digits before it.");
                }
            }
        }

        private static void PrintStack(Stack<decimal> stack)
        {
            decimal[] stackArray = stack.ToArray();

            for (int i = stackArray.Length - 1; i >= 0; i--)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("{0,-8:F3}", stackArray[i]);
            }

            Console.WriteLine();
        }
    }

}

