using UnityEngine;
using System.Collections;

namespace Adic.Examples.BindingsSetup.Data {
    /// <summary>
    /// Cube rotation speed value object.
    /// </summary>
    public class CubeRotationSpeed {
        /// <summary>The rotation speed.</summary>
        public float speed { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Examples.BindingsSetup.Data.CubeRotationSpeed"/> class.
        /// </summary>
        /// <param name="speed">The rotation speed.</param>
        public CubeRotationSpeed(float speed) {
            this.speed = speed;
        }
    }
}