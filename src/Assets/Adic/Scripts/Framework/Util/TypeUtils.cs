using System;

namespace Adic.Util {
	/// <summary>
	/// Uutility class for types.
	/// </summary>
	public static class TypeUtils {
		/// <summary>
		/// Determines whether <paramref name="potentialDescendant"/> is the same
		/// or a subclass of <paramref name="potentialBase"/>.
		/// </summary>
		/// <param name="potentialBase">Potential base type.</param>
		/// <param name="potentialDescendant">Potential descendant type.</param>
		/// <returns>Boolean.</returns>
		public static bool IsAssignable(Type potentialBase, Type potentialDescendant) {
			return potentialBase.IsAssignableFrom(potentialDescendant);
		}
	}
}