using System;

namespace Adic.Binding {
	/// <summary>
	/// Binding instance type.
	/// </summary>
	public enum BindingInstance {
		/// <summary>A new instance is given during dependency resolution.</summary>
		Transient,
		/// <summary>The same instance is given during dependency resolution.</summary>
		Singleton,
		/// <summary>The instance is requested through a factory during resolution.</summary>
		Factory
	}
}