using System;
using Adic.Binding;
using Adic.Cache;
using Adic.Injection;

namespace Adic.Container {
	/// <summary>
	/// Defines a container for dependency injection.
	/// 
	/// A container holds binding references, resolves types and injects dependencies.
	/// </summary>
	public interface IInjectionContainer : IBinder, IInjector, IDisposable {
		/// <summary>Container identifier.</summary>
		object identifier { get; }

		/// <summary>Reflection cache used to get type info.</summary>
		IReflectionCache cache { get; }

		/// <summary>
		/// Registers a container extension.
		/// </summary>
		/// <typeparam name="T">The type of the extension to be registered.</param>
		/// <returns>The injection container for chaining.</returns>
		IInjectionContainer RegisterExtension<T>() where T : IContainerExtension;

		/// <summary>
		/// Registers a container extension.
		/// </summary>
		/// <param name="extension">The extension to be registered.</param>
		/// <returns>The injection container for chaining.</returns>
		IInjectionContainer RegisterExtension(IContainerExtension extension);
		
		/// <summary>
		/// Unegisters a container extension.
		/// </summary>
		/// <typeparam name="T">The type of the extension(s) to be unregistered.</param>
		/// <returns>The injection container for chaining.</returns>
		IInjectionContainer UnregisterExtension<T>() where T : IContainerExtension;
		
		/// <summary>
		/// Unegisters a container extension.
		/// </summary>
		/// <param name="extension">The extension to be unregistered.</param>
		/// <returns>The injection container for chaining.</returns>
		IInjectionContainer UnregisterExtension(IContainerExtension extension);
	}
}