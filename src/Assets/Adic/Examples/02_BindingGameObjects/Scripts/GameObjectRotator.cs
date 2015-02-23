using UnityEngine;
using System.Collections;

namespace Intentor.Adic.BindingGameObjects {
	/// <summary>
	/// Game object rotator script.
	/// </summary>
	public class GameObjectRotator : MonoBehaviour {
		[Inject]
		public Transform objectToRotate;

		protected void Start() {
			this.Inject();
		}

		protected void Update () {
			this.objectToRotate.Rotate(1.0f, 1.0f, 1.0f);
		}
	}
}