using UnityEngine;
using Adic;

namespace Adic.Examples.BindingsSetup {
	/// <summary>
	/// Game context root.
	/// </summary>
	public class GameRoot : ContextRoot {
		public override void SetupContainers() {
			//Create the container.
			var container = new InjectionContainer();
			//Register any extensions the container may use.
			container.RegisterExtension<UnityBindingContainerExtension>();

			//Setups bindings from a namespace.
			container.SetupBindings("Adic.Examples.BindingsSetup.Bindings");

			//Add the container to the context.
			this.AddContainer(container);
		}
		
		public override void Init() {

		}
	}
}