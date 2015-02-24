using System;

namespace Adic.Binding {
	/// <summary>
	/// Represents a binding.
	/// </summary>
	public class BindingInfo {
		/// <summary>The type from which the binding is bound to.</summary>
		public Type type { get; private set; }
		/// <summary>The value to which the binding is bound to.</summary>
		public object value { get; set; }
		/// <summary>The binding instance type.</summary>
		public BindingInstance instanceType { get; private set; }
		/// <summary>The binding identifier. The identifier will group bindings of the same type.</summary>
		public object identifier { get; set; }
		/// <summary>The binding condition.</summary>
		public BindingCondition condition { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.Binding.BindingInfo"/> class.
		/// </summary>
		/// <param name="type">The type from which the binding is bound to.</param>
		/// <param name="value">The value to which the binding is bound to.</param>
		/// <param name="instanceType">The binding instance type.</param>
		public BindingInfo(Type type, object value, BindingInstance instanceType) {
			this.type = type;
			this.value = value;
			this.instanceType = instanceType;
		}
	}
}