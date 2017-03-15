using UnityEngine;
using Adic;

namespace Adic.Examples.Testbed {
    /// <summary>
    /// Game context root.
    /// </summary>
    public class GameRoot : ContextRoot {
        public override void SetupContainers() {
            var container = this.AddContainer<InjectionContainer>();

            container.RegisterExtension<UnityBindingContainerExtension>()
                .Bind<TestBehaviour>().ToGameObject();
        }

        public override void Init() {
            // Init the game.
        }
    }
}