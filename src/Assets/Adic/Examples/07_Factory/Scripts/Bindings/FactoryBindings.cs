using UnityEngine;
using Adic.Container;

namespace Adic.Examples.Factory.Bindings {
    /// <summary>
    /// Bindings for factories.
    /// </summary>
    public class PrefabsBindings : Adic.IBindingsSetup {
        public void SetupBindings(IInjectionContainer container) {
            // Bind "GameObject" to the "CubeFactory".
            container.Bind<GameObject>().ToFactory<CubeFactory>();
        }
    }
}