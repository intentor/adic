using System;
using System.Collections.Generic;

namespace Adic.Binding {
	/// <summary>
	/// Defines a binder that binds a type to another type or instance.
	/// </summary>
	public interface IBinder {
		/// <summary>Occurs before a binding is added.</summary>
		event BindingAddedHandler beforeAddBinding;
		/// <summary>Occurs after a binding is added.</summary>
		event BindingAddedHandler afterAddBinding;
		/// <summary>Occurs before a binding is removed.</summary>
		event BindingRemovedHandler beforeRemoveBinding;
		/// <summary>Occurs after a binding is removed.</summary>
		event BindingRemovedHandler afterRemoveBinding;

		/// <summary>
		/// Binds a type to another type or instance.
		/// </summary>
		/// <typeparam name="T">The type to bind to.</typeparam>
		/// <returns>The binding.</returns>
		IBindingFactory Bind<T>();

		/// <summary>
		/// Binds a type to another type or instance.
		/// </summary>
		/// <param name="type">The type to bind to.</param>
		/// <returns>The binding.</returns>
		IBindingFactory Bind(Type type);
		
		/// <summary>
		/// Adds a binding.
		/// </summary>
		/// <param name="binding">The binding to be added.</param>
		void AddBinding(BindingInfo binding);
		
		/// <summary>
		/// Gets all bindings.
		/// </summary>
		/// <returns>Bindings list.</returns>
		IList<BindingInfo> GetBindings();
		
		/// <summary>
		/// Gets the bindings for a certain <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type to get the bindings from.</typeparam>
		/// <returns>The bindings for the desired type.</returns>
		IList<BindingInfo> GetBindingsFor<T>();
		
		/// <summary>
		/// Gets the bindings for a certain <param name="type">.
		/// </summary>
		/// <param name="type">The type to get the bindings from.</param>
		/// <returns>The bindings for the desired type.</returns>
		IList<BindingInfo> GetBindingsFor(Type type);
		
		/// <summary>
		/// Unbinds any bindings to a certain <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type to be unbound.</typeparam>
		void Unbind<T>();
		
		/// <summary>
		/// Unbinds any bindings to a certain <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The type to be unbound.</param>
		void Unbind(Type type);
	}
}