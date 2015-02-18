using System;

namespace Adic {
	/// <summary>
	/// Defines the context for dependency injection.
	/// </summary>
	public interface IContext : IBinder, IInjector {
		/// <summary>
		/// Setups the context bindings.
		/// </summary>
		void SetupBindings();
	}
}