using UnityEngine;
using System;
using System.Collections;

namespace Intentor.Adic {
	/// <summary>
	/// Provides binding capabilities to Unity entities.
	/// </summary>
	public static class UnityBindingExtension {
		private const string TYPE_NOT_COMPONENT = 
			"The component type must be derived from UnityEngine.Component.";
		private const string GAMEOBJECT_IS_NULL = 
			"There's no GameObject to bind the type to.";
		private const string PREFAB_IS_NULL = 
			"There's no prefab to bind the type to.";

		/// <summary>
		/// Binds the key type to a singleton of itself on a new GameObject.
		/// 
		/// The key type must be derived either from <see cref="UnityEngine.GameObject"/>
		/// or <see cref="UnityEngine.Component"/>.
		/// </summary>
		/// <param name="bindingFactory">The original binding factory.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public static IBindingConditionFactory ToGameObject(this IBindingFactory bindingFactory) {
			return bindingFactory.ToGameObject(bindingFactory.bindingType, null);
		}

		/// <summary>
		/// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
		/// of <typeparamref name="T"/> on a new GameObject.
		/// </summary>
		/// <typeparam name="T">The component type to bind the GameObject to.</typeparam>
		/// <param name="bindingFactory">The original binding factory.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public static IBindingConditionFactory ToGameObject<T>(this IBindingFactory bindingFactory) where T : Component {
			return bindingFactory.ToGameObject(typeof(T), null);
		}

		/// <summary>
		/// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
		/// of <paramref name="type"/> on a new GameObject.
		/// </summary>
		/// <param name="bindingFactory">The original binding factory.</param>
		/// <param name="type">The component type.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public static IBindingConditionFactory ToGameObject(this IBindingFactory bindingFactory, Type type) {
			return bindingFactory.ToGameObject(type, null);
		}
				
		/// <summary>
		/// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
		/// of itself on a GameObject of a given <paramref name="name"/>.
		/// 
		/// The key type must be derived either from <see cref="UnityEngine.GameObject"/>
		/// or <see cref="UnityEngine.Component"/>.
		/// 
		/// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
		/// </summary>
		/// <param name="bindingFactory">The original binding factory.</param>
		/// <param name="name">The GameObject name.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public static IBindingConditionFactory ToGameObject(this IBindingFactory bindingFactory, string name) {
			return bindingFactory.ToGameObject(bindingFactory.bindingType, name);
		}

		/// <summary>
		/// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
		/// of <typeparamref name="T"/> on a GameObject of a given <paramref name="name"/>.
		/// 
		/// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
		/// </summary>
		/// <typeparam name="Type">The component type to bind the GameObject to.</typeparam>
		/// <param name="bindingFactory">The original binding factory.</param>
		/// <param name="name">The GameObject name.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public static IBindingConditionFactory ToGameObject<T>(this IBindingFactory bindingFactory, string name) where T : Component {
			return bindingFactory.ToGameObject(typeof(T), name);
		}

		/// <summary>
		/// Binds the key type to a singleton  <paramref name="type"/> on a GameObject 
		/// of a given <paramref name="name"/>.
		/// 
		/// If <paramref name="type"/> is <see cref="UnityEngine.GameObject"/>, binds the
		/// key to the GameObject itself.
		/// 
		/// If <paramref name="type"/> is see cref="UnityEngine.Component"/>, binds the key
		/// to the the instance of the component.
		/// 
		/// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
		/// </summary>
		/// <param name="bindingFactory">The original binding factory.</param>
		/// <param name="type">The component type.</param>
		/// <param name="name">The GameObject name.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public static IBindingConditionFactory ToGameObject(this IBindingFactory bindingFactory, Type type, string name) {
			if (!TypeUtils.IsAssignable(bindingFactory.bindingType, type)) {
				throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
			}

			var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
			var isComponent = TypeUtils.IsAssignable(typeof(Component), type);
			if (!isGameObject && !isComponent) {
				throw new BindingException(TYPE_NOT_COMPONENT);
			}

			GameObject gameObject = null;
			if (string.IsNullOrEmpty(name)) {
				gameObject = new GameObject(type.Name);
			} else {
				gameObject = GameObject.Find(name);
			}

			return CreateSingletonBinding(bindingFactory, gameObject, type, isGameObject);
		}

		/// <summary>
		/// Binds the key type to a transient of itself on the prefab.
		/// 
		/// The key type must be derived either from <see cref="UnityEngine.GameObject"/>
		/// or <see cref="UnityEngine.Component"/>.
		/// 
		/// If the <see cref="UnityEngine.Component"/> is not found on the prefab
		/// at the moment of the instantiation, it will be added.
		/// </summary>
		/// <param name="bindingFactory">The original binding factory.</param>
		/// <param name="name">Prefab name. It will be loaded using <c>Resources.Load<c/>.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public static IBindingConditionFactory ToPrefab(this IBindingFactory bindingFactory, string name) {
			return bindingFactory.ToPrefab(bindingFactory.bindingType, name);
		}		
		
		/// <summary>
		/// Binds the key type to a transient <see cref="UnityEngine.Component"/>
		/// of <typeparamref name="T"/> on the prefab.
		/// 
		/// If the <see cref="UnityEngine.Component"/> is not found on the prefab
		/// at the moment of the instantiation, it will be added.
		/// </summary>
		/// <typeparam name="Type">The component type to bind the GameObject to.</typeparam>
		/// <param name="bindingFactory">The original binding factory.</param>
		/// <param name="name">Prefab name. It will be loaded using <c>Resources.Load<c/>.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public static IBindingConditionFactory ToPrefab<T>(this IBindingFactory bindingFactory, string name) where T : Component {
			return bindingFactory.ToPrefab(typeof(T), name);
		}

		/// <summary>
		/// Binds the key type to a transient <see cref="UnityEngine.Component"/>
		/// of <paramref name="type"/> on the prefab.
		/// 
		/// If the <see cref="UnityEngine.Component"/> is not found on the prefab
		/// at the moment of the instantiation, it will be added.
		/// </summary>
		/// <param name="bindingFactory">The original binding factory.</param>
		/// <param name="type">The component type.</param>
		/// <param name="name">Prefab name. It will be loaded using <c>Resources.Load<c/>.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public static IBindingConditionFactory ToPrefab(this IBindingFactory bindingFactory, Type type, string name) {
			if (!TypeUtils.IsAssignable(bindingFactory.bindingType, type)) {
				throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
			}

			var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
			var isComponent = TypeUtils.IsAssignable(typeof(Component), type);
			if (!isGameObject && !isComponent) {
				throw new BindingException(TYPE_NOT_COMPONENT);
			}

			var prefab = Resources.Load(name);
			if (prefab == null) {
				throw new BindingException(PREFAB_IS_NULL);
			}

			var prefabBinding = new PrefabBinding(prefab, type);

			return bindingFactory.CreateBinding(prefabBinding, BindingInstance.Transient);
		}
			
		/// <summary>
		/// Binds the key type to a singleton of itself on a newly instantiated prefab.
		/// 
		/// If the <see cref="UnityEngine.Component"/> is not found on the prefab
		/// at the moment of the instantiation, it will be added.
		/// 
		/// The key type must be derived either from <see cref="UnityEngine.GameObject"/>
		/// or <see cref="UnityEngine.Component"/>.
		/// </summary>
		/// <param name="bindingFactory">The original binding factory.</param>
		/// <param name="name">Prefab name.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public static IBindingConditionFactory ToPrefabSingleton(this IBindingFactory bindingFactory, string name) {
			return bindingFactory.ToPrefabSingleton(bindingFactory.bindingType, name);
		}

		/// <summary>
		/// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
		/// of <typeparamref name="T"/> on a newly instantiated prefab.
		/// 
		/// If the <see cref="UnityEngine.Component"/> is not found on the prefab
		/// at the moment of the instantiation, it will be added.
		/// </summary>
		/// <typeparam name="Type">The component type to bind the GameObject to.</typeparam>
		/// <param name="bindingFactory">The original binding factory.</param>
		/// <param name="name">Prefab name. It will be loaded using <c>Resources.Load<c/>.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public static IBindingConditionFactory ToPrefabSingleton<T>(this IBindingFactory bindingFactory, string name) where T : Component {
			return bindingFactory.ToPrefabSingleton(typeof(T), name);
		}
			
		/// <summary>
		/// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
		/// of <paramref name="type"/> on a newly instantiated prefab.
		/// 
		/// If the <see cref="UnityEngine.Component"/> is not found on the prefab
		/// at the moment of the instantiation, it will be added.
		/// </summary>
		/// <param name="bindingFactory">The original binding factory.</param>
		/// <param name="type">The component type.</param>
		/// <param name="name">Prefab name. It will be loaded using <c>Resources.Load<c/>.</param>
		/// <returns>The binding condition object related to this binding.</returns>
		public static IBindingConditionFactory ToPrefabSingleton(this IBindingFactory bindingFactory, Type type, string name) {
			if (!TypeUtils.IsAssignable(bindingFactory.bindingType, type)) {
				throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
			}

			var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
			var isComponent = TypeUtils.IsAssignable(typeof(Component), type);
			if (!isGameObject && !isComponent) {
				throw new BindingException(TYPE_NOT_COMPONENT);
			}

			var prefab = Resources.Load(name);
			if (prefab == null) {
				throw new BindingException(PREFAB_IS_NULL);
			}
			
			var gameObject = (GameObject)MonoBehaviour.Instantiate(prefab);
			
			return CreateSingletonBinding(bindingFactory, gameObject, type, isGameObject);
		}

		/// <summary>
		/// Creates a singleton binding.
		/// </summary>
		/// <param name="bindingFactory">The binding factory.</param>
		/// <param name="gameObject">The GameObject to bind to.</param>
		/// <param name="type">The type of the binding.</param>
		/// <param name="typeIsGameObject">Indicates whether the type is a GameObject.</param>
		/// <returns>The binding condition object related to the binding.</returns>
		private static IBindingConditionFactory CreateSingletonBinding(IBindingFactory bindingFactory,
			GameObject gameObject,
			Type type,
			bool typeIsGameObject) {
			if (gameObject == null) {
				throw new BindingException(GAMEOBJECT_IS_NULL);
			}
			
			if (typeIsGameObject) {
				return bindingFactory.CreateBinding(gameObject, BindingInstance.Singleton);
			} else {
				var component = gameObject.GetComponent(type);
				
				if (component == null) {
					component = gameObject.AddComponent(type);
				}
				
				return bindingFactory.CreateBinding(component, BindingInstance.Singleton);
			}
		}
	}
}