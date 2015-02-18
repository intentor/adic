using UnityEngine;
using System.Collections;

namespace Adic.Example {
	/// <summary>
	/// Cube rotator script.
	/// </summary>
	public class CubeRotator : MonoBehaviour {
		[Inject]
		public Dependency seeOnConsole;
		[Inject("cube")]
		public Transform cube;
		
		[PostConstruct]
		protected void PostConstruct() {
			Debug.Log("PostConstruct called.");
		}

		protected void Start() {
			this.Inject();
			Debug.Log(this.seeOnConsole.text);
		}

		protected void Update () {
			this.cube.Rotate(1.0f, 1.0f, 1.0f);
		}
	}
}