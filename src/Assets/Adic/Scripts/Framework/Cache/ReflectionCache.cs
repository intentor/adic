using System;
using System.Collections.Generic;
using System.Reflection;
using Adic.Binding;

namespace Adic.Cache {
    /// <summary>
    /// Basic cache for reflection data.
    /// 
    /// When requesting a type, if it doesn't exist on the cache,
    /// it's automatically created on the cache.
    /// </summary>
    public class ReflectionCache : IReflectionCache {
        /// <summary>Gets the <see cref="Adic.ReflectedClass"/> with the specified type.</summary>
        public ReflectedClass this[Type type] { get { return this.GetClass(type); } }

        /// <summary>Reflection factory used to generate items on the cache.</summary>
        public IReflectionFactory reflectionFactory { get; set; }

        /// <summary>Reflected classes on the cache.</summary>
        private Dictionary<Type, ReflectedClass> classes = new Dictionary<Type, ReflectedClass>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Cache.ReflectionCache"/> class.
        /// </summary>
        public ReflectionCache() {
            this.reflectionFactory = this.ReflectionFactoryProvider();
        }

        /// <summary>
        /// Adds a type to the cache.
        /// </summary>
        /// <param name="type">Type to be added.</param>
        public void Add(Type type) {
            if (type == null) {
                return;
            }

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
        /// Gets an<see cref="Adic.Cache.ReflectedClass"/> for a certain type.
        /// </summary>
        /// <remarks>If the type being getted doesn't exist, it'll be created.</remarks>
        /// <param name="type">Type to look for.</param>
        /// <returns>The reflected class.</returns>
        public ReflectedClass GetClass(Type type) {
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

            for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++) {
                var binding = bindings[bindingIndex];

                if (binding.instanceType == BindingInstance.Transient && binding.value is Type) {
                    this.Add(binding.value as Type);
                } else if (binding.instanceType == BindingInstance.Singleton) {
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