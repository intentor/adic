using UnityEngine;
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

        public class LogHelloWorldTestBehaviour : BaseSceneTestBehaviour {
            protected override string SceneToTest {
                get { return "HelloWorld"; }
            }

            protected override void Evaluate() {
                LogAssert.Expect(LogType.Log, "Hello, world!");
                LogAssert.NoUnexpectedReceived();
            }
        }
    }
}
