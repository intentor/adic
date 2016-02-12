using System;
using System.Collections.Generic;
using Adic.Exceptions;

namespace Adic.Binding {
	/// <summary>
	/// Binds a type to another type or an instance.
	/// </summary>
	public class Binder : IBinder {
		/// <summary>Occurs before a binding is added.</summary>
		public event BindingAddedHandler beforeAddBinding;
		/// <summary>Occurs after a binding is added.</summary>
		public event BindingAddedHandler afterAddBinding;
		/// <summary>Occurs before a binding is removed.</summary>
		public event BindingRemovedHandler beforeRemoveBinding;
		/// <summary>Occurs after a binding is removed.</summary>
		public event BindingRemovedHandler afterRemoveBinding;

		/// <summary>Type bindings of the binder.</summary>
		protected Dictionary<Type, IList<BindingInfo>> typeBindings = new Dictionary<Type, IList<BindingInfo>>();
		
		/// <summary>
		/// Binds a type to another type or instance.
		/// </summary>
		/// <typeparam name="T">The type to bind to.</typeparam>
		/// <returns>The binding.</returns>
		public IBindingFactory Bind<T>() {
			return this.Bind(typeof(T));
		}
		
		/// <summary>
		/// Binds a type to another type or instance.
		/// </summary>
		/// <param name="type">The type to bind to.</param>
		/// <returns>The binding.</returns>
		public IBindingFactory Bind(Type type) {
			return this.BindingFactoryProvider(type);
		}
		
		/// <summary>
		/// Adds a binding.
		/// </summary>
		/// <param name="binding">The binding to be added.</param>
		public void AddBinding(BindingInfo binding) {
			if (binding == null) {
				throw new BinderException(BinderException.NULL_BINDING);
			} else if (binding.value is Type && (binding.value as Type).IsInterface) {
				throw new BinderException(BinderException.BINDING_TO_INTERFACE);
			}
			
			if (this.beforeAddBinding != null) {
				this.beforeAddBinding(this, ref binding);
			}
			
			if (this.typeBindings.ContainsKey(binding.type)) {
				this.typeBindings[binding.type].Add(binding);
			} else {
				var bindingList = new List<BindingInfo>(1);
				bindingList.Add(binding);
				this.typeBindings.Add(binding.type, bindingList);
			}

			if (this.afterAddBinding != null) {
				this.afterAddBinding(this, ref binding);
			}
		}
		
		/// <summary>
		/// Gets all bindings.
		/// </summary>
		/// <returns>Bindings list.</returns>
		public IList<BindingInfo> GetBindings() {
			var bindings = new List<BindingInfo>();
			
			foreach (var binding in this.typeBindings) {
				bindings.AddRange(binding.Value);
			}
			
			return bindings;
		}
		
		/// <summary>
		/// Gets the bindings for a certain <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type to get the bindings from.</typeparam>
		/// <returns>The bindings for the desired type.</returns>
		public IList<BindingInfo> GetBindingsFor<T>() {
			return this.GetBindingsFor(typeof(T));
		}
		
		/// <summary>
		/// Gets the bindings for a certain <param name="type">.
		/// </summary>
		/// <param name="type">The type to get the bindings from.</param>
		/// <returns>The bindings for the desired type.</returns>
		public IList<BindingInfo> GetBindingsFor(Type type) {
			if (this.typeBindings.ContainsKey(type)) {
				return this.typeBindings[type];
			} else {
				return null;
			}
		}

		/// <summary>
		/// Gets the bindings for a given <param name="identifier">.
		/// </summary>
		/// <param name="identifier">The identifier to get the bindings from.</param>
		/// <returns>The bindings for the desired type.</returns>
		public IList<BindingInfo> GetBindingsFor(object identifier) {
			var bindings = new List<BindingInfo>();
			
			foreach (var entry in this.typeBindings) {
				for (var bindingIndex = 0; bindingIndex < entry.Value.Count; bindingIndex++) {
					var binding = entry.Value[bindingIndex];
					
					if (binding.identifier != null && binding.identifier.Equals(identifier)) {
						bindings.Add(binding);
					}
				}
			}
			
			return bindings;
		}

		/// <summary>
		/// Checks whether this binder contains a binding for a given <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="type">The type to be checked.</typeparam>
		/// <returns><c>true</c>, if binding was contained, <c>false</c> otherwise.</returns>
		public bool ContainsBindingFor<T>() {
			return this.ContainsBindingFor(typeof(T));
		}
		
		/// <summary>
		/// Checks whether this binder contains a binding for a given <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The type to be checked.</param>
		/// <returns><c>true</c>, if binding was contained, <c>false</c> otherwise.</returns>
		public bool ContainsBindingFor(Type type) {
			return this.typeBindings.ContainsKey(type);
		}
		
		/// <summary>
		/// Checks whether this binder contains a binding for a given <paramref name="identifier"/>.
		/// </summary>
		/// <param name="type">The identifier to be checked.</param>
		/// <returns><c>true</c>, if binding was contained, <c>false</c> otherwise.</returns>
		public bool ContainsBindingFor(object identifier) {
			var contains = false;

			foreach (var entry in this.typeBindings) {
				for (var bindingIndex = 0; bindingIndex < entry.Value.Count; bindingIndex++) {
					var id = entry.Value[bindingIndex].identifier;

					if (id != null && id.Equals(identifier)) {
						contains = true;
						break;
					}
				}

				if (contains) {
					break;
				}
			}

			return contains;
		}
		
		/// <summary>
		/// Unbinds any bindings to a certain <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type to be unbound.</typeparam>
		public void Unbind<T>() {
			this.Unbind(typeof(T));
		}
		
		/// <summary>
		/// Unbinds any bindings to a certain <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The type to be unbound.</param>
		public void Unbind(Type type) {
			if (!this.ContainsBindingFor(type)) return;

			var bindings = this.GetBindingsFor(type);



			if (this.beforeRemoveBinding != null) {
				this.beforeRemoveBinding(this, type, bindings);
			}

			this.typeBindings.Remove(type);
			
			if (this.afterRemoveBinding != null) {
				this.afterRemoveBinding(this, type, bindings);
			}
		}
		
		/// <summary>
		/// Unbinds any bindings to a certain <paramref name="identifier"/>.
		/// </summary>
		/// <param name="identifier">The identifier to be unbound.</param>
		public void Unbind(object identifier) {
			var bindingsToRemove = new List<BindingInfo>();

			foreach (var entry in this.typeBindings) {
				for (var bindingIndex = 0; bindingIndex < entry.Value.Count; bindingIndex++) {
					var binding = entry.Value[bindingIndex];
					bindingsToRemove.Clear();
					
					if (binding.identifier != null && binding.identifier.Equals(identifier)) {						
						bindingsToRemove.Add(binding);

						if (this.beforeRemoveBinding != null) {
							this.beforeRemoveBinding(this, binding.type, bindingsToRemove);
						}

						entry.Value.RemoveAt(bindingIndex--);						
						
						if (this.afterRemoveBinding != null) {
							this.afterRemoveBinding(this, binding.type, bindingsToRemove);
						}
					}
				}
			}
		}

		/// <summary>
		/// Resolves the binding provider.
		/// </summary>
		/// <param name="type">The type being bound.</param>
		/// <returns>The binding provider.</returns>
		protected virtual IBindingFactory BindingFactoryProvider(Type type) {
			return new BindingFactory(type, this);
		}
	}
}