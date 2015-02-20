using System;

namespace Intentor.Adic {
	/// <summary>
	/// Defines a container for dependency injection.
	/// 
	/// A container holds binding references, resolves types and injects dependencies.
	/// </summary>
	public interface IInjectionContainer : IBinder, IInjector, IDisposable {
		/// <summary>
		/// Registers a container extension.
		/// </summary>
		/// <typeparam name="T">The type of the extension to be registered.</param>
		void RegisterExtension<T>() where T : IContainerExtension;

		/// <summary>
		/// Registers a container extension.
		/// </summary>
		/// <param name="extension">The extension to be registered.</param>
		void RegisterExtension(IContainerExtension extension);
		
		/// <summary>
		/// Unegisters a container extension.
		/// </summary>
		/// <typeparam name="T">The type of the extension(s) to be unregistered.</param>
		void UnregisterExtension<T>() where T : IContainerExtension;
		
		/// <summary>
		/// Unegisters a container extension.
		/// </summary>
		/// <param name="extension">The extension to be unregistered.</param>
		void UnregisterExtension(IContainerExtension extension);
	}
}