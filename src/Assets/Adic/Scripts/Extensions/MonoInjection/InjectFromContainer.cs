using System;

namespace Adic {
    /// <summary>
    /// Marks a <see cref="UnityEngine.MonoBehaviour"/> to be injected only from a container
    /// with a given identifier.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,
        AllowMultiple = true,
        Inherited = true)]
    public class InjectFromContainer : Attribute {
        /// <summary>The identifier of the container from which injections will occur.</summary>
        public object identifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="InjectFromContainer"/> class.
        /// </summary>
        /// <param name="identifier">The identifier of the container from which injections will occur.</param>
        public InjectFromContainer(object identifier) {
            this.identifier = identifier;
        }
    }
}