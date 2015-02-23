using UnityEngine;
using System.Collections;

namespace Intentor.Adic.UsingConditions {
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
			this.Inject();
		}

		protected void Update () {
			this.objectToRotate.Rotate(1.0f, 1.0f, 1.0f);
		}
	}
}