# ![Adic](https://cloud.githubusercontent.com/assets/5340818/6597415/4b95cb42-c7db-11e4-863f-9a284bfab310.png)

***Another* Dependency Injection Container for Unity and beyond**

Currently the project is **DISCONTINUED**. However, feel free to fork it and continue its development!

[![Unity Asset Store](https://cloud.githubusercontent.com/assets/5340818/6855739/9e14c9e0-d3d9-11e4-9171-594941ed056f.png)](https://assetstore.unity.com/packages/tools/adic-dependency-injection-container-32157) [![Donate](https://cloud.githubusercontent.com/assets/5340818/12418027/9434b3ea-be93-11e5-8395-253a3a1aade5.png)](http://donate.intentor.com.br/)

## Contents

1. <a href="#introduction">Introduction</a>
2. <a href="#features">Features</a>
3. <a href="#concepts">Concepts</a>
	1. <a href="#about-di">About dependency injection (DI)</a>
		1. <a href="#what-is">What is a DI container?</a>
		2. <a href="#why-use-it">Why use a DI container?</a>
		3. <a href="#why-use-with-unity">Why use it with Unity?</a>
		4. <a href="#common-use-cases">Common use cases</a>
		5. <a href="#further-readings">Further readings</a>
	2. <a href="#structure">Structure</a>
	3. <a href="#types-of-bindings">Types of bindings</a>
	4. <a href="#namespace-conventions">Namespace conventions</a>
	5. <a href="#chaining">Chaining</a>
4. <a href="#quick-start">Quick start</a>
5. <a href="#api">API</a>
	1. <a href="#bindings">Bindings</a>
	2. <a href="#constructor-injection">Constructor injection</a>
	4. <a href="#member-injection">Member injection</a>
	3. <a href="#method-injection">Method injection</a>
	5. <a href="#multiple-constructors">Multiple constructors</a>
	6. <a href="#multiple-injection">Multiple injection</a>
	7. <a href="#behaviour-injection">Behaviour injection</a>
		1. <a href="#monobehaviour-injection">MonoBehaviour injection</a>
		2. <a href="#statemachinebehaviour-injection">StateMachineBehaviour injection</a>
		3. <a href="#scene-injection">Scene injection</a>
		4. <a href="#injecting-multiple-containers">Injecting from multiple containers</a>
	8. <a href="#conditions">Conditions</a>
	9. <a href="#tags">Tags</a>
	10. <a href="#update">Update</a>
	11. <a href="#dispose">Dispose</a>
	12. <a href="#instance-resolution-modes">Instance resolution modes</a>
	13. <a href="#manual-type-resolution">Manual type resolution</a>
	14. <a href="#factories">Factories</a>
	15. <a href="#bindings-setup">Bindings setup</a>
	16. <a href="#using-commands">Using commands</a>
6. <a href="#multiple-scenes">Multiple scenes</a>
7. <a href="#order-of-events">Order of events</a>
8. <a href="#script-execution-order">Script execution order</a>
9. <a href="#performance">Performance</a>
10. <a href="#il2cpp-aot">IL2CPP, AOT and code stripping</a>
11. <a href="#general-notes">General notes</a>
12. <a href="#container-extensions">Extensions</a>
	1. <a href="#available-extensions">Available extensions</a>
		1. <a href="#extension-bindings-printer">Bindings Printer</a>
		1. <a href="#extension-bindings-setup">Bindings Setup</a>
		2. <a href="#extension-commander">Commander</a>
		3. <a href="#extension-context-root">Context Root</a>
		4. <a href="#extension-event-caller">Event Caller</a>
		5. <a href="#extension-mono-injection">Mono Injection</a>
		5. <a href="#extension-state-injection">State Injection</a>
		6. <a href="#extension-unity-binding">Unity Binding</a>
	2. <a href="#creating-extensions">Creating extensions</a>
	3. <a href="#container-events">Container events</a>
		1. <a href="#binder-events">Binder events</a>
		2. <a href="#injector-events">Injector events</a>
13. <a href="#binaries">Binaries</a>
14. <a href="#examples">Examples</a>
15. <a href="#changelog">Changelog</a>
16. <a href="#support">Support</a>
17. <a href="#license">License</a>

## <a id="introduction"></a>Introduction

*Adic* is a lightweight dependency injection container for Unity and any C# (or .Net) project.

Based on the proof of concept container from [Sebastiano Mandalà](http://blog.sebaslab.com/ioc-container-for-unity3d-part-1/) and studies of [StrangeIoC](http://strangeioc.github.io/strangeioc/), the intention of the project is to create a dependency injection container that is simple to use and extend, having on its roots the simplicity of the work of Mandalà and the extensibility of StrangeIoC, also borrowing some ideas from the classic [Unity Application Block](https://unity.codeplex.com/).

The project is compatible with Unity 5 and 4. Tested on Windows/Mac/Linux, Android, iOS, WP10 (IL2CPP), Web Player and WebGL.

Also available on the [Unity Asset Store](https://www.assetstore.unity3d.com/en/#!/content/32157).

## <a id="features"></a>Features

* Bind types, singleton instances, factories, game objects and prefabs.
* Instance resolution by type, identifier and complex conditions.
* Injection on constructor, fields and properties.
* Can inject multiple objects of the same type.
* Can inject on non public members.
* Can resolve and inject instances from types that are not bound to the container.
* Can inject automatically on components of a scene.
* Fast dependency resolution with internal cache.<a href=#performance>\*</a>
* Use of attributes to indicate injections, preferable constructors and post constructors.
* Can be easily extended through extensions.
* Framework decoupled from Unity - all Unity based API is achieved through extensions.
* Organized and well documented code written in C#.

## <a id="concepts"></a>Concepts

### <a id="about-di"></a>About dependency injection (DI)

#### <a id="what-is"></a>What is a DI container?

A *dependency injection container* is a piece of software that handles the resolution of dependencies in objects. It's related to the [dependency injection](http://en.wikipedia.org/wiki/Dependency_injection) and [inversion of control](http://en.wikipedia.org/wiki/Inversion_of_control) design patterns.

The idea is that any dependency an object may need should be resolved by an external entity rather than the own object. Practically speaking, an object should not use `new` to create the objects it uses, having those instances *injected* into it by another object whose sole existence is to resolve dependencies.

So, a *dependency injection container* holds information about dependencies (the *bindings*) that can be injected into another objects by demand (injecting into existing objects) or during resolution (when you are creating a new object of some type).

#### <a id="why-use-it"></a>Why use a DI container?

In a nutshell, **to decouple your code**.

A DI container, in pair with a good architecture, can ensure [SOLID principles](http://en.wikipedia.org/wiki/SOLID_%28object-oriented_design%29) and help you write better code.

Using such container, you can easily work with abstractions without having to worry about the specifics of each external implementation, focusing just on the code you are writing. It's all related to dependencies: any dependency your code needs is not resolved directly by your code, but externally, allowing your code to deal only with its responsibilities.

As a plus, there are other benefits from using a DI container:

1. **Refactorability**: with your code decoupled, it's easy to refactor it without affecting the entire codebase.
2. **Reusability**: thinking about abstractions allows your code to be even more reusable by making it small and focused on a single responsibility.
3. **Easily change implementations**: given all dependencies are configured in the container, it's easy to change a implementation for a given abstraction. It helps e.g. when implementing generic functionality in a platform specific way.
4. **Testability**: by focusing on abstractions and dependency injection, it's easy to replace implementations with mock objects to test your code.
5. **Improved architecture**: your codebase will be naturally better and more organized because you'll think about the relationships of your code.
6. **Staying sane**: by focusing on small parts of the code and having a consistent architecture, the sanity of the developer is also ensured!

#### <a id="why-use-with-unity"></a>Why use it with Unity?

Unity is not SOLID friendly out of the box. Even the official examples may give a wrong idea on how to code on Unity. Using a DI container in conjunction with Unity, it's possible to write code that is more extensible, reusable and less `MonoBehaviour` centric (in most cases, a regular class can do just fine or better).

This way your code can become more modular and your components less tightly coupled to each other.

#### <a id="common-use-cases"></a>Common use cases

##### Class dependency

Imagine you class depend on a given service that provides some action it may need:

```cs
public class MyClass {
	public void DoAction() {
		var service = new SomeService();
		service.SomeAction();
	}
}
```

If in the future you need to change the implementation of the service, you'll have to get back to the class and change it. It can work just fine for small projects, but as the codebase grows, it can become a (error prone) nightmare to chase all these references.

So, you can change to a more decoupled code, making `MyClass` not having to worry about the specific implementation of `SomeService` it uses:

```cs
public class MyClass {
	private IService service;

	public MyClass(IService service) {
		this.service = service;
	}

	public void DoAction() {
		this.service.SomeAction();
	}
}
```

The idea is that you invert the resolution of the dependency up into the execution flow.

Now, any class that needs to use `MyClass` also has to to provide a service reference to it by constructor:

```cs
public class MyOtherClass {
	private IService service;

	public MyOtherClass(IService service) {
		this.service = service;
	}

	public void DoAction() {
		var myClass = new MyClass(this.service)
		myClass.DoAction();
	}
}
```

But you could write it even better: given `MyOtherClass` depends only on `MyClass` (`IService` is just a *tramp variable* - a variable that is there to be passed to other object), there's no need to store a reference to the `IService` object:

```cs
public class MyOtherClass {
	private MyClass myClass;

	public MyOtherClass(MyClass myClass) {
		this.myClass = myClass;
	}

	public void DoAction() {
		this.myClass.DoAction();
	}
}
```

However, any class that uses `MyOtherClass` must also fullfill any dependencies it needs, again up into the execution flow, until a place where all the dependencies are resolved. This place is called the [composition root](http://blog.ploeh.dk/2011/07/28/CompositionRoot/).

And that's where a DI container come in handy. In the composition root, a DI container is created and configured to resolve and wire all dependencies of any objects used by your code so you don't have to worry about it!

#### <a id="further-readings"></a>Further readings

- [IoC container solves a problem you might not have but it's a nice problem to have](http://kozmic.net/2012/10/23/ioc-container-solves-a-problem-you-might-not-have-but-its-a-nice-problem-to-have/)
- [IoC Container for Unity3D – part 1](http://www.sebaslab.com/ioc-container-for-unity3d-part-1/)
- [IoC Container for Unity3D – part 2](http://www.sebaslab.com/ioc-container-for-unity3d-part-2/)
- [The truth behind Inversion of Control – Part I – Dependency Injection](http://www.sebaslab.com/the-truth-behind-inversion-of-control-part-i-dependency-injection/)
- [The truth behind Inversion of Control – Part II – Inversion of Control](http://www.sebaslab.com/the-truth-behind-inversion-of-control-part-ii-inversion-of-control/)
- [The truth behind Inversion of Control – Part III – Entity Component Systems](http://www.sebaslab.com/the-truth-behind-inversion-of-control-part-iii-entity-component-systems/)
- [The truth behind Inversion of Control – Part IV – Dependency Inversion Principle](http://www.sebaslab.com/the-truth-behind-inversion-of-control-part-iii-entity-component-systems/)
- [From STUPID to SOLID Code!](http://williamdurand.fr/2013/07/30/from-stupid-to-solid-code/)

### <a id="structure"></a>Structure

The structure of *Adic* is divided into five parts:

1. **InjectionContainer/Container:** binds, resolves, injects and holds dependencies. Technically, the container is a *Binder* and an *Injector* at the same time.
2. **Binder:** binds a type to another type or instance with inject conditions.
3. **Injector:** resolves and injects dependencies.
4. **Context Root:** main context in which the containers are in. Acts as an entry point for the game. It's implemented through an <a href="#extension-context-root">extension</a>.
5. **Extensions:** provides additional features to the framework.

### <a id="types-of-bindings"></a>Types of bindings

* **Transient:** a new instance is created each time a dependency needs to be resolved.
* **Singleton:** a single instance is created and used on any dependency resolution.
* **Factory:** creates the instance and returns it to the container.

### <a id="namespace-conventions"></a>Namespace conventions

*Adic* is organized internally into different namespaces that represents the framework components. However, the commonly used components are under `Adic` namespace:

1. `Inject` attribute;
2. `InjectionContainer`;
3. `IFactory`;
4. Extensions (like `ContextRoot` and `UnityBinding`).

### <a id="chaining"></a>Chaining

Methods from the container and bindings creation can be chained to achieve a more compact code:

```cs
//Create the container.
this.AddContainer<InjectionContainer>()
	//Register any extensions the container may use.
	.RegisterExtension<CommanderContainerExtension>()
	.RegisterExtension<EventCallerContainerExtension>()
	.RegisterExtension<UnityBindingContainerExtension>()
	//Add bindings.
    .Bind<Type1>.To<AnotherType1>()
    .Bind<Type2>.To<AnotherType2>().As("Identifier")
    .Bind<Type3>.ToGameObject("GameObjectName").AsObjectName()
    .Bind<Type4>.ToSingleton<AnotherType3>();
```

**Good practice:** when chaining, always place the bindings in the end of the chain or use <a href="#bindings-setup">bindings setup</a> to organize your bindings.

## <a id="quick-start"></a>Quick start

1\. Create the context root (e.g. GameRoot.cs) of your scene by inheriting from `Adic.ContextRoot` and attaching it to an empty game object.

```cs
using UnityEngine;

namespace MyNamespace {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : Adic.ContextRoot {
		public override void SetupContainers() {
			//Setup the containers.
		}

		public override void Init() {
			//Init the game.
		}
	}
}
```

**Hint:** when using a context root for each scene of your game, to make the project more organized, create folders for each of your scenes that will hold their own scripts and context roots.

2\. In the `SetupContainers()` method, create and add any containers you may need, also configuring their bindings.

```cs
public override void SetupContainers() {
	//Create a container.
	this.AddContainer<InjectionContainer>()
		//Setup bindinds.
		.Bind<Whatever>().ToSelf();
}
```

**Attention:** the order in which you place the bindings is very important. If class A requires an injection of class B, class B should be bound to the container before class A.

**Hint:** in *Adic*, the lifetime of your bindings is the lifetime of your containers - you can create as many containers as you want to hold your dependencies.

**Good practice:** if you have many bindings to add to a container, it's better to create reusable objects that can setup related bindings together. Please see <a href="#bindings-setup">Bindings setup</a> for more information.

3\. On the `Init()` method, place any code to start your game.

**Note:** the idea of this method is to work as an entry point for your game, like a `main()` method on console applications.

4\. Attach the context root created by you on an empty game object in your scene.

5\. Start dependency injecting!

## <a id="api"></a>API

### <a id="bindings"></a>Bindings

Binding is the action of linking a type to another type or instance. *Adic* makes it simple by providing different ways to create your bindings.

Every binding must occur from a certain key type by calling the `Bind()` method of the container.

The simple way to bind e.g. some interface to its class implementation is as below:

```cs
container.Bind<SomeInterface>().To<ClassImplementation>();
```

It's also possible to bind a class to an existing instance:

```cs
container.Bind<SomeInterface>().To(someInstance);
```

You can also bind a Unity component to a game object that has that particular component:

```cs
// By name...
container.Bind<Transform>().ToGameObject("GameObjectNameOnHierarchy");
// ...or by passing the actual GameObject.
container.Bind<Transform>().ToGameObject(myGameObject);
```

Or a loaded prefab:

```cs
container.Bind<Transform>().ToPrefab(myPrefab);
```

And, if needed, non-generics versions of bindings' methods are also available:

```cs
container.Bind(someType).To(anotherType);
```

The next sections will cover all the available bindings *Adic* provides.

#### To Self

Binds the key type to a transient of itself. The key must be a class.

```cs
container.Bind<ClassType>().ToSelf();
```

#### To Singleton

Binds the key type to a singleton of itself. The key must be a class.

```cs
container.Bind<ClassType>().ToSingleton();
```

It's also possible to create a singleton of the key type to another type. In this case, the key may not be a class.

```cs
//Using generics...
container.Bind<InterfaceType>().ToSingleton<ClassType>();
//...or instance type.
container.Bind<InterfaceType>().ToSingleton(classTypeObject);
```

#### To another type

Binds the key type to a transient of another type. In this case, the *To* type will be instantiated every time a resolution of the key type is asked.

```cs
//Using generics...
container.Bind<InterfaceType>().To<ClassType>();
//..or instance type.
container.Bind<InterfaceType>().To(classTypeObject);
```

#### To instance

Binds the key type to an instance.

```cs
//Using generics...
container.Bind<InterfaceType>().To<ClassType>(instanceOfClassType);
//..or instance type.
container.Bind<InterfaceType>().To(classTypeObject, instanceOfClassType);
```

#### To all types in a namespace as transient

Binds the key type to all assignable types in a given namespace as transient bindings.

**Note 1:** it will create a <a href="#multiple-injection">multiple binding</a> if there's more than one type in the namespace that is assignable to the key type.

**Note 2:** currently it's not possible to use conditions when binding to all types in a namespace.

```cs
// Only the informed namespace.
container.Bind<SomeType>().ToNamespace("MyNamespace.Whatever");

// Including children namespaces.
container.Bind<SomeType>().ToNamespace("MyNamespace.Whatever", true);
```

#### To all types in a namespace as singleton

Binds the key type to all assignable types in a given namespace as singleton bindings.

**Note 1:** it will create a <a href="#multiple-injection">multiple binding</a> if there's more than one type in the namespace that is assignable to the key type.

**Note 2:** currently it's not possible to use conditions when binding to all types in a namespace.

```cs
// Only the informed namespace.
container.Bind<SomeType>().ToNamespaceSingleton("MyNamespace.Whatever");

// Including children namespaces.
container.Bind<SomeType>().ToNamespaceSingleton("MyNamespace.Whatever", true);
```

#### To a Factory

Binds the key type to a factory. The factory must implement the `Adic.IFactory` interface.

```cs
//Binding factory by generics...
container.Bind<InterfaceType>().ToFactory<Factory>();
//...or type instance...
container.Bind<InterfaceType>().ToFactory(typeFactory);
//...or a factory instance.
container.Bind<InterfaceType>().ToFactory(factoryInstance);
```

See <a href="#factories">Factories</a> for more information.

#### To game object

Binds the key type to a singleton of itself or some type on a new game object.

**Good practice:** to prevent references to destroyed objects, only bind to game objects that won't be destroyed in the scene.

```cs
//Binding to itself...
container.Bind<SomeMonoBehaviour>().ToGameObject();
//...or some other component using generics...
container.Bind<SomeInterface>().ToGameObject<SomeMonoBehaviour>();
//..or some other component by instance type.
container.Bind<SomeInterface>().ToGameObject(someMonoBehaviourType);
```

The newly created game object will have the same name as the key type.

#### To game object by name

Binds the key type to a singleton `UnityEngine.Component` of itself or some type on a game object of a given name.

**Good practice:** to prevent references to destroyed objects, only bind to game objects that won't be destroyed in the scene.

If the component is not found on the game object, it will be added.

##### By name

```cs
//Binding to itself by name...
container.Bind<SomeMonoBehaviour>().ToGameObject("GameObjectName");
//...or some other component using generics and name...
container.Bind<SomeInterface>().ToGameObject<SomeMonoBehaviour>("GameObjectName");
//..or some other component by instance type and name.
container.Bind<SomeInterface>()().ToGameObject(someMonoBehaviourType, "GameObjectName");
```

##### By instance

```cs
//Binding to itself by instance...
container.Bind<SomeMonoBehaviour>().ToGameObject(myGameObject);
//...or some other component using generics and instance...
container.Bind<SomeInterface>().ToGameObject<SomeMonoBehaviour>(myGameObject);
//..or some other component by instance type and instance.
container.Bind<SomeInterface>()().ToGameObject(someMonoBehaviourType, myGameObject);
```

#### To game object with tag

Binds the key type to a singleton `UnityEngine.Component` of itself or some type on a game object of a given tag.

**Good practice:** to prevent references to destroyed objects, only bind to game objects that won't be destroyed in the scene.

If the component is not found on the game object, it will be added.

```cs
//Binding to itself by tag...
container.Bind<SomeMonoBehaviour>().ToGameObjectWithTag("Tag");
//...or some other component using generics and tag...
container.Bind<SomeInterface>().ToGameObjectWithTag<SomeMonoBehaviour>("Tag");
//..or some other component by instance type and tag.
container.Bind<SomeInterface>().ToGameObjectWithTag(someMonoBehaviourType, "Tag");
```

#### To game objects with tag

Binds the key type to singletons `UnityEngine.Component` of itself or some type on a game object of a given tag.

**Good practice:** to prevent references to destroyed objects, only bind to game objects that won't be destroyed in the scene.

If the component is not found on the game object, it will be added.

```cs
//Binding to itself by tag...
container.Bind<SomeMonoBehaviour>().ToGameObjectsWithTag("Tag");
//...or some other component using generics and tag...
container.Bind<SomeInterface>().ToGameObjectsWithTag<SomeMonoBehaviour>("Tag");
//..or some other component by instance type and tag.
container.Bind<SomeInterface>().ToGameObjectsWithTag(someMonoBehaviourType, "Tag");
```

#### To prefab transient

Binds the key type to a transient `UnityEngine.Component` of itself or some type on the prefab.

If the component is not found on the game object, it will be added.

**Note:** every resolution of a transient prefab will generate a new instance. So, even if the component resolved from the prefab is destroyed, it won't generate any missing references in the container.

```cs
//Binding prefab to itself...
container.Bind<SomeMonoBehaviour>().ToPrefab(myPrefab);
//...or to another component on the prefab using generics...
container.Bind<SomeInterface>().ToPrefab<SomeMonoBehaviour>(myPrefab);
//...or to another component on the prefab using instance tyoe.
container.Bind<SomeInterface>().ToPrefab(someMonoBehaviourType, myPrefab);
```

#### To prefab singleton

Binds the key type to a singleton `UnityEngine.Component` of itself or some type on a newly instantiated prefab.

**Good practice:** to prevent references to destroyed objects, only bind to prefabs that won't be destroyed in the scene.

```cs
//Binding singleton prefab to itself...
container.Bind<SomeMonoBehaviour>().ToPrefabSingleton(myPrefab);
//...or to another component on the prefab using generics...
container.Bind<SomeInterface>().ToPrefabSingleton<SomeMonoBehaviour>(myPrefab);
//...or to another component on the prefab using instance type.
container.Bind<SomeInterface>().ToPrefabSingleton(someMonoBehaviourType, myPrefab);
```

### <a id="constructor-injection"></a>Constructor injection

*Adic* will always try to resolve any dependencies the constructor may need by using information from its bindings or trying to instantiate any types that are unknown to the binder.

There's no need to decorate constructors' parameteres with `Inject` attributes - they will be resolved automatically. However, if you are using identified parameters, you should use the `Inject` attribute:

```cs
namespace MyNamespace {
	/// <summary>
	/// My class summary.
	/// </summary>
	public class MyClass {
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="param1">Identified parameter description.</param>
		/// <param name="param2">Parameter description.</param>
		public MyClass([Inject("Identifier")] param1, SomeType param2) {
			...
		}
	}
}
```

**Note:** if there's more than one constructor, *Adic* always look for the one with less parameteres. However, <a href="#multiple-constructors">it's possible to indicate which constructor should be used</a> on a multi constructor class.

### <a id="member-injection"></a>Member injection

*Adic* can perform dependency injection on public and non public (private, protected and internal) fields and properties of classes. To make it happen, just decorate the members with the `Inject` attribute:

```cs
namespace MyNamespace {
	/// <summary>
	/// My class summary.
	/// </summary>
	public class MyClass {
		/// <summary>Field to be injected.</summary>
		[Inject]
		public SomeClass fieldToInject;
		/// <summary>Field NOT to be injected.</summary>
		public SomeClass fieldNotToInject;

		/// <summary>Property to be injected.</summary>
		[Inject]
		private SomeOtherClass propertyToInject { get; set; }
		/// <summary>Property NOT to be injected.</summary>
		private SomeOtherClass propertyNotToInject { get; set; }
	}
}
```

**Attention:** injecting on reference objects will only occur if the object has no value (its value is `null`).

### <a id="method-injection"></a>Method injection

Method injection works like constructor injection, but on methods:

```cs
namespace MyNamespace {
	/// <summary>
	/// My class summary.
	/// </summary>
	public class MyClass {
		/// <summary>
		/// Injected method, called after all the dependencies have been resolved.
		/// </summary>
		/// <param name="param1">Identified parameter description.</param>
		/// <param name="param2">Parameter description.</param>
		[Inject]
		public void MyMethod1([Inject("Identifier")] SomeType param1, SomeType param2) {
			...
		}

		/// <summary>
		/// Injected method, called after all the dependencies have been resolved.
		/// </summary>
		/// <param name="param">Parameter description.</param>
		[Inject]
		private void MyMethod2(SomeType param) {
			...
		}
	}
}
```

Method injection occurs after all injections on the class. So, it's possible to use it as a post constructor:

```cs
namespace MyNamespace {
	/// <summary>
	/// My class summary.
	/// </summary>
	public class MyClass {
		/// <summary>Field to be injected.</summary>
		[Inject]
		private SomeClass fieldToInject;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public MyClass() {
			...
		}

		/// <summary>
		/// Injected method acting as a post constructor.
		/// </summary>
		[Inject]
		private void PostConstructor() {
			...
		}
	}
}
```

**Good practice:** injected methods can be used as constructors on `MonoBehaviour` components.

**Good practice:** injected methods can be used to perform initilizations or configurations on objects.

**Good practice:** always use non-generic injected methods to prevent [`JIT compile method`](http://docs.unity3d.com/Manual/TroubleShootingIPhone.html) exceptions on [AOT platforms](https://en.wikipedia.org/wiki/Ahead-of-time_compilation) (like iOS and WebGL).

### <a id="multiple-constructors"></a>Multiple constructors

In case you have multiple constructors, it's possible to indicate to *Adic* which one should be used by decorating it with the `Inject` attribute:

```cs
namespace MyNamespace {
	/// <summary>
	/// My class summary.
	/// </summary>
	public class MyClass {
		/// <summary>
		/// Class constructor.
		/// </summary>
		public MyClass() {
			...
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parameterName">Parameter description</param>
		[Inject]
		public MyClass(Type parameterName) {
			...
		}
	}
}
```

### <a id="multiple-injection"></a>Multiple injection

It's possible to inject multiple objects of the same type by creating a series of bindings of the same key type. In this case, the injection occurs on an array of the key type.

Binding multiple objects to the same key:

```cs
container
	.Bind<GameObject>().ToGameObject("Enemy1")
	.Bind<GameObject>().ToGameObject("Enemy2")
	.Bind<GameObject>().ToGameObject("Enemy3")
	.Bind<GameObject>().ToGameObject("Enemy4")
	.Bind<GameObject>().ToGameObject("Enemy5");
```

Multiple injection in a field:

```cs
namespace MyNamespace {
	/// <summary>
	/// My class summary.
	/// </summary>
	public class MyClass {
		/// <summary>Enemies on the game.</summary>
		[Inject]
		public GameObject[] enemies;
	}
}
```

It's possible to manually resolve multiple objects. Please see <a href="#manual-type-resolution">Manual type resolution</a> for more information.

### <a id="behaviour-injection"></a>Behaviour injection

It's possible to perform injection on custom `MonoBehaviour` and `StateMachineBehaviour` scripts through the extensions <a href="#extension-mono-injection">Mono Injection</a> and <a href="#extension-state-injection">State Injection</a>, which are enabled by default.

#### <a id="monobehaviour-injection"></a>MonoBehaviour injection

To perform injection on custom `MonoBehaviour` fields and properties, simply call the `Inject()` extension method of the `MonoBehaviour`:


```cs
using Unity.Engine;

namespace MyNamespace {
	/// <summary>
	/// My MonoBehaviour summary.
	/// </summary>
	public class MyBehaviour : MonoBehaviour {
		/// <summary>Field to be injected.</summary>
		[Inject]
		public SomeClass fieldToInject;

		protected void Start() {
			this.Inject();
		}
	}
}
```

##### <a id="base-monobehaviour"></a>Base MonoBehaviour

To make injection even simpler, create a base behaviour from which all your `MonoBehaviour` can inherit, calling the `Inject()` method during `Start()`:

```cs
using Unity.Engine;

namespace MyNamespace {
	/// <summary>
	/// Base MonoBehaviour.
	/// </summary>
	public abstract class BaseBehaviour : MonoBehaviour {
		/// <summary>
		/// Called when the component is starting.
		///
		/// If overriden on derived classes, always call base.Start().
		/// </summary>
		protected virtual void Start() {
			this.Inject();
		}
	}
}
```

#### <a id="statemachinebehaviour-injection"></a>StateMachineBehaviour injection

To perform injection on custom `StateMachineBehaviour` fields and properties, simply call the `Inject()` extension method on any of the state events:

```cs
using Unity.Engine;

namespace MyNamespace {
	/// <summary>
	/// My StateMachineBehaviour summary.
	/// </summary>
	public class MyStateMachineBehaviour : StateMachineBehaviour {
		/// <summary>Field to be injected.</summary>
		[Inject]
		public SomeClass fieldToInject;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			this.Inject();
		}
	}
}
```

**Note:** only available on Unity 5+.

##### <a id="base-statemachinebehaviour"></a>Base StateMachineBehaviour

To make injection even simpler, create a base behaviour from which all your `StateMachineBehaviour` can inherit:

```cs
using Unity.Engine;

namespace MyNamespace {
	/// <summary>
	/// Base StateMachineBehaviour.
	/// </summary>
	public abstract class BaseStateBehaviour : StateMachineBehaviour {
		/// <summary>
		/// Behaviour constructor.
		/// </summary>
		public BaseStateBehaviour() {
			this.Inject();
		}
	}
}
```

#### <a id="scene-injection"></a>Scene injection

On some performance sensitive games it's important to ensure that every injection occurs before the game starts, in a scene level. *Adic* provides three ways to perform a scene wide injection, which are configured by selecting the appropriate injection type on the <a href="#extension-context-root">Context Root</a> inspector.

**Note:** there's no better or worse strategy. It only depends on the game you are working on and developer preferences, given all strategies can achieve the same performance goals if all objects used during the game are created before the game starts. Read the <a href="#performance">Performance</a> section for more performance considerations.

##### Manual

The injection is performed manually, without a scene wide automatic injection. This is the default setting.

It's recommended to use a <a id="base-monobehaviour">base MonoBehaviour</a> to perform injections.

##### Children

The injection is performed on all `MonoBehaviour` or any `MonoBehaviour` derived type (e.g. a <a id="base-monobehaviour">base `MonoBehaviour`</a>) that are children of the context root.

##### Base type

The injection is performed on all `MonoBehaviour` or any `MonoBehaviour` derived type (e.g. a <a id="base-monobehaviour">base `MonoBehaviour`</a>) throughout the scene.

#### <a id="injecting-multiple-containers"></a>Injecting from multiple containers

When injecting into `MonoBehaviour`/`StateMachineBehaviour` using the `this.Inject()` method, every available container in the <a href="#quick-start">context root</a> is used. If you want to restrict the containers from which injection occurs, use the `InjectFromContainer` attribute in conjunction with a container identifier.

##### Setting a container identifier

When creating the container, set an identifier through its constructor:

```cs
//Create the container with an identifier.
this.AddContainer(new InjectionContainer("identifier"))
	//Register any extensions the container may use.
	.RegisterExtension<UnityBindingContainerExtension>();
```

**Good practice:** identifiers can be any object. However, it's recommended to use only strings and enums.

##### Adding the attribute

In the `MonoBehaviour`/`StateMachineBehaviour` that should receive injection only from a certain container, add the `InjectFromContainer` attribute with the container's identifier:

##### MonoBehaviour

```cs
using Unity.Engine;

namespace MyNamespace {
	/// <summary>
	/// My MonoBehaviour summary.
	/// </summary>
	[InjectFromContainer("identifier")]
	public class MyBehaviour : MonoBehaviour {
		/// <summary>Field to be injected.</summary>
		[Inject]
		public SomeClass fieldToInject;

		protected void Start() {
			this.Inject();
		}
	}
}
```

##### StateMachineBehaviour

```cs
using Unity.Engine;

namespace MyNamespace {
	/// <summary>
	/// My StateMachineBehaviour summary.
	/// </summary>
	[InjectFromContainer("identifier")]
	public class MyStateMachineBehaviour : StateMachineBehaviour {
		/// <summary>Field to be injected.</summary>
		[Inject]
		public SomeClass fieldToInject;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			this.Inject();
		}
	}
}
```

### <a id="conditions"></a>Conditions

Conditions allow a more customized approach when injecting dependencies into constructors and fields/properties.

Using conditions you can:

1\. Identify a binding with an identifier, so you can indicate it as a parameter in the `Inject` attribute on constructors and fields/properties:

When binding:

```cs
container.Bind<SomeInterface>().To<SomeClass>().As("Identifier");
```

When binding to Unity Objects, it's possible to use the object name automatically as the binding:

```cs
container.Bind<SomeInterface>().ToGameObject("GameObjectName").AsObjectName();
```

When injecting into constructor parameters:

```cs
namespace MyNamespace {
	/// <summary>
	/// My class summary.
	/// </summary>
	public class MyClass {
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parameter">Parameter to be injected.</param>
		public MyClass([Inject("Identifier")] SomeInterface parameter) {
			//Code.
		}
	}
}
```

When injecting into fields/properties:

```cs
namespace MyNamespace {
	/// <summary>
	/// My class summary.
	/// </summary>
	public class MyClass {
		/// <summary>Field to be injected.</summary>
		[Inject("Identifier")]
		public SomeInterface field;

		/// <summary>Property to be injected.</summary>
		[Inject("Identifier")]
		public SomeInterface property { get; set; }
	}
}
```

**Good practice:** identifiers can be any object. However, it's recommended to use only strings and enums.

2\. Indicate in which objects a binding can be injected, by type or instance:

```cs
//Using generics...
container.Bind<SomeInterface>().To<SomeClass>().WhenInto<MyClass>();
//...or type instance...
container.Bind<SomeInterface>().To<SomeClass>().WhenInto(myClassInstanceType);
//...or by a given instance.
container.Bind<SomeInterface>().To<SomeClass>().WhenIntoInstance(myClassInstanceType);
```

3\. Create complex conditions by using an anonymous method:

```cs
container.Bind<SomeInterface>().To<SomeClass>().When(context =>
		context.member.Equals(InjectionMember.Field) &&
        context.parentType.Equals(typeof(MyClass))
	);
```

The context provides the following fields:

1. **member** (`Adic.InjectionMember` enum): the class member in which the injection is occuring (*None*, *Constructor*, *Field* or *Property*).
2. **memberType** (`System.Type`): the type of the member in which the injection is occuring.
3. **memberName** (`string`): the name of the member in which the injection is occuring.
4. **identifier** (`object`): the identifier of the member in which the injection is occuring (from `Inject` attribute).
5. **parentType** (`System.Type`): the type of the object in which the injection is occuring.
6. **parentInstance** (`object`): the instance of the object in which the injection is occuring.
7. **injectType** (`System.Type`): the type of the object being injected.

### <a id="tags"></a>Tags

Tags can be used to easily unbind bindings.

1\. Tag a binding with as many tags as required:

```cs
container.Bind<SomeInterface>().To<SomeClass>().As("Identifier").Tag("Tag1", "Tag2", "Tag3");
```

2\. Unbind all bindings with a given tag:

```cs
container.UnbindByTag("Tag1");
```

### <a id="update"></a>Update

It's possible to have an `Update()` method on regular classes (that don't inherit from `MonoBehaviour`) by implementing the `Adic.IUpdatable` interface.

See <a href="#extension-event-caller">Event Caller</a> for more information.

### <a id="dispose"></a>Dispose

When a scene is destroyed, it's possible to have a method that can be called to e.g. free up resources. To do it, implement the `System.IDisposable` interface on any class that you want to have this option.

See <a href="#extension-event-caller">Event Caller</a> for more information.

### <a id="instance-resolution-modes"></a>Instance resolution modes

*Adic* provides two instance resolution modes:

1. **ALWAYS_RESOLVE** (default): always try to resolve every type that requires injection, even ones that are not bound to the container.
2. **RETURN_NULL**: only resolves types that are bound to the container. Trying to resolve a non-bound type will return a null reference.

#### Setting a resolution mode

Instance resolution modes can be configured through the `Adic.InjectionContainer` constructor or by changing the `resolutionMode` property:

```cs
//Setting a resolution mode through constructor...
var container = new InjectionContainer(ResolutionMode.RETURN_NULL);
//...and changing it through property.
container.resolutionMode = ResolutionMode.ALWAYS_RESOLVE;
```

### <a id="manual-type-resolution"></a>Manual type resolution

If you need to get a type from the container but do not want to use injection through constructor or fields/properties, it's possible to execute a manual resolution directly by calling the `Resolve()` method:

```cs
//Resolving using generics...
var instance = container.Resolve<Type>();
//...or using generics for objects with a given identifier...
var instance = container.Resolve<Type>("Identifier");
//...or by type instance...
instance = container.Resolve(typeInstance);
//...or by objects with a given identifier...
instance = container.Resolve("Identifier");
//...or by type instance for objects with a given identifier.
instance = container.Resolve(typeInstance, "Identifier");
```

It's also possible to resolve all objects of a given type:

```cs
//Resolving all objects using generics...
var instances = container.ResolveAll<Type>();
//...or using generics for objects with a given identifier...
var instance = container.ResolveAll<Type>("Identifier");
//...or by type instance...
instances = container.ResolveAll(typeInstance);
//...or by objects with a given identifier...
instance = container.ResolveAll("Identifier");
//...or by type instance for objects with a given identifier.
instance = container.ResolveAll(typeInstance, "Identifier");
```

**Note:** although it's possible to resolve instances by identifier, currently manual resolution of bindings that have other conditions is not supported.

### <a id="factories"></a>Factories

When you need to handle the instantiation of an object manually, it's possible to create a factory class by inheriting from `Adic.IFactory`:

```cs
using Adic.Injection;

namespace MyNamespace {
	/// <summary>
	/// My factory.
	/// </summary>
	public class MyFactory : Adic.IFactory {
		/// <summary>
		/// Creates an instance of the object of the type created by the factory.
		/// </summary>
		/// <param name="context">Injection context.</param>
		/// <returns>The instance.</returns>
		public object Create(InjectionContext context) {
			//Instantiate and return the object.
			var myObject = new MyObject();
			return myObject;
		}
	}
}
```

The `InjectionContext` object contains information about the current injection/resolution, which can be used to help deciding how the instance will be created by the factory.

To bind a type to a factory class, use the `ToFactory()`:

```cs
//Binding factory by generics...
container.Bind<SomeType>().ToFactory<MyFactory>();
//...or type instance...
container.Bind<SomeType>().ToFactory(typeMyFactory);
//...or a factory instance.
container.Bind<SomeType>().ToFactory(factoryInstance);
```

**Note:** factories are resolved and injected by the container. So, it's possible to receive dependencies either by construtor and/or fields/properties.

### <a id="bindings-setup"></a>Bindings setup

Sometimes the bindings list can become (very) large and bloat the `SetupContainers()` method of the context root. For better organization, it's possible to create reusable objects which will group and setup related bindings in a given container.

To create a bindings setup object, implement the `Adic.IBindingsSetup` interface:

```cs
using Adic;
using Adic.Container;

namespace MyNamespace.Bindings {
	/// <summary>
	/// My bindings.
	/// </summary>
	public class MyBindings : IBindingsSetup {
		public void SetupBindings (IInjectionContainer container) {
			container.Bind<SomeType>().ToSingleton<AnotherType>();
			//...more related bindings.
		}
	}
}
```

To perform a bindings setup, call the `SetupBindings()` method in the container, passing either the binding setup object as a parameter or the namespace in which the setups reside:

```cs
//Setup by generics...
container.SetupBindings<MyBindings>();
//...or by type...
container.SetupBindings(typeof(MyBindings));
//...or from an instance...
var bindings = MyBindings();
container.SetupBindings(bindings);
//...or using a namespace.
container.SetupBindings("MyNamespace.Bindings");
```

**Note:** the default behaviour of `SetupBindings()` with namespace is to use all `IBindingsSetup` objects under the given namespace and all its children namespaces. If you need that only `IBindingsSetup` objects in the given namespace are used, call the overload that allows indication of children namespace evaluation:

```cs
container.SetupBindings("MyNamespace.Bindings", false);
```

#### Binding setup priorities

The order of bindings setups matters. In case an `IBindingsSetup` object relies on bindings from another `IBindingsSetup` object, add the other setup first.

However, if you are using `SetupBindings()` with a namespace, it's not possible to manually order the `IBindingsSetup` objects. In this case, you have to decorate the `IBindingsSetup` classes with a `BindingPriority` attribute to define the priority in which each bindings setup will be executed:

```cs
using Adic;
using Adic.Container;

namespace MyNamespace.Bindings {
	/// <summary>
	/// My bindings.
	/// </summary>
	[BindingPriority(1)]
	public class MyBindings : IBindingsSetup {
		public void SetupBindings (IInjectionContainer container) {
			container.Bind<SomeType>().ToSingleton<AnotherType>();
			//...more related bindings.
		}
	}
}
```

Higher values indicate higher priorities. If no priority value is provided, the default value of `1` will be used.

### <a id="using-commands"></a>Using commands

#### What are commands?

Commands are actions executed by your game, usually in response to an event.

The concept of commands on *Adic* is to place everything the action/event needs in a single place, so it's easy to understand and maintain it.

Suppose you have an event of enemy destroyed. When that occurs, you have to update UI, dispose the enemy, spawn some particles and save statistics somewhere. One approach would be dispatch the event to every object that has to do something about it, which is fine given it keeps single responsibility by allowing every object to take care of their part on the event. However, when your project grows, it can be a nightmare to find every place a given event is handled. That's when commands come in handy: you place all the code and dependencies for a certain action/event in a single place.

#### Creatings commands

To create a command, inherit from `Adic.Command` and override the `Execute()` method, where you will place all the code needed to execute the command. If you have any dependencies to be injected before executing the command, add them as fields or properties and decorate them with an `Inject` attribute:

```cs
namespace MyNamespace.Commands {
	/// <summary>
	/// My command.
	/// </summary>
	public class MyCommand : Adic.Command {
		/// <summary>Some dependency to be injected.</summary>
		[Inject]
		public object someDependency;

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="parameters">Command parameters.</param>
		public override void Execute(params object[] parameters) {
			//Execution code.
		}
	}
}
```
**Hint:** it's also possible to wire any dependencies through constructor. However, in this case the dependencies will only be resolved once, during instantiation.

**Good practice:** place all your commands under the same namespace, so it's easy to register them.

##### Types of commands

###### Pooled

The command is kept in a pool for reuse, avoiding new instantiations. It's useful for commands that need to maintain state when executing. This is the default behaviour.

When creating pooled commands, it's possible to set the initial and maximum size of the pool for a particular command by setting, respectively, the `preloadPoolSize` and `maxPoolSize` properties:

```cs
namespace MyNamespace.Commands {
	/// <summary>
	/// My command.
	/// </summary>
	public class MyCommand : Adic.Command {
		/// <summary>The quantity of the command to preload on pool (default: 1).</summary>
		public override int preloadPoolSize { get { return 5; } }
		/// <summary>The maximum size pool for this command (default: 10).</summary>
		public override int maxPoolSize { get { return 20; } }

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="parameters">Command parameters.</param>
		public override void Execute(params object[] parameters) {
			//Execution code.
		}
	}
}
```

###### Singleton

There's only one copy of the command available, which is used every time the command is dispatched. It's useful for commands that don't need state or every dependency the command needs is given during execution. To make a command singleton, return `true` in the `singleton` property of the command:

```cs
namespace MyNamespace.Commands {
	/// <summary>
	/// My command.
	/// </summary>
	public class MyCommand : Adic.Command {
		/// <summary>Indicates whether this command is a singleton (there's only one instance of it).</summary>
		public override bool singleton { get { return true; } }

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="parameters">Command parameters.</param>
		public override void Execute(params object[] parameters) {
			//Execution code.
		}
	}
}
```

**Note 1:** this is the default command type.

**Note 2:** when using singleton commands, injection is done only through constructors or injection after command instantiation.

#### Registering commands

To register a command, call the `Register()` method on the container, usually in the context root:

```cs
using UnityEngine;

namespace MyNamespace {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : Adic.ContextRoot {
		public override void SetupContainers() {
			//Create the container.
			this.AddContainer<InjectionContainer>()
				//Register any extensions the container may use.
				.RegisterExtension<CommanderContainerExtension>()
				//Registering by generics...
				.RegisterCommand<MyCommand>()
				//...or by type.
				.RegisterCommand(typeof(MyCommand));

		}

		public override void Init() {
			//Init the game.
		}
	}
}
```

**Note:** when registering a command, it's placed in the container, so it's easier to resolve it and its dependencies.

It's also possible to register all commands under the same namespace by calling the `RegisterCommands()` method in the container and passing the full name of the namespace:

```cs
public override void SetupContainers() {
	//Create the container.
	this.AddContainer<InjectionContainer>()
		//Register any extensions the container may use.
		.RegisterExtension<CommanderContainerExtension>()
		//Register all commands under the namespace "MyNamespace.Commands".
		.RegisterCommands("MyNamespace.Commands");
}
```

**Note:** the default behaviour of `RegisterCommands()` is to register all commands under the given namespace and all its children namespaces. If you need that only commands in the given namespace are registered, call the overload that allows indication of children namespace evaluation:

```cs
container.RegisterCommands("MyNamespace.Commands", false);
```

#### Dispatching commands

##### From code using Command Dispatcher

To dispatch a command, just call the `Dispatch()` method on `Adic.ICommandDispatcher`, using either the generics or the by `System.Type` versions:

```cs
/// <summary>
/// My method that dispatches a command.
/// </summary>
public void MyMethodThatDispatchesCommands() {
	dispatcher
		//Dispatching by generics...
		.Dispatch<MyCommand>();
		//...or by type...
		.Dispatch(typeof(MyCommand));
		//...and it's also possible to tag commands to make it easier to release them later.
		.Dispatch<MyCommand>().Tag("MyTag");
}
```

**Note:** tags are only added to singleton or alive commands. When tagging singleton commands, any previous tags will be replaced.

It's also possible to dispatch a command after a given period of time by calling the `InvokeDispatch()` method:

```cs
//Timed dispatching by generics...
dispatcher.InvokeDispatch<MyCommand>(1.0f);
//...or by type.
dispatcher.InvokeDispatch(typeof(MyCommand), 1.0f);
```

To use `Adic.ICommandDispatcher`, you have to inject it wherever you need to use it:

```cs
namespace MyNamespace {
	/// <summary>
	/// My class that dispatches commands.
	/// </summary>
	public class MyClassThatDispatchesCommands {
		/// <summary>The command dispatcher.</summary>
		[Inject]
		public ICommandDispatcher dispatcher;

		/// <summary>
		/// My method that dispatches a command.
		/// </summary>
		public void MyMethodThatDispatchesCommands() {
			this.dispatcher.Dispatch<MyCommand>();
		}
	}
}
```

**Hint:** commands already have a reference to its dispatcher (`this.dispatcher`).

**Note 1:** when dispatching a command, it's placed in a list in the command dispatcher object, which is the one responsible for pooling and managing existing commands.

**Note 2:** commands in the pool that are not singleton are *reinjected* every time they are executed.

##### From code using CommandReference type

The `Adic.CommandReference` type allows the creation of properties on `MonoBehaviour` that represents a command that can be manually dispatched from code.

```cs
using Unity.Engine;

namespace MyNamespace {
	/// <summary>
	/// My MonoBehaviour summary.
	/// </summary>
	public class MyBehaviour : MonoBehaviour {
		/// <summary>Reference to the command. Can be edited on Inspector.</summary>
		public CommandReference someCommand;

		/// <summary>
		/// Manually dispatches the command.
		/// </summary>
		protected void DispatchCommand() {
			this.someCommand.DispatchCommand();
		}
	}
}
```

##### From game objects

It's possible to dispatch commands directly from game objects without the need to write any code using the components available in the <a href="#extension-commander">Commander extension</a>.

To use them, just add the desired component to a game object.

###### Command Dispatch

Provides a routine to call a given command. The routine name is `DispatchCommand()`.

Using this component, you can e.g. call the `DispatchCommand()` method from a button in the UI or in your code.

It can be found at the `Component/Adic/Commander/Command dispatch` menu.

###### Timed Command Dispatch

Dispatches a command based on a timer.

This component also provides the `DispatchCommand()` routine, in case you want to call it before the timer ends.

It can be found at the `Component/Adic/Commander/Timed command dispatch` menu.

#### Retaining commands

When a command needs to continue its execution beyond the `Execute()` method, it has to be retained. This way the command dispatcher knows the command should only be pooled/disposed when it finishes its execution.

This is useful not only for commands that implement `Adic.IUpdatable`, but also for commands that have to wait until certain actions (e.g. some network call) are completed.

To retain a command, just call the `Retain()` method during main execution:

```cs
/// <summary>
/// Executes the command.
/// </summary>
/// <param name="parameters">Command parameters.</param>
public override void Execute(params object[] parameters) {
	//Execution code.

	//Retains the command until some long action is completed.
	this.Retain();
}
```

If a command is retained, it has to be released. The command dispatcher will automatically releases commands during the destruction of scenes. However, in some situations you may want to release it manually (e.g. after some network call is completed).

To release a command, just call the `Release()` method when the execution is finished:

```cs
/// <summary>
/// Called when some action is finished.
/// </summary>
public void OnSomeActionFinished() {
	//Releases the command.
	this.Release();
}
```

It's also possible to manually release all commands of a specified type or tag by calling the `ReleaseAll()` method of the `CommandDispatcher`:

```cs
dispatcher
	//Releasing all commands of a given type by generics...
	.ReleaseAll<SomeCommand>();
	//...or by type instance...
	.ReleaseAll(typeof(SomeCommand));
	//...or by tag.
	.ReleaseAll("MyTag");
```

#### Timed invoke

It's possible to use timed method invocation inside a command by calling the `Invoke()` method:

```cs
namespace MyNamespace.Commands {
	/// <summary>
	/// My command.
	/// </summary>
	public class MyCommand : Adic.Command {
		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="parameters">Command parameters.</param>
		public override void Execute(params object[] parameters) {
			//Invokes the "MyMethodToInvoke" method after 1 second.
			this.Invoke(this.MyMethodToInvoke, 1.0f);

			//Retains the command until the invocation is finished.
			this.Retain();
		}

		/// <summary>
		/// My method to invoke.
		/// </summary>
		private void MyMethodToInvoke() {
			//Method code.

			//Releases the command after the invocation.
			this.Release();
		}
	}
}
```

**Note 1:** when an invocation is scheduled, the command is automatically retained. So, when the invocation method is called, always release the command.

**Note 2:** when a command is released, all invocations are discarded.

#### Coroutines

It's possible to use [coroutines](http://docs.unity3d.com/Manual/Coroutines.html) inside a command by calling the `StartCoroutine()` method:

```cs
namespace MyNamespace.Commands {
	/// <summary>
	/// My command.
	/// </summary>
	public class MyCommand : Adic.Command {
		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="parameters">Command parameters.</param>
		public override void Execute(params object[] parameters) {
			//Starts the coroutine.
			this.StartCoroutine(this.MyCoroutine());

			//Retains the command until the coroutine is finished.
			this.Retain();
		}

		/// <summary>
		/// My coroutine.
		/// </summary>
		private IEnumerator MyCoroutine() {
			//Coroutine code.

			//Releases the command after execution.
			this.Release();
		}
	}
}
```

If needed, it's also possible to stop a coroutine after it's started by calling the `StopCoroutine()` method.

**Note 1:** when a coroutine is started, the command is automatically retained. So, when the coroutine completes its execution, always release the command.

**Note 2:** when a command is released, all coroutines started from that command are stopped.

#### A note about scene destruction and commands

When a scene is destroyed, all commands will be released and all registrations will be disposed.

So, if you're using a <a href="#static-containers">container that will live through scenes</a>, be aware that all commands will have to be registered again.

## <a id="multiple-scenes"></a>Multiple scenes

There are different strategies when working with *Adic* in a multiple scene architecture, each one offering its own advantages.

All strategies are related to how the <a href="#quick-start">context root</a> and the lifetime of the container(s) are used.

### Single context root for all scenes

The game uses a single context root for all scenes. In this strategy, all bindings are recreated each time a scene is loaded.

It's useful for games that use a single scene or when the recreation of the bindings is not an issue. This is the default strategy, as seem on <a href="#quick-start">Quick start</a>.

### One context root per scene

The game has one context root per scene, each one with its own container(s). In this case, it's important to use <a href="#bindings-setup">bindings setup</a> to share bindings among all containers and a <a href="#performance">single reflection cache</a> to improve performance and memory consumption.

It's useful for games that have different bindings per scene.

### Shared container

A single container is shared among all scenes. In this strategy, it's common to have a single context root that is executed only on the first scene.

To use a shared container, when adding containers using `AddContainer()`, keep them alive between scenes by setting the `destroyOnLoad` to `false`.

It's useful for games that will always use the same bindings across all scenes.

**Note:** when using a shared container, it's recommended to only bind singleton instances of objects that should live up through all scenes (e.g. a common service) to prevent missing references - when a scene is destroyed, eventually some singleton objects that are bound to the container may not exist in the new scene anymore. Also, factories that rely on state to create their objects could also be affected by missing references.

### Additive scenes

A single context root is executed in the first scene and all other scenes are loaded additively.

To load a scene additively, please consult [Application.LoadLevelAdditiveAsync](http://docs.unity3d.com/ScriptReference/Application.LoadLevelAdditiveAsync.html) on the Unity Documentation.

It's useful for games that use different scenes for each part of the level but share the same bindings across all of those scenes.

#### A combination of all of the above

Each game has its own characteristics, and eventually the same game could feature more than one multiple scene strategy. It's important to test which one would suit the needs of the project and use different strategies when required.

## <a id="order-of-events"></a>Order of events

1. Unity Awake()
2. ContextRoot calls SetupContainers()
3. ContextRoot calls Init() on all containers
4. Each container generates cache for its types
5. Each container initializes their extensions
6. ContextRoot calls Init()
7. Unity Start() on all `MonoBehaviour`
8. Injection on `MonoBehaviour`
9. Update() is called on every object that implements `Adic.IUpdatable`
10. Scene is destroyed
11. Dispose() is called on every object that implements `System.IDisposable`

## <a id="script-execution-order"></a>Script execution order

Sometimes you may face some strange exceptions about null containers, even with containers correctly configured. This may occur because of the [Script Execution Order](http://docs.unity3d.com/Manual/class-ScriptExecution.html) of the injected scripts, which are being called before the <a href="#quick-start">ContextRoot</a> creates the containers.

To prevent this from happening, the execution order of the context root should be set by either clicking on the `Set execution order` button on the context root inspector or by accessing the menu `Edit > Project Settings > Script Execution Order` on Unity.

## <a id="performance"></a>Performance

*Adic* was created with speed in mind, using internal cache to minimize the use of [reflection](http://en.wikipedia.org/wiki/Reflection_%28computer_programming%29) (which is usually slow), ensuring a good performance when resolving and injecting into objects - the container can resolve a 1.000 objects in 1ms<a href="#about-performance-tests">\*</a>.

To maximize performance:

1. always bind all types that will be resolved/injected in the <a href="#quick-start">ContextRoot</a>, so *Adic* can generate cache of the objects and use that information during runtime.

2. always create all game objects that will be used during runtime before the game starts. [Object pooling](https://unity3d.com/pt/learn/tutorials/modules/beginner/live-training-archive/object-pooling) can help achieve that and increase performance by creating (and injecting) game objects upfront and reusing them throughout the game.

3. when injecting on `MonoBehaviour`, use scene wide injection during game start instead of per `MonoBehaviour` injection. Read <a href="#monobehaviour-injection">MonoBehaviour injection</a> for more details about injecting on the entire scene.

4. if you have more than one container on the same scene, it's possible to share cache between them. To do so, create an instance of `Adic.Cache.ReflectionCache` and pass it to any container you create:

```cs
using UnityEngine;

namespace MyNamespace {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : Adic.ContextRoot {
		public override void SetupContainers() {
			//Create the reflection cache.
			var cache = new ReflectionCache();

			//Create a new container.
			var container1 = this.AddContainer(new InjectionContainer(cache));

			//Container configurations and bindings...

			//Create a new container.
			var container2 = this.AddContainer(new InjectionContainer(cache));

			//Container configurations and bindings...
		}

		public override void Init() {
			//Init the game.
		}
	}
}
```

<sup><a id="about-performance-tests">\*</a> See *Tests/Editor/SpeedTest.cs* for more details on performance tests. Tested on a MacBook Pro late 2014 (i7 2.5/3.7 GHz).</sup>

<sup>\- A thousand simple resolves in 1ms</sup><br>
<sup>\- A million simple resolves in 1330ms</sup><br>
<sup>\- A thousand complex resolves in 2ms</sup><br>
<sup>\- A million complex resolves in 2428ms</sup>

<sup>A *simple resolve* is the resolution of a class without any `Inject` attributes.</sup><br>
<sup>A *complex resolve* is the resolution of a class that is not bound to the container and has a `Inject` attribute in a field.</sup>

## <a id="il2cpp-aot"></a>IL2CPP, AOT and code stripping

When compiling to [AOT platforms](https://en.wikipedia.org/wiki/Ahead-of-time_compilation) using IL2CPP (like iOS and WebGL), Unity performs *code stripping*, removing any code that is not being used. Although this is useful to reduce build size, it also affects classes that are only instantiated through *Adic*, since they are not created directly. To prevent non-inclusion of these classes, Unity provides the [`Preserve` attribute](http://docs.unity3d.com/ScriptReference/Scripting.PreserveAttribute.html), which should be added to all classes that are only created through the container.

## <a id="general-notes"></a>General notes

1. If an instance is not found, it will be resolved to NULL.
2. Multiple injections must occur in an array of the desired type.
3. Order of bindings is controlled by just reordering the bindings during container setup.
4. Avoid singleton bindings of objects that will be destroyed during execution. This can lead to missing references in the container.
5. Any transient object, once resolved, is not tracked by *Adic*. So, if you want e.g. a list of all prefabs that were resolved by the container, you'll have to store it manually. Singleton objects are kept inside the container, given there is just a single instance of them.
6. *Adic* relies on Unity Test Tools for unit testing. You can download it at [Unity Asset Store](https://www.assetstore.unity3d.com/#!/content/13802).

## <a id="container-extensions"></a>Extensions

Extensions are a way to enhance *Adic* without having to edit it to suit different needs. By using extensions, the core of *Adic* is kept agnostic, so it can be used on any C# environment.

## <a id="available-extensions"></a>Available extensions

### <a id="extension-bindings-printer"></a>Bindings Printer

Prints all bindings on any containers on the current context root. It must be executed on Play Mode.

To open the Bindings Printer windows, click on *Windows/Adic/Bindings Printer* menu.

#### Format

```
[Container Type Full Name] (index: [Container Index on ContextRoot], [destroy on load/singleton])

	[For each binding]
	Type: [Binding Type Full Name]
	Bound to: [Bound To Type Full Name] ([type/instance])
	Binding type: [Transient/Singleton/Factory]
	Identifier [Identifier/-]
	Conditions: [yes/no]
```

#### Dependencies

* <a href="#extension-context-root">Context Root</a>

### <a id="extension-bindings-setup"></a>Bindings Setup

Provides a convenient place to setup bindings and reuse them in different containers and scenes.

#### Configuration

Please see <a href="#bindings-setup">Bindings setup</a> for more information.

#### Dependencies

None

### <a id="extension-commander"></a>Commander

Provides dispatching of commands, with pooling for better performance.

For more information on commands, see <a href="#using-commands">Using commands</a>.

#### Configuration

Register the extension on any containers that will use it:

```cs
//Create the container.
this.AddContainer<InjectionContainer>()
	//Register any extensions the container may use.
	.RegisterExtension<CommanderContainerExtension>();
```

If you use `IDiposable` or `IUpdatable` events, also register the <a href="#extension-event-caller">Event Caller</a> extension:

```cs
//Create the container.
this.AddContainer<InjectionContainer>()
	//Register any extensions the container may use.
	.RegisterExtension<CommanderContainerExtension>()
	.RegisterExtension<EventCallerContainerExtension>();
```

#### Dependencies

* <a href="#extension-event-caller">Event Caller</a>

### <a id="extension-context-root"></a>Context Root

Provides an entry point for the game on Unity.

#### Configuration

Please see <a href="#quick-start">Quick start</a> for more information.

#### Dependencies

None

### <a id="extension-event-caller"></a>Event Caller

Calls events on classes that implement certain interfaces. The classes must be bound to a container.

#### Available events

##### Update

Calls `Update()` method on classes that implement `Adic.IUpdatable` interface. It's not called when the game is paused.

```cs
namespace MyNamespace {
	/// <summary>
	/// My updateable class.
	/// </summary>
	public class MyClass : Adic.IUpdatable {
		public void Update() {
			//Update code.
		}
	}
}
```

##### LateUpdate

Calls `LateUpdate()` method on classes that implement `Adic.ILateUpdatable` interface. It's not called when the game is paused.

```cs
namespace MyNamespace {
	/// <summary>
	/// My late updateable class.
	/// </summary>
	public class MyClass : Adic.ILateUpdatable {
		public void LateUpdate() {
			//Late update code.
		}
	}
}
```

##### FixedUpdate

Calls `FixedUpdate()` method on classes that implement `Adic.IFixedUpdatable` interface. It's called even when the game is paused.

```cs
namespace MyNamespace {
	/// <summary>
	/// My fixed updateable class.
	/// </summary>
	public class MyClass : Adic.IFixedUpdatable {
		public void FixedUpdate() {
			//Fixed update code.
		}
	}
}
```

##### IPausable

Calls `OnApplicationPause()` method on classes that implement `Adic.IPausable` interface.

```cs
namespace MyNamespace {
	/// <summary>
	/// My pausable class.
	/// </summary>
	public class MyClass : Adic.IPausable {
		public void OnApplicationPause(bool isPaused) {
			//Called when the application is paused.
		}
	}
}
```

##### IFocusable

Calls `OnApplicationFocus()` method on classes that implement `Adic.IFocusable` interface.

```cs
namespace MyNamespace {
	/// <summary>
	/// My focusable class.
	/// </summary>
	public class MyClass : Adic.IFocusable {
		public void OnApplicationFocus(hasFocus) {
			//Called when the application focus is changing.
		}
	}
}
```

##### IQuitable

Calls `OnApplicationQuit()` method on classes that implement `Adic.IQuitable` interface.

```cs
namespace MyNamespace {
	/// <summary>
	/// My quitable class.
	/// </summary>
	public class MyClass : Adic.IQuitable {
		public void OnApplicationQuit() {
			//Called when the application quits.
		}
	}
}
```

##### Dispose

When a scene is destroyed, calls `Dispose()` method on classes that implement `System.IDisposable` interface.

```cs
namespace MyNamespace {
	/// <summary>
	/// My disposable class.
	/// </summary>
	public class MyClass : System.IDisposable {
		public void Dispose() {
			//Dispose code.
		}
	}
}
```

#### Configuration

Register the extension on any containers that will use it:

```cs
//Create the container.
this.AddContainer<InjectionContainer>()
	//Register any extensions the container may use.
	.RegisterExtension<EventCallerContainerExtension>();ge
```

#### Notes

1. Currently, any objects that are updateable are not removed from the update's list when they're not in use anymore. So, it's recommended to implement the `Adic.IUpdatable` interface only on singleton or transient objects that will live until the scene is destroyed;
2. When the scene is destroyed, the update's list is cleared. So, any objects that will live between scenes that implement the `Adic.IUpdatable` interface will not be readded to the list. **It's recommeded to use updateable objects only on the context of a single scene**.
3. Be aware of singleton objects on containers that will live through scenes. Eventually these objects may try to use references that may not exist anymore.

#### Dependencies

None

### <a id="extension-mono-injection"></a>Mono Injection

Allows injection on `MonoBehaviour` by providing an `Inject()` method.

#### Configuration

Please see <a href="#monobehaviour-injection">MonoBehaviour injection</a> for more information.

#### Dependencies

* <a href="#extension-context-root">Context Root</a>

### <a id="extension-state-injection"></a>State Injection

Allows injection on `StateMachineBehaviour` by providing an `Inject()` method.

#### Configuration

Please see <a id="statemachinebehaviour-injection"></a>StateMachineBehaviour injection</a> for more information.

#### Dependencies

* <a href="#extension-context-root">Context Root</a>
* <a href="#extension-mono-injection">Mono Injection</a>

### <a id="extension-unity-binding"></a>Unity Binding

Provides Unity bindings to the container.

Please see <a href="#bindings">Bindings</a> for more information.

#### Configuration

Register the extension on any containers that you may use it:

```cs
//Create the container.
this.AddContainer<InjectionContainer>()
	//Register any extensions the container may use.
	.RegisterExtension<UnityBindingContainerExtension>();
```

#### Notes

1. **ALWAYS CALL `Inject()` FROM 'Start'**! (use the <a href="#extension-mono-injection">Mono Injection</a> Extension).

#### Dependencies

None

## <a id="creating-extensions"></a>Creating extensions

Extensions on *Adic* can be created in 3 ways:

1. Creating a framework extension extending the base APIs through their interfaces;
2. Creating extension methods to any part of the framework;
3. Creating a container extension, which allows for the interception of internal events, which can alter the inner workings of the framework.

**Note 1:** always place the public parts of extensions into *Adic* namespace.
**Note 2:** a container can have only a single instance of each extension.

To create a *container extension*, which can intercept internal *Adic* events, you have to:

1\. Create the extension class with `ContainerExtension` sufix.

2\. Implement `Adic.Container.IContainerExtension`.

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

5\. Use the `Init()` method to initialize the extension. It's always called after all extensions and bindings have been added to the container, so it can be used as a late initialization method that may rely on bindings that were not available during registration.

## <a id="container-events"></a>Container events

Container events provide a way to intercept internal actions of the container and change its inner workings to suit the needs of your extension.

All events are available through `Adic.InjectionContainer`.

### <a id="binder-events"></a>Binder events

* `beforeAddBinding`: occurs before a binding is added.
* `afterAddBinding`: occurs after a binding is added.
* `beforeRemoveBinding`: occurs before a binding is removed.
* `afterRemoveBinding`: occurs after a binding is removed.

### <a id="injector-events"></a>Injector events

* `beforeResolve`: occurs before a type is resolved.
* `afterResolve`: occurs after a type is resolved.
* `bindingEvaluation`: occurs when a binding is available for resolution.
* `bindingResolution`: occures when a binding is resolved to an instance.
* `beforeInject`: occurs before an instance receives injection.
* `afterInject`: occurs after an instance receives injection.

## <a id="binaries"></a>Binaries

If you need compiled binaries of *Adic*, look for them at the [releases page](https://github.com/intentor/adic/releases) (starting from version [2.13](https://github.com/intentor/adic/releases/tag/v2.13)).

The project is divided into 2 binaries:

1. **Adic.Framework.dll**: main framework, decoupled from Unity.
2. **Adic.Extensions.dll** Unity extensions.

## <a id="examples"></a>Examples

There are some examples that are bundled to the main package that teach the basics and beyond of *Adic*.

### 1. Hello World

Exemplifies the basics of how to setup a scene for dependency injection using the ContextRoot.

### 2. Binding Game Objects

Exemplifies how to bind components to new and existing game objects and allows them to share dependencies.

### 3. Using conditions

Exemplifies the use of condition identifiers on injections.

### 4. Prefabs

Exemplifies how to bind to prefabs and the use of method injection as a second constructor.

### 5. Commander

Exemplifies the use of commands through a simple spawner of a prefab.

### 6. Bindings Setup

Exemplifies the use of bindings setups to better organize the bindings for a container.

### 7. Factory

Exemplifies the use of a factory to create and position cubes as a matrix.

### 8. Unity Events

Exemplifies the use of Unity events on regular classes.

## <a id="changelog"></a>Changelog

Please see [CHANGELOG.txt](src/Assets/Adic/CHANGELOG.txt).

## <a id="support"></a>Support

Found a bug? Please create an issue on the [GitHub project page](https://github.com/intentor/adic/issues) or send a pull request if you have a fix or extension.

You can also send me a message at support@intentor.com.br to discuss more obscure matters about *Adic*.

## <a id="license"></a>License

Licensed under the [The MIT License (MIT)](http://opensource.org/licenses/MIT). Please see [LICENSE](LICENSE) for more information.
