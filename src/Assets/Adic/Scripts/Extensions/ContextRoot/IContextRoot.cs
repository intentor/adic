using System;
using Adic.Container;

namespace Adic {
	/// <summary>
	/// Defines the context root.
	/// 
	/// A context root is a simple entry point for container creation and game initialization.
	/// </summary>
	public interface IContextRoot {
		/// <summary>Contexts list.</summary>
		IInjectionContainer[] containers { get; }
		
		/// <summary>
		/// Adds the specified container.
		/// </summary>
		/// <param name="container">The container to be added.</param>
		void AddContainer(IInjectionContainer container);

		/// <summary>
		/// Adds the specified container.
		/// </summary>
		/// <param name="container">The container to be added.</param>
		/// <param name="destroyOnLoad">
		/// Indicates whether the container should be destroyed when a new scene is loaded.
		/// </param>
		void AddContainer(IInjectionContainer container, bool destroyOnLoad);
		
		/// <summary>
		/// Setups the containers.
		/// </summary>
		void SetupContainers();

		/// <summary>
		/// Inits the game.
		/// 
		/// The idea is to use this method to instantiate any containers and initialize the game.
		/// </summary>
		void Init();
	}
}