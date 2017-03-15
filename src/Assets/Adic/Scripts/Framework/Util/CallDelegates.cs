using System;

namespace Adic.Util {
    /// <summary>
    /// Delegate for a constructor call without parameters.
    /// </summary>
	public delegate object ConstructorCall();

    /// <summary>
    /// Delegate for a constructor call with parameters.
    /// </summary>
    /// <typeparam name="T">Constructor's object type.</typeparam>
    /// <param name="parameters">Constructor parameters.</param>
	public delegate object ParamsConstructorCall(object[] parameters);

    /// <summary>
    /// Delegate for a method call without parameters.
    /// </summary>
    /// <param name="instance">Instance to call the post constructor.</param>
	public delegate void MethodCall(object instance);
	
    /// <summary>
    /// Delegate for a method call with parameters.
    /// </summary>
    /// <param name="instance">Instance to call the post constructor.</param>
    /// <param name="parameters">Post constructor parameters.</param>
    public delegate void ParamsMethodCall(object instance, object[] parameters);

    /// <summary>
    /// Calls a getter method for a field or property.
    /// </summary>
    /// <param name="instance">Instance to have the field/property gotten.</param>
    /// <returns>Getter value.</returns>
    public delegate object GetterCall(object instance);
	
    /// <summary>
    /// Calls a setter method for a field or property.
    /// </summary>
    /// <param name="instance">Instance to have the field/property settled.</param>
    /// <param name="value">Value to set.</param>
	public delegate void SetterCall(object instance, object value);
}