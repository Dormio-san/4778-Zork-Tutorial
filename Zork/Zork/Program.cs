using System;
using System.Collections.Generic;
using System.IO;

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

        // Create a dictionary that stores the name of each room and the room itself. The string room name is the key and the room object is the value.
        // Doing this makes it easier to reference the rooms, making the code more readable and maintainable.
        private static readonly Dictionary<string, Room> roomMap;

        // Static constructor that runs before the Main method, ensuring that any code that needs the dictoinary gets it once it has already been populated.
        static Program()
        {            
            roomMap = new Dictionary<string, Room>();
            foreach (Room room in rooms)
            {
                // Searches the dictionary for the following key and overwrites the value with the one on the right.
                // If the key is not found, it adds the key and value to the dictionary.
                roomMap[room.Name] = room;

                // Another way to add to the dictionary, but this will throw a System.ArgumentException if the key already exists.
                // Either will work because we are starting with an empty dictionary, but the first way is safer.
                //roomMap.Add(room.Name, room);
            }
        }

        // Tuple that holds the current location of the player.
        // More on tuples here: https://docs.microsoft.com/en-us/dotnet/csharp/tuples.
        private static (int row, int column) currentLocation = (1, 1);

        private static void Main(string[] args)
        {
            // Set a default location for the rooms.txt file that cannot be changed.
            const string defaultRoomsFilename = "Rooms.txt";
/*
* To input a custom rooms text file, open your file explorer and find where your script files are located.
* In this area, the Zork.csproj is also there. Once in this area, right click in the blank space and click "Open in Terminal."
* Now, simply type "dotnet run filename" and click enter. For example, I'd type "dotnet run Rooms2.txt" then click enter
*/
            // If the user enters an argument in the command line, use their inputted location for the rooms.txt file.
            // If the user didn't input a location, use the default one.
            string roomsFilename = (args.Length > 0 ? args[(int)CommandLineArgument.RoomsFilename] : defaultRoomsFilename);
            InitializeRoomDescriptions(roomsFilename);

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

                    case Commands.BREAKIN:
                        Console.WriteLine("You tried to break in but failed.");
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
            { new Room("Forest"), new Room("West of House"), new Room("Behind House"), },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };

        private static void InitializeRoomDescriptions(string roomsFilename)
        {
            // Const variables that will not change for the delimiter and the expected number of fields.
            const string fieldDelimiter = "##";
            const int expectedFieldCount = 2;

            // A way to do what is below except using LINQ.
            var roomQuery = from line in File.ReadLines(roomsFilename)
                            let fields = line.Split(fieldDelimiter)
                            where fields.Length == expectedFieldCount
                            select (Name: fields[(int)Fields.Name],
                                    Description: fields[(int)Fields.Description]);

            // Assigns the description to the corresponding name (key) in the dictionary.
            foreach (var (Name, Description) in roomQuery)
            {
                roomMap[Name].Description = Description;
            }


            //// Reads all of the lines in the text file and stores them in an array of strings.
            //string[] lines = File.ReadAllLines(roomsFilename);

            //// Iterates through the array and does things for each line.
            //foreach (string line in lines)
            //{
            //    // Splits the line into an array of strings based on the field delimiter. In this case, array index 0 is the name and array index 1 is the description.
            //    string[] fields = line.Split(fieldDelimiter);

            //    // This ensures that the line has the expected number of fields. If it doesn't, it throws an exception.
            //    if (fields.Length != expectedFieldCount)
            //    {
            //        throw new InvalidDataException($"Invalid record: {line}");
            //    }

            //    string name = fields[(int)Fields.Name];
            //    string description = fields[(int)Fields.Description];

            //    roomMap[name].Description = description;
            //}

            // Using \n puts the following text on a new line. The \n is called an escape sequence and there are a number of them in C#.
            // Check them out on the dcoumentation page here: https://learn.microsoft.com/en-us/cpp/c-language/escape-sequences?view=msvc-170&viewFallbackFrom=vs-2019.
        }

        // Create this enum to make the code more readable and easier to modify if changes occur.
        private enum Fields
        {
            Name = 0,
            Description
        }

        // Enum for the location of the text file that the user can input.
        // Doing this helps prevent magic numbers.
        private enum CommandLineArgument
        {
            RoomsFilename = 0
        }
    }
}