using UnityEngine;
using System.Collections;

namespace Adic.Examples.Prefabs {
    /// <summary>
    /// Cube rotator script.
    /// </summary>
    public class CubeRotator : MonoBehaviour {
        /// <summary>Cube to rotate. It will only rotate the cube with identifier "cube".</summary>
        [Inject("cube")]
        public Transform cube;

        /// <summary>Messages to show on screen.</summary>
        private string messages;

        /// <summary>
        /// Called after all injections.
        /// </summary>
        [Inject]
        protected void MethodInjection() {
            // Setup some messages.
            this.messages = string.Concat(this.messages, "MethodInjection called.", System.Environment.NewLine);
            var cubeInjected = (this.cube == null ? "No..." : "Yes!");
            this.messages = string.Concat(this.messages, "Cube injected? " + cubeInjected, System.Environment.NewLine);
        }

        /// <summary>
        /// Start is called after PostConstruct.
        /// </summary>
        protected void Start() {
            // Call "Inject" to inject any dependencies in the component.
            // In a production game, it's useful to place this in a base component.
            this.Inject();
        }

        protected void Update() {
            if (this.cube != null) {
                this.cube.Rotate(1.0f, 1.0f, 1.0f);
            }
        }

        protected void OnGUI() {
            GUI.Label(new Rect(10, 10, 300, 100), this.messages);
        }
    }
}