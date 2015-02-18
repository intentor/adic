# Adic - Another Dependency Injector Container

## Contents

* <a href="#introduction">Introduction</a>
* <a href="#features">Features</a>
* <a href="#concepts">Concepts</a>
	* <a href="#structure">Structure
	* <a href="#types-of-bindings">Types of bindings
	* <a href="#pipeline">Pipeline
* <a href="#how-to-use">How to use</a>
* <a href="#notes">Notes</a>
* <a href="#examples">Examples</a>
* <a href="#license">License</a>

## <a id="introduction"></a>Introduction

Adic is a lightweight dependency injection container for Unity 3D.

Based on studies from [StrangeIoC](http://strangeioc.github.io/strangeioc/) and the proof-of-concept container from [Sebastiano Mandalà](http://blog.sebaslab.com/ioc-container-for-unity3d-part-1/), the ideia of the project was to create a dependency injection container that is simple to use and extend, having on its roots the simplicity of the work of Mandalà and the extensibility of StrangeIoC.

The project was tested on Unity 4.3+ and should work on lower versions of the 4 cicle.

## <a id="features"></a>Features

* Bind types and identifiers to other types and singleton instances;
* Instance resolution by type and identifier;
* Injection on constructor, fields and properties;
* Fast dependency resolution with internal cache;
* Use of attributes to indicate injections;
* MonoBehaviour injector;
* Extensible interface based API;
* Game root concept can keep containers alive through scenes.

## <a id="concepts"></a>Concepts

### <a id="structure"></a>Structure

* **Context**: context for dependency injection. Acts as a Binder and an Injector at the same time.
* **Binder**: binds a type or identifier to another type or instance with inject conditions.
* **Binding**: executes the binding of a type on a binder and holds information about its relationship.
* **Injector**: resolves and injects dependencies.
* **Context Root**: context in which the containers are in. Acts as an entry point for the game.

### <a id="types-of-bindings"></a>Types of bindings

* **Transient**: a new instance is created each time a dependency needs to be resolved;
* **Singleton**: a single instance is created and used on any dependency resolution;
* **Factory**: creates the instance and returns it to the container.

### <a id="pipeline"></a>Pipeline

1. Unity Awake() on Context Root;
2. Context Root calls SetupContainers();
3. Each container calls SetupBindings();
4. Each container generates cache for its types;
5. Context Root calls Init();
6. Unity Start() on all MonoBehaviours.

Order of bindings is controlled by just reordering the bindings

## <a id="how-to-use"></a>How to use

1\. Create your own dependency injection contexts (e. g. GameContext.cs) by inheriting from `Adic.Context`.
   
```cs
using UnityEngine;

namespace MyNamespace {
	/// <summary>
	/// Dependency injection context.
	/// </summary>
	public class GameContext : Adic.Context {
		public override void SetupBindings() {

		}
	}
}
```
   
2\. Setup all your dependencies on the `SetupBindings()` method.

3\. Create the context root (e. g. GameRoot.cs) of your scene by inheriting from `Adic.ContextRoot`.

**NOTE**: there should be only one context root per scene.
   
```cs
using UnityEngine;

namespace MyNamespace {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : Adic.ContextRoot {
		public override void SetupContexts() {
			this.AddContext<GameContext>();
		}

		public override void Init() {
			//Start the game.
		}
	}
}
```
   
4\. On the SetupContexts() method, add any contexts you may have created by calling `this.Add<ContextType>()`.

5\. On the `Init()` method, place any codes to start your game.

**NOTE**: the idea of this method is to work as an entry point for your game.

6\. Attach the context root created by you on an empty GameObject at your scene.

7\. Start dependency injecting!

## <a id="notes"></a>Notes

1. If an instance is not found, it will be resolved to NULL;
2. Adic relies on Unity Test Tools for unit testing. You can download it at [Unity Asset Store](https://www.assetstore.unity3d.com/#!/content/13802).

## <a id="examples"></a>Example

The provided example shows the basics of how to setup a scene for dependency injection and the use of the context root.

## <a id="license"></a>License

Licensed under the [The MIT License (MIT)](http://opensource.org/licenses/MIT). Please read LICENSE for more information.