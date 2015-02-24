using UnityEngine;
using Adic;

namespace Adic.Examples.Prefabs {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : ContextRoot {
		public override void SetupContainers() {
			//Creates the container.
			var container = new InjectionContainer();
			//Adds any extensions the container may use.
			container.RegisterExtension<UnityBindingContainerExtension>();
			
			//Binds the "Cube" prefab. It will be injected on CubeRotator.
			container.Bind<Transform>().ToPrefab("Cube").As("cube");
			//Binds the "Plane" prefab. It exists just to make the scene less empty.
			container.Bind<GameObject>().ToPrefabSingleton("Plane");
			
			//Adds the container to the context.
			this.AddContainer(container);
		}
		
		public override void Init() {
			//Init the game.
		}
	}
}