using System;

namespace Adic.Binding {
	/// <summary>
	/// Defines the conditions of a binding.
	/// </summary>
	public interface IBindingConditionFactory {
		//// <summary>
		/// Conditions the binding to be injected through an identifier.
		/// </summary>
		/// <param name="identifier">Identifier of the binding.</param>
		/// <returns>The current binding condition.</returns>
		IBindingConditionFactory As(object identifier);

		/// <summary>
		/// Conditions the binding to be injected only if BindingConditionEvaluator returns true.
		/// </summary>
		/// <param name="condition">Condition.</param>
		/// <returns>The current binding condition.</returns>
		IBindingConditionFactory When(BindingCondition condition);

		/// <summary>
		/// Conditions the binding to be injected only when on an object of a certain type <typeparamref name="T">.
		/// </summary>
		/// <typeparam name="T">The enclosing type.</typeparam>
		/// <returns>The current binding condition.</returns>
		IBindingConditionFactory WhenOn<T>();
		
		/// <summary>
		/// Conditions the binding to be injected only when on an object of a certain type <paramref name="type"/>.
		/// </summary>
		/// </summary>
		/// <param name="type">The enclosing type.</param>
		/// <returns>The current binding condition.</returns>
		IBindingConditionFactory WhenOn(Type type);
		
		/// <summary>
		/// Conditions the binding to be injected only when on a certain <paramref name="instance"/>.
		/// </summary>
		/// </summary>
		/// <param name="instance">The enclosing instance.</param>
		/// <returns>The current binding condition.</returns>
		IBindingConditionFactory WhenOnInstance(object instance);
	}
}