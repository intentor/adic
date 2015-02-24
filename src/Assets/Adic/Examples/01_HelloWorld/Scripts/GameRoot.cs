using UnityEngine;
using Adic;

namespace Adic.Examples.HelloWorld {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : ContextRoot {
		public override void SetupContainers() {
			//Creates the container.
			var container = new InjectionContainer();

			//Binds a class to itself.
			container.Bind<HelloWorld>().ToSelf();
			//Resolves the class and calls its "HelloWorld" method, which will display
			//"Hello World!" on the console.
			container.Resolve<HelloWorld>().DisplayHelloWorld();

			//Adds the container to the context.
			this.AddContainer(container);
		}
		
		public override void Init() {
			//Init the game.
		}
	}
}