using UnityEngine;
using Adic.Container;

namespace Adic.Examples.BindingsSetup.Bindings {
    /// <summary>
    /// Bindings for prefabs.
    /// </summary>
    public class PrefabBindings : Adic.IBindingsSetup {
        public void SetupBindings(IInjectionContainer container) {
            container
                // Bind "CubeA" prefab.
				.Bind<Transform>().ToPrefabSingleton("06_BindingsSetup/CubeA")
                // Bind "CubeB" prefab.
				.Bind<Transform>().ToPrefabSingleton("06_BindingsSetup/CubeB")
                // Bind "CubeC" prefab.
				.Bind<Transform>().ToPrefabSingleton("06_BindingsSetup/CubeC");
        }
    }
}