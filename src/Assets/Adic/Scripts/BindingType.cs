using System;

namespace Adic {
	/// <summary>
	/// Binding type.
	/// </summary>
	public enum BindingType {
		/// <summary>A new instance is given during dependency resolution.</summary>
		Default,
		/// <summary>The same instance is given during dependency resolution.</summary>
		Singleton,
		/// <summary>The instance is requested through a factory during resolution.</summary>
		Factory
	}
}