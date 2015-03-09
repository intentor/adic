using System;

namespace Adic {
	/// <summary>
	/// Marks a preferable constructor.
	/// </summary>
	/// <remarks>
	/// If no constructor is marked as preferable, the shortest available will be used.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Constructor,
		AllowMultiple = false,
		Inherited = true)]
	public class Construct : Attribute {  }
}