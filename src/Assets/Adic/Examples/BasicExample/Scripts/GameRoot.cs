using UnityEngine;
using Intentor.Adic;

namespace Adic.Example {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : Intentor.Adic.ContextRoot {
		public override void SetupContainers() {
			var container = new InjectionContainer();
			container.RegisterExtension<UnityBindingContainerExtension>();

			//Binds a simple depency that will be injected on CubeRotator.
			container.Bind<Dependency>().ToSelf();
			//Binds the "Cube" prefab. It will be injected on CubeRotator.
			container.Bind<Transform>().ToPrefab("Cube").As("cube");
			//Binds the "Plane" prefab. It exists just to make the scene less empty.
			container.Bind<GameObject>().ToPrefabSingleton("Plane");
			//Binds the CubeRotator to a new GameObject, so it can start rotating the "Cube".
			container.Bind<CubeRotator>().ToGameObject();

			this.AddContainer(container);
		}
		
		public override void Init() {
			//Init the game.
		}
	}
}