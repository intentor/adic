using UnityEngine;
using Adic;

namespace Adic.Examples.Prefabs {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : ContextRoot {
		public override void SetupContainers() {
			//Create the container.
			var container = new InjectionContainer();
			//Register any extensions the container may use.
			container.RegisterExtension<UnityBindingContainerExtension>();
						
			//Bindings.
			container
				//Bind the "Cube" prefab. It will be injected in CubeRotator.
				.Bind<Transform>().ToPrefab("04_Prefabs/Cube").As("cube")
				//Bind the "Plane" prefab. It exists just to make the scene less empty.
				.Bind<GameObject>().ToPrefabSingleton("04_Prefabs/Plane");
			
			//Add the container to the context.
			this.AddContainer(container);
		}
		
		public override void Init() {
			//Init the game.
		}
	}
}