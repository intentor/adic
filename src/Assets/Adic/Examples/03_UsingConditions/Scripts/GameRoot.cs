using UnityEngine;
using Adic;

namespace Adic.Examples.UsingConditions {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : ContextRoot {
		public override void SetupContainers() {
			//Creates the container.
			var container = new InjectionContainer();
			//Adds any extensions the container may use.
			container.RegisterExtension<UnityBindingContainerExtension>();

			//Binds a Transform component to the two cubes on the scene, using a "As" condition
			//to define their identifiers.
			container.Bind<Transform>().ToGameObject("LeftCube").As("LeftCube");
			container.Bind<Transform>().ToGameObject("RightCube").As("RightCube");
			//Binds the "GameObjectRotator" component to a new GameObject of the same name.
			//This component will then receive the reference to the "LeftCube" so only it
			//will rotate.
			container.Bind<GameObjectRotator>().ToGameObject();
			
			//Adds the container to the context.
			this.AddContainer(container);
		}
		
		public override void Init() {
			//Init the game.
		}
	}
}