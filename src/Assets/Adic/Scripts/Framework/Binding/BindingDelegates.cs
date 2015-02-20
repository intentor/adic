using System;
using System.Collections.Generic;

namespace Intentor.Adic {
	/// <summary>
	/// Binding condition evaluator handler.
	/// </summary>
	/// <param name="context">The injection context.</param>
	public delegate bool BindingCondition(InjectionContext context);

	/// <summary>
	/// Binding added handler.
	/// </summary>
	/// <param name="source">The source of the event.</param>
	/// <param name="binding">The binding to be added, by reference.</param>
	public delegate void BindingAddedHandler(IBinder source, ref Binding binding);
	
	/// <summary>
	/// Binding removed handler.
	/// </summary>
	/// <param name="source">The source of the event.</param>
	/// <param name="type">The type of the binding being removed.</param>
	/// <param name="bindings">The bindings being removed.</param>
	public delegate void BindingRemovedHandler(IBinder source, Type type, IList<Binding> bindings);
}