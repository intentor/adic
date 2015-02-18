using System;
using System.Collections.Generic;
using System.Reflection;

namespace Adic {
	/// <summary>
	/// Defines a reflected class.
	/// 
	/// Reflection is a very slow process. The reflected class
	/// contains reflection informations about a given
	/// class, making it easier to cache the results.
	/// </summary>
	public interface IReflectedClass {		
		/// <summary>The constructor of the class.</summary>
		ConstructorInfo constructor { get; set; }
		/// <summary>The parameters of the constructor of the class.</summary>
		Type[] constructorParameters { get; set; }
		/// <summary>Methods that have the PostConstruct attribute.</summary>
		MethodInfo[] postConstructors { get; set; }
		/// <summary>Public properties of the type that can receive injection.</summary>
		KeyValuePair<object, PropertyInfo>[] properties { get; set; }
		/// <summary>Public fields of the type that can receive injection.</summary>
		KeyValuePair<object, FieldInfo>[] fields { get; set; }
	}
}