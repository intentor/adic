using UnityEngine;
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

        public class MultiSceneTestBehaviour : BaseSceneTestBehaviour {
            protected override string SceneToTest {
                get { return "TestMultiSceneScene1"; }
            }
            public override float wait {
                get { return 0; }
            }

            protected override void Evaluate() {
                LogAssert.Expect(LogType.Log, "Updating...");
            }
        }
    }
}
