using UnityEngine;
using Adic;
using Adic.Examples.Factory.Commands;

namespace Adic.Examples.Factory {
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

			//Setup bindings from a namespace.
			container.SetupBindings("Adic.Examples.Factory.Bindings");
			
			//Register all commands under the namespace "Adic.Examples.Factories.Commands".
			container.RegisterCommands("Adic.Examples.Factory.Commands");
			
			//Get a reference to the command dispatcher.
			this.dispatcher = container.GetCommandDispatcher();

			//Add the container to the context.
			this.AddContainer(container);
		}
		
		public override void Init() {
			//Init the game.
			this.dispatcher.Dispatch<SpawnObjectsCommand>();
		}
	}
}