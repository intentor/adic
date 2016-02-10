using System;
using System.Collections.Generic;
using System.Reflection;

namespace Adic.Util {
	/// <summary>
	/// Utility class for types.
	/// </summary>
	public static class TypeUtils {

		/// <summary>
		/// Determines whether a type is a class.
		/// </summary>
		/// <param name="type">Type to check.</param>
		/// <returns><c>true</c> if the specified type is a class; otherwise, <c>false</c>.</returns>
		public static bool IsClass(Type type) {
			#if !UNITY_EDITOR && (UNITY_WSA || UNITY_WP8 || UNITY_WP8_1)

			return type.GetTypeInfo().IsClass;

			#else

			return type.IsClass;

			#endif
		}

		/// <summary>
		/// Determines whether a type is an interface.
		/// </summary>
		/// <param name="type">Type to check.</param>
		/// <returns><c>true</c> if the specified type is an interface; otherwise, <c>false</c>.</returns>
		public static bool IsInterface(Type type) {
			#if !UNITY_EDITOR && (UNITY_WSA || UNITY_WP8 || UNITY_WP8_1)

			return type.GetTypeInfo().IsInterface;

			#else

			return type.IsInterface;

			#endif
		}

		/// <summary>
		/// Determines whether a type is an abstract class.
		/// </summary>
		/// <param name="type">Type to check.</param>
		/// <returns><c>true</c> if the specified type is an abstract; otherwise, <c>false</c>.</returns>
		public static bool IsAbsract(Type type) {
			#if !UNITY_EDITOR && (UNITY_WSA || UNITY_WP8 || UNITY_WP8_1)

			return type.GetTypeInfo().IsAbstract;

			#else

			return type.IsAbstract;

			#endif
		}

        /// <summary>
        /// Determines whether <paramref name="potentialDescendant"/> is the same
        /// or a subclass of <paramref name="potentialBase"/>.
        /// </summary>
        /// <param name="potentialBase">Potential base type.</param>
        /// <param name="potentialDescendant">Potential descendant type.</param>
        /// <returns>Boolean.</returns>
		public static bool IsAssignable(Type potentialBase, Type potentialDescendant) {
			#if !UNITY_EDITOR && (UNITY_WSA || UNITY_WP8 || UNITY_WP8_1)

			return potentialBase.GetTypeInfo().IsAssignableFrom(potentialDescendant.GetTypeInfo());

			#else

			return potentialBase.IsAssignableFrom(potentialDescendant);

			#endif
		}

		/// <summary>
		/// Gets all types assignable from a given <paramref name="baseType"/>.
		/// </summary>
		/// <remarks>
		/// Excludes any types in assemblies from Unity or Mono.
		/// </remarks>
		/// <param name="baseType">Base type from which the types in the namespace must be assignable.</param>
		/// <returns>The assignable types in the namespace.</returns>
		public static Type[] GetAssignableTypes(Type baseType) {
			return GetAssignableTypes(baseType, string.Empty, false);
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
		/// <returns>The assignable types in the namespace.</returns>
		public static Type[] GetAssignableTypes(Type baseType, string namespaceName) {
			return GetAssignableTypes(baseType, namespaceName, false);
		}

		/// <summary>
		/// Gets custom attributes from a type.
		/// </summary>
		/// <param name="source">Source object.</param>
		/// <param name="inherit">If set to <c>true</c>, search this member's inheritance chain to find the attributes.</param>
		/// <returns>The custom attributes.</returns>
		public static object[] GetCustomAttributes(object source, bool inherit) {
			#if !UNITY_EDITOR && (UNITY_WSA || UNITY_WP8 || UNITY_WP8_1)

			return source.GetTypeInfo().GetCustomAttributes(inherit);

			#else

			return source.GetType().GetCustomAttributes(inherit);

			#endif
		}

		/// <summary>
		/// Gets custom attributes from a type.
		/// </summary>
		/// <param name="source">Source object.</param>
		/// <param name="attributeType">Attribute type.</param>
		/// <param name="inherit">If set to <c>true</c>, search this member's inheritance chain to find the attributes.</param>
		/// <returns>The custom attributes.</returns>
		public static object[] GetCustomAttributes(object source, Type attributeType, bool inherit) {
			#if !UNITY_EDITOR && (UNITY_WSA || UNITY_WP8 || UNITY_WP8_1)

			return source.GetTypeInfo().GetCustomAttributes(attributeType, inherit);

			#else

			return source.GetType().GetCustomAttributes(attributeType, inherit);

			#endif
		}

		/// <summary>
		/// Gets constructors for a given type.
		/// </summary>
		/// <param name="type">Type to get constructors from.</param>
		/// <returns>The constructors.</returns>
		public static ConstructorInfo[] GetConstructors(Type type) {
			#if !UNITY_EDITOR && (UNITY_WSA || UNITY_WP8 || UNITY_WP8_1)

			return type.GetTypeInfo().GetConstructors(
				BindingFlags.FlattenHierarchy | 
				BindingFlags.Public |
				BindingFlags.Instance |
				BindingFlags.InvokeMethod);
			#else

			return type.GetConstructors(
				BindingFlags.FlattenHierarchy | 
				BindingFlags.Public |
				BindingFlags.Instance |
				BindingFlags.InvokeMethod);
			
			#endif
		}

		/// <summary>
		/// Gets methods for a given type.
		/// </summary>
		/// <param name="type">Type to get methods from.</param>
		/// <returns>The methods.</returns>
		public static MethodInfo[] GetMethods(Type type) {
            #if !UNITY_EDITOR && (UNITY_WSA || UNITY_WP8 || UNITY_WP8_1)

			return type.GetTypeInfo().GetMethods(
				BindingFlags.FlattenHierarchy |
				BindingFlags.Public |
				BindingFlags.NonPublic |
				BindingFlags.Instance);

            #else

            return type.GetMethods(
                BindingFlags.FlattenHierarchy |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance);
			
			#endif
		}

		/// <summary>
		/// Gets properties for a given type.
		/// </summary>
		/// <param name="type">Type to get properties from.</param>
		/// <returns>The properties.</returns>
		public static PropertyInfo[] GetProperties(Type type) {
			#if !UNITY_EDITOR && (UNITY_WSA || UNITY_WP8 || UNITY_WP8_1)

			return type.GetTypeInfo().GetProperties(
				BindingFlags.Instance | 
				BindingFlags.Static |
				BindingFlags.NonPublic |
				BindingFlags.Public);

			#else

			return type.GetProperties(
				BindingFlags.Instance | 
				BindingFlags.Static |
				BindingFlags.NonPublic |
				BindingFlags.Public);
			
			#endif
		}

		/// <summary>
		/// Gets fields for a given type.
		/// </summary>
		/// <param name="type">Type to get fields from.</param>
		/// <returns>The fields.</returns>
		public static FieldInfo[] GetFields(Type type) {
			#if !UNITY_EDITOR && (UNITY_WSA || UNITY_WP8 || UNITY_WP8_1)

			return type.GetTypeInfo().GetFields(
				BindingFlags.Instance | 
				BindingFlags.Static |
				BindingFlags.NonPublic |
				BindingFlags.Public);

			#else

			return type.GetFields(
				BindingFlags.Instance | 
				BindingFlags.Static |
				BindingFlags.NonPublic |
				BindingFlags.Public);
			
			#endif
		}

		/// <summary>
		/// Gets all available assemblies.
		/// </summary>
		/// <returns>The assemblies.</returns>
		public static Assembly[] GetAssemblies() {
			#if !UNITY_EDITOR && (UNITY_WSA || UNITY_WP8 || UNITY_WP8_1)

			var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

			var assemblies = new List<Assembly>();
			foreach (var file in await folder.GetFilesAsync()) {
				if (file.FileType == ".dll" || file.FileType == ".exe") {
					var name = new AssemblyName() { Name = file.Name };
					var asm = Assembly.Load(name);
					assemblies.Add(asm);
				}
			}

			return assemblies.ToArray();

			#else

			return AppDomain.CurrentDomain.GetAssemblies();

			#endif
		}

		/// <summary>
		/// Gets the types from an assembly.
		/// </summary>
		/// <param name="assembly">Assembly from which types will be read.</param>
		/// <returns>The types from assembly.</returns>
		public static Type[] GetTypesFromAssembly(Assembly assembly) {
			#if !UNITY_EDITOR && (UNITY_WSA || UNITY_WP8 || UNITY_WP8_1)

			return assembly.ExportedTypes;

			#else

			return assembly.GetTypes();

			#endif
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
		public static Type[] GetAssignableTypes(Type baseType, string namespaceName, bool includeChildren) {
			var typesToBind = new List<Type>();

			//Looks for assignable types in all available assemblies.
			var assemblies = GetAssemblies();
			for (int assemblyIndex = 0; assemblyIndex < assemblies.Length; assemblyIndex++) {
				var assembly = assemblies[assemblyIndex];

				if (assembly.FullName.StartsWith("Unity") ||
				    assembly.FullName.StartsWith("Boo") ||
				    assembly.FullName.StartsWith("Mono") ||
				    assembly.FullName.StartsWith("System") ||
				    assembly.FullName.StartsWith("mscorlib")) {
					continue;
				}

				try {
					var allTypes = GetTypesFromAssembly(assemblies[assemblyIndex]);
					for (int typeIndex = 0; typeIndex < allTypes.Length; typeIndex++) {
						var type = allTypes[typeIndex];

						var isTypeInNamespace = 
							(string.IsNullOrEmpty(namespaceName)) ||
							(includeChildren && !string.IsNullOrEmpty(type.Namespace) && type.Namespace.StartsWith(namespaceName)) ||
							(!includeChildren && type.Namespace == namespaceName);
						
						if (isTypeInNamespace && 
							IsClass(type) &&
						    TypeUtils.IsAssignable(baseType, type)) {
							typesToBind.Add(type);
						}
					}
				} catch (ReflectionTypeLoadException) {
					//If the assembly can't be read, just continue.
					continue;
				}
			}
			
			return typesToBind.ToArray();
		}
		
		/// <summary>
		/// Gets a type from a type name.
		/// </summary>
		/// <remarks>
		/// Excludes any type in assemblies from Unity or Mono.
		/// </remarks>
		/// <param name="typeName">Type name.</param>
		/// <returns>The type or NULL.</returns>
		public static Type GetType(string typeName) {
			return GetType(string.Empty, typeName);
		}

		/// <summary>
		/// Gets a type from a namespace and type names.
		/// </summary>
		/// <remarks>
		/// Excludes any type in assemblies from Unity or Mono.
		/// </remarks>
		/// <param name="namespaceName">Namespace name.</param>
		/// <param name="typeName">Type name.</param>
		/// <returns>The type or NULL.</returns>
		public static Type GetType(string namespaceName, string typeName) {
			string fullName = null;
			if (!string.IsNullOrEmpty(typeName)) {
				if (string.IsNullOrEmpty(namespaceName) || namespaceName == "-") {
					fullName = typeName;
				} else {
					fullName = string.Format("{0}.{1}", namespaceName, typeName);
				}
			}

			if (string.IsNullOrEmpty(fullName)) return null;
			
			//Looks for the type in all available assemblies.
			var assemblies = GetAssemblies();
			for (int assemblyIndex = 0; assemblyIndex < assemblies.Length; assemblyIndex++) {
				var assembly = assemblies[assemblyIndex];
				
				if (assembly.FullName.StartsWith("Unity") ||
				    assembly.FullName.StartsWith("Boo") ||
				    assembly.FullName.StartsWith("Mono") ||
				    assembly.FullName.StartsWith("System") ||
				    assembly.FullName.StartsWith("mscorlib")) {
					continue;
				}
				
				try {
					var allTypes = GetTypesFromAssembly(assemblies[assemblyIndex]);
					for (int typeIndex = 0; typeIndex < allTypes.Length; typeIndex++) {
						var type = allTypes[typeIndex];
						if (type.FullName == fullName) {
							return type;
						}
					}
				} catch (ReflectionTypeLoadException) {
					//If the assembly can't be read, just continue.
					continue;
				}
			}

			return null;
		}
	}
}