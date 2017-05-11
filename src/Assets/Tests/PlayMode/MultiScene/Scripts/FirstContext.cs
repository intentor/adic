using Adic;

namespace Assets.Test.MultiScene {
    /// <summary>
    /// Context root for the first scene.
    /// </summary>
    public class FirstContext : ContextRoot {
        public override void SetupContainers() {
            this.AddContainer(new InjectionContainer("FirstSceneDontDestroy"), false)
                .RegisterExtension<EventCallerContainerExtension>()
                .Bind<UpdateLogger>().ToSingleton();

            this.AddContainer(new InjectionContainer("FirstSceneDestroy"))
                .RegisterExtension<EventCallerContainerExtension>();
        }

        public override void Init() {
            // Init the game.
        }
    }
}
