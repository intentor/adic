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
            // Create the container.
            this.dispatcher = this.AddContainer<InjectionContainer>()				
                // Register any extensions the container may use.
				.RegisterExtension<CommanderContainerExtension>()
				.RegisterExtension<EventCallerContainerExtension>()
				.RegisterExtension<UnityBindingContainerExtension>()
                // Setup bindings from a namespace.
				.SetupBindings("Adic.Examples.Factory.Bindings")			
                // Register all commands under the namespace "Adic.Examples.Factories.Commands".
				.RegisterCommands("Adic.Examples.Factory.Commands")
                // Get a reference to the dispatcher so it can be used to dispatch commands in the Init() method.
				.GetCommandDispatcher();
        }

        public override void Init() {
            // Init the game.
            this.dispatcher.Dispatch<SpawnObjectsCommand>();
        }
    }
}