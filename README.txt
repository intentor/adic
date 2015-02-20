

--------------------------------------------------------------------------------
 CREATING CONTAINER EXTENSIONS
--------------------------------------------------------------------------------

Extensions on Adic can be made through 3 ways:

1. Creating a framework extension extending the base APIs through their interfaces

2. Creating extension methods to any part of the framework

3. Creating a container extension, which allows for the interception
of internal events, which can alter the inner workings of the framework

How to

Create the extension class with ContainerExtension sufix
Derive from IContainerExtension
Subscribe to any events on the container on OnRegister method
Unsubscribe to any events the extension may use on the container on 
OnUnregister method

	Available events on IInjectionContainer:
		/// <summary>Occurs before a binding is added.</summary>
		event BindingAddedHandler beforeAddBinding;
		/// <summary>Occurs after a binding is added.</summary>
		event BindingAddedHandler afterAddBinding;
		/// <summary>Occurs before a binding is removed.</summary>
		event BindingRemovedHandler beforeRemoveBinding;
		/// <summary>Occurs after a binding is removed.</summary>
		event BindingRemovedHandler afterRemoveBinding;

		// <summary>Occurs before a type is resolved.</summary>
		event TypeResolutionHandler beforeResolve;
		/// <summary>Occurs after a type is resolved.</summary>
		event TypeResolutionHandler afterResolve;
		/// <summary>Occurs when a binding is available for resolution.</summary>
		event BindingEvaluationHandler bindingEvaluation;
		/// <summary>Occurs before an instance receives injection.</summary>
		event InstanceInjectionHandler beforeInject;
		/// <summary>Occurs after an instance receives injection.</summary>
		event InstanceInjectionHandler afterInject;


--------------------------------------------------------------------------------
 AVAILABLE EXTENSIONS
--------------------------------------------------------------------------------
	
	Commander
		Dependencies: none

	Disposable
		Dependencies: none

	UnityBinding
		Dependencies: none		

	Updateable
		Dependencies: none

	BindingsPrinter (Editor only, runtime)
		Dependencies: ContextRoot Extension

		Prints all bindings on any containers on the ContextRoot.

	ObjectGraphPrinter ? (Editor only, runtime)
		Dependencies: ContextRoot Extension

		Prints the entire dependencies through all types on the binder.

		Maybe this can validate the ENTIRE scene?


--------------------------------------------------------------------------------
 EXAMPLES
--------------------------------------------------------------------------------

Hello World
	Simple binding a dependency to a MonoBehaviour that displays "Hello World"
	Shows the basics of how to setup a scene for dependency using the ContextRoot

Binding Game Objects
	Binds MonoBehaviours to new and existing game objects and allows them to
	share dependencies.

Binding Interfaces
	Binds interfaces to MonoBehaviours and regular classes

Using conditions
	Exemplify the use of identifiers and conditions for a better dependency injection

Prefabs
	Exemplify the use of binding to prefabs

Commander
	Exemplify the binding and using of commands to execute complex actions.

InjectCrush
	A very simple Candy Crush like game to exemplify the complete use of the framework and its extensions