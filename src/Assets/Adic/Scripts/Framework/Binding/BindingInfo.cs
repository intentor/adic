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

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Adic.Binding.BindingInfo"/>.
		/// </summary>
		/// <returns>This object to string.</returns>
		public override string ToString() {
			return string.Format(
				"Type: {0}\n" + 
				"Bound to: {1} ({2})\n" + 
				"Binding type: {3}\n" + 
				"Identifier: {4}\n" + 
				"Conditions: {5}\n",
				this.type.FullName,
				(this.value == null ? "-" : this.value.ToString()),
				(this.value is Type ? "type" : "instance"),
				this.instanceType.ToString(),
				(this.identifier == null ? "-" : this.identifier.ToString()),
				(this.condition == null ? "no" : "yes")
			);
		}
	}
}