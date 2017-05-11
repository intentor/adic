using UnityEngine;
using UnityEngine.SceneManagement;
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

        public class GameObjectCheckTestBehaviour : BaseTestBehaviour {
            protected override void Init() {
                SceneManager.LoadScene("Prefabs", LoadSceneMode.Additive);
            }

            protected override void Evaluate() {
                Assert.NotNull(GameObject.Find("Plane(Clone)"));
                Assert.NotNull(GameObject.Find("Cube(Clone)"));
                LogAssert.NoUnexpectedReceived();
                isFinished = true;
            }
        }
    }
}
