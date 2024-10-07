using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Diagnostics.Contracts;

namespace Zork
{
    public class Game
    {
        [JsonIgnore]
        public static Game Instance { get; private set; }

        public World World { get; set; }

        [JsonIgnore]
        public Player Player { get; private set; }

        [JsonIgnore]
        public CommandManager CommandManager { get; }

        [JsonIgnore]
        public bool IsRunning { get; }

        private bool mIsRunning;
        private bool mIsRestarting;

        public Game() => CommandManager = new CommandManager();

        public Game(World world, Player player)
        {
            World = world;
            Player = player;
        }

        public static void Start(string gameFilename)
        {
            if (!File.Exists(gameFilename))
            {
                throw new FileNotFoundException("Expected the game file to exist.", gameFilename);
            }

            while (Instance == null || Instance.mIsRestarting)
            {
                Instance = Load(gameFilename);
                Instance.LoadCommands();
                Instance.LoadScripts();
                Instance.DisplayWelcomeMessage();
                Instance.Run();
            }
        }

        public void Run()
        {
            mIsRunning = true;
            Room previousRoom = null;
            while (mIsRunning)
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

        public void Restart()
        {
            mIsRunning = false;
            mIsRestarting = true;
            Console.Clear();
        }

        public void Quit() => mIsRunning = false;

        public static Game Load(string filename)
        {
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(filename));
            game.Player = game.World.SpawnPlayer();

            return game;
        }

        private void LoadCommands()
        {
            var commandMethods = (from type in Assembly.GetExecutingAssembly().GetTypes()
                                  from method in type.GetMethods()
                                  let attribute = method.GetCustomAttribute<CommandAttribute>()
                                  where type.IsClass && type.GetCustomAttribute<CommandClassAttribute>() != null
                                  where attribute != null
                                  select new Command(attribute.CommandName, attribute.Verbs,
                                  (Action<Game, CommandContext>)Delegate.CreateDelegate(typeof(Action<Game, CommandContext>), method)));

            CommandManager.AddCommands(commandMethods);
        }

        // Directory and file extension for the scripts used in the load scripts method.
        private static readonly string ScriptDirectory = "Scripts";
        private const string ScriptFileExtension = "*.csx";

        private void LoadScripts()
        {
            foreach (string file in Directory.EnumerateFiles(ScriptDirectory, ScriptFileExtension))
            {
                try
                {
                    var scriptOptions = ScriptOptions.Default.AddReferences(Assembly.GetExecutingAssembly());
#if (DEBUG)
                    scriptOptions = scriptOptions.WithEmitDebugInformation(true)
                        .WithFilePath(new FileInfo(file).FullName)
                        .WithFileEncoding(Encoding.UTF8);
#endif
                    string script = File.ReadAllText(file);
                    CSharpScript.RunAsync(script, scriptOptions).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error compiling script file: {file}\nError: {ex.Message}");
                }
            }
        }

        public bool ConfirmAction(string prompt)
        {
            Console.Write(prompt);

            while (true)
            {
                string response = Console.ReadLine().Trim().ToUpper();
                if (response == "Y" || response == "YES")
                {
                    return true;
                }
                else if (response == "N" || response == "NO")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Please answer with 'Y' or 'N'.");
                }
            }
        }

        private void DisplayWelcomeMessage() => Console.WriteLine(WelcomeMessage);

        public static readonly Random Random = new Random();

        [JsonProperty]
        private string WelcomeMessage = null;
    }
}