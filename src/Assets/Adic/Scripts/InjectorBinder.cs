using System;

namespace Adic {
	/// <summary>
	/// An injector that is also a binder.
	/// </summary>
	public class InjectorBinder : Binder, IInjector {
		/// <summary>Reflection cache.</summary>
		public IReflectionCache cache { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="IoC.InjectorBinder"/> class.
		/// </summary>
		public InjectorBinder() {
			this.cache = this.ReflectionCacheProvider();
		}

		/// <summary>
		/// Binds a type to a mapper using a binding.
		/// </summary>
		/// <param name="binding">The binding representation.</param>
		public override void Bind(IBinding binding) {
			if (binding == null) {
				throw new BinderException(BinderException.NULL_BINDING);
			} else if (this.bindings.ContainsKey(binding.key)) {
				throw new BinderException(BinderException.BINDING_KEY_ALREADY_EXISTS);
			} else if (binding.value is Type && (binding.value as Type).IsInterface) {
				throw new BinderException(BinderException.BINDING_TO_INTERFACE);
			}

			var isSingleton = (binding.bindingType == BindingType.Singleton);
			var isType = (binding.value is Type);
			if (isSingleton && isType) {
				binding.value = this.Resolve(binding.value as Type);
			}
			
			this.bindings.Add(binding.key, binding);
		}

		/// <summary>
		/// Resolves dependencies for an object of type <typeparamref name="T"/>
		/// and returns its instance.
		/// </summary>
		/// <typeparam name="T">The type to be resolved.</typeparam>
		/// <returns>The instance.</returns>
		public T Resolve<T>() {
			return (T)this.Resolve(typeof(T));
		}
		
		/// <summary>
		/// Resolves dependencies for an object of <paramref name="type"/>.
		/// and returns its instance.
		/// </summary>
		/// <param name="type">The type to be resolved.</param>
		/// <returns>The instance.</returns>
		public object Resolve(Type type) {
			return this.ResolveBinding(type);
		}
		
		/// <summary>
		/// Resolves dependencies for a named binding
		/// and returns its instance.
		/// </summary>
		/// <param name="name">The name of the binding.</param>
		/// <returns>The instance.</returns>
		public object Resolve(string name) {
			return this.ResolveBinding(name);
		}
		
		/// <summary>
		/// Inject dependencies on an instance of an object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be resolved.</typeparam>
		/// <param name="instance">Instance to receive injection.</param>
		/// <returns>The instance with all its dependencies injected.</returns>
		public T Inject<T>(T instance) where T : class {
			return (T)this.ResolveDependencies(instance);
		}

		/// <summary>
		/// Inject dependencies on an instance of an object.
		/// </summary>
		/// <param name="instance">Instance to receive injection.</param>
		/// <returns>The instance with all its dependencies injected.</returns>
		public object Inject(object instance) {
			return this.ResolveDependencies(instance);
		}

		/// <summary>
		/// Resolve an instance from a specified binding key.
		/// </summary>
		/// <param name="key">Binding key.</param>
		protected object ResolveBinding(object key) {
			//First look for an instance on the binder.
			var binding = this.GetBinding(key);
			object instance = null;
			
			if (binding == null) {
				instance = this.Instantiate(key as Type);
			} else if (binding.bindingType == BindingType.Default) {
				instance = this.Instantiate(binding.value as Type);
			} else if (binding.bindingType == BindingType.Factory) {
				instance = (binding.value as IFactory).Create();
			} else {
				//Binding is a singleton object.
				instance = binding.value;
			}
			
			return instance;
		}

		/// <summary>
		/// Instantiate and resolve the dependencies for the specified type.
		/// </summary>
		/// <param name="type">The type to be instantiated.</param>
		protected object Instantiate(Type type) {
			var reflectedClass = this.cache.GetClass(type);
			object instance = null;

			if (reflectedClass.constructor == null) {
				throw new InjectorException(string.Format(InjectorException.NO_CONSTRUCTORS, type.ToString()));
			}

			if (reflectedClass.constructorParameters.Length == 0) {
				instance = reflectedClass.constructor.Invoke(null);
			} else {
				object[] parameters = new object[reflectedClass.constructorParameters.Length];

				for (int paramIndex = 0; paramIndex < parameters.Length; paramIndex++) {
					parameters[paramIndex] = this.Resolve(reflectedClass.constructorParameters[paramIndex]);
				}

				instance = reflectedClass.constructor.Invoke(parameters);
			}

			instance = this.ResolveDependencies(instance, reflectedClass);

			return instance;
		}
		
		/// <summary>
		/// Resolves dependencies for an instance.
		/// </summary>
		/// <param name="instance">The instance to have its dependencies resolved.</param>
		protected object ResolveDependencies(object instance) {
			var reflectedClass = this.cache.GetClass(instance.GetType());
			return this.ResolveDependencies(instance, reflectedClass);
		}
		
		/// <summary>
		/// Resolves dependencies for an instance.
		/// </summary>
		/// <param name="instance">The instance to have its dependencies resolved.</param>
		/// <param name="reflectedClass">The reflected class related to the <paramref name="instance"/>.</param>
		protected object ResolveDependencies(object instance, IReflectedClass reflectedClass) {
			if (reflectedClass.properties.Length > 0) {
				for (int propertyIndex = 0; propertyIndex < reflectedClass.properties.Length; propertyIndex++) {
					var property = reflectedClass.properties[propertyIndex];
					var binding = this.ResolveBinding(property.Key);
					property.Value.SetValue(instance, binding, null);
				}
			 }

			if (reflectedClass.fields.Length > 0) {
				for (int fieldIndex = 0; fieldIndex < reflectedClass.fields.Length; fieldIndex++) {
					var field = reflectedClass.fields[fieldIndex];
					var binding = this.ResolveBinding(field.Key);
					field.Value.SetValue(instance, binding);
				}
			}

			//Calls post constructors, if there's any.
			if (reflectedClass.postConstructors.Length > 0) {
				for (int constIndex = 0; constIndex < reflectedClass.postConstructors.Length; constIndex++) {
					var method = reflectedClass.postConstructors[constIndex];
					method.Invoke(instance, null);
				}
			}

			return instance;
		}
		
		/// <summary>
		/// Resolves the reflection cache provider.
		/// </summary>
		/// <returns>The reflection cache provider.</returns>
		protected virtual IReflectionCache ReflectionCacheProvider() {
			return new ReflectionCache();
		}
	}
}