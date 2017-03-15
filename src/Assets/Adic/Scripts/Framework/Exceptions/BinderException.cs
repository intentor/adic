using System;

namespace Adic.Exceptions {
    /// <summary>
    /// Binder exception.
    /// </summary>
    public class BinderException : Exception {
        public const string NULL_BINDING = "There is no binding to be bound.";
        public const string BINDING_KEY_ALREADY_EXISTS = "There's already a binding with the same key.";
        public const string BINDING_TO_INTERFACE = "It's not possible to bind a key to an interface.";

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Exceptions.BinderException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public BinderException(string message) : base(message) {
        }
    }
}