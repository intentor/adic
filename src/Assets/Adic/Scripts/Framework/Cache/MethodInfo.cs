using System;
using Adic.Util;

namespace Adic.Cache {
	/// <summary>
	/// Method info.
	/// </summary>
	public class MethodInfo {
		/// <summary>The parameterless method.</summary>
		public MethodCall method;
		/// <summary>The method with parameters.</summary>
		public ParamsMethodCall paramsMethod;
		/// <summary>Method parameters' infos.</summary>
		public ParameterInfo[] parameters;

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.Cache.MethodInfo"/> class.
		/// </summary>
		/// <param name="parameters">Method parameters' infos.</param>
		public MethodInfo(ParameterInfo[] parameters) {
			this.parameters = parameters;
		}
	}
}