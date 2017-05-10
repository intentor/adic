using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Tests MultiContainer.
    /// </summary>
    public class MultiContainerTest {
        [UnityTest]
        public IEnumerator TestLogContainersData() {
            yield return new MonoBehaviourTest<MultiContainerTestBehaviour>();
        }

        public class MultiContainerTestBehaviour : BaseTestBehaviour {
            protected override void Init() {
                SceneManager.LoadScene("MultiContainer", LoadSceneMode.Additive);
            }

            protected override void Evaluate() {
                LogAssert.Expect(LogType.Log, "666");
                LogAssert.Expect(LogType.Log, "2411");
                LogAssert.NoUnexpectedReceived();
                isFinished = true;
            }

            protected override void Clean() {
                //SceneManager.UnloadSceneAsync("MultiContainer");
            }
        }
    }
}
