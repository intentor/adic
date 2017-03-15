using UnityEngine;
using System.Collections;

namespace Adic.Examples.Factory.Behaviours {
    /// <summary>
    /// Represents a cube.
    /// 
    /// It requires that the game object in which this component is added
    /// has also a Renderer component added.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class Cube : MonoBehaviour {
        /// <summary>Cube's color.</summary>
        public Color color {
            get { return this.GetComponent<Renderer>().material.color; }
            set {
                // Create a new material and set its color.
                // This is not an ideal scenario for a production game given it will generate lots of draw calls 
                // because of different materials.
                var material = new Material(Shader.Find("Standard")) {
                    color = value
                };
                this.GetComponent<Renderer>().material = material;
            }
        }
    }
}