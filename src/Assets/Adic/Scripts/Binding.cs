using System;

namespace Adic {
	/// <summary>
	/// Binding of a certain type to a value.
	/// </summary>
	public class Binding : IBinding {		
		/// <summary>The key being bound.</summary>
		public object key { get; private set; }		
		/// <summary>The value of the bind.</summary>
		public object value { get; set; }
		/// <summary>The binding type.</summary>
		public BindingType bindingType { get; private set; }

		/// <summary>Binder to bind dependencies.</summary>
		private IBinder binder { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="IoC.Binding"/> class.
		/// </summary>
		/// <param name="key">The key being bound.</param>
		/// <param name="binder">The binder that will bind this binding.</param>
		public Binding(object key, IBinder binder) {
			this.key = key;
			this.binder = binder;
		}
		
		/// <summary>
		/// Binds the key to a singleton. The key must be a type.
		/// </summary>
		public void AsSingleton() {
			if (!(key is Type)) {
				throw new BindingException(BindingException.KEY_IS_NOT_TYPE);
			}

			this.value = this.key;
			this.bindingType = BindingType.Singleton;

			this.binder.Bind(this);
		}
		
		/// <summary>
		/// Binds the key to a singleton of type <typeparamref name="T">.
		/// </summary>
		/// <typeparam name="T">The related type.</typeparam>
		public void AsSingleton<T>() where T : class {
			if ((key is Type)) {
				if (!this.IsAssignable(this.key as Type, typeof(T))) {
					throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
				} else if (this.IsAssignable(typeof(UnityEngine.MonoBehaviour), this.key as Type)) {
					throw new BindingException(BindingException.SINGLETON_MONOBEHAVIOUR);
				}
			}

			this.value = typeof(T);
			this.bindingType = BindingType.Singleton;
			
			this.binder.Bind(this);
		}
		
		/// <summary>
		/// Binds the key to a type <typeparamref name="T">.
		/// </summary>
		/// <typeparam name="T">The type to bind to.</typeparam>
		public void To<T>() where T : class {
			if ((key is Type)) {
				if (!this.IsAssignable(this.key as Type, typeof(T))) {
					throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
				} else if (this.IsAssignable(typeof(UnityEngine.MonoBehaviour), this.key as Type)) {
					throw new BindingException(BindingException.SINGLETON_MONOBEHAVIOUR);
				}
			}

			this.value = typeof(T);
			this.bindingType = BindingType.Default;
			
			this.binder.Bind(this);
		}

		// <summary>
		/// Binds the key to <paramref name="instance"/>.
		/// </summary>
		/// <typeparam name="T">The related type.</typeparam>
		/// <param name="instance">The instance to bind the type to.</param>
		public void To<T>(T instance) {
			if ((key is Type) && !this.IsAssignable(this.key as Type, typeof(T))) {
				throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
			}
			this.value = instance;
			this.bindingType = BindingType.Singleton;
			
			this.binder.Bind(this);
		}
		
		/// <summary>
		/// Binds the key to a <paramref name="factory"/>.
		/// </summary>
		/// <param name="factory">Factory to be bound to.</param>
		/// <returns>The created binding.</returns>
		public void ToFactory(IFactory factory) {
			if ((key is Type) && !this.IsAssignable(this.key as Type, factory.factoryType)) {
				throw new BindingException(BindingException.FACTORY_TYPE_INCORRECT);
			}

			this.value = factory;
			this.bindingType = BindingType.Factory;
			
			this.binder.Bind(this);
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
	}
}