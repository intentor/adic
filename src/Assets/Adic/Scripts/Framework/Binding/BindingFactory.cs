using System;
using Adic.Exceptions;
using Adic.Util;

namespace Adic.Binding {
	//// <summary>
	/// Binding types to another types or instances.
	/// </summary>
	public class BindingFactory : IBindingFactory {
		//// <summary>Binder used by the Binding Factory.</summary>
		public IBinder binder { get; private set; }
		//// <summary>The type being bound.</summary>
		public Type bindingType { get; private set; }
		
		//// <summary>
		/// Initializes a new instance of the <see cref="Adic.BindingFactory"/> class.
		/// </summary>
		/// <param name="bindingType">The type being bound.</param>
		/// <param name="binder">The binder that will bind this binding.</param>
		public BindingFactory(Type bindingType, IBinder binder) {
			this.bindingType = bindingType;
			this.binder = binder;
		}
		
		//// <summary>
		/// Binds the key type to a transient of itself. The key must be a class.
		/// </summary>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory ToSelf() {
			return this.AddBinding(this.bindingType, BindingInstance.Transient);
		}
		
		//// <summary>
		/// Binds the key type to a singleton of itself. The key must be a class.
		/// </summary>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory ToSingleton() {
			return this.AddBinding(this.bindingType, BindingInstance.Singleton);
		}
		
		//// <summary>
		/// Binds the key type to a singleton of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The related type.</typeparam>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory ToSingleton<T>() where T : class {
			return this.ToSingleton(typeof(T));
		}
		
		//// <summary>
		/// Binds the key type to a singleton of type <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The related type.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory ToSingleton(Type type) {
			if (!TypeUtils.IsAssignable(this.bindingType, type)) {
				throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
			}
			
			return this.AddBinding(type, BindingInstance.Singleton);
		}
		
		//// <summary>
		/// Binds the key type to a transient of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type to bind to.</typeparam>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory To<T>() where T : class {
			return this.To(typeof(T));
		}
		
		//// <summary>
		/// Binds the key type to a transient of type <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The related type.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory To(Type type) {
			if (!TypeUtils.IsAssignable(this.bindingType, type)) {
				throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
			}

			return this.AddBinding(type, BindingInstance.Transient);
		}
		
		//// <summary>
		/// Binds the key type to an <paramref name="instance"/>.
		/// </summary>
		/// <typeparam name="T">The related type.</typeparam>
		/// <param name="instance">The instance to bind the type to.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory To<T>(T instance) {
			return this.To(typeof(T), instance);
		}

		//// <summary>
		/// Binds the key type to an <paramref name="instance"/>.
		/// </summary>
		/// <param name="type">The related type.</typeparam>
		/// <param name="instance">The instance to bind the type to.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory To(Type type, object instance) {
			if (!TypeUtils.IsAssignable(this.bindingType, type)) {
				throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
			} else if (!TypeUtils.IsAssignable(type, instance.GetType())) {
				throw new BindingException(BindingException.INSTANCE_NOT_ASSIGNABLE);
			}
			
			return this.AddBinding(instance, BindingInstance.Singleton);
		}

		/// <summary>
		/// Binds the key type to all assignable types in a given <paramref name="namespaceName"/> 
		/// as transient bindings.
		/// </summary>
		/// <param name="namespaceName">Namespace name.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public void ToNamespace(string namespaceName) {
			var types = TypeUtils.GetAssignableTypesInNamespace(this.bindingType, namespaceName);

			for (int typeIndex = 0; typeIndex < types.Length; typeIndex++) {
				this.AddBinding(types[typeIndex], BindingInstance.Transient);
			}
		}
		
		/// <summary>
		/// Binds the key type to all assignable types in a given <paramref name="namespaceName"/>
		/// as singleton bindings.
		/// </summary>
		/// <param name="namespaceName">Namespace name.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public void ToNamespaceSingleton(string namespaceName) {
			var types = TypeUtils.GetAssignableTypesInNamespace(this.bindingType, namespaceName);
			
			for (int typeIndex = 0; typeIndex < types.Length; typeIndex++) {
				this.AddBinding(types[typeIndex], BindingInstance.Singleton);
			}
		}

		//// <summary>
		/// Binds the key type to a <typeparamref name="T"/> factory.
		/// </summary>
		/// <typeparam name="T">The factory type.</typeparam>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory ToFactory<T>() where T : IFactory {
			return this.ToFactory(typeof(T));
		}
		
		//// <summary>
		/// Binds the key type to a factory of a certain <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The factory type.</typeparam>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory ToFactory(Type type) {
			if (!TypeUtils.IsAssignable(typeof(IFactory), type)) {
				throw new BindingException(BindingException.TYPE_NOT_FACTORY);
			}
			
			return this.AddBinding(type, BindingInstance.Factory);
		}
		
		//// <summary>
		/// Binds the key type to a <paramref name="factory"/>.
		/// </summary>
		/// <param name="factory">Factory to be bound to.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory ToFactory(IFactory factory) {			
			return this.AddBinding(factory, BindingInstance.Factory);
		}

		//// <summary>
		/// Adds a binding.
		/// </summary>
		/// <param name="value">Binding value.</param>
		/// <param name="instanceType">Binding instance type.</param>
		/// <returns>The binding condition factory.</returns>
		public IBindingConditionFactory AddBinding(object value, BindingInstance instanceType) {
			var binding = new BindingInfo(this.bindingType, value, instanceType);
			this.binder.AddBinding(binding);

			return this.BindingConditionFactoryProvider(binding);
		}
		
		//// <summary>
		/// Resolves the binding provider.
		/// </summary>
		/// <param name="type">The type being bound.</param>
		/// <returns>The binding provider.</returns>
		protected virtual IBindingConditionFactory BindingConditionFactoryProvider(BindingInfo binding) {
			return new SingleBindingConditionFactory(binding, this.binder);
		}
	}
}