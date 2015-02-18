using System;

namespace Adic {
	/// <summary>
	/// Binding exception.
	/// </summary>
	public class BindingException : Exception {
		public const string KEY_IS_NOT_TYPE = "The key cannot be resolved to a singleton because it's not a type.";
		public const string TYPE_NOT_ASSIGNABLE = "Typeparam T is not assignable from key.";
		public const string SINGLETON_MONOBEHAVIOUR = "A MonoBehaviour cannot be made singleton directly.";
		public const string FACTORY_TYPE_INCORRECT = "The type of the key and the factory are not the same.";

		/// <summary>
		/// Initializes a new instance of the <see cref="IoC.BindingException"/> class.
		/// </summary>
		/// <param name="message">Exception message.</param>
		public BindingException(string message) : base(message) { }
	}
}