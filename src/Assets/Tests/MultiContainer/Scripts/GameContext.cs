using Adic;
using Adic.Container;
using Assets.Test.MultiContainer.Command;
using Assets.Test.MultiContainer.Example;

namespace Assets.Test.MultiContainer {
    /// <summary>
    /// Test context root.
    /// </summary>
    public class GameContext : ContextRoot {
        /// <summary>Command to test.</summary>
        [Inject] 
        private TestCommand Command;

        public override void SetupContainers() {
            var containerOne = this.AddContainer<InjectionContainer>("container1");
            containerOne.RegisterExtension<CommanderContainerExtension>()
                .Bind<ITestInterface>().ToNamespace("Assets.Test.MultiContainer.Example");
            containerOne.RegisterCommands("Assets.Test.MultiContainer.Command");
                
            this.AddContainer<InjectionContainer>("container2");
        }

        public override void Init() {
            this.Inject();

            //Should log test values.
            this.Command.Execute();
        }
    }
}