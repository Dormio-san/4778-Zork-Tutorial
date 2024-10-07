using System;

namespace Zork
{
    [CommandClass]
    public static class QuitCommand
    {
        [Command("QUIT", new string[] { "QUIT", "Q", "EXIT", "GOODBYE", "BYE", "SO LONG" })]
        public static void Quit(Game game, CommandContext commandContext)
        {
            if (game.ConfirmAction("Are you sure you want to quit? "))
            {
                game.Quit();
            }
        }
    }
}