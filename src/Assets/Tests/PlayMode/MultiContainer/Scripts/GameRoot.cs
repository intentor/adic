using Adic;
using Adic.Container;
using Assets.Test.MultiContainer.Command;
using Assets.Test.MultiContainer.Example;

namespace Assets.Test.MultiContainer {
    /// <summary>
    /// Test context root.
    /// </summary>
    public class GameRoot : ContextRoot {
        /// <summary>Command to test.</summary>
        [Inject] 
        private TestCommand Command;

        public override void SetupContainers() {
            this.AddContainer<InjectionContainer>("container1")
                .RegisterExtension<CommanderContainerExtension>()
                .RegisterCommands("Assets.Test.MultiContainer.Command")
                .Bind<ITestInterface>().ToNamespace("Assets.Test.MultiContainer.Example");
                
            this.AddContainer<InjectionContainer>("container2");
        }

        public override void Init() {
            this.Inject();

            //Should log test values.
            this.Command.Execute();
        }
    }
}