using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Base test behaviour.
    /// </summary>
    public abstract class BaseSceneTestBehaviour : MonoBehaviour, IMonoBehaviourTest {
        /// <summary>Indicates whether the test is finished.</summary>
        protected bool isFinished = false;

        /// <summary>Indicates whether the test is finished.</summary>
        public bool IsTestFinished  {
            get { return isFinished; }
        }

        /// <summary>Scene to test.</summary>
        protected abstract string SceneToTest {
            get;
        }
        /// <summary>Time to wait before evaluation (seconds).</summary>
        public virtual float wait {
            get { return 0.1f; }
        }

    	private void Awake() {
            this.LoadScene();
    	}
    	
        private void Start() {
            this.StartCoroutine(this.CheckEvaluation());
    	}

        /// <summary>
        /// Checks whether the evaluation shout take place.
        /// </summary>
        /// <returns>Coroutine enumerator.</returns>
        private IEnumerator CheckEvaluation() {
            yield return new WaitForEndOfFrame();

            if (this.wait > 0) {
                yield return new WaitForSeconds(this.wait);
            }

            this.Evaluate();

            yield return new WaitForEndOfFrame();

            this.ClearContainers();
            this.UnloadScene();

            yield return new WaitForEndOfFrame();

            this.isFinished = true;

            yield return null;
        }

        /// <summary>
        /// Clear all available containers.
        /// </summary>
        private void ClearContainers() {
            foreach (var containerData in ContextRoot.containersData) {
                containerData.container.Clear();
            }
            ContextRoot.containersData.Clear();
        }

        /// <summary>
        /// Load the test scene.
        /// </summary>
        private void LoadScene() {
            SceneManager.LoadScene(this.SceneToTest, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Unload the scene.
        /// </summary>
        private void UnloadScene() {
            SceneManager.UnloadSceneAsync(this.SceneToTest);
        }

        /// <summary>
        /// Executes evaluations.
        /// </summary>
        protected abstract void Evaluate();
    }
}
