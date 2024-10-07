using System;
using Zork;

string[] responses = new string[]
{
    "Good day.",
    "Nice weather we've been having lately.",
    "Nice to see you.",
    "Why hello there.",
    "How ya doin?",
    "Hello, my friend."
};

var command = new Command("HELLO", new string[] { "HELLO", "HI", "HEY", "HOWDY", "YO", "HEYO" },
    (game, commandContext) =>
    {
        string selectedResponse = responses[Game.Random.Next(responses.Length)];
        Console.WriteLine(selectedResponse);
    });

Game.Instance.CommandManager.AddCommand(command);