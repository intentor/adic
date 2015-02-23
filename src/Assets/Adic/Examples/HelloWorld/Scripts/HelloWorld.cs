using UnityEngine;
using System.Collections;

namespace Intentor.Adic.Examples.HelloWorld {
	/// <summary>
	/// MonoBehaviour that displays "Hello World".
	/// </summary>
	public class HelloWorld : MonoBehaviour {
		protected void OnGUI() {
			GUI.Label(new Rect(10, 10, 100, 30), "Hello, world!");
		}
	}
}