using System;

namespace Adic.Binding {
    /// <summary>
    /// Represents a binding.
    /// </summary>
    public class BindingInfo {
        /// <summary>Type from which the binding is bound to.</summary>
        public Type type { get; private set; }

        /// <summary>Value to which the binding is bound to.</summary>
        public object value { get; set; }

        /// <summary>Binding instance type.</summary>
        public BindingInstance instanceType { get; private set; }

        /// <summary>Binding identifier. The identifier will group bindings of the same type.</summary>
        public object identifier { get; set; }

        /// <summary>Binding condition.</summary>
        public BindingCondition condition { get; set; }

        /// <summary>Binding tags.</summary>
        public string[] tags { 
            get {
                if (this.originalBinding != null) {
                    return this.originalBinding.tags;
                } else {
                    return this.bindingTags;
                }
            }
            set {
                this.bindingTags = value;
            }
        }

        /// <summary>Binding tags.</summary>
        private string[] bindingTags;
        /// <summary>
        /// Original binding from which this one is derived. 
        /// <para />
        /// Used in cases when a singleton binding needs to be created from another binding.
        /// </summary>
        private BindingInfo originalBinding;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Binding.BindingInfo"/> class.
        /// </summary>
        /// <param name="type">Type from which the binding is bound to.</param>
        /// <param name="value">Value to which the binding is bound to.</param>
        /// <param name="instanceType">Binding instance type.</param>
        public BindingInfo(Type type, object value, BindingInstance instanceType) : this(type, value, instanceType, null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Binding.BindingInfo"/> class.
        /// </summary>
        /// <param name="type">Type from which the binding is bound to.</param>
        /// <param name="value">Value to which the binding is bound to.</param>
        /// <param name="instanceType">Binding instance type.</param>
        /// <param name="originalBinding">Original binding.</param>
        public BindingInfo(Type type, object value, BindingInstance instanceType, BindingInfo originalBinding) {
            this.type = type;
            this.value = value;
            this.instanceType = instanceType;
            this.originalBinding = originalBinding;
        }

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <returns>The value type.</returns>
        public Type GetValueType() {
            return (this.value is Type ? (Type) this.value : this.value.GetType());
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
                "Conditions: {5}\n" +
                "Tags: {6}\n",
                this.type.FullName,
                this.value == null ? "-" : this.value.ToString(),
                this.value == null ? "-" : this.value is Type ? "type" : "instance [" + this.value.GetHashCode() + "]",
                this.instanceType.ToString(),
                this.identifier == null ? "-" : this.identifier.ToString(),
                this.condition == null ? "no" : "yes",
                tags == null ? "[]" : String.Join(",", tags)
            );
        }
    }
}