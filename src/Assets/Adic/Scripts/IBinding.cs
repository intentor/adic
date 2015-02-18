using System;

namespace Adic {
	/// <summary>
	/// Defines the bindings of a type.
	/// 
	/// A key can be anything, but is always bound to a type
	/// or instance of a type.
	/// </summary>
	public interface IBinding {
		/// <summary>The key being bound.</summary>
		object key { get; }
		/// <summary>The value of the bind.</summary>
		object value { get; set; }
		/// <summary>The binding type.</summary>
		BindingType bindingType { get; }

		/// <summary>
		/// Binds the key to a singleton. The key must be a type.
		/// </summary>
		void AsSingleton();
		
		/// <summary>
		/// Binds the key to a singleton of type <typeparamref name="T">.
		/// </summary>
		/// <typeparam name="T">The related type.</typeparam>
		void AsSingleton<T>() where T : class;

		/// <summary>
		/// Binds the key to a type <typeparamref name="T">.
		/// </summary>
		/// <typeparam name="T">The type to bind to.</typeparam>
		void To<T>() where T : class;
		
		// <summary>
		/// Binds the key to <paramref name="instance"/>.
		/// </summary>
		/// <typeparam name="T">The related type.</typeparam>
		/// <param name="instance">The instance to bind the type to.</param>
		void To<T>(T instance);

		/// <summary>
		/// Binds the key to a <paramref name="factory"/>.
		/// </summary>
		/// <param name="factory">Factory to be bound to.</param>
		/// <returns>The created binding.</returns>
		void ToFactory(IFactory factory);
	}
}