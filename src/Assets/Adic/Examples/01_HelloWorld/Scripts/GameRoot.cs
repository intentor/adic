using UnityEngine;
using Adic;

namespace Adic.Examples.HelloWorld {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : ContextRoot {
		public override void SetupContainers() {
			//Create the container.
			var container = new InjectionContainer();

			//Bind a class to itself.
			container.Bind<HelloWorld>().ToSelf();

			//Resolve the class and calls its "HelloWorld" method, which will display
			//"Hello World!" on the console.
			container.Resolve<HelloWorld>().DisplayHelloWorld();

			//Add the container to the context.
			this.AddContainer(container);
		}
		
		public override void Init() {
			//Init the game.
		}
	}
}