using System;

namespace Intentor.Adic {
	/// <summary>
	/// Binding condition factory.
	/// </summary>
	public class BindingConditionFactory : IBindingConditionFactory {
		/// <summary>The binding to have its conditions defined.</summary>
		protected Binding binding;

		/// <summary>
		/// Initializes a new instance of the <see cref="Intentor.Adic.BindingConditionFactory"/> class.
		/// </summary>
		/// <param name="binding">The binding to have its conditions settled.</param>
		public BindingConditionFactory(Binding binding) {
			this.binding = binding;
		}

		public IBindingConditionFactory As(object identifier) {
			this.binding.identifier = identifier;

			return this;
		}
		
		public IBindingConditionFactory When(BindingCondition condition) {
			this.binding.condition = condition;

			return this;
		}
		
		public IBindingConditionFactory WhenOn<T>() {
			this.binding.condition = context => context.parentType.Equals(typeof(T));
			
			return this;
		}
		
		public IBindingConditionFactory WhenOn(Type type) {
			this.binding.condition = context => context.parentType.Equals(type);
			
			return this;
		}
		
		public IBindingConditionFactory WhenOnInstance(object instance) {
			this.binding.condition = context => context.parentInstance.Equals(instance);
			
			return this;
		}
	}
}

