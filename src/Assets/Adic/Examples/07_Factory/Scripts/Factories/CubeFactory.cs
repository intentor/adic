using UnityEngine;
using Adic.Container;
using Adic.Injection;
using Adic.Examples.Factory.Behaviours;

namespace Adic.Examples.Factory.Bindings {
    /// <summary>
    /// Creates and positions cubes as a matrix.
    /// 
    /// The factory creates only one cube per call. The idea is that every time the
    /// factory is called to resolve an object, it will create a new cube and place
    /// it in the last position of the matrix.
    /// 
    /// This factory creates <see cref="UnityEngine.GameObject"/> objects.
    /// </summary>
    public class CubeFactory : Adic.IFactory {
        /// <summary>Maximum number of columns to position the cubes.</summary>
        protected const int MAX_COLUMNS = 6;

        /// <summary>Container in which the factory is bounded.</summary>
        protected IInjectionContainer container;
        /// <summary>Current line to position the cube.</summary>
        protected int currentLine;
        /// <summary>Current column to position the cube.</summary>
        protected int currentColumn;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Examples.Factories.Bindings.CubeFactory"/> class.
        /// </summary>
        /// <param name="container">Container in which the factory is bounded.</param>
        public CubeFactory(IInjectionContainer container) {
            this.container = container;
			
            // To make instantiation easier, binds the "Cube" behaviour to the "Cube" prefab.
            // In this example, "Cube" is not in the prefab and will be added during resolution.
            this.container.Bind<Cube>().ToPrefab("07_Factory/Cube");
        }

        /// <summary>
        /// Creates an instance of the object of the type created by the factory.
        /// </summary>
        /// <param name="context">Injection context.</param>
        /// <returns>The instance.</returns>
        public object Create(InjectionContext context) {
            // Resolve a cube.
            var cube = this.container.Resolve<Cube>();

            // Add the "Rotator" behaviour to the cube and sets its speed.
            // This script could already be in the prefab. It's added here only to show that factories can be used 
            // to fully configure any object they create.
            var rotator = cube.gameObject.AddComponent<Rotator>();
            rotator.speed = Random.Range(0.05f, 5.0f);

            // Set the cube's color.
            cube.color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));

            // Set its position in the matrix.
            var transform = cube.GetComponent<Transform>();
            transform.position = new Vector3(1.5f * this.currentColumn++, -1.5f * this.currentLine, 0);

            // Check for line break.
            if (this.currentColumn >= MAX_COLUMNS) {
                this.currentLine++;
                this.currentColumn = 0;
            }

            return cube.gameObject;
        }
    }
}