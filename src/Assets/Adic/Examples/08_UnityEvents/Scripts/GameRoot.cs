using UnityEngine;
using Adic;

namespace Adic.Examples.Events {
    /// <summary>
    /// Game context root.
    /// </summary>
    public class GameRoot : ContextRoot {
        public override void SetupContainers() {
            // Create the container.
            this.AddContainer<InjectionContainer>()
                .RegisterExtension<EventCallerContainerExtension>()
                .Bind<EventReceiver>().ToSingleton();
        }

        public override void Init() {
            // Init the game.
        }
    }
}