using UnityEngine;
using System.Collections;
using Adic.Examples.BindingsSetup.Data;

namespace Adic.Examples.BindingsSetup.Behaviours {
	/// <summary>
	/// Cube rotator.
	/// </summary>
	public class CubeRotator : MonoBehaviour {
		[Inject]
		public CubeRotationSpeed speedData;

		/// <summary>The cached transform object.</summary>
		protected Transform cachedTransform;

		protected void Start() {
			this.cachedTransform = this.GetComponent<Transform>();

			//Calls "Inject" to inject any dependencies to the component.
			//In a production game, it's useful to place this in a base component.
			this.Inject();
		}

		protected void Update() {
			this.cachedTransform.Rotate(this.speedData.speed, this.speedData.speed, this.speedData.speed);
		}
	}
}