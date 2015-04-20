using UnityEngine;
using Adic;

namespace Adic.Examples.Prefabs {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : ContextRoot {
		public override void SetupContainers() {
			//Create the container.
			this.AddContainer<InjectionContainer>()
				//Register any extensions the container may use.
				.RegisterExtension<UnityBindingContainerExtension>()
				//Bind the "Cube" prefab. It will be injected in CubeRotator.
				.Bind<Transform>().ToPrefab("04_Prefabs/Cube").As("cube")
				//Bind the "Plane" prefab. It exists just to make the scene less empty.
				.Bind<GameObject>().ToPrefabSingleton("04_Prefabs/Plane");
		}
		
		public override void Init() {
			//Init the game.
		}
	}
}