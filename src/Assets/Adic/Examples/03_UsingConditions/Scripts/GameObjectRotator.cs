using UnityEngine;
using System.Collections;

namespace Adic.Examples.UsingConditions {
    /// <summary>
    /// Game object rotator script.
    /// </summary>
    public class GameObjectRotator : MonoBehaviour {
        /// <summary>Object to rotate. It will only rotate the object with identifier "RightCube".</summary>
        [Inject("RightCube")]
        public Transform objectToRotate;

        protected void Start() {
            // Call "Inject" to inject any dependencies to the component.
            // On a production game, it's useful to place this in a base component.
            this.Inject();
        }

        protected void Update() {
            if (this.objectToRotate != null) {
                this.objectToRotate.Rotate(1.0f, 1.0f, 1.0f);
            }
        }
    }
}