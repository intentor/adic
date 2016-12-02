using System;
using Adic.Util;

namespace Adic.Cache {
	/// <summary>
	/// Setter info.
	/// </summary>
	public class SetterInfo : ParameterInfo {
		/// <summary>Setter method.</summary>
		public SetterCall setter;

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.Cache.SetterInfo"/> class.
		/// </summary>
        /// <param name="type">Setter type.</param>
        /// <param name="name">Parameter name.</param>
		/// <param name="identifier">Resolution identifier.</param>
		/// <param name="setter">Setter method.</param>
        public SetterInfo(Type type, string name, object identifier, SetterCall setter) : base(type, name, identifier) {
			this.setter = setter;
		}
	}
}