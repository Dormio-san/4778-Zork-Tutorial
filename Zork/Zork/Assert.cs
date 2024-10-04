using System;
using System.Diagnostics;

namespace Zork
{
    public static class Assert
    {
        // Takes in a condition and an optional message. If the condition is false, throw an exception with the message.
        // The Conditional attribute is used to tell the compiler to only include this method in the build if the DEBUG symbol is defined.
        // By default, the DEBUG symbol is defined in debug builds and not defined in release builds.
        // Therefore, this code will only run in debug builds and will not be included in release builds, helping to improve performance in the release build.
        [Conditional("DEBUG")]
        public static void IsTrue(bool expression, string message = "")
        {
            if (!expression)
            {
                throw new Exception(message);
            }
        }
    }
}
