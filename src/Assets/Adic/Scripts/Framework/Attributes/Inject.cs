using System;

namespace Adic {
    /// <summary>
    /// Marks a setter injection point.
    /// 
    /// If an identifier is provided, the injector looks the binder for a key with the given name.
    /// 
    /// If no identifier is provided, the injector looks the binder for a key of the type of the field/property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
#if UNITY_2017_1_OR_NEWER
    public class Inject : UnityEngine.Scripting.PreserveAttribute    
#else
 public class Inject : Attribute  
#endif
	{
        /// <summary>The identifier of the binding to inject.</summary>
        public object identifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="Inject"/> class.
        /// </summary>
        public Inject() {
            this.identifier = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Inject"/> class.
        /// </summary>
        /// <param name="identifier">The identifier of the binding to inject.</param>
        public Inject(object identifier) {
            this.identifier = identifier;
        }
    }
}