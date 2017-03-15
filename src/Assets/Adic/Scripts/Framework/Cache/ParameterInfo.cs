using System;
using Adic.Util;

namespace Adic.Cache {
    /// <summary>
    /// Parameter info.
    /// </summary>
    public class ParameterInfo {
        /// <summary>Setter type.</summary>
        public Type type;
        /// <summary>Parameter name.</summary>
        public string name;
        /// <summary>Resolution identifier.</summary>
        public object identifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Cache.ParameterInfo"/> class.
        /// </summary>
        /// <param name="type">Setter type.</param>
        /// <param name="name">Parameter name.</param>
        /// <param name="identifier">Resolution identifier.</param>
        public ParameterInfo(Type type, string name, object identifier) {
            this.type = type;
            this.name = name;
            this.identifier = identifier;
        }
    }
}