using UnityEngine;
using Adic.Container;

namespace Adic.Examples.BindingsSetup.Bindings {
	/// <summary>
	/// Bindings for prefabs.
	/// </summary>
	public class PrefabsBindings : Adic.IBindingsSetup {
		public void SetupBindings(IInjectionContainer container) {
			//Bind the "CubeA" prefab.
			container.Bind<Transform>().ToPrefabSingleton("06_BindingsSetup/CubeA");
			//Bind the "CubeB" prefab.
			container.Bind<Transform>().ToPrefabSingleton("06_BindingsSetup/CubeB");
			//Bind the "CubeC" prefab.
			container.Bind<Transform>().ToPrefabSingleton("06_BindingsSetup/CubeC");
		}		
	}
}