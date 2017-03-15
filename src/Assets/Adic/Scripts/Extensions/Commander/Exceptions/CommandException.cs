using System;

namespace Adic.Commander.Exceptions {
    /// <summary>
    /// Command exception.
    /// </summary>
    public class CommandException : Exception {
        public const string TYPE_NOT_A_COMMAND = "The type is not a command.";
        public const string MAX_POOL_SIZE = "Reached max pool size for command {0}.";
        public const string NO_COMMAND_FOR_TYPE = "There is no command registered for the type {0}.";

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Commander.Exceptions.CommandException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public CommandException(string message) : base(message) {
        }
    }
}