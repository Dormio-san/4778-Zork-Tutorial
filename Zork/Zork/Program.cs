using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Zork
{
    class Program
    {
        private static void Main(string[] args)
        {
            // Set a default location for the rooms.json file that cannot be changed.
            const string defaultGameFilename = "Zork.json";
/*
* To input a custom rooms text file, open your file explorer and find where your script files are located.
* In this area, the Zork.csproj is also there. Once in this area, right click in the blank space and click "Open in Terminal."
* Now, simply type "dotnet run filename" and click enter. For example, I'd type "dotnet run Rooms2.txt" then click enter
*/
            // If the user enters an argument in the command line, use their inputted location for the rooms.json file.
            // If the user didn't input a location, use the default one.
            string gameFilename = (args.Length > 0 ? args[(int)CommandLineArgument.GameFilename] : defaultGameFilename);

            Game game = Game.Load(gameFilename);
            Console.WriteLine($"Welcome to Zork!");
            game.Run();
            Console.WriteLine("Thank you for playing!");
        }

        // Enum for the location of the text file that the user can input.
        // Doing this helps prevent magic numbers.
        private enum CommandLineArgument
        {
            GameFilename = 0
        }
    }
}