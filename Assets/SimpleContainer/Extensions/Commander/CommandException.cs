using System;

namespace SimpleContainer.Container
{
    public class CommandException : Exception
    {
        public const string TYPE_NOT_A_COMMAND = "The type is not a command.";
        public const string MAX_POOL_SIZE = "Reached max pool size for command {0}.";
        public const string NO_COMMAND_FOR_TYPE = "no command registered for the type {0}.";

        #region constructor

        public CommandException(string message) : base(message) { }

        #endregion
    }
}