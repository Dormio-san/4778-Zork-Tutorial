using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            Commands command = Commands.UNKNOWN;

            while (command != Commands.QUIT)
            {
                Console.WriteLine("> ");
                command = ToCommand(Console.ReadLine().Trim());
            }

            // Reads the input from the user and removes any leading or trailing white space.
            // Additionally, it converts the whole string to uppercase. All uppercase prevents any issues with case sensitivity.
            // You can also use ToLower() to convert the string to lowercase.
            //string inputString = Console.ReadLine();
            //Commands command = ToCommand(inputString.Trim());
            //Console.WriteLine(command);

            //if (inputString == "QUIT")
            //{
            //    // If quit, thank the player for playing and exit the game.
            //    Console.WriteLine("Thank you for playing.\a");
            //}
            //else if (inputString == "LOOK")
            //{
            //    // If look, print the description of the current location.
            //    // Using \n puts the following text on a new line. The \n is called an escape sequence and there are a number of them in C#.
            //    // Check them out on the dcoumentation page here: https://learn.microsoft.com/en-us/cpp/c-language/escape-sequences?view=msvc-170&viewFallbackFrom=vs-2019.
            //    Console.WriteLine("This is an open field west of a white house, with a boarded front door.\nA rubber mat saying \"Welcome to Zork!\" lies by the door.");
            //}
            //else
            //{
            //    Console.WriteLine("Unrecognized command.");
            //}
        }

        // Input a string, try to parse through the commands enum to see if we find a matching command.
        // If we do, return the command. If we don't, return the UNKNOWN command.
        private static Commands ToCommand(string commandString) => Enum.TryParse<Commands>(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
    }
}