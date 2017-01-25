using Adic;

namespace Assets.Test.MultiScene {
    /// <summary>
    /// Context root for the second scene.
    /// </summary>
    public class SecondContext : ContextRoot {
        public override void SetupContainers() {
            this.AddContainer(new InjectionContainer("SecondSceneDestroy"))
                .RegisterExtension<EventCallerContainerExtension>();
        }

        public override void Init() {
            //Init the game.
        }
    }
}
