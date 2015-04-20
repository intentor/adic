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
			var container = this.AddContainer<InjectionContainer>();

			container
				//Register any extensions the container may use.
				.RegisterExtension<CommanderContainerExtension>()
				.RegisterExtension<EventCallerContainerExtension>()
				.RegisterExtension<UnityBindingContainerExtension>()
				//Register all commands under the namespace "Adic.Examples.Commander.Commands".
				.RegisterCommands("Adic.Examples.Commander.Commands")
				//Bind the "Prism" prefab.
				.Bind<Transform>().ToPrefab("05_Commander/Prism");
		
			//Get a reference to the command dispatcher so it can be used to dispatch
			//commands in the Init() method.
			this.dispatcher = container.GetCommandDispatcher();
		}
		
		public override void Init() {
			//Init the game.
			this.dispatcher.Dispatch<SpawnGameObjectCommand>();
		}
	}
}