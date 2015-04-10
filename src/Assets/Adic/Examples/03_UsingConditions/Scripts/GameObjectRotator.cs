using UnityEngine;
using System.Collections;

namespace Adic.Examples.UsingConditions {
	/// <summary>
	/// Game object rotator script.
	/// </summary>
	public class GameObjectRotator : MonoBehaviour {
		/// <summary>
		/// The object to rotate. It will only rotate the injection of identifier "LeftCube".
		/// </summary>
		[Inject("LeftCube")]
		public Transform objectToRotate;

		protected void Start() {
			//Calls "Inject" to inject any dependencies to the component.
			//On a production game, it's useful to place this in a base component.
			this.Inject();
		}

		protected void Update () {
			this.objectToRotate.Rotate(1.0f, 1.0f, 1.0f);
		}
	}
}