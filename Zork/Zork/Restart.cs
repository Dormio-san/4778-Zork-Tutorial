using System;

namespace Zork
{
    [CommandClass]
    public static class RestartCommand
    {
        [Command("RESTART", new string[] { "RESTART", "R", "RESET" })]
        public static void Restart(Game game, CommandContext commandContext)
        {
            if (game.ConfirmAction("Are you sure you want to restart? "))
            {
                game.Restart();
            }
        }
    }
}