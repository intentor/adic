using System;
using System.Collections;

namespace Adic {
	/// <summary>
	/// Defines a factory for <see cref="IReflectedClass"/>.
	/// </summary>
	public interface IReflectionFactory {
		/// <summary>
		/// Creates the <see cref="IReflectedClass"/>>
		/// </summary>
		/// <param name="type">Type from which the reflected class will be created.</param>
		IReflectedClass Create(Type type);
	}
}