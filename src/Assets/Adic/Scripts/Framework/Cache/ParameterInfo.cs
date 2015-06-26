using System;
using System.Collections.Generic;
using System.Reflection;
using Adic.Util;

namespace Adic.Cache {
	/// <summary>
	/// Parameter info.
	/// </summary>
	public class ParameterInfo {
		/// <summary>Setter type.</summary>
		public Type type;
		/// <summary>Resolution identifier.</summary>
		public object identifier;

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.Cache.ParameterInfo"/> class.
		/// </summary>
		/// <param name="type">Setter type.</param>
		/// <param name="identifier">Resolution identifier.</param>
		public ParameterInfo(Type type, object identifier) {
			this.type = type;
			this.identifier = identifier;
		}
	}
}