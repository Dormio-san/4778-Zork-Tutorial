using System;

namespace Zork
{
    [CommandClass]
    public static class Secret
    {
        [Command("SECRET", new string[] {"SECRET", "SHHH", "FORBIDDEN", "UNKNOWN"})]
        public static void SecretCommand(Game game, CommandContext commandContext) => Console.WriteLine("The secret is yours!");
    }
}