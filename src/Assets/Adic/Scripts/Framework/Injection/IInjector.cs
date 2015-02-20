using System;
using System.Collections.Generic;

namespace Intentor.Adic {
	/// <summary>
	/// Defines a dependency injector which injects dependencies on instances.
	/// </summary>
	public interface IInjector {
		/// <summary>Occurs before a type is resolved.</summary>
		event TypeResolutionHandler beforeResolve;
		/// <summary>Occurs after a type is resolved.</summary>
		event TypeResolutionHandler afterResolve;
		/// <summary>Occurs when a binding is available for resolution.</summary>
		event BindingEvaluationHandler bindingEvaluation;
		/// <summary>Occurs before an instance receives injection.</summary>
		event InstanceInjectionHandler beforeInject;
		/// <summary>Occurs after an instance receives injection.</summary>
		event InstanceInjectionHandler afterInject;

		/// <summary>
		/// Resolves an instance for a specified type.
		/// </summary>
		/// <remarks>
		/// If the type has multiple instances, it will return an IList<[type]>.
		/// </remarks>
		/// <param name="type">Type to be resolved.</param>
		/// <returns>The instance or NULL.</returns>
		object Resolve(Type type);
		
		/// <summary>
		/// Resolves an instance for a specified type.
		/// </summary>
		/// <remarks>
		/// If the type has multiple instances, please use ResolveAll<T>().
		/// </remarks>
		/// <typeparam name="T">Type to be resolved.</typeparam>
		/// <returns>The instance or NULL.</returns>
		T Resolve<T>();

		/// <summary>
		/// Resolves a list of instances for a specified type.
		/// </summary>
		/// <typeparam name="T">Type to be resolved.</typeparam>
		/// <returns>The list of instances or NULL if there are no instances.</returns>
		IList<T> ResolveAll<T>();
		
		/// <summary>
		/// Injects dependencies on an instance of an object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be resolved.</typeparam>
		/// <param name="instance">Instance to receive injection.</param>
		/// <returns>The instance with all its dependencies injected.</returns>
		T Inject<T>(T instance) where T : class;
		
		/// <summary>
		/// Injects dependencies on an instance of an object.
		/// </summary>
		/// <param name="instance">Instance to receive injection.</param>
		/// <returns>The instance with all its dependencies injected.</returns>
		object Inject(object instance);
	}
}