using System;
using Adic.Binding;

namespace Adic.Cache {
    /// <summary>
    /// Defines a cache for reflection data.
    /// 
    /// The idea of this cache is to reduce the execution of reflection by caching data about types.
    /// 
    /// The cache should always look for constructors and Inject attributes.
    /// </summary>
    public interface IReflectionCache {
        /// <summary>Gets the <see cref="Adic.ReflectedClass"/> with the specified type.</summary>
        ReflectedClass this[Type type] { get; }

        /// <summary>Reflection factory used to generate items on the cache.</summary>
        IReflectionFactory reflectionFactory { get; set; }

        /// <summary>
        /// Adds a type to the cache.
        /// </summary>
        /// <param name="type">Type to be added.</param>
        void Add(Type type);

        /// <summary>
        /// Removes a type from the cache.
        /// </summary>
        /// <param name="type">Type to be removed.</param>
        void Remove(Type type);

        /// <summary>
        /// Gets an<see cref="Adic.Cache.IReflectedClass"/> for a certain type.
        /// </summary>
        /// <remarks>If the type being getted doesn't exist, it'll be created.</remarks>
        /// <param name="type">Type to look for.</param>
        /// <returns>The reflected class.</returns>
        ReflectedClass GetClass(Type type);

        /// <summary>
        /// Checks whether a cache exists for a certain type.
        /// </summary>
        /// <param name="type">Type to be removed.</param>
        /// <returns>Boolean.</returns>
        bool Contains(Type type);

        /// <summary>
        /// Caches reflected classes from a binder.
        /// 
        /// It'll use as reference all the binding type values.
        /// </summary>
        /// <param name="binder">The binder.</param>
        void CacheFromBinder(IBinder binder);
    }
}