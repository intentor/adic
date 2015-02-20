using System;
using System.Collections;

namespace Intentor.Adic {
	/// <summary>
	/// Defines a factory for <see cref="IReflectedClass"/>.
	/// </summary>
	public interface IReflectionFactory {
		/// <summary>
		/// Creates a <see cref="ReflectedClass"/> from a <paramref name="type"/>.
		/// </summary>
		/// <param name="type">Type from which the reflected class will be created.</param>
		ReflectedClass Create(Type type);
	}
}