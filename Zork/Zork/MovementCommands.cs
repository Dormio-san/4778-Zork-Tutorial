using System;

namespace Zork
{
    [CommandClass]
    public static class MovementCommands
    {
        [Command("NORTH", new string[] { "NORTH", "N", "UP", "U" })]
        public static void North(Game game, CommandContext commandContext) => Move(game, Directions.North);

        [Command("SOUTH", new string[] { "SOUTH", "S", "DOWN", "D" })]
        public static void South(Game game, CommandContext commandContext) => Move(game, Directions.South);

        [Command("EAST", new string[] { "EAST", "E", "RIGHT", "R" })]
        public static void East(Game game, CommandContext commandContext) => Move(game, Directions.East);

        [Command("WEST", new string[] { "WEST", "W", "LEFT", "L" })]
        public static void West(Game game, CommandContext commandContext) => Move(game, Directions.West);

        private static void Move(Game game, Directions direction)
        {
            bool playerMoved = game.Player.Move(direction);
            if (!playerMoved)
            {
                Console.WriteLine("The way is shut!");
            }
        }
    }
}