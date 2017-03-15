using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Adic.Container;
using Adic.Extensions.ContextRoots;
using Adic.Util;

namespace Adic {
    /// <summary>
    /// Context root MonoBehaviour.
    /// </summary>
    public abstract class ContextRoot : UnityEngine.MonoBehaviour, IContextRoot {
        /// <summary>
        /// Injection container data.
        /// </summary>
        public class InjectionContainerData {
            /// <summary>The injection container.</summary>
            public IInjectionContainer container;
            /// <summary>Indicates whether the container should be destroyed on load.</summary>
            public bool destroyOnLoad;

            /// <summary>
            /// Initializes a new instance of <see cref="Adic.ContextRoot+InjectionContainerData"/>.
            /// </summary>
            /// <param name="container">The injection container.</param>
            /// <param name="destroyOnLoad">Indicates whether the container should be destroyed on load.</param>
            public InjectionContainerData(IInjectionContainer container, bool destroyOnLoad) {
                this.container = container;
                this.destroyOnLoad = destroyOnLoad;
            }
        }

        /// <summary>
        /// MonoBehaviour injection type.
        /// </summary>
        [Serializable]
        public enum MonoBehaviourInjectionType {
            Manual,
            Children,
            BaseType
        }

        [Tooltip("Type of injection on MonoBehaviours."), HideInInspector]
        public MonoBehaviourInjectionType injectionType;
        [Tooltip("Name of the base behaviour type to perform scene injection."), HideInInspector]
        public string baseBehaviourTypeName;

        /// <summary>Internal containers list that will be kept through scenes.</summary>
        public static List<InjectionContainerData> containersData { get; set; }

        /// <summary>Containers list.</summary>
        public IInjectionContainer[] containers { 
            get {
                var allContainers = new IInjectionContainer[containersData.Count];

                for (var containerIndex = 0; containerIndex < containersData.Count; containerIndex++) {
                    allContainers[containerIndex] = containersData[containerIndex].container;
                }

                return allContainers;
            } 
        }

        protected void Awake() {
            if (containersData == null) {
                containersData = new List<InjectionContainerData>(1);
            }
            this.SetupContainers();
            this.InitContainers();
        }

        protected void Start() {
            // The scene injector is added now so its Awake is called before any other Start(), given the script 
            // execution order of the context root is correctly defined.
            this.gameObject.AddComponent<SceneInjector>();

            this.Init();
        }

        protected void OnDestroy() {
            for (var containerIndex = 0; containerIndex < containersData.Count; containerIndex++) {
                var data = containersData[containerIndex];
				
                if (!data.destroyOnLoad)
                    continue;
				
                data.container.Dispose();
                containersData.Remove(data);
                containerIndex--;
            }
        }

        /// <summary>
        /// Adds the specified container.
        /// </summary>
        /// <typeparam name="T">Container type.</typeparam>
        /// <returns>The injection container for chaining.</returns>
        public IInjectionContainer AddContainer<T>() where T : IInjectionContainer, new() {
            var container = Activator.CreateInstance<T>();
            return this.AddContainer(container, true);
        }

        /// <summary>
        /// Adds the specified container.
        /// </summary>
        /// <param name="identifier">The container identifier.</param>
        /// <typeparam name="T">Container type.</typeparam>
        /// <returns>The injection container for chaining.</returns>
        public IInjectionContainer AddContainer<T>(object identifier) where T : IInjectionContainer {
            return this.AddContainer<T>(new Type[] { typeof(object) }, new object[] { identifier });
        }

        /// <summary>
        /// Adds the specified container.
        /// </summary>
        /// <param name="resolutionMode">Instance resolution mode.</param>
        /// <typeparam name="T">Container type.</typeparam>
        /// <returns>The injection container for chaining.</returns>
        public IInjectionContainer AddContainer<T>(ResolutionMode resolutionMode) where T : IInjectionContainer {
            return this.AddContainer<T>(new Type[] { typeof(ResolutionMode) }, new object[] { resolutionMode });
        }

        /// <summary>
        /// Adds the specified container.
        /// </summary>
        /// <param name="identifier">The container identifier.</param>
        /// <param name="resolutionMode">Instance resolution mode.</param>
        /// <typeparam name="T">Container type.</typeparam>
        /// <returns>The injection container for chaining.</returns>
        public IInjectionContainer AddContainer<T>(object identifier, ResolutionMode resolutionMode) where T : IInjectionContainer {
            return this.AddContainer<T>(new Type[] { typeof(object), typeof(ResolutionMode) }, 
                new object[] { identifier, resolutionMode });
        }

        /// <summary>
        /// Adds the specified container.
        /// </summary>
        /// <param name="parameterTypes">Type of the constructor parameters.</param>
        /// <param name="parameterValues">Construtor parameters values.</param>
        /// <typeparam name="T">Container type.</typeparam>
        /// <returns>The injection container for chaining.</returns>
        private IInjectionContainer AddContainer<T>(Type[] parameterTypes, object[] parameterValues) where T : IInjectionContainer {
            var containerType = typeof(T);
            var constructor = containerType.GetConstructor(parameterTypes);
            var container = (IInjectionContainer) constructor.Invoke(parameterValues);
            return this.AddContainer(container, true);
        }

        /// <summary>
        /// Adds the specified container.
        /// </summary>
        /// <param name="container">The container to be added.</param>
        /// <returns>The injection container for chaining.</returns>
        public IInjectionContainer AddContainer(IInjectionContainer container) {
            return this.AddContainer(container, true);
        }

        /// <summary>
        /// Adds the specified container.
        /// </summary>
        /// <param name="container">The container to be added.</param>
        /// <param name="destroyOnLoad">
        /// Indicates whether the container should be destroyed when a new scene is loaded.
        /// </param>
        /// <returns>The injection container for chaining.</returns>
        public IInjectionContainer AddContainer(IInjectionContainer container, bool destroyOnLoad) {
            containersData.Add(new InjectionContainerData(container, destroyOnLoad));

            return container;
        }

        /// <summary>
        /// Setups injection containers.
        /// </summary>
        public abstract void SetupContainers();

        /// <summary>
        /// Inits the game.
        /// 
        /// The idea is to use this method to instantiate any containers and initialize the game.
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Initializes all containers.
        /// </summary>
        private void InitContainers() {
            for (var containerIndex = 0; containerIndex < containersData.Count; containerIndex++) {
                containersData[containerIndex].container.Init();
            }
        }
    }
}