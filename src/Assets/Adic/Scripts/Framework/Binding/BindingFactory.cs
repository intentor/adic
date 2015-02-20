using System;

namespace Intentor.Adic {
	/// <summary>
	/// Binding types to another types or instances.
	/// </summary>
	public class BindingFactory : IBindingFactory {
		/// <summary>Binder used by the Binding Factory.</summary>
		public IBinder binder { get; private set; }
		/// <summary>The type being bound.</summary>
		public Type bindingType { get; private set; }
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Intentor.Adic.BindingFactory"/> class.
		/// </summary>
		/// <param name="bindingType">The type being bound.</param>
		/// <param name="binder">The binder that will bind this binding.</param>
		public BindingFactory(Type bindingType, IBinder binder) {
			this.bindingType = bindingType;
			this.binder = binder;
		}
		
		/// <summary>
		/// Binds the key type to a transient of itself. The key must be a class.
		/// </summary>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory ToSelf() {
			return this.CreateBinding(this.bindingType, BindingInstance.Transient);
		}
		
		/// <summary>
		/// Binds the key type to a singleton of itself. The key must be a class.
		/// </summary>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory ToSingleton() {
			return this.CreateBinding(this.bindingType, BindingInstance.Singleton);
		}
		
		/// <summary>
		/// Binds the key type to a singleton of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The related type.</typeparam>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory ToSingleton<T>() where T : class {
			return this.ToSingleton(typeof(T));
		}
		
		/// <summary>
		/// Binds the key type to a singleton of type <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The related type.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory ToSingleton(Type type) {
			if (!TypeUtils.IsAssignable(this.bindingType, type)) {
				throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
			}
			
			return this.CreateBinding(type, BindingInstance.Singleton);
		}
		
		/// <summary>
		/// Binds the key type to a transient of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type to bind to.</typeparam>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory To<T>() where T : class {
			return this.To(typeof(T));
		}
		
		/// <summary>
		/// Binds the key type to a transient of type <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The related type.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory To(Type type) {
			if (!TypeUtils.IsAssignable(this.bindingType, type)) {
				throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
			}

			return this.CreateBinding(type, BindingInstance.Transient);
		}
		
		// <summary>
		/// Binds the key type to an <paramref name="instance"/>.
		/// </summary>
		/// <typeparam name="T">The related type.</typeparam>
		/// <param name="instance">The instance to bind the type to.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory To<T>(T instance) {
			return this.To(typeof(T), instance);
		}

		// <summary>
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
			
			return this.CreateBinding(instance, BindingInstance.Singleton);
		}
		
		/// <summary>
		/// Binds the key type to a <paramref name="factory"/>.
		/// </summary>
		/// <param name="factory">Factory to be bound to.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public IBindingConditionFactory ToFactory(IFactory factory) {
			if (!TypeUtils.IsAssignable(this.bindingType, factory.factoryType)) {
				throw new BindingException(BindingException.FACTORY_TYPE_INCORRECT);
			}
			
			return this.CreateBinding(factory, BindingInstance.Factory);
		}

		/// <summary>
		/// Creates a binding.
		/// </summary>
		/// <returns>The binding.</returns>
		/// <param name="value">Binding value.</param>
		/// <param name="instanceType">Binding instance type.</param>
		public IBindingConditionFactory CreateBinding(object value, BindingInstance instanceType) {
			var binding = new Binding(this.bindingType, value, instanceType);
			this.binder.AddBinding(binding);

			return this.BindingConditionFactoryProvider(binding);
		}
		
		/// <summary>
		/// Determines whether <paramref name="potentialDescendant"/> is the same
		/// or a subclass of <paramref name="potentialBase"/>.
		/// </summary>
		/// <param name="potentialBase">Potential base type.</param>
		/// <param name="potentialDescendant">Potential descendant type.</param>
		/// <returns>Boolean.</returns>
		protected bool IsAssignable(Type potentialBase, Type potentialDescendant) {
			return potentialBase.IsAssignableFrom(potentialDescendant);
		}
		
		/// <summary>
		/// Resolves the binding provider.
		/// </summary>
		/// <param name="type">The type being bound.</param>
		/// <returns>The binding provider.</returns>
		protected virtual IBindingConditionFactory BindingConditionFactoryProvider(Binding binding) {
			return new BindingConditionFactory(binding);
		}
	}
}