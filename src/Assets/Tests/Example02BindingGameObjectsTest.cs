using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Tests for example 02 - Binding GameObjects.
    /// </summary>
    public class Example02BindingGameObjectsTest {
        [UnityTest]
        public IEnumerator TestBinding() {
            yield return new MonoBehaviourTest<GameObjectCheckTestBehaviour>();
        }

        public class GameObjectCheckTestBehaviour : BaseTestBehaviour {
            public override float wait {
                get { return 0.1f; }
            }

            protected override void Init() {
                SceneManager.LoadScene("BindingGameObjects", LoadSceneMode.Additive);
            }

            protected override void Evaluate() {
                Assert.NotNull(GameObject.Find("GameObjectRotator"));
                LogAssert.NoUnexpectedReceived();
                isFinished = true;
            }

            protected override void Clean() {
                //SceneManager.UnloadSceneAsync("BindingGameObjects");
            }
        }
    }
}
