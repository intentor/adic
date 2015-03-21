using System;
using Adic.Container;

namespace Adic {
	/// <summary>
	/// Provides bindings setup capabilities to <see cref="Adic.Container.IInjectionContainer"/>.
	/// </summary>
	public static class BindingsSetupExtension {
		/// <summary>
		/// Setups bindings in the container.
		/// </summary>
		/// <typeparam name="T">The bindings setup object type.</typeparam>
		/// <param name="container">Container in which the bindings will be setup.</param>
		/// <param name="setup">The bindings setup.</param>
		public static void SetupBindings<T>(this IInjectionContainer container) where T : IBindingsSetup, new() {
			container.SetupBindings(typeof(T));
		}

		/// <summary>
		/// Setups bindings in the container.
		/// </summary>
		/// <param name="container">Container in which the bindings will be setup.</param>
		/// <param name="type">The bindings setup object type.</param>
		public static void SetupBindings(this IInjectionContainer container, Type type) {
			var setup = (IBindingsSetup)Activator.CreateInstance(type);
			container.SetupBindings(setup);
		}

		/// <summary>
		/// Setups bindings in the container.
		/// </summary>
		/// <param name="container">Container in which the bindings will be setup.</param>
		/// <param name="setup">The bindings setup.</param>
		public static void SetupBindings(this IInjectionContainer container, IBindingsSetup setup) {
			setup.SetupBindings(container);
		}
	}
}