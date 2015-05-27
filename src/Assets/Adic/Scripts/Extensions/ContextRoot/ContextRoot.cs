using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Adic.Container;
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
			this.CacheBindings();
		}

		protected void Start() {
			this.Init();
		}
		
		protected void OnDestroy() {
			for (var containerIndex = 0; containerIndex < containersData.Count; containerIndex++) {
				var data = containersData[containerIndex];
				
				if (!data.destroyOnLoad) continue;
				
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
			var container = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile()();
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
		/// Caches all the bindings on containers.
		/// </summary>
		private void CacheBindings() {
			for (var containerIndex = 0; containerIndex < containersData.Count; containerIndex++) {
				var container = containersData[containerIndex].container;
				container.cache.CacheFromBinder(container);
			}
		}
	}
}