using System;
using System.Collections.Generic;
using System.Reflection;

namespace Adic {
	/// <summary>
	/// Basic cache for reflection data.
	/// 
	/// When requesting a type, if it doesn't exist on the cache,
	/// it's automatically created on the cache.
	/// </summary>
	public class ReflectionCache : IReflectionCache {
		/// <summary>Gets the <see cref="IoC.IReflectionCache"/> with the specified key.</summary>
		public IReflectedClass this[Type key] { get { return this.GetClass(key); } }

		/// <summary>Reflection factory used to generate items on the cache.</summary>
		public IReflectionFactory reflectionFactory { get; set; }
		
		/// <summary>Reflected classes on the cache.</summary>
		private Dictionary<Type, IReflectedClass> classes = new Dictionary<Type, IReflectedClass>();

		/// <summary>
		/// Initializes a new instance of the <see cref="IoC.ReflectionCache"/> class.
		/// </summary>
		public ReflectionCache() {
			this.reflectionFactory = this.ReflectionFactoryProvider();
		}
		
		/// <summary>
		/// Adds a type to the cache.
		/// </summary>
		/// <param name="type">Type to be added.</param>
		public void Add(Type type) {
			if (!this.Contains(type)) {
				this.classes.Add(type, this.reflectionFactory.Create(type));
			}
		}
		
		/// <summary>
		/// Removes a type from the cache.
		/// </summary>
		/// <param name="type">Type to be removed.</param>
		public void Remove(Type type) {
			if (this.Contains(type)) {
				this.classes.Remove(type);
			}
		}
		
		/// <summary>
		/// Gets an<see cref="IoC.IReflectedClass"/> for a certain type.
		/// </summary>
		/// <remarks>If the type being getted doesn't exist, it'll be created.</remarks>
		/// <param name="type">Type to look for.</param>
		/// <returns>The reflected class.</returns>
		public IReflectedClass GetClass(Type type) {
			if (!this.Contains(type)) {
				this.Add(type);
			}

			return this.classes[type];
		}
		
		/// <summary>
		/// Checks whether a cache exists for a certain type.
		/// </summary>
		/// <param name="type">Type to be removed.</param>
		/// <returns>Boolean.</returns>
		public bool Contains(Type type) {
			return this.classes.ContainsKey(type);
		}

		/// <summary>
		/// Caches reflected classes from a binder.
		/// 
		/// It'll use as reference all the binding type values.
		/// </summary>
		/// <param name="binder">The binder.</param>
		public void CacheFromBinder(IBinder binder) {
			var bindings = binder.GetBindings();

			for (int bindingIndex = 0; bindingIndex < bindings.Length; bindingIndex++) {
				var binding = bindings[bindingIndex];

				if (binding.bindingType == BindingType.Default) {
					this.Add(binding.value as Type);
				} else if (binding.bindingType == BindingType.Singleton) {
					this.Add(binding.value.GetType());
				}
			}
		}
		
		/// <summary>
		/// Resolves the reflection factory provider.
		/// </summary>
		/// <returns>The reflection factory provider.</returns>
		protected virtual IReflectionFactory ReflectionFactoryProvider() {
			return new ReflectionFactory();
		}
	}
}