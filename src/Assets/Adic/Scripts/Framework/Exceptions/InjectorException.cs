using System;

namespace Adic.Exceptions {
    /// <summary>
    /// Injector exception.
    /// </summary>
    public class InjectorException : Exception {
        public const string NO_CONSTRUCTORS = "There are no constructors on the type \"{0}\".";
        public const string CANNOT_INSTANTIATE_INTERFACE = "Interface \"{0}\" cannot be instantiated.";

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Exceptions.InjectorException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public InjectorException(string message) : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Exceptions.InjectorException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="cause">Exception that caused this exception to throw.</param>
        public InjectorException(string message, Exception cause) : base(message, cause) {
        }
    }
}