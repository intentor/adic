using UnityEngine;
using Adic;

namespace Adic.Examples.UsingConditions {
    /// <summary>
    /// Game context root.
    /// </summary>
    public class GameRoot : ContextRoot {
        [Tooltip("On scene right cube.")]
        public GameObject rightCube;

        public override void SetupContainers() {
            // Create the container.
            this.AddContainer<InjectionContainer>()
                // Register any extensions the container may use.
				.RegisterExtension<UnityBindingContainerExtension>()
                // Bind a Transform component to the two cubes on the scene, using a "As" condition.
                .Bind<Transform>().ToGameObject("LeftCube").As("LeftCube")
                // Bind a Transform component to the two cubes on the scene, using a "AsObjectName" condition.
                // This condition will use the Unity Object name as identifier. In this case, the Game Object name.
                .Bind<Transform>().ToGameObject(rightCube).AsObjectName()
                // Bind the "GameObjectRotator" component to a new game object with the name "Rotator".
                // This component will then receive the reference to the "LeftCube", making only this cube rotate.
                .Bind<GameObjectRotator>().ToGameObject("Rotator");
        }

        public override void Init() {
            // Init the game.
        }
    }
}