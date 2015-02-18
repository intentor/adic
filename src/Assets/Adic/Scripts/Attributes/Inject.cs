using System;

/// <summary>
/// Marks a setter injection point.
/// 
/// If a name is provided, the injector looks the binder
/// for a key with the given name.
/// 
/// If no name is provided, the injector looks the binder
/// for a key of the type of the field/property.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
                AllowMultiple = false,
                Inherited = true)]
public class Inject : Attribute {
	/// <summary>The name of the key of the binding to inject.</summary>
	public string name;

	/// <summary>
	/// Initializes a new instance of the <see cref="IoC.InjectAttribute"/> class.
	/// </summary>
	public Inject() {
		this.name = string.Empty;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="IoC.InjectAttribute"/> class.
	/// </summary>
	/// <param name="name">The name of the object from which take the injection.</param>
	public Inject(string name) {
		this.name = name;
	}
}