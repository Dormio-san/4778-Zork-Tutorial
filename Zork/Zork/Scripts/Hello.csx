using System;
using Zork;

string[] responses = new string[]
{
    "Good day.",
    "Nice weather we've been having lately.",
    "Nice to see you.",
    "Why hello there.",
    "How ya doin?",
    "Hello, my friend.",
    "How's it hangin?",
    "Heyyyy!",
    "What's up?",
    "Hey there!",
    "Hello!",
    "Hi!",
    "Hey!",
    "Howdy!",
    "Yo!",
    "Heyo!"
};

var command = new Command("HELLO", new string[] { "HELLO", "HI", "HEY", "HOWDY", "YO", "HEYO", "HEYA", "WHAT'S UP", "WAZ UP", "WHAT UP" },
    (game, commandContext) =>
    {
        string selectedResponse = responses[Game.Random.Next(responses.Length)];
        Console.WriteLine(selectedResponse);
    });

Game.Instance.CommandManager.AddCommand(command);