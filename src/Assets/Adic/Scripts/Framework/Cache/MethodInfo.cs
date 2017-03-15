using System;
using Adic.Util;

namespace Adic.Cache {
    /// <summary>
    /// Method info.
    /// </summary>
    public class MethodInfo {
        /// <summary>The parameterless method.</summary>
        public MethodCall method;
        /// <summary>Method name.</summary>
        public string name;
        /// <summary>The method with parameters.</summary>
        public ParamsMethodCall paramsMethod;
        /// <summary>Method parameters' infos.</summary>
        public ParameterInfo[] parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.Cache.MethodInfo"/> class.
        /// </summary>
        /// <param name="name">Method name.</param>
        /// <param name="parameters">Method parameters' infos.</param>
        public MethodInfo(String name, ParameterInfo[] parameters) {
            this.name = name;
            this.parameters = parameters;
        }
    }
}