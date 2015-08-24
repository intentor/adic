using System;
using Adic.Util;

namespace Adic.Cache {
	/// <summary>
	/// Post constructor info.
	/// </summary>
	public class PostConstructorInfo {
		/// <summary>The parameterless post constructor.</summary>
		public PostConstructor postConstructor;
		/// <summary>The post constructor with parameters.</summary>
		public ParamsPostConstructor paramsPostConstructor;
		/// <summary>Post constructor parameters' infos.</summary>
		public ParameterInfo[] parameters;

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.Cache.PostConstructorInfo"/> class.
		/// </summary>
		/// <param name="parameters">Post constructor parameters' infos.</param>
		public PostConstructorInfo(ParameterInfo[] parameters) {
			this.parameters = parameters;
		}
	}
}