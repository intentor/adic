using UnityEngine;
using Adic;

namespace Adic.Examples.BindingsSetup {
    /// <summary>
    /// Game context root.
    /// </summary>
    public class GameRoot : ContextRoot {
        public override void SetupContainers() {
            // Create the container.
            this.AddContainer<InjectionContainer>()
                // Register any extensions the container may use.
    			.RegisterExtension<UnityBindingContainerExtension>()
                // Setup bindings from a namespace.
				.SetupBindings("Adic.Examples.BindingsSetup.Bindings");
        }

        public override void Init() {

        }
    }
}