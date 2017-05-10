using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Tests MultiScene.
    /// </summary>
    public class MultiSceneTest {
        [UnityTest]
        public IEnumerator TestLogUpdateData() {
            yield return new MonoBehaviourTest<MultiSceneTestBehaviour>();
        }

        public class MultiSceneTestBehaviour : BaseTestBehaviour {
            protected override void Init() {
                SceneManager.LoadScene("TestMultiSceneScene1", LoadSceneMode.Additive);
            }

            protected override void Evaluate() {
                LogAssert.Expect(LogType.Log, "Updating...");
                LogAssert.NoUnexpectedReceived();
                isFinished = true;
            }

            protected override void Clean() {
                //SceneManager.UnloadSceneAsync("TestMultiSceneScene1");
                //SceneManager.UnloadSceneAsync("TestMultiSceneScene2");
            }
        }
    }
}
