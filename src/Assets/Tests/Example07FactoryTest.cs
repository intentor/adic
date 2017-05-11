using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Tests for example 07 - Factory.
    /// </summary>
    public class Example07FactoryTest {
        [UnityTest]
        public IEnumerator TestBindings() {
            yield return new MonoBehaviourTest<GameObjectCheckTestBehaviour>();
        }

        public class GameObjectCheckTestBehaviour : BaseTestBehaviour {
            protected override void Init() {
                SceneManager.LoadScene("Factory", LoadSceneMode.Additive);
            }

            protected override void Evaluate() {
                for (var index = 0; index <= 35; index++) {
                    Assert.NotNull(GameObject.Find(string.Format("Cube {0:00}", index)));
                }
                LogAssert.NoUnexpectedReceived();
                isFinished = true;
            }
        }
    }
}
