using System;

namespace Adic {
	/// <summary>
	/// Marks a setter injection point.
	/// 
	/// If a name is provided, the injector looks the binder for a key with the given name.
	/// 
	/// If no name is provided, the injector looks the binder for a key of the type of the field/property.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
		AllowMultiple = false,
		Inherited = true)]
	public class Inject : Attribute {
		/// <summary>The identifier of the binding to inject.</summary>
		public object identifier;

		/// <summary>
		/// Initializes a new instance of the <see cref="InjectAttribute"/> class.
		/// </summary>
		public Inject() {
			this.identifier = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InjectAttribute"/> class.
		/// </summary>
		/// <param name="name">The identifier of the binding to inject.</param>
		public Inject(object identifier) {
			this.identifier = identifier;
		}
	}
}