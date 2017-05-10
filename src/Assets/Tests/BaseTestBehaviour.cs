using UnityEngine;
using UnityEngine.TestTools;

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

    	private void Awake() {
            this.Init();
    	}
    	
        private void Start() {
            this.Evaluate();
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
