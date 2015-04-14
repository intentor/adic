using UnityEngine;
using Adic;

namespace Adic.Examples.UsingConditions {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : ContextRoot {
		public override void SetupContainers() {
			//Create the container.
			var container = new InjectionContainer();
			//Register any extensions the container may use.
			container.RegisterExtension<UnityBindingContainerExtension>();

			//Bind a Transform component to the two cubes on the scene, using a "As" condition
			//to define their identifiers.
			container.Bind<Transform>().ToGameObject("LeftCube").As("LeftCube");
			container.Bind<Transform>().ToGameObject("RightCube").As("RightCube");
			//Bind the "GameObjectRotator" component to a new game object of the same name.
			//This component will then receive the reference to the "LeftCube", making only
			//this cube rotate.
			container.Bind<GameObjectRotator>().ToGameObject();
			
			//Add the container to the context.
			this.AddContainer(container);
		}
		
		public override void Init() {
			//Init the game.
		}
	}
}