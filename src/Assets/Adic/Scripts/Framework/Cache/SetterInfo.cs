using System;
using System.Collections.Generic;
using System.Reflection;
using Adic.Util;

namespace Adic.Cache {
	/// <summary>
	/// Setter info.
	/// </summary>
	public class SetterInfo : ParameterInfo {
		/// <summary>Setter method.</summary>
		public Setter setter;

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.Cache.SetterInfo"/> class.
		/// </summary>
		/// <param name="type">Setter type.</param>
		/// <param name="identifier">Resolution identifier.</param>
		/// <param name="setter">Setter method.</param>
		public SetterInfo(Type type, object identifier, Setter setter) : base(type, identifier) {
			this.setter = setter;
		}
	}
}