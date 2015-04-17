using System;
using System.Collections.Generic;
using System.Reflection;
using Adic.Util;

namespace Adic.Cache {
	/// <summary>
	/// Basic reflected class.
	/// </summary>
	public class SetterInfo {
		/// <summary>Setter type.</summary>
		public Type type;
		/// <summary>Resolution identifier.</summary>
		public string identifier;
		/// <summary>Setter method.</summary>
		public Setter setter;

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.Cache.SetterInfo"/> class.
		/// </summary>
		/// <param name="type">Setter type.</param>
		/// <param name="identifier">Resolution identifier.</param>
		/// <param name="setter">Setter method.</param>
		public SetterInfo(Type type, string identifier, Setter setter) {
			this.type = type;
			this.identifier = identifier;
			this.setter = setter;
		}
	}
}