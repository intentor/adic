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
		/// <param name="baseType">Base type from which the types in the namespace must be assignable.</param>
		/// <param name="namespaceName">Namespace name.</param>
		/// <param name="includeChildren">Indicates whether child namespaces should be included.</param>
		/// <returns>The assignable types in the namespace.</returns>
		public static Type[] GetAssignableTypesInNamespace(Type baseType, string namespaceName, bool includeChildren) {
			var typesToBind = new List<Type>();

			//Fills the assembly list.
			var assemblies = new List<Assembly>();
			var executingAssembly = Assembly.GetExecutingAssembly();
			if (executingAssembly != null) {
				assemblies.Add(executingAssembly);
			}
			var callingAssembly = Assembly.GetCallingAssembly();
			if (callingAssembly != null && callingAssembly != executingAssembly) {
				assemblies.Add(callingAssembly);
			}
			var typeAssembly = Assembly.GetAssembly(baseType);
			if (typeAssembly != null && typeAssembly != executingAssembly && typeAssembly != callingAssembly) {
				assemblies.Add(typeAssembly);
			}

			//Looks for assignable types in all available assemblies.
			for (int assemblyIndex = 0; assemblyIndex < assemblies.Count; assemblyIndex++) {
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