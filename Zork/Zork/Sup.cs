using System;

namespace Zork
{
    [CommandClass]
    public static class SupCommand
    {
        [Command("SUP", "SUP")]
        public static void Sup(Game game, CommandContext commandContext) => Console.WriteLine("Sup, dawg.");
    }
}