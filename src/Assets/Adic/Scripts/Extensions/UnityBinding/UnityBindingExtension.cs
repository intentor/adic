using UnityEngine;
using System;
using System.Collections;
using Adic.Binding;
using Adic.Exceptions;
using Adic.Util;

namespace Adic {
    /// <summary>
    /// Provides binding capabilities to Unity entities in <see cref="Adic.Container.IInjectionContainer"/>.
    /// </summary>
    public static class UnityBindingExtension {
        private const string TYPE_NOT_OBJECT = 
            "The type must be derived from UnityEngine.Object.";
        private const string TYPE_NOT_COMPONENT = 
            "The type must be derived from UnityEngine.Component.";
        private const string GAMEOBJECT_IS_NULL = 
            "There's no GameObject to bind the type to.";
        private const string GAMEOBJECT_TAG_TYPE_IS_NULL =
            "There's no GameObject tagged \"{0}\" to bind the type {1} to.";
        private const string PREFAB_NAME_TYPE_IS_NULL =
            "There's no prefab named \"{0}\" to bind the type {1} to.";
        private const string RESOURCE_IS_NULL = 
            "There's no resource to bind the type to.";
        private static readonly Type TYPE_GAME_OBJECT;
        private static readonly Type TYPE_COMPONENT;

        static UnityBindingExtension() {
            TYPE_GAME_OBJECT = typeof(GameObject);
            TYPE_COMPONENT = typeof(Component);
        }

        /// <summary>
        /// Binds the key type to a singleton of itself on a new GameObject.
        /// 
        /// The key type must be derived either from <see cref="UnityEngine.GameObject"/>
        /// or <see cref="UnityEngine.Component"/>.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToGameObject(this IBindingFactory bindingFactory) {
            return bindingFactory.ToGameObject(bindingFactory.bindingType, String.Empty);
        }

        /// <summary>
        /// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
        /// of <typeparamref name="T"/> on a new GameObject.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <typeparam name="T">The component type to bind the GameObject to.</typeparam>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToGameObject<T>(this IBindingFactory bindingFactory) where T : Component {
            return bindingFactory.ToGameObject(typeof(T), String.Empty);
        }

        /// <summary>
        /// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
        /// of <paramref name="type"/> on a new GameObject.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="type">The component type.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToGameObject(this IBindingFactory bindingFactory, Type type) {
            return bindingFactory.ToGameObject(type, String.Empty);
        }

        /// <summary>
        /// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
        /// of itself on a GameObject of a given <paramref name="name"/>.
        /// 
        /// If the GameObject of a given <paramref name="name"/> is not found, it's added to the scene.
        /// 
        /// The key type must be derived either from <see cref="UnityEngine.GameObject"/>
        /// or <see cref="UnityEngine.Component"/>.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="name">The GameObject name.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToGameObject(this IBindingFactory bindingFactory, string name) {
            return bindingFactory.ToGameObject(bindingFactory.bindingType, name);
        }

        /// <summary>
        /// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
        /// of <typeparamref name="T"/> on a GameObject of a given <paramref name="name"/>.
        /// 
        /// If the GameObject of a given <paramref name="name"/> is not found, it's added to the scene.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <typeparam name="Type">The component type to bind the GameObject to.</typeparam>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="name">The GameObject name.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToGameObject<T>(this IBindingFactory bindingFactory, string name) where T : Component {
            return bindingFactory.ToGameObject(typeof(T), name);
        }

        /// <summary>
        /// Binds the key type to a singleton <paramref name="type"/> on a GameObject 
        /// of a given <paramref name="name"/>.
        /// 
        /// If the GameObject of a given <paramref name="name"/> is not found, it's added to the scene.
        /// 
        /// If <paramref name="type"/> is <see cref="UnityEngine.GameObject"/>, binds the
        /// key to the GameObject itself.
        /// 
        /// If <paramref name="type"/> is see cref="UnityEngine.Component"/>, binds the key
        /// to the the instance of the component.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="type">The component type.</param>
        /// <param name="name">The GameObject name.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToGameObject(this IBindingFactory bindingFactory, Type type, string name) {
            if (!TypeUtils.IsAssignable(bindingFactory.bindingType, type)) {
                throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
            }

            var isGameObject = TypeUtils.IsAssignable(TYPE_GAME_OBJECT, type);
            var isComponent = TypeUtils.IsAssignable(TYPE_COMPONENT, type);
            if (!isGameObject && !isComponent) {
                throw new BindingException(TYPE_NOT_COMPONENT);
            }

            GameObject gameObject = null;
            if (string.IsNullOrEmpty(name)) {
                gameObject = new GameObject(type.Name);
            } else {
                gameObject = GameObject.Find(name);

                if (gameObject == null) {
                    gameObject = new GameObject(name);
                }
            }

            return CreateSingletonBinding(bindingFactory, gameObject, type, isGameObject);
        }

        /// <summary>
        /// Binds the key type to a singleton <see cref="UnityEngine.Component"/> on a given <paramref name="gameObject"/>.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="gameObject">The GameObject object.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToGameObject(this IBindingFactory bindingFactory, GameObject gameObject) {
            return bindingFactory.ToGameObject(bindingFactory.bindingType, gameObject);
        }

        /// <summary>
        /// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
        /// of <typeparamref name="T"/> on a given <paramref name="gameObject"/>.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <typeparam name="Type">The component type to bind the GameObject to.</typeparam>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="gameObject">The GameObject object.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToGameObject<T>(this IBindingFactory bindingFactory, GameObject gameObject) {
            return bindingFactory.ToGameObject(typeof(T), gameObject);
        }

        /// <summary>
        /// Binds the key type to a singleton <paramref name="type"/> to a given <paramref name="gameObject"/>.
        /// 
        /// If <paramref name="type"/> is <see cref="UnityEngine.GameObject"/>, binds the
        /// key to the GameObject itself.
        /// 
        /// If <paramref name="type"/> is see cref="UnityEngine.Component"/>, binds the key
        /// to the the instance of the component.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="type">The component type.</param>
        /// <param name="gameObject">The GameObject object.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToGameObject(this IBindingFactory bindingFactory, Type type, GameObject gameObject) {
            var isGameObject = TypeUtils.IsAssignable(TYPE_GAME_OBJECT, type);
            var isComponent = TypeUtils.IsAssignable(TYPE_COMPONENT, type);
            if (!isGameObject && !isComponent) {
                throw new BindingException(TYPE_NOT_COMPONENT);
            }
            
            return CreateSingletonBinding(bindingFactory, gameObject, type, isGameObject);
        }

        /// <summary>
        /// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
        /// of itself on a GameObject of a given <paramref name="tag"/>.
        /// 
        /// If more than one object is returned, just the first one will be binded.
        /// 
        /// The key type must be derived either from <see cref="UnityEngine.GameObject"/>
        /// or <see cref="UnityEngine.Component"/>.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="tag">The GameObject tag.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToGameObjectWithTag(this IBindingFactory bindingFactory, string tag) {
            return bindingFactory.ToGameObjectWithTag(bindingFactory.bindingType, tag);
        }

        /// <summary>
        /// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
        /// of <typeparamref name="T"/> on a GameObject with a given <paramref name="tag"/>.
        /// 
        /// If more than one object is returned, just the first one will be binded.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <typeparam name="Type">The component type to bind the GameObject to.</typeparam>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="tag">The GameObject tag.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToGameObjectWithTag<T>(this IBindingFactory bindingFactory, string tag) where T : Component {
            return bindingFactory.ToGameObjectWithTag(typeof(T), tag);
        }

        /// <summary>
        /// Binds the key type to a singleton  <paramref name="type"/> on a GameObject 
        /// with a given <paramref name="tag"/>.
        /// 
        /// If more than one object is returned, just the first one will be binded.
        /// 
        /// If <paramref name="type"/> is <see cref="UnityEngine.GameObject"/>, binds the
        /// key to the GameObject itself.
        /// 
        /// If <paramref name="type"/> is see cref="UnityEngine.Component"/>, binds the key
        /// to the the instance of the component.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="type">The component type.</param>
        /// <param name="tag">The GameObject tag.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToGameObjectWithTag(this IBindingFactory bindingFactory, Type type, string tag) {
            if (!TypeUtils.IsAssignable(bindingFactory.bindingType, type)) {
                throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
            }
			
            var isGameObject = TypeUtils.IsAssignable(TYPE_GAME_OBJECT, type);
            var isComponent = TypeUtils.IsAssignable(TYPE_COMPONENT, type);
            if (!isGameObject && !isComponent) {
                throw new BindingException(TYPE_NOT_COMPONENT);
            }
			
            var gameObject = GameObject.FindWithTag(tag);
			
            if (gameObject == null) {
                throw new BindingException(string.Format(GAMEOBJECT_TAG_TYPE_IS_NULL, tag, type.ToString()));
            }
			
            return CreateSingletonBinding(bindingFactory, gameObject, type, isGameObject);
        }

        /// <summary>
        /// Binds the key type to singletons <see cref="UnityEngine.Component"/>
        /// of itself on game objects of a given <paramref name="tag"/>.
        /// 
        /// The key type must be derived either from <see cref="UnityEngine.GameObject"/>
        /// or <see cref="UnityEngine.Component"/>.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="tag">The GameObject tag.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static IBindingConditionFactory ToGameObjectsWithTag(this IBindingFactory bindingFactory, string tag) {
            return bindingFactory.ToGameObjectsWithTag(bindingFactory.bindingType, tag);
        }

        /// <summary>
        /// Binds the key type to singletons <see cref="UnityEngine.Component"/>
        /// of itself on game objects of a given <paramref name="tag"/>.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <typeparam name="Type">The component type to bind the GameObject to.</typeparam>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="tag">The GameObject tag.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static IBindingConditionFactory ToGameObjectsWithTag<T>(this IBindingFactory bindingFactory, string tag) where T : Component {
            return bindingFactory.ToGameObjectsWithTag(typeof(T), tag);
        }

        /// <summary>
        /// Binds the key type to singletons <see cref="UnityEngine.Component"/>
        /// of itself on game objects of a given <paramref name="tag"/>.
        /// 
        /// If <paramref name="type"/> is <see cref="UnityEngine.GameObject"/>, binds the
        /// key to the GameObject itself.
        /// 
        /// If <paramref name="type"/> is see cref="UnityEngine.Component"/>, binds the key
        /// to the the instance of the component.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="type">The component type.</param>
        /// <param name="tag">The GameObject tag.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static IBindingConditionFactory ToGameObjectsWithTag(this IBindingFactory bindingFactory, Type type, string tag) {
            if (!TypeUtils.IsAssignable(bindingFactory.bindingType, type)) {
                throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
            }
			
            var isGameObject = TypeUtils.IsAssignable(TYPE_GAME_OBJECT, type);
            var isComponent = TypeUtils.IsAssignable(TYPE_COMPONENT, type);
            if (!isGameObject && !isComponent) {
                throw new BindingException(TYPE_NOT_COMPONENT);
            }
			
            var gameObjects = GameObject.FindGameObjectsWithTag(tag);
            var bindingFactories = new IBindingConditionFactory[gameObjects.Length];

            for (int gameObjectIndex = 0; gameObjectIndex < gameObjects.Length; gameObjectIndex++) {
                bindingFactories[gameObjectIndex] = 
					CreateSingletonBinding(bindingFactory, gameObjects[gameObjectIndex], type, isGameObject);
            }

            return new MultipleBindingConditionFactory(bindingFactories, bindingFactory.binder);
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
        /// <remarks>
        /// Every resolution of a transient prefab will generate a new instance. So, even
        /// if the component resolved from the prefab is destroyed, it won't generate any
        /// missing references in the container.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="prefab">Prefab object.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToPrefab(this IBindingFactory bindingFactory, GameObject prefab) {
            return bindingFactory.ToPrefab(bindingFactory.bindingType, prefab);
        }

        /// <summary>
        /// Binds the key type to a transient <see cref="UnityEngine.Component"/>
        /// of <typeparamref name="T"/> on the prefab.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the prefab
        /// at the moment of the instantiation, it will be added.
        /// </summary>
        /// <remarks>
        /// Every resolution of a transient prefab will generate a new instance. So, even
        /// if the component resolved from the prefab is destroyed, it won't generate any
        /// missing references in the container.
        /// </remarks>
        /// <typeparam name="Type">The component type to bind the GameObject to.</typeparam>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="prefab">Prefab object.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToPrefab<T>(this IBindingFactory bindingFactory, GameObject prefab) where T : Component {
            return bindingFactory.ToPrefab(typeof(T), prefab);
        }

        /// <summary>
        /// Binds the key type to a transient <see cref="UnityEngine.Component"/>
        /// of <paramref name="type"/> on the prefab.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the prefab
        /// at the moment of the instantiation, it will be added.
        /// </summary>
        /// <remarks>
        /// Every resolution of a transient prefab will generate a new instance. So, even
        /// if the component resolved from the prefab is destroyed, it won't generate any
        /// missing references in the container.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="type">The component type.</param>
        /// <param name="prefab">Prefab object.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToPrefab(this IBindingFactory bindingFactory, Type type, GameObject prefab) {
            if (!TypeUtils.IsAssignable(bindingFactory.bindingType, type)) {
                throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
            }

            var isGameObject = TypeUtils.IsAssignable(TYPE_GAME_OBJECT, type);
            var isComponent = TypeUtils.IsAssignable(TYPE_COMPONENT, type);
            if (!isGameObject && !isComponent) {
                throw new BindingException(TYPE_NOT_COMPONENT);
            }

            return new UnityBindingConditionFactory(bindingFactory.AddBinding(new PrefabBinding(prefab, type), 
                    BindingInstance.Transient), prefab.name);
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
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to prefabs that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="prefab">Prefab object.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToPrefabSingleton(this IBindingFactory bindingFactory, GameObject prefab) {
            return bindingFactory.ToPrefabSingleton(bindingFactory.bindingType, prefab);
        }

        /// <summary>
        /// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
        /// of <typeparamref name="T"/> on a newly instantiated prefab.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the prefab
        /// at the moment of the instantiation, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to prefabs that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <typeparam name="Type">The component type to bind the GameObject to.</typeparam>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="prefab">Prefab object.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToPrefabSingleton<T>(this IBindingFactory bindingFactory, GameObject prefab) where T : Component {
            return bindingFactory.ToPrefabSingleton(typeof(T), prefab);
        }

        /// <summary>
        /// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
        /// of <paramref name="type"/> on a newly instantiated prefab.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the prefab
        /// at the moment of the instantiation, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to prefabs that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="type">The component type.</param>
        /// <param name="prefab">Prefab object.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static UnityBindingConditionFactory ToPrefabSingleton(this IBindingFactory bindingFactory, Type type, GameObject prefab) {
            if (!TypeUtils.IsAssignable(bindingFactory.bindingType, type)) {
                throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
            }

            var isGameObject = TypeUtils.IsAssignable(TYPE_GAME_OBJECT, type);
            var isComponent = TypeUtils.IsAssignable(TYPE_COMPONENT, type);
            if (!isGameObject && !isComponent) {
                throw new BindingException(TYPE_NOT_COMPONENT);
            }

            var gameObject = (GameObject) MonoBehaviour.Instantiate(prefab);

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
        /// <remarks>
        /// Every resolution of a transient prefab will generate a new instance. So, even
        /// if the component resolved from the prefab is destroyed, it won't generate any
        /// missing references in the container.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="name">Prefab name. It will be loaded using <c>Resources.Load<c/>.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        [Obsolete("Loading from Resources is not recommended by Unity. Please use ToPrefab(GameObject) instead.")]
        public static UnityBindingConditionFactory ToPrefab(this IBindingFactory bindingFactory, string name) {
            return bindingFactory.ToPrefab(bindingFactory.bindingType, name);
        }

        /// <summary>
        /// Binds the key type to a transient <see cref="UnityEngine.Component"/>
        /// of <typeparamref name="T"/> on the prefab.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the prefab
        /// at the moment of the instantiation, it will be added.
        /// </summary>
        /// <remarks>
        /// Every resolution of a transient prefab will generate a new instance. So, even
        /// if the component resolved from the prefab is destroyed, it won't generate any
        /// missing references in the container.
        /// </remarks>
        /// <typeparam name="Type">The component type to bind the GameObject to.</typeparam>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="name">Prefab name. It will be loaded using <c>Resources.Load<c/>.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        [Obsolete("Loading from Resources is not recommended by Unity. Please use ToPrefab<T>(GameObject) instead.")]
        public static UnityBindingConditionFactory ToPrefab<T>(this IBindingFactory bindingFactory, string name) where T : Component {
            return bindingFactory.ToPrefab(typeof(T), name);
        }

        /// <summary>
        /// Binds the key type to a transient <see cref="UnityEngine.Component"/>
        /// of <paramref name="type"/> on the prefab.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the prefab
        /// at the moment of the instantiation, it will be added.
        /// </summary>
        /// <remarks>
        /// Every resolution of a transient prefab will generate a new instance. So, even
        /// if the component resolved from the prefab is destroyed, it won't generate any
        /// missing references in the container.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="type">The component type.</param>
        /// <param name="name">Prefab name. It will be loaded using <c>Resources.Load<c/>.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        [Obsolete("Loading from Resources is not recommended by Unity. Please use ToPrefab(Type, GameObject) instead.")]
        public static UnityBindingConditionFactory ToPrefab(this IBindingFactory bindingFactory, Type type, string name) {
            if (!TypeUtils.IsAssignable(bindingFactory.bindingType, type)) {
                throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
            }

            var isGameObject = TypeUtils.IsAssignable(TYPE_GAME_OBJECT, type);
            var isComponent = TypeUtils.IsAssignable(TYPE_COMPONENT, type);
            if (!isGameObject && !isComponent) {
                throw new BindingException(TYPE_NOT_COMPONENT);
            }

            var prefab = Resources.Load(name);
            if (prefab == null) {
                throw new BindingException(string.Format(PREFAB_NAME_TYPE_IS_NULL, name, type.ToString()));
            }

            var prefabBinding = new PrefabBinding(prefab, type);

            return new UnityBindingConditionFactory(bindingFactory.AddBinding(prefabBinding, BindingInstance.Transient), prefab.name);
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
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to prefabs that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="name">Prefab name.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        [Obsolete("Loading from Resources is not recommended by Unity. Please use ToPrefabSingleton(GameObject) instead.")]
        public static UnityBindingConditionFactory ToPrefabSingleton(this IBindingFactory bindingFactory, string name) {
            return bindingFactory.ToPrefabSingleton(bindingFactory.bindingType, name);
        }

        /// <summary>
        /// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
        /// of <typeparamref name="T"/> on a newly instantiated prefab.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the prefab
        /// at the moment of the instantiation, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to prefabs that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <typeparam name="Type">The component type to bind the GameObject to.</typeparam>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="name">Prefab name. It will be loaded using <c>Resources.Load<c/>.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        [Obsolete("Loading from Resources is not recommended by Unity. Please use ToPrefabSingleton(GameObject) instead.")]
        public static UnityBindingConditionFactory ToPrefabSingleton<T>(this IBindingFactory bindingFactory, string name) where T : Component {
            return bindingFactory.ToPrefabSingleton(typeof(T), name);
        }

        /// <summary>
        /// Binds the key type to a singleton <see cref="UnityEngine.Component"/>
        /// of <paramref name="type"/> on a newly instantiated prefab.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the prefab
        /// at the moment of the instantiation, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to prefabs that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="type">The component type.</param>
        /// <param name="name">Prefab name. It will be loaded using <c>Resources.Load<c/>.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        [Obsolete("Loading from Resources is not recommended by Unity. Please use ToPrefabSingleton(GameObject) instead.")]
        public static UnityBindingConditionFactory ToPrefabSingleton(this IBindingFactory bindingFactory, Type type, string name) {
            if (!TypeUtils.IsAssignable(bindingFactory.bindingType, type)) {
                throw new BindingException(BindingException.TYPE_NOT_ASSIGNABLE);
            }

            var isGameObject = TypeUtils.IsAssignable(TYPE_GAME_OBJECT, type);
            var isComponent = TypeUtils.IsAssignable(TYPE_COMPONENT, type);
            if (!isGameObject && !isComponent) {
                throw new BindingException(TYPE_NOT_COMPONENT);
            }

            var prefab = Resources.Load(name);
            if (prefab == null) {
                throw new BindingException(string.Format(PREFAB_NAME_TYPE_IS_NULL, name, type.ToString()));
            }
			
            var gameObject = (GameObject) MonoBehaviour.Instantiate(prefab);
			
            return CreateSingletonBinding(bindingFactory, gameObject, type, isGameObject);
        }

        /// <summary>
        /// Binds the key type to a singleton <see cref="UnityEngine.Object"/> loaded
        /// from the Resources folder.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to resources that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="name">Resource name. It will be loaded using <c>Resources.Load<c/>.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        [Obsolete("Loading from Resources is not recommended by Unity. Please use other binding methods.")]
        public static UnityBindingConditionFactory ToResource(this IBindingFactory bindingFactory, string name) {			
            if (!TypeUtils.IsAssignable(typeof(UnityEngine.Object), bindingFactory.bindingType)) {
                throw new BindingException(TYPE_NOT_OBJECT);
            }

            var resource = Resources.Load(name);
            if (resource == null) {
                throw new BindingException(RESOURCE_IS_NULL);
            }

            return new UnityBindingConditionFactory(bindingFactory.AddBinding(resource, BindingInstance.Singleton), resource.name);
        }

        /// <summary>
        /// Creates a singleton binding.
        /// </summary>
        /// <param name="bindingFactory">The binding factory.</param>
        /// <param name="gameObject">The GameObject to bind to.</param>
        /// <param name="type">The type of the binding.</param>
        /// <param name="typeIsGameObject">Indicates whether the type is a GameObject.</param>
        /// <returns>The binding condition object related to the binding.</returns>
        private static UnityBindingConditionFactory CreateSingletonBinding(IBindingFactory bindingFactory, 
                                                                           GameObject gameObject, Type type, bool typeIsGameObject) {
            if (gameObject == null) {
                throw new BindingException(GAMEOBJECT_IS_NULL);
            }
			
            if (typeIsGameObject) {
                return new UnityBindingConditionFactory(bindingFactory.AddBinding(gameObject, BindingInstance.Singleton), gameObject.name);
            } else {
                var component = gameObject.GetComponent(type);
				
                if (component == null) {
                    component = gameObject.AddComponent(type);
                }
				
                return new UnityBindingConditionFactory(bindingFactory.AddBinding(component, BindingInstance.Singleton), gameObject.name);
            }
        }
    }
}
