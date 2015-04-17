using System;

namespace Adic.Util {
	/// <summary>
	/// Delegate for a constructor call without parameters.
	/// </summary>
	public delegate object Constructor();

	/// <summary>
	/// Delegate for a constructor call with parameters.
	/// </summary>
	/// <typeparam name="T">Constructor's object type.</typeparam>
	/// <param name="parameters">Constructor parameters.</param>
	public delegate object ParamsConstructor(params object[] parameters);

	/// <summary>
	/// Calls a post constructor in an instance.
	/// </summary>
	/// <param name="instance">Instance to call the post constructor.</param>
	public delegate void PostConstructor(object instance);
	
	/// <summary>
	/// Calls a setter method for a field or property.
	/// </summary>
	/// <param name="instance">Instance to have the field/property settled.</param>
	/// <param name="value">Value to set.</param>
	public delegate void Setter(object instance, object value);
}