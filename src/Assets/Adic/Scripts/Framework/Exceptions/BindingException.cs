using System;

namespace Adic.Exceptions {
	/// <summary>
	/// Binding exception.
	/// </summary>
	public class BindingException : Exception {
		public const string TYPE_NOT_ASSIGNABLE = "The related type is not assignable from the source type.";
		public const string INSTANCE_NOT_ASSIGNABLE = "The instance is not of the given type.";
		public const string FACTORY_TYPE_INCORRECT = "The type and the factory are not the same.";

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.Exceptions.BindingException"/> class.
		/// </summary>
		/// <param name="message">Exception message.</param>
		public BindingException(string message) : base(message) { }
	}
}