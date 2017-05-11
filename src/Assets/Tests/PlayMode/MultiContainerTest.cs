using UnityEngine;
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

        public class MultiContainerTestBehaviour : BaseSceneTestBehaviour {
            protected override string SceneToTest {
                get { return "MultiContainer"; }
            }

            protected override void Evaluate() {
                LogAssert.Expect(LogType.Log, "666");
                LogAssert.Expect(LogType.Log, "2411");
            }
        }
    }
}
