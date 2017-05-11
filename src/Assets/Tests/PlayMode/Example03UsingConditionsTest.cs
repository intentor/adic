using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Tests for example 03 - Using Conditions.
    /// </summary>
    public class Example03UsingConditionsTest {
        [UnityTest]
        public IEnumerator TestBindings() {
            yield return new MonoBehaviourTest<GameObjectCheckTestBehaviour>();
        }

        public class GameObjectCheckTestBehaviour : BaseSceneTestBehaviour {
            protected override string SceneToTest {
                get { return "UsingConditions"; }
            }

            protected override void Evaluate() {
                Assert.NotNull(GameObject.Find("LeftCube"));
                Assert.NotNull(GameObject.Find("RightCube"));

                GameObject rotator = GameObject.Find("Rotator");
                Assert.NotNull(rotator);
                Assert.NotNull(rotator.GetComponent<Adic.Examples.UsingConditions.GameObjectRotator>());

                LogAssert.NoUnexpectedReceived();
            }
        }
    }
}
