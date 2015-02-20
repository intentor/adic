using System;

namespace Intentor.Adic {
	/// <summary>
	/// Class members in which injection can occur.
	/// </summary>
	public enum InjectionMember {
		None,
		Constructor,
		Field,
		Property
	}
}