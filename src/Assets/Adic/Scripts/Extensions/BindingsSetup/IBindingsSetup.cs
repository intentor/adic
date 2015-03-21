using Adic.Container;

namespace Adic {
	/// <summary>
	/// Represents a bindings setup object.
	/// </summary>
	public interface IBindingsSetup {
		/// <summary>
		/// Setups the bindings in a given <paramref name="container"/>.
		/// </summary>
		/// <param name="container">Container in which the bindings will be setup.</param>
		void SetupBindings(IInjectionContainer container);
	}
}