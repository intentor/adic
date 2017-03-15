using System.Collections;

namespace Adic {
    /// <summary>
    /// Defines how instance resolution is done.
    /// </summary>
    public enum ResolutionMode {
        /// <summary>
        /// Always try to resolve every type that requires injection, even ones that are not bound to the container.
        /// 
        /// This is the default resoltion mode.
        /// </summary>
        ALWAYS_RESOLVE,
        /// <summary>
        /// Only resolves types that are bound to the container. Trying to resolve a non-bound type will return a
        /// null reference.
        /// </summary>
        RETURN_NULL
    }
}