using Newtonsoft.Json;
using System;
using System.IO;

namespace Zork
{
    public class Game
    {
        public World World { get; set; }

        [JsonIgnore]
        public Player Player { get; private set; }

        [JsonIgnore]
        private bool IsRunning { get; set; }

        [JsonIgnore]
        private CommandManager CommandManager { get; }

        public Game(World world, Player player)
        {
            World = world;
            Player = player;
        }

        public Game()
        {
            Command[] commands =
            {
                new Command("LOOK", new string[] { "LOOK", "L" },
                    (game, commandContext) => Console.WriteLine(game.Player.Location.Description)),

                new Command("QUIT", new string[] { "QUIT", "Q", "EXIT" },
                    (game, commandContext) => game.IsRunning = false),

                new Command("NORTH", new string[] { "NORTH", "N", "UP", "U" }, MovementControls.North),

                new Command("SOUTH", new string[] { "SOUTH", "S", "DOWN", "D" }, MovementControls.South),

                new Command("EAST", new string[] { "EAST", "E", "RIGHT", "R" }, MovementControls.East),

                new Command("WEST", new string[] { "WEST", "W", "LEFT", "L" }, MovementControls.West)
            };

            CommandManager = new CommandManager(commands);
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
                    CommandManager.PerformCommand(this, "LOOK");
                    previousRoom = Player.Location;
                }

                // Ask for input. The > helps convey to the player that they need to input something.
                Console.Write("\n> ");

                if (CommandManager.PerformCommand(this, Console.ReadLine().Trim()))
                {
                    Player.Moves++;
                }
                else
                {
                    // Using \n puts the following text on a new line. The \n is called an escape sequence and there are a number of them in C#.
                    // Check them out on the dcoumentation page here: https://learn.microsoft.com/en-us/cpp/c-language/escape-sequences?view=msvc-170&viewFallbackFrom=vs-2019.
                    Console.WriteLine("I don't recognise that command.\a\nUser...");
                }
            }
        }

        public static Game Load(string filename)
        {
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(filename));
            game.Player = game.World.SpawnPlayer();

            return game;
        }
    }
}