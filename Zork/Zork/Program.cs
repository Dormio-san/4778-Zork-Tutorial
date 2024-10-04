using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            // Variable that has a reference to the Commands enum, and has a default value of UNKNOWN.
            Commands command = Commands.UNKNOWN;

            // While the command is not QUIT, keep asking for, interpreting, and responding to input.
            while (command != Commands.QUIT)
            {
                // Ask for input. The > helps convey to the player that they need to input something.
                Console.WriteLine("> ");

                // Reads the input from the user and removes any leading or trailing white space.
                command = ToCommand(Console.ReadLine().Trim());

                string outputString;

                // Switch statement that checks the command and responds accordingly.
                switch (command)
                {
                    case Commands.LOOK:
                        // If look, print the description of the current location.
                        // Using \n puts the following text on a new line. The \n is called an escape sequence and there are a number of them in C#.
                        // Check them out on the dcoumentation page here: https://learn.microsoft.com/en-us/cpp/c-language/escape-sequences?view=msvc-170&viewFallbackFrom=vs-2019.
                        outputString = "This is an open field west of a white house, with a boarded front door.\nA rubber mat saying \"Welcome to Zork!\" lies by the door.";
                        break;
                    case Commands.NORTH:
                        // If north, print that the player moved north in lowercase.
                        outputString = $"You moved {command.ToString().ToLower()}";
                        break;
                    case Commands.SOUTH:
                        // If south, print that the player moved south in lowercase.
                        outputString = $"You moved {command.ToString().ToLower()}";
                        break;
                    case Commands.EAST:
                        // If east, print that the player moved east in lowercase.
                        outputString = $"You moved {command.ToString().ToLower()}";
                        break;
                    case Commands.WEST:
                        // If west, print that the player moved west in lowercase.
                        outputString = $"You moved {command.ToString().ToLower()}";
                        break;
                    case Commands.QUIT:
                        // If quit, thank the player for playing and exit the game.
                        outputString = "Thank you for playing.\a";
                        break;
                    default:
                        outputString = "Are you serious right neow?\a\nI don't recognize that command.";
                        break;
                }

                // Print the output string.
                Console.WriteLine(outputString);
            }
        }

        // Input a string and try to parse through the commands enum to see if we find a matching command.
        // If we do, return the command. If we don't, return the UNKNOWN command.
        private static Commands ToCommand(string commandString) => Enum.TryParse<Commands>(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
    }
}