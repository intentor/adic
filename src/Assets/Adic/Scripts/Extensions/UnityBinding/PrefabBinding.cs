using UnityEngine;

namespace Adic {
    /// <summary>
    /// Represents a prefab binding.
    /// </summary>
    public class PrefabBinding {
        // <summary>The prefab to be instantiated.</summary>
        public Object prefab { get; private set; }

        /// <summary>The type that will be resolved from the prefab.</summary>
        public System.Type type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.PrefabBinding"/> class.
        /// </summary>
        /// <param name="prefab">The prefab to be instantiated.</param>
        /// <param name="type">The type that will be resolved from the prefab.</param>
        public PrefabBinding(Object prefab, System.Type type) {
            this.prefab = prefab;
            this.type = type;
        }
    }
}