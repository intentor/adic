using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Tests for example 04 - Prefabs.
    /// </summary>
    public class Example04PrefabsTest {
        [UnityTest]
        public IEnumerator TestBindings() {
            yield return new MonoBehaviourTest<GameObjectCheckTestBehaviour>();
        }

        public class GameObjectCheckTestBehaviour : BaseSceneTestBehaviour {
            protected override string SceneToTest {
                get { return "Prefabs"; }
            }

            protected override void Evaluate() {
                Assert.NotNull(GameObject.Find("Plane(Clone)"));
                Assert.NotNull(GameObject.Find("Cube(Clone)"));
                LogAssert.NoUnexpectedReceived();
            }
        }
    }
}
