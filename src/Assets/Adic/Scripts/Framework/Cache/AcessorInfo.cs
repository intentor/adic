using System;
using Adic.Util;

namespace Adic.Cache {
    /// <summary>
    /// Acessor (fields and properties) info.
    /// </summary>
    public class AcessorInfo : ParameterInfo {
        /// <summary>Getter method.</summary>
        public GetterCall getter;
        /// <summary>Setter method.</summary>
        public SetterCall setter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Cache.SetterInfo"/> class.
        /// </summary>
        /// <param name="type">Setter type.</param>
        /// <param name="name">Parameter name.</param>
        /// <param name="identifier">Resolution identifier.</param>
        /// <param name="getter">Getter method.</param>
        /// <param name="setter">Setter method.</param>
        public AcessorInfo(Type type, string name, object identifier, GetterCall getter, SetterCall setter)
            : base(type, name, identifier) {
            this.getter = getter;
            this.setter = setter;
        }
    }
}