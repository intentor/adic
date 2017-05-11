using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Tests for example 06 - Bindings Setup.
    /// </summary>
    public class Example06BindingsSetupTest {
        [UnityTest]
        public IEnumerator TestBindings() {
            yield return new MonoBehaviourTest<GameObjectCheckTestBehaviour>();
        }

        public class GameObjectCheckTestBehaviour : BaseSceneTestBehaviour {
            protected override string SceneToTest {
                get { return "BindingsSetup"; }
            }

            protected override void Evaluate() {
                Assert.NotNull(GameObject.Find("CubeA(Clone)"));
                Assert.NotNull(GameObject.Find("CubeB(Clone)"));
                Assert.NotNull(GameObject.Find("CubeC(Clone)"));
                LogAssert.NoUnexpectedReceived();
            }
        }
    }
}
