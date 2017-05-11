using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Tests for example 07 - Factory.
    /// </summary>
    public class Example07FactoryTest {
        [UnityTest]
        public IEnumerator TestBindings() {
            yield return new MonoBehaviourTest<GameObjectCheckTestBehaviour>();
        }

        public class GameObjectCheckTestBehaviour : BaseSceneTestBehaviour {
            protected override string SceneToTest {
                get { return "Factory"; }
            }

            protected override void Evaluate() {
                for (var index = 0; index <= 35; index++) {
                    this.assertGameObject(string.Format("Cube {0:00}", index));
                }
                LogAssert.NoUnexpectedReceived();
            }

            /// <summary>
            /// Asserts a game object for the test.
            /// </summary>
            /// <param name="path">Game object path on the scene.</param>
            private void assertGameObject(string path) {
                GameObject gameObjectToAssert = GameObject.Find(path);

                Assert.NotNull(gameObjectToAssert);
                Assert.NotNull(gameObjectToAssert.GetComponent<Adic.Examples.Factory.Behaviours.Cube>());
                Assert.NotNull(gameObjectToAssert.GetComponent<Adic.Examples.Factory.Behaviours.Rotator>());
            }
        }
    }
}
