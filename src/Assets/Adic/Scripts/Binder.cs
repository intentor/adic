using System;
using System.Collections.Generic;

namespace Adic {
	/// <summary>
	/// Binds a string key or a type to a another type or an instance.
	/// </summary>
	public class Binder : IBinder {
		/// <summary>Bindings of the binder.</summary>
		protected Dictionary<object, IBinding> bindings = new Dictionary<object, IBinding>();

		/// <summary>
		/// Binds a type.
		/// </summary>
		/// <typeparam name="T">The type to bind to.</typeparam>
		/// <returns>The binding.</returns>
		public IBinding Bind<T>() {
			return this.BindingProvider(typeof(T));
		}
		
		/// <summary>
		/// Binds a key name to a type or instance.
		/// </summary>
		/// <param name="name">The key name.</param>
		/// <returns>The binding.</returns>
		public IBinding Bind(string name) {
			return this.BindingProvider(name);
		}

		/// <summary>
		/// Binds a type to a mapper using a binding.
		/// </summary>
		/// <param name="binding">The binding representation.</param>
		public virtual void Bind(IBinding binding) {
			if (binding == null) {
				throw new BinderException(BinderException.NULL_BINDING);
			} else if (this.bindings.ContainsKey(binding.key)) {
				throw new BinderException(BinderException.BINDING_KEY_ALREADY_EXISTS);
			} else if (binding.value is Type && (binding.value as Type).IsInterface) {
				throw new BinderException(BinderException.BINDING_TO_INTERFACE);
			}
		
			this.bindings.Add(binding.key, binding);
		}
		
		/// <summary>
		/// Gets the binding of a certain type.
		/// </summary>
		/// <typeparam name="T">The type to get the binding.</typeparam>
		/// <returns>The binding.</returns>
		public IBinding GetBinding<T>() {
			return this.GetBinding(typeof(T));
		}
		
		/// <summary>
		/// Gets the binding of a certain name.
		/// </summary>
		/// <param name="key">Key to be checked.</param>
		/// <returns>The binding.</returns>
		public IBinding GetBinding(object key) {
			if (this.bindings.ContainsKey(key)) {
				return this.bindings[key];
			} else {
				return null;
			}
		}
		
		/// <summary>
		/// Gets all bindings.
		/// </summary>
		/// <returns>Bindings list.</returns>
		public IBinding[] GetBindings() {
			var bindings = new IBinding[this.bindings.Count];

			var bindingIndex = 0;
			foreach (var entry in this.bindings) {
				bindings[bindingIndex++] = entry.Value;
			}

			return bindings;
		}
		
		/// <summary>
		/// Unbinds any bindings to a certain <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type to unbind.</typeparam>
		public void Unbind<T>() {
			this.bindings.Remove(typeof(T));
		}
		
		/// <summary>
		/// Unbinds any bindings with the name <paramref name="name"/>
		/// </summary>
		/// <param name="name">Name of the binding.</param>
		public void Unbind(string name) {
			this.bindings.Remove(name);
		}
		
		/// <summary>
		/// Resolves the binding provider.
		/// </summary>
		/// <param name="key">The key being bound.</param>
		/// <returns>The binding provider.</returns>
		protected virtual IBinding BindingProvider(object key) {
			return new Binding(key, this);
		}
	}
}