# Adic

**Another Dependency Injection Container for Unity 3D and beyond**

## Contents

* <a href="#introduction">Introduction</a>
* <a href="#features">Features</a>
* <a href="#concepts">Concepts</a>
	* <a href="#what-is">What is a dependency injector container?
	* <a href="#structure">Structure
	* <a href="#types-of-bindings">Types of bindings
* <a href="#quick-start">Quick start</a>
* <a href="#api">API</a>
	* <a href="#bindings">Bindings</a>
	* <a href="#conditions">Conditions</a>
	* <a href="#constructor-injection">Constructor injection</a>
	* <a href="#member-injection">Member injection</a>
	* <a href="#multiple-constructors">Multiple constructors</a>
	* <a href="#monobehaviour-injection">MonoBehaviour injection</a>
	* <a href="#manual-type-resolution">Manual type resolution</a>
	* <a href="#factories">Factories</a>
* <a href="#pipeline">Pipeline
* <a href="#container-extensions">Extensions</a>
	* <a href="#available-extensions">Available extensions</a>
		* <a href="#extension-context-root">Context Root</a>
		* <a href="#extension-mono-injection">Mono Injection</a>
		* <a href="#extension-unity-binding">Unity Binding</a>
	* <a href="#creating-extensions">Creating extensions</a>
	* <a href="#container-events">Container events</a>
		* <a id="binder-events">Binder events</a>
		* <a id="injector-events">Injector events</a>
* <a href="#notes">Notes</a>
* <a href="#examples">Examples</a>
* <a href="#support">Support</a>
* <a href="#license">License</a>

## <a id="introduction"></a>Introduction

*Adic* is a lightweight dependency injection container for Unity 3D.

Based on studies from [StrangeIoC](http://strangeioc.github.io/strangeioc/) and the proof-of-concept container from [Sebastiano Mandalà](http://blog.sebaslab.com/ioc-container-for-unity3d-part-1/), the ideia of the project was to create a dependency injection container that is simple to use and extend, having on its roots the simplicity of the work of Mandalà and the extensibility of StrangeIoC.

The project was tested on Unity 4.3+ and should work on lower versions of the 4 cicle.

## <a id="features"></a>Features

* Binds types, singleton instances, factories, game objects and prefabs.
* Instance resolution by type, identifier and complex conditions.
* Injection on constructor, fields and properties.
* Can inject multiple objects of the same type.
* Fast dependency resolution with internal cache.
* Use of attributes to indicate injections, preferable construtors and post constructors.
* Can be easily extented through extensions.
* Framework decoupled from Unity - all Unity based API is achieved through extensions.
* Organized and well documented code written in C#.

## <a id="concepts"></a>Concepts

### <a id="what-is"></a>What is a dependency injection container?

A *dependency injection container* is a piece of software that handles the resolution of dependencies of objects. It's related to the (dependency injection)[http://en.wikipedia.org/wiki/Dependency_injection] and (inversion of control)[http://en.wikipedia.org/wiki/Inversion_of_control] design patterns.

The idea is that any dependency an object may need should be resolved by an external entity rather than the own object. Practically speaking, a object should not use `new` to create the objects it uses, having those instances *injected* into it by another object whose sole existance is to resolve dependencies.

So, a *dependency injection container* holds information about dependencies (the *bindings*) that can be injected into another objects by demand (injecting into existing objects) or during resolution (when you are creating a new object of some type).

### <a id="structure"></a>Structure

The structure of *Adic* is divided into five parts:

1. **InjectionContainer/Container**: binds, resolves, injects and holds dependencies. Technically, the container is a *Binder* and an *Injector* at the same time.
2. **Binder**: binds a type to another type or instance with inject conditions.
3. **Injector**: resolves and injects dependencies.
4. **Context Root**: main context in which the containers are in. Acts as an entry point for the game. It's implemented through an <a href="#extension-context-root">extension</a>.
5. **Extensions**: provides additional features to the framework.

### <a id="types-of-bindings"></a>Types of bindings

* **Transient**: a new instance is created each time a dependency needs to be resolved.
* **Singleton**: a single instance is created and used on any dependency resolution.
* **Factory**: creates the instance and returns it to the container.

## <a id="quick-start"></a>Quick start

1\. Create your own dependency injection contexts (e. g. GameContainer.cs) by inheriting from `Intentor.Adic.Container`.
   
```cs
using UnityEngine;

namespace MyNamespace {
	/// <summary>
	/// Dependency injection container.
	/// </summary>
	public class GameContainer : Intentor.Adic.Container {
		public override void SetupBindings() {
			//Setup any bindings.
		}
	}
}
```
   
2\. Setup all your dependencies on the `SetupBindings()` method.

3\. Create the context root (e. g. GameRoot.cs) of your scene by inheriting from `Intentor.Adic.ContextRoot`.

**NOTE**: there should be only one context root per scene.

**HINT**: when using a context root for each scene of your game, to make the project more organized, on `Scripts` folder create folders for each of your scenes that will hold their own scripts and context roots.
   
```cs
using UnityEngine;

namespace MyNamespace {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : Intentor.Adic.ContextRoot {
		public override void SetupContainers() {
			this.AddContainer<GameContainer>();
		}

		public override void Init() {
			//Init the game.
		}
	}
}
```
   
4\. On `SetupContainers()` method, add any contexts you may have created by calling `Add<ContainerType>()`.

5\. On the `Init()` method, place any codes to start your game.

**NOTE**: the idea of this method is to work as an entry point for your game.

6\. Attach the context root created by you on an empty GameObject at your scene.

7\. Start dependency injecting!

## <a id="api"></a>API

In development.

## <a id="pipeline"></a>Pipeline

1. Unity Awake()
2. ContextRoot calls SetupContainers()
3. ContextRoot asks for each container to generate cache for its types
4. ContextRoot calls Init()
5. Unity Start() on all MonoBehaviours
6. Injection on MonoBehaviours
7. Unity Update() is called on every object that implemented IUpdateable
8. Scene is destroyed
9. Dispose() is called on every object that implemented IDispose

## <a id="container-extensions"></a>Extensions

Extensions are a way to enhance *Adic* without having to edit it to suit different needs. By using extensions, the core of *Adic* is kept agnostic, so it can be used on any C# environment.

## <a id="available-extensions"></a>Available extensions

### <a id="extension-context-root"></a>Context Root

Provides an entry point for the game on Unity 3D.

#### Notes

1. When adding a container using `AddContainer()`, it's possible to keep it alive between scenes by setting the `destroyOnLoad` to `false`.

**Dependencies**: none

### <a id="extension-mono-injection"></a>Mono Injection

Allows injection on MonoBehaviours by provinding an `Inject` method to `UnityEngine.MonoBehaviour`.

**Dependencies**: <a href="#extension-context-root">Context Root</a>

### <a id="extension-unity-binding"></a>Unity Binding

Provides Unity 3D bindings to the container.

#### Notes

1. ALWAYS CALL Inject FROM 'Start'! (use the <a href="#extension-mono-injection">Mono Injection</a> Extension).

**Dependencies**: none

## <a id="creating-extensions"></a>Creating extensions

Extensions on *Adic* can be created in 3 ways:

1. Creating a framework extension extending the base APIs through their interfaces;
2. Creating extension methods to any part of the framework;
3. Creating a container extension, which allows for the interception of internal events, which can alter the inner workings of the framework.

Basically, to create a *container extension*, you have to:

1\. Create the extension class with `ContainerExtension` sufix.

2\. Implement `Intentor.Adic.IContainerExtension`.

3\. Subscribe to any events on the container on OnRegister method.

```cs
public void OnRegister(IInjectionContainer container) {
	container.beforeAddBinding += this.OnBeforeAddBinding;
}
```

4\. Unsubscribe to any events the extension may use on the container on OnUnregister method.

```cs
public void OnUnregister(IInjectionContainer container) {
	container.beforeAddBinding -= this.OnBeforeAddBinding;
}
```

## <a id="container-events"></a>Container events

Container events provide a way to intercept internal actions of the container and change its inner workings to suit the needs of your extension.

All events are available through `Intentor.Adic.InjectionContainer`.

### <a id="binder-events"></a>Binder events

* `beforeAddBinding`: Occurs before a binding is added.
* `afterAddBinding`: Occurs after a binding is added.
* `beforeRemoveBinding`: Occurs before a binding is removed.
* `afterRemoveBinding`: Occurs after a binding is removed.

### <a id="injector-events"></a>Injector events

* `beforeResolve`: Occurs before a type is resolved.
* `afterResolve`: Occurs after a type is resolved.
* `bindingEvaluation`: Occurs when a binding is available for resolution.
* `beforeInject`: Occurs before an instance receives injection.
* `afterInject`: Occurs after an instance receives injection.

## <a id="notes"></a>Notes

1. If an instance is not found, it will be resolved to NULL;
2. Multiple injections should occur on an array of the desired type;
3. Order of bindings is controlled by just reordering the bindings;
4. Adic relies on Unity Test Tools for unit testing. You can download it at [Unity Asset Store](https://www.assetstore.unity3d.com/#!/content/13802).

## <a id="examples"></a>Examples

There are some examples that are bundled to the main package that teach the basics and beyond of *Adic*.

**NOTE**: these examples are not yet implemented on the current version.

### 1. Hello World

Exemplifies the basics of how to setup a scene for dependency injection using the ContextRoot.

### 2. Binding Game Objects

Exemplifies how to bind components to new and existing game objects and allows them to share dependencies.

### 3. Using conditions

Exemplifies the use of condition identifiers on injections.

### 4. Prefabs

Exemplifies how to bind to prefabs and the use of PostConstruct as a second constructor.

### 5. Commander

Exemplifies the binding and using of commands to execute complex actions.

### 6. InjectCrush

A very simple Candy Crush like game to exemplify the complete use of the framework and its extensions.

## <a id="support"></a>Support

Found a bug? Please create an issue on the [GitHub project page](https://github.com/intentor/adic/) or send a pull request if have a fix or extension.

You can also send me a message at andre@intentor.com.br to discuss more obscure matters about *Adic*.

## <a id="license"></a>License

Licensed under the [The MIT License (MIT)](http://opensource.org/licenses/MIT). Please read LICENSE for more information.