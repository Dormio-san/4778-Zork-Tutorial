using Newtonsoft.Json;
using System;
using System.IO;

namespace Zork
{
    public class Game
    {
        public World World { get; private set; }

        [JsonIgnore]
        public Player Player { get; private set; }

        [JsonIgnore]
        private bool IsRunning { get; set; }

        public Game(World world, Player player)
        {
            World = world;
            Player = player;
        }

        public void Run()
        {
            IsRunning = true;
            Room previousRoom = null;
            while (IsRunning)
            {
                Console.WriteLine(Player.Location);
                if (previousRoom != Player.Location)
                {
                    // If the player has moved to a new room, print the description of the new room.
                    Console.WriteLine(Player.Location.Description);
                    previousRoom = Player.Location;
                }

                // Ask for input. The > helps convey to the player that they need to input something.
                Console.Write("\n> ");

                // Reads the input from the user and removes any leading or trailing white space.
                Commands command = ToCommand(Console.ReadLine().Trim());

                // Switch statement that checks the command and responds accordingly.
                switch (command)
                {
                    case Commands.QUIT:
                        // If quit, thank the player for playing and exit the game.
                        //Console.WriteLine("Thank you for playing!\a");
                        IsRunning = false;
                        break;

                    case Commands.LOOK:
                        // If look, print the description of the current location.
                        Console.WriteLine(Player.Location.Description);
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        // If the player was not able to move, write the following string.
                        Directions direction = Enum.Parse<Directions>(command.ToString(), true);
                        if (!Player.Move(direction))
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
                        // Using \n puts the following text on a new line. The \n is called an escape sequence and there are a number of them in C#.
                        // Check them out on the dcoumentation page here: https://learn.microsoft.com/en-us/cpp/c-language/escape-sequences?view=msvc-170&viewFallbackFrom=vs-2019.
                        Console.WriteLine("I don't recognise that command.\a\nUser...");
                        break;
                }
            }
        }

        public static Game Load(string filename)
        {
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(filename));
            game.Player = game.World.SpawnPlayer();

            return game;
        }

        // Input a string and try to parse through the commands enum to see if we find a matching command.
        // If we do, return the command. If we don't, return the UNKNOWN command.
        private static Commands ToCommand(string commandString) =>
            Enum.TryParse<Commands>(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
    }
}