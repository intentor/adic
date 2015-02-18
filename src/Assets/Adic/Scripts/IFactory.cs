using System;

namespace Adic {
	/// <summary>
	/// Defines a factory of instances.
	/// </summary>
	public interface IFactory {
		/// <summary>Type that the factory creates.</summary>
		Type factoryType { get; }

		/// <summary>
		/// Creates an instance of the object of the type created by the factory.
		/// </summary>
		/// <returns>The instance.</returns>
		object Create();
	}
}