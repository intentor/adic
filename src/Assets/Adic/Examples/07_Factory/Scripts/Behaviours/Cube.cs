using UnityEngine;
using System.Collections;

namespace Adic.Examples.Factory.Behaviours {
	/// <summary>
	/// Represents a cube.
	/// </summary>
	[RequireComponent(typeof(Renderer))]
	public class Cube : MonoBehaviour {
		/// <summary>Cube's color.</summary>
		public Color color {
			get { return this.GetComponent<Renderer>().material.color; }
			set {
				//Create a new material and sets its color.
				//This is not an ideal scenario for a production game given
				//it will generate a lot of draw calls because of different
				//materials.
				var material = new Material(Shader.Find("Standard")) {
					color = value
				};
				this.GetComponent<Renderer>().material = material;
			}
		}
	}
}