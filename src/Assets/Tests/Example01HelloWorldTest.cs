using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Tests for example 01 - Hello World.
    /// </summary>
    public class Example01HelloWorldTest {
        [UnityTest]
        public IEnumerator TestLogHelloWorld() {
            yield return new MonoBehaviourTest<LogHelloWorldTestBehaviour>();
        }

        public class LogHelloWorldTestBehaviour : BaseTestBehaviour {
            protected override void Init() {
                SceneManager.LoadScene("HelloWorld", LoadSceneMode.Additive);
            }

            protected override void Evaluate() {
                LogAssert.Expect(LogType.Log, "Hello, world!");
                LogAssert.NoUnexpectedReceived();
                isFinished = true;
            }

            protected override void Clean() {
                //SceneManager.UnloadSceneAsync("HelloWorld");
            }
        }
    }
}
