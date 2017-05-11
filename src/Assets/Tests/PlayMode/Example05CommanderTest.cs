using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Tests for example 05 - Commander.
    /// </summary>
    public class Example05CommanderTest {
        [UnityTest]
        public IEnumerator TestBindings() {
            yield return new MonoBehaviourTest<GameObjectCheckTestBehaviour>();
        }

        public class GameObjectCheckTestBehaviour : BaseSceneTestBehaviour {
            protected override string SceneToTest {
                get { return "Commander"; }
            }
            public override float wait {
                get { return 1.25f; }
            }

            protected override void Evaluate() {
                Assert.NotNull(GameObject.Find("Prism(Clone)"));
                LogAssert.Expect(LogType.Log, "RotateGameObjectCommand released");
                LogAssert.NoUnexpectedReceived();
            }
        }
    }
}
