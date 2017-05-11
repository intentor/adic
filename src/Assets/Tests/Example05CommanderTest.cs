using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Tests for example 05 - Commander.
    /// </summary>
    public class Example05CommanderTest {
        [UnityTest]
        public IEnumerator TestBindings() {
            yield return new MonoBehaviourTest<GameObjectCheckTestBehaviour>();
        }

        public class GameObjectCheckTestBehaviour : BaseTestBehaviour {
            protected override void Init() {
                SceneManager.LoadScene("Commander", LoadSceneMode.Additive);
            }

            protected override void Evaluate() {
                Assert.NotNull(GameObject.Find("Prism(Clone)"));
                LogAssert.NoUnexpectedReceived();
                isFinished = true;
            }
        }
    }
}
