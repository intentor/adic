using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

namespace Adic.Tests {
    /// <summary>
    /// Base test behaviour.
    /// </summary>
    public abstract class BaseTestBehaviour : MonoBehaviour, IMonoBehaviourTest {
        /// <summary>Indicates whether the test is finished.</summary>
        protected bool isFinished = false;

        /// <summary>Indicates whether the test is finished.</summary>
        public bool IsTestFinished  {
            get { return isFinished; }
        }

        /// <summary>Time to wait before evaluation (seconds).</summary>
        public virtual float wait {
            get { return 0.1f; }
        }

    	private void Awake() {
            this.Init();
    	}
    	
        private void Start() {
            if (this.wait == 0) {
                this.Evaluate();
            } else {
                this.StartCoroutine(this.CheckEvaluation());
            }
    	}

        /// <summary>
        /// Checks whether the evaluation shout take place.
        /// </summary>
        /// <returns>Coroutine enumerator.</returns>
        private IEnumerator CheckEvaluation() {
            yield return new WaitForEndOfFrame();

            yield return new WaitForSeconds(this.wait);

            this.Evaluate();

            yield return null;
        }

        /// <summary>
        /// Initializes the test.
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// Executes evaluations.
        /// </summary>
        protected abstract void Evaluate();
    }
}
