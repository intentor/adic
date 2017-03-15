using UnityEngine;
using System.Collections;

namespace Adic.Examples.Factory.Behaviours {
    /// <summary>
    /// Rotates a game object.
    /// </summary>
    public class Rotator : MonoBehaviour {
        /// <summary>Rotation speed.</summary>
        public float speed;

        /// <summary>The cached transform object.</summary>
        protected Transform cachedTransform;

        /// <summary>
        /// Called when the script is started.
        /// </summary>
        protected void Start() {
            this.cachedTransform = this.GetComponent<Transform>();
        }

        /// <summary>
        /// Called when the script is updated.
        /// </summary>
        protected void Update() {
            this.cachedTransform.Rotate(this.speed, this.speed, this.speed);
        }
    }
}