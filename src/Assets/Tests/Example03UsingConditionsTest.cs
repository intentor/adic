using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Tests for example 03 - Using Conditions.
    /// </summary>
    public class Example03UsingConditionsTest {
        [UnityTest]
        public IEnumerator TestBindings() {
            yield return new MonoBehaviourTest<GameObjectCheckTestBehaviour>();
        }

        public class GameObjectCheckTestBehaviour : BaseTestBehaviour {
            protected override void Init() {
                SceneManager.LoadScene("UsingConditions", LoadSceneMode.Additive);
            }

            protected override void Evaluate() {
                Assert.NotNull(GameObject.Find("LeftCube"));
                Assert.NotNull(GameObject.Find("RightCube"));
                Assert.NotNull(GameObject.Find("GameObjectRotator"));
                LogAssert.NoUnexpectedReceived();
                isFinished = true;
            }
        }
    }
}
