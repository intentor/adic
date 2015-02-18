using System;

namespace Adic {
	/// <summary>
	/// Base context class for dependency injection.
	/// </summary>
	public abstract class Context : InjectorBinder, IContext {
		/// <summary>
		/// Initializes a new instance of the <see cref="IoC.Context"/> class.
		/// </summary>
		public Context() {
			this.SetupBindings();
			this.cache.CacheFromBinder(this);
		}

		/// <summary>
		/// Setups the context bindings.
		/// </summary>
		/// <remarks>
		/// It's a good practice to setup all the binds on this routine,
		/// leaving no other bindings to be bound.
		/// </remarks>
		public abstract void SetupBindings();
	}
}