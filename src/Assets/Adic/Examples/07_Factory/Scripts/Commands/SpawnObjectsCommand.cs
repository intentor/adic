using UnityEngine;
using Adic.Container;
using Adic.Examples.Factory.Behaviours;

namespace Adic.Examples.Factory.Commands {
    /// <summary>
    /// Spawns a series of cubes in the scene.
    /// </summary>
    public class SpawnObjectsCommand : Command {
        /// <summary>Container that dispatched the command.</summary>
        [Inject]
        public IInjectionContainer container;

        public override void Execute(params object[] parameters) {
            // Spawn 36 cubes in the scene.
            // Cubes have been bound to the "GameObject" type.
            for (var cubeIndex = 0; cubeIndex < 36; cubeIndex++) {
                var cube = container.Resolve<GameObject>();
                cube.name = string.Format("Cube {0:00}", cubeIndex);
            }
        }
    }
}