using System;
using System.Collections.Generic;

namespace Zork
{
    internal class Program
    {
        // String that holds the name of the current room the player is in.
        // Doing this here helps to make the code more readable and maintainable.
        private static Room currentRoom
        {
            // Using an expression-bodied member to return the name of the current room rather than the { return... }
            // More on them here: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members.
            get => rooms[currentLocation.row, currentLocation.column];
        }

        // Tuple that holds the current location of the player.
        // More on tuples here: https://docs.microsoft.com/en-us/dotnet/csharp/tuples.
        private static (int row, int column) currentLocation = (1, 1);

        private static void Main(string[] args)
        {
            InitializeRoomDescriptions();

            Console.WriteLine($"Welcome to Zork!");

            // The previous room the player was in (used for auto displaying room descriptions).
            Room previousRoom = null;

            // Variable that has a reference to the Commands enum, and has a default value of UNKNOWN.
            Commands command = Commands.UNKNOWN;

            // While the command is not QUIT, keep asking for, interpreting, and responding to input.
            while (command != Commands.QUIT)
            {
                // Indicate to the player where they currently are.
                Console.WriteLine($"Location: {currentRoom}");

                if (previousRoom != currentRoom)
                {
                    // If the player has moved to a new room, print the description of the new room.
                    Console.WriteLine(currentRoom.Description);
                    previousRoom = currentRoom;
                }

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
                        Console.WriteLine(currentRoom.Description);
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        // If the player was not able to move, write the following string.
                        if (!Move(command))
                        {
                            Console.WriteLine("The way is shut!");
                        }
                        else
                        {
                            // They were able to move, so print the direction they moved.
                            Console.WriteLine($"You moved {command.ToString().ToLower()}.");
                        }
                        break;

                    case Commands.HELLO:
                        // If hello, greet the player.
                        Console.WriteLine("Why hello there user.");
                        break;

                    default:
                        Console.WriteLine("Are you serious right neow?\a\nI don't recognise that command.");
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
            Assert.IsTrue(IsDirectionalCommand(command), "That ain't no direction bucko.");

            // Bool used to indicate if the movement was successful.
            bool movementSuccessful = true;

            switch (command)
            {
                case Commands.NORTH when currentLocation.row < rooms.GetLength(0) - 1:
                    // If player isn't at the last row (highest row number), move them north.
                    currentLocation.row++;
                    break;

                case Commands.SOUTH when currentLocation.row > 0:
                    // If player isn't at the last row (lowest row number), move them south.
                    currentLocation.row--;
                    break;

                case Commands.EAST when currentLocation.column < rooms.GetLength(1) - 1:
                    // If player isn't at the last column (highest number column), move them east.
                    currentLocation.column++;
                    break;

                case Commands.WEST when currentLocation.column > 0:
                    // If player isn't at the last column (lowest number column), move them west.
                    currentLocation.column--;
                    break;

                default:
                    // If none of the above conditions are met, the movement was not successful.
                    movementSuccessful = false;
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

        // 2D array that holds the names of the rooms the player can be in.
        private static readonly Room[,] rooms =
        {
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };

        private static void InitializeRoomDescriptions()
        {
            // Using \n puts the following text on a new line. The \n is called an escape sequence and there are a number of them in C#.
            // Check them out on the dcoumentation page here: https://learn.microsoft.com/en-us/cpp/c-language/escape-sequences?view=msvc-170&viewFallbackFrom=vs-2019.

            // Create a dictionary that stores the name of each room and the room itself. The string room name is the key and the room object is the value.
            // Doing this makes it easier to reference the rooms, making the code more readable and maintainable.
            var roomMap = new Dictionary<string, Room>();
            foreach (Room room in rooms)
            {
                // Searches the dictionary for the following key and overwrites the value with the one on the right.
                // If the key is not found, it adds the key and value to the dictionary.
                roomMap[room.Name] = room;

                // Another way to add to the dictionary, but this will throw a System.ArgumentException if the key already exists.
                // Either will work because we are starting with an empty dictionary, but the first way is safer.
                //roomMap.Add(room.Name, room);
            }

            // Assign each room a description.
            roomMap["Rocky Trail"].Description = "You are on a rock-strewn trail.";
            roomMap["South of House"].Description = "You are facing the south side of a white house.\nThere is no door here, and all the windows are barred.";
            roomMap["Canyon View"].Description = "You are at the top of the Great Canyon on its south wall.";
            roomMap["Forest"].Description = "This is a forest, with trees in all directions around you.";
            roomMap["West of House"].Description = "This is an open field west of a white house, with a boarded front door.";
            roomMap["Behind House"].Description = "You are behind the white house.\nIn one corner of the house there is a small window which is slightly ajar.";
            roomMap["Dense Woods"].Description = "This is a dimly lit forest, with large trees all around.\nTo the east, there appears to be sunlight.";
            roomMap["North of House"].Description = "You are facing the north side of a white house.\nThere is no door here, and all the windows are barred.";
            roomMap["Clearing"].Description = "You are in a clearing, with a forest surrounding you on the west and south.";
        }
    }
}