using System;

namespace Adic {
	/// <summary>
	/// Injector exception.
	/// </summary>
	public class InjectorException : Exception {
		public const string NO_CONSTRUCTORS = "There are no constructors on the type {0}.";
				
		/// <summary>
		/// Initializes a new instance of the <see cref="IoC.InjectorException"/> class.
		/// </summary>
		/// <param name="message">Exception message.</param>
		public InjectorException(string message) : base(message) { }
	}
}