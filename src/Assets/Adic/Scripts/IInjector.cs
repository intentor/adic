using System;

namespace Adic {
	/// <summary>
	/// Defines a dependency injector which injects
	/// dependencies on instances.
	/// 
	/// The injector itself doesn't instantiate objects,
	/// using a Factory to it.
	/// </summary>
	public interface IInjector {
		/// <summary>Reflection cache.</summary>
		IReflectionCache cache { get; }

		/// <summary>
		/// Resolves dependencies for an object of type <typeparamref name="T"/>
		/// and returns its instance.
		/// </summary>
		/// <typeparam name="T">The type to be resolved.</typeparam>
		/// <returns>The instance.</returns>
		T Resolve<T>();

		/// <summary>
		/// Resolves dependencies for an object of <paramref name="type"/>.
		/// and returns its instance.
		/// </summary>
		/// <param name="type">The type to be resolved.</param>
		/// <returns>The instance.</returns>
		object Resolve(Type type);
		
		/// <summary>
		/// Resolves dependencies for a named binding
		/// and returns its instance.
		/// </summary>
		/// <param name="name">The name of the binding.</param>
		/// <returns>The instance.</returns>
		object Resolve(string name);
		
		/// <summary>
		/// Inject dependencies on an instance of an object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be resolved.</typeparam>
		/// <param name="instance">Instance to receive injection.</param>
		/// <returns>The instance with all its dependencies injected.</returns>
		T Inject<T>(T instance) where T : class;
		
		/// <summary>
		/// Inject dependencies on an instance of an object.
		/// </summary>
		/// <param name="instance">Instance to receive injection.</param>
		/// <returns>The instance with all its dependencies injected.</returns>
		object Inject(object instance);
	}
}