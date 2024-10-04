using System;
using System.Collections.Generic;

namespace Zork
{
    class Program
    {
        private static string[] rooms =
        {
            "Forest",
            "West of House",
            "Behind House",
            "Clearing",
            "Canyon View"
        };

        private static int currentRoom = 1;

        static void Main(string[] args)
        {
            Console.WriteLine($"Welcome to Zork!\nYou are {rooms[currentRoom]}.");

            // Variable that has a reference to the Commands enum, and has a default value of UNKNOWN.
            Commands command = Commands.UNKNOWN;

            // While the command is not QUIT, keep asking for, interpreting, and responding to input.
            while (command != Commands.QUIT)
            {
                // Ask for input. The > helps convey to the player that they need to input something.
                Console.Write("> ");

                // Reads the input from the user and removes any leading or trailing white space.
                command = ToCommand(Console.ReadLine().Trim());

                // Switch statement that checks the command and responds accordingly.
                switch (command)
                {
                    case Commands.QUIT:
                        // If quit, thank the player for playing and exit the game.
                        Console.WriteLine("Thank you for playing!\a");
                        break;
                    case Commands.LOOK:
                        // If look, print the description of the current location.
                        // Using \n puts the following text on a new line. The \n is called an escape sequence and there are a number of them in C#.
                        // Check them out on the dcoumentation page here: https://learn.microsoft.com/en-us/cpp/c-language/escape-sequences?view=msvc-170&viewFallbackFrom=vs-2019.
                        Console.WriteLine("This is an open field west of a white house, with a boarded front door." +
                            "\nA rubber mat saying \"Welcome to Zork!\" lies by the door.");
                        break;
                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        // Print the direction that the player moved, in lowercase. 
                        if(!Move(command))
                        {
                            Console.WriteLine("The way is shut!");
                            Console.WriteLine($"Location: {rooms[currentRoom]}");
                        }
                        break;
                    default:
                        Console.WriteLine("Are you serious right neow?\a\nI don't recognize that command.");
                        break;
                }
            }
        }

        // Input a string and try to parse through the commands enum to see if we find a matching command.
        // If we do, return the command. If we don't, return the UNKNOWN command.
        private static Commands ToCommand(string commandString) => 
            Enum.TryParse<Commands>(commandString, true, out Commands result) ? result : Commands.UNKNOWN;

        private static bool Move(Commands command)
        {
            // If the entered command is not a directional command, print an error message and return false.
            if (!IsDirectionalCommand(command))
            {
                Console.WriteLine("That ain't no direction bucko.");
                return false;
            }

            // Bool used to indicate if the movement was successful.
            bool movementSuccessful = false;

            switch (command)
            {
                // If north or south, not implemented yet, so return false.
                case Commands.NORTH:
                case Commands.SOUTH:
                    movementSuccessful = false;
                    break;
                case Commands.EAST:
                    if (currentRoom == rooms.Length - 1)
                    {
                        // If the player is at the last room, they can't move more east, so return false.
                        movementSuccessful = false;
                        break;
                    }
                    else
                    {
                        // If player isn't at the last room, move them east, print the direction they moved
                        // and what room they moved to, and return true.
                        currentRoom = currentRoom + 1;
                        Console.WriteLine($"You moved {command.ToString().ToLower()} to {rooms[currentRoom]}.");
                        movementSuccessful = true;
                        break;
                    }
                case Commands.WEST:
                    if (currentRoom == 0)
                    {
                        // If the player is at the last room, they can't move more west, so return false.
                        movementSuccessful = false;
                        break;
                    }
                    else
                    {
                        // If player isn't at the last room, move them west, print the direction they moved
                        // and what room they moved to, and return true.
                        currentRoom = currentRoom - 1;
                        Console.WriteLine($"You moved {command.ToString().ToLower()} to {rooms[currentRoom]}.");
                        movementSuccessful = true;
                        break;
                    }
                default:
                    Console.WriteLine("Are you serious right neow?\a\nI don't recognize that command.");
                    break;
            }
            // Return whether or not the move was successful.
            return movementSuccessful;
        }

        // Check if the inputted command is a direction.
        private static bool IsDirectionalCommand(Commands command) => PossibleDirections.Contains(command);

        // The only possible directions the player can move. It is set to readonly so that it cannot be changed.
        private static readonly List<Commands> PossibleDirections = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };
    }
}