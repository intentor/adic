using System;
using System.Collections.Generic;
using System.Reflection;

namespace Intentor.Adic {
	/// <summary>
	/// Basic reflected class.
	/// </summary>
	public class ReflectedClass {
		/// <summary>The type the reflected class represents.</summary>
		public Type type { get; set; }
		/// <summary>The constructor of the class.</summary>
		public ConstructorInfo constructor { get; set; }
		/// <summary>The parameters of the constructor of the class.</summary>
		public Type[] constructorParameters { get; set; }
		/// <summary>Methods that have the PostConstruct attribute.</summary>
		public MethodInfo[] postConstructors { get; set; }
		/// <summary>Public properties of the type that can receive injection.</summary>
		public KeyValuePair<object, PropertyInfo>[] properties { get; set; }
		/// <summary>Public fields of the type that can receive injection.</summary>
		public KeyValuePair<object, FieldInfo>[] fields { get; set; }
	}
}