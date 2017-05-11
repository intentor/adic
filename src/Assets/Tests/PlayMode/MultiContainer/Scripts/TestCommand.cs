using Adic;
using Assets.Test.MultiContainer.Example;

namespace Assets.Test.MultiContainer.Command {
    /// <summary>
    /// Test command.
    /// </summary>
    public class TestCommand : Adic.Command {
        /// <summary>The test classes.</summary>
        [Inject]
        private ITestInterface[] testClasses;

        public override void Execute(params object[] parameters) {
            foreach (var testClass in testClasses) {
                UnityEngine.Debug.Log(testClass.value);
            }
        }
    }
}