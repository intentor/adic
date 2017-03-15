using System;
using System.Collections.Generic;
using Adic.Binding;
using Adic.Cache;

namespace Adic.Injection {
    /// <summary>
    /// Type resolution handler.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="type">Binding type.</param>
    /// <param name="member">Member for which the binding is being resolved.</param>
    /// <param name="parentInstance">Parent object in which the resolve is occuring.</param>
    /// <param name="identifier">The binding identifier to be looked for.</param>
    /// <param name="resolutionInstance">The instance resolved, by reference.</param>
    /// <returns>
    /// Boolean value indicating whether the resolution of the type should continue.
    /// 
    /// If the method returns false, the resolution system will return the value
    /// of <paramref name="resolutionInstance"/>.
    /// </returns>
	public delegate bool TypeResolutionHandler(IInjector source, 
  		Type type,
  		InjectionMember member,
  		object parentInstance,
       	object identifier,
		ref object resolutionInstance);

    /// <summary>
    /// Binding evaluation handler, called for each binding that is available to a certain
    /// resolution evaluation.
    /// 
    /// The event is dispatched inside the ResolveBinding method right after all the resolution
    /// conditions have been fulfilled and before the instance is actually resolved.
    /// 
    /// This handler can be used e.g. to alter the instance before it's actually resolved.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="binding">The binding to have an instance resolved.</param>
    /// <returns>The evaluated instance or NULL if the evaluation should occur by the injector.</returns>
	public delegate object BindingEvaluationHandler(IInjector source,
        ref BindingInfo binding);
	
    /// <summary>
    /// Binding resolution handler, called for each instance that is resolved from a certain binding.
    /// 
    /// The event is dispatched inside the ResolveBinding method right after the instance is 
    /// actually resolved.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="binding">The binding from which the instance has been resolved.</param>
    /// <param name="instance">The resolved instance.</param>
	public delegate void BindingResolutionHandler(IInjector source,
        ref BindingInfo binding,
        ref object instance);

    /// <summary>
    /// Instance injection handler.
    /// </summary>
    /// <param name="source">The source of the event.</param>
    /// <param name="instance">The instance to have its dependencies resolved, passed by reference.</param>
    /// <param name="reflectedClass">The reflected class related to the <paramref name="instance"/>.</param>
	public delegate void InstanceInjectionHandler(IInjector source,
		ref object instance,
      	ReflectedClass reflectedClass);
}