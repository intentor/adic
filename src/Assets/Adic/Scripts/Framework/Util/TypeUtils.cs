using System;
using System.Collections.Generic;
using System.Reflection;

namespace Adic.Util {
	/// <summary>
	/// Utility class for types.
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

		/// <summary>
		/// Gets all types assignable from a given <paramref name="baseType"/> 
		/// in a given <paramref name="namespaceName"/>.
		/// </summary>
		/// <param name="baseType">Base type from which the types in the namespace must be assignable.</param>
		/// <param name="namespaceName">Namespace name.</param>
		/// <returns>The assignable types in the namespace.</returns>
		public static Type[] GetAssignableTypesInNamespace(Type baseType, string namespaceName) {
			return GetAssignableTypesInNamespace(baseType, namespaceName, false);
		}
		
		/// <summary>
		/// Gets all types assignable from a given <paramref name="baseType"/> 
		/// in a given <paramref name="namespaceName"/>.
		/// </summary>
		/// <remarks>
		/// Excludes any types in assemblies from Unity or Mono.
		/// </remarks>
		/// <param name="baseType">Base type from which the types in the namespace must be assignable.</param>
		/// <param name="namespaceName">Namespace name.</param>
		/// <param name="includeChildren">Indicates whether child namespaces should be included.</param>
		/// <returns>The assignable types in the namespace.</returns>
		public static Type[] GetAssignableTypesInNamespace(Type baseType, string namespaceName, bool includeChildren) {
			var typesToBind = new List<Type>();

			//Looks for assignable types in all available assemblies.
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int assemblyIndex = 0; assemblyIndex < assemblies.Length; assemblyIndex++) {
				var assemly = assemblies[assemblyIndex];

				if (assemly.FullName.StartsWith("Unity") ||
				    assemly.FullName.StartsWith("Boo") ||
				    assemly.FullName.StartsWith("Mono") ||
				    assemly.FullName.StartsWith("System") ||
				    assemly.FullName.StartsWith("mscorlib")) {
					continue;
				}

				var allTypes = assemblies[assemblyIndex].GetTypes();
				for (int typeIndex = 0; typeIndex < allTypes.Length; typeIndex++) {
					var type = allTypes[typeIndex];

					var isTypeInNamespace = 
						(includeChildren && !string.IsNullOrEmpty(type.Namespace) && type.Namespace.StartsWith(namespaceName)) ||
						(!includeChildren && type.Namespace == namespaceName);
					
					if (isTypeInNamespace && 
					    type.IsClass &&
					    TypeUtils.IsAssignable(baseType, type)) {
						typesToBind.Add(type);
					}
				}
			}
			
			return typesToBind.ToArray();
		}
	}
}