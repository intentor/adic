using UnityEngine;
using System.Collections;

namespace Adic.Examples.BindingGameObjects {
	/// <summary>
	/// Game object rotator script.
	/// </summary>
	public class GameObjectRotator : MonoBehaviour {
		[Inject]
		public Transform objectToRotate;

		protected void Start() {
			//Calls "Inject" to inject any dependencies to the component.
			//In a production game, it's useful to place this in a base component.
			this.Inject();
		}

		protected void Update () {
			this.objectToRotate.Rotate(1.0f, 1.0f, 1.0f);
		}
	}
}