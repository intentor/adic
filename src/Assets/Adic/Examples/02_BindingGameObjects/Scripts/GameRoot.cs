using UnityEngine;
using Adic;

namespace Adic.Examples.BindingGameObjects {
    /// <summary>
    /// Game context root.
    /// </summary>
    public class GameRoot : ContextRoot {
        public override void SetupContainers() {
            // Create the container.
            this.AddContainer<InjectionContainer>()
                // Register any extensions the container may use.
				.RegisterExtension<UnityBindingContainerExtension>()
                // Bind a Transform component to the "Cube" game object in the scene.
				.Bind<Transform>().ToGameObject("Cube")
                // Bind the "GameObjectRotator" component to a new ame object of the same name.
                // This component will then receive the reference above so it can rotate the cube.
				.Bind<GameObjectRotator>().ToGameObject();
        }

        public override void Init() {
            // Init the game.
        }
    }
}