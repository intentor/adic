using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Tests for example 08 - Unity Events.
    /// </summary>
    public class Example08UnityEventsTest {
        [UnityTest]
        public IEnumerator TestBindings() {
            yield return new MonoBehaviourTest<GameObjectCheckTestBehaviour>();
        }

        public class GameObjectCheckTestBehaviour : BaseSceneTestBehaviour {
            protected override string SceneToTest {
                get { return "UnityEvents"; }
            }

            protected override void Evaluate() {
                LogAssert.Expect(LogType.Log, "Fixed updating...");
                LogAssert.Expect(LogType.Log, "Updating...");
                LogAssert.Expect(LogType.Log, "Late updating...");
            }
        }
    }
}
