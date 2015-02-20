using UnityEngine;
using System.Collections;

namespace Adic.Example {
	/// <summary>
	/// Cube rotator script.
	/// </summary>
	public class CubeRotator : MonoBehaviour {
		[Inject]
		public Dependency dependency;
		[Inject("cube")]
		public Transform cube;

		/// <summary>Messages to show on screen.</summary>
		private string messages;
		
		[PostConstruct]
		protected void PostConstruct() {
			this.messages = string.Concat(this.messages, "PostConstruct called.", System.Environment.NewLine);
			var cubeInjected = (this.cube == null ? "No..." : "Yes!");
			this.messages = string.Concat(this.messages, "Cube injected? " + cubeInjected, System.Environment.NewLine);
		}

		protected void Start() {
			this.Inject();
			this.messages = string.Concat(this.messages, this.dependency.text, System.Environment.NewLine);
		}

		protected void Update () {
			this.cube.Rotate(1.0f, 1.0f, 1.0f);
		}

		protected void OnGUI() {
			GUI.Label(new Rect(10, 10, 300, 100), this.messages);
		}
	}
}