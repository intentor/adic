using System;
using System.Collections.Generic;
using System.Reflection;
using Adic.Util;

namespace Adic.Cache {
    /// <summary>
    /// Basic reflected class.
    /// </summary>
    public class ReflectedClass {
        /// <summary>The type the reflected class represents.</summary>
        public Type type { get; set; }

        /// <summary>The parameterless constructor of the class.</summary>
        public ConstructorCall constructor { get; set; }

        /// <summary>The constructor with parameters of the class.</summary>
        public ParamsConstructorCall paramsConstructor { get; set; }

        /// <summary>Constructor parameters' infos.</summary>
        public ParameterInfo[] constructorParameters { get; set; }

        /// <summary>Methods that have the Inject attribute.</summary>
        public MethodInfo[] methods { get; set; }

        /// <summary>Public properties of the type that can receive injection.</summary>
        public AcessorInfo[] properties { get; set; }

        /// <summary>Public fields of the type that can receive injection.</summary>
        public AcessorInfo[] fields { get; set; }
    }
}