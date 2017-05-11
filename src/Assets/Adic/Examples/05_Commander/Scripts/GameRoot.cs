using UnityEngine;
using Adic;
using Adic.Examples.Commander.Commands;

namespace Adic.Examples.Commander {
    /// <summary>
    /// Game context root.
    /// </summary>
    public class GameRoot : ContextRoot {
        /// <summary>Rotator command tag.</summary>
        public const string ROTATOR_COMMAND_TAG = "Rotator";

        [Tooltip("Prefab for the prism.")]
        public GameObject prismPrefab;

        /// <summary>The command dispatcher.</summary>
        protected ICommandDispatcher dispatcher;

        public override void SetupContainers() {
            // Create the container.
            var container = this.AddContainer<InjectionContainer>();

            container
                // Register any extensions the container may use.
				.RegisterExtension<CommanderContainerExtension>()
				.RegisterExtension<EventCallerContainerExtension>()
				.RegisterExtension<UnityBindingContainerExtension>()
                // Register all commands under the namespace "Adic.Examples.Commander.Commands".
				.RegisterCommands("Adic.Examples.Commander.Commands")
                // Bind the "Prism" prefab.
                .Bind<Transform>().ToPrefab(prismPrefab);
		
            // Get a reference to the command dispatcher so it can be used to dispatch commands in the Init() method.
            this.dispatcher = container.GetCommandDispatcher();
        }

        public override void Init() {
            // Init the game.
            this.dispatcher.Dispatch<SpawnGameObjectCommand>();

            this.Invoke("StopRotation", 1.0f);
        }

        /// <summary>
        /// Stops the rotation.
        /// 
        /// <see cref="RotateGameObjectCommand"/> is dispatched with the tag "Rotator", so it can be released.
        /// </summary>
        private void StopRotation() {
            this.dispatcher.ReleaseAll(ROTATOR_COMMAND_TAG);
        }
    }
}