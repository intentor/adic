using UnityEngine;
using Intentor.Adic;

namespace Intentor.Adic.Examples.HelloWorld {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : Intentor.Adic.ContextRoot {
		public override void SetupContainers() {
			//Creates the container.
			var container = new InjectionContainer();
			//Adds any extensions the container may use.
			container.RegisterExtension<UnityBindingContainerExtension>();

			//Binds a MonoBehaviour to a GameObject.
			//The GameObject will be automaticaly created by the UnityBinding extension.
			container.Bind<HelloWorld>().ToGameObject();

			//Adds the container to the context.
			this.AddContainer(container);
		}
		
		public override void Init() {
			//Init the game.
		}
	}
}