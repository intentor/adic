using UnityEngine;
using Adic;

namespace Adic.Examples.BindingGameObjects {
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
				//Bind a Transform component to the "Cube" game object in the scene.
				.Bind<Transform>().ToGameObject("Cube")
				//Bind the "GameObjectRotator" component to a new ame object of the same name.
				//This component will then receive the reference above so it can rotate the cube.
				.Bind<GameObjectRotator>().ToGameObject();
			
			//Add the container to the context.
			this.AddContainer(container);
		}
		
		public override void Init() {
			//Init the game.
		}
	}
}