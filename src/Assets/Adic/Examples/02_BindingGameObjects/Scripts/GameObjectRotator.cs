using UnityEngine;
using System.Collections;

namespace Adic.Examples.BindingGameObjects {
	/// <summary>
	/// Game object rotator.
	/// </summary>
	public class GameObjectRotator : MonoBehaviour {
		/// <summary>Object to rotate, injected by the container.</summary>
		[Inject]
		public Transform objectToRotate;

		protected void Start() {
			// Call "Inject" to inject any dependencies in the component.
			// In a production game, it's useful to place this in a base component.
			this.Inject();
		}

		protected void Update() {
            if (this.objectToRotate != null) {
                this.objectToRotate.Rotate(1.0f, 1.0f, 1.0f);
            }
		}
	}
}