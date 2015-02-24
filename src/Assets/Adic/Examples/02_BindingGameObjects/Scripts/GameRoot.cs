using UnityEngine;
using Adic;

namespace Adic.Examples.BindingGameObjects {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : ContextRoot {
		public override void SetupContainers() {
			//Creates the container.
			var container = new InjectionContainer();
			//Adds any extensions the container may use.
			container.RegisterExtension<UnityBindingContainerExtension>();

			//Binds a Transform component to the "Cube" GameObject.
			container.Bind<Transform>().ToGameObject("Cube");
			//Binds the "GameObjectRotator" component to a new GameObject of the same name.
			//This component will then receive the reference above so it can rotate the cube.
			container.Bind<GameObjectRotator>().ToGameObject();
			
			//Adds the container to the context.
			this.AddContainer(container);
		}
		
		public override void Init() {
			//Init the game.
		}
	}
}