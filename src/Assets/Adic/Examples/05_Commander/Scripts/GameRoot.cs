using UnityEngine;
using Adic;
using Adic.Examples.Commander.Commands;

namespace Adic.Examples.Commander {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : ContextRoot {
		/// <summary>The command dispatcher.</summary>
		protected ICommandDispatcher dispatcher;

		public override void SetupContainers() {
			//Create the container.
			var container = new InjectionContainer();
			//Register any extensions the container may use.
			container.RegisterExtension<CommanderContainerExtension>();
			container.RegisterExtension<EventCallerContainerExtension>();
			container.RegisterExtension<UnityBindingContainerExtension>();

			//Bind the "Prism" prefab.
			container.Bind<Transform>().ToPrefab("Prism");

			//Register all commands under the namespace "Adic.Examples.Commander.Commands".
			container.RegisterCommands("Adic.Examples.Commander.Commands");

			//Get a reference to the command dispatcher.
			this.dispatcher = container.GetCommandDispatcher();

			//Add the container to the context.
			this.AddContainer(container);
		}
		
		public override void Init() {
			//Init the game.
			this.dispatcher.Dispatch<SpawnGameObjectCommand>();
		}
	}
}