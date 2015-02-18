using System;

namespace Adic {
	/// <summary>
	/// Defines a binder that binds a string key or a type to a another type or an instance.
	/// </summary>
	public interface IBinder {
		/// <summary>
		/// Binds a type to another type or instance.
		/// </summary>
		/// <typeparam name="T">The type to bind to.</typeparam>
		/// <returns>The binding.</returns>
		IBinding Bind<T>();
		
		/// <summary>
		/// Binds a key name to a type or instance.
		/// </summary>
		/// <param name="name">The key name.</param>
		/// <returns>The binding.</returns>
		IBinding Bind(string name);

		/// <summary>
		/// Binds a type to a mapper using a binding.
		/// </summary>
		/// <param name="binding">The binding representation.</param>
		void Bind(IBinding binding);

		/// <summary>
		/// Gets the binding of a certain type.
		/// </summary>
		/// <typeparam name="T">The type to get the binding.</typeparam>
		/// <returns>The binding.</returns>
		IBinding GetBinding<T>();
		
		/// <summary>
		/// Gets the binding of a certain name.
		/// </summary>
		/// <param name="key">Key to be checked.</param>
		/// <returns>The binding.</returns>
		IBinding GetBinding(object key);
		
		/// <summary>
		/// Gets all bindings.
		/// </summary>
		/// <returns>Bindings list.</returns>
		IBinding[] GetBindings();

		/// <summary>
		/// Unbinds any bindings to a certain <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type to unbind.</typeparam>
		void Unbind<T>();
		
		/// <summary>
		/// Unbinds any bindings with the name <paramref name="name"/>
		/// </summary>
		/// <param name="name">Name of the binding.</param>
		void Unbind(string name);
	}
}