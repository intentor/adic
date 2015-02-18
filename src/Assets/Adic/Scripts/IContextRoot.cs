using System;

namespace Adic {
	/// <summary>
	/// Defines the context root.
	/// </summary>
	public interface IContextRoot {
		/// <summary>Contexts list.</summary>
		IContext[] contexts { get; }
		
		/// <summary>
		/// Adds the specified context.
		/// </summary>
		/// <typeparam name="T">The type of the context to be added.</typeparam>
		void AddContext<T>() where T : IContext;
		
		/// <summary>
		/// Setup the contexts.
		/// </summary>
		void SetupContexts();

		/// <summary>
		/// Init the game.
		/// 
		/// The ideia is to use this method to instantiate any contexts
		/// and initialize the game.
		/// </summary>
		void Init();
	}
}