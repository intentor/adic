using UnityEngine;
using Adic;

namespace Adic.Examples.UsingConditions {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : ContextRoot {
		public override void SetupContainers() {
			//Create the container.
			this.AddContainer<InjectionContainer>()
				//Register any extensions the container may use.
				.RegisterExtension<UnityBindingContainerExtension>()
				//Bind a Transform component to the two cubes on the scene, using a "As" condition
				//to define their identifiers.
				.Bind<Transform>().ToGameObject("LeftCube").As("LeftCube")
				.Bind<Transform>().ToGameObject("RightCube").As("RightCube")
				//Bind the "GameObjectRotator" component to a new game object of the same name.
				//This component will then receive the reference to the "LeftCube", making only
				//this cube rotate.
				.Bind<GameObjectRotator>().ToGameObject();
		}
		
		public override void Init() {
			//Init the game.
		}
	}
}