using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Adic.Cache {
	/// <summary>
	/// Factory for <see cref="IReflectedClass"/>.
	/// </summary>
	public class ReflectionFactory : IReflectionFactory {
		/// <summary>
		/// Creates a <see cref="ReflectedClass"/> from a <paramref name="type"/>.
		/// </summary>
		/// <param name="type">Type from which the reflected class will be created.</param>
		public ReflectedClass Create(Type type) {
			var reflectedClass = new ReflectedClass();

			reflectedClass.type = type;
			reflectedClass.constructor = this.ResolveConstructor(type);
			reflectedClass.constructorParameters = this.ResolveConstructorParameters(reflectedClass.constructor);
			reflectedClass.postConstructors = this.ResolvePostConstructors(type);
			reflectedClass.properties = this.ResolveProperties(type);
			reflectedClass.fields = this.ResolveFields(type);

			return reflectedClass;
		}
		
		/// <summary>
		/// Selects the constructor marked with <see cref="ConstructAttribute"/>
		/// or with the minimum amount of parameters.
		/// </summary>
		/// <param name="type">Type from which reflection will be resolved.</param>
		/// <returns>The constructor.</returns>
		protected ConstructorInfo ResolveConstructor(Type type) {
			var constructors = type.GetConstructors(BindingFlags.FlattenHierarchy | 
													BindingFlags.Public |
													BindingFlags.Instance |
													BindingFlags.InvokeMethod);

			if (constructors.Length == 0) {
				return null;
			}

			if (constructors.Length == 1) {
				return constructors[0];
			}

			ConstructorInfo shortestConstructor = null;
			for (int i = 0, length = 0, shortestLength = int.MaxValue; i < constructors.Length; i++) {
				var constructor = constructors[i];

				object[] attributes = constructor.GetCustomAttributes(typeof(Construct), true);

				if (attributes.Length > 0) {
					return constructor;
				}

				length = constructor.GetParameters().Length;
				if (length < shortestLength) {
					shortestLength = length;
					shortestConstructor = constructor;
				}
			}

			return shortestConstructor;
		}

		/// <summary>
		/// Resolves the constructor parameters.
		/// </summary>
		/// <param name="constructor">The constructor to have the parameters resolved.</param>
		/// <returns>The constructor parameters.</returns>
		protected Type[] ResolveConstructorParameters(ConstructorInfo constructor) {
			if (constructor == null) return null;

			var parameters = constructor.GetParameters();			
			
			var constructorParameters = new Type[parameters.Length];
			for (int paramIndex = 0; paramIndex < constructorParameters.Length; paramIndex++) {
				constructorParameters[paramIndex] = parameters[paramIndex].ParameterType;
			}

			return constructorParameters;
		}

		/// <summary>
		/// Resolves the post constructors for the type.
		/// </summary>
		/// <returns>The post constructors.</returns>
		protected MethodInfo[] ResolvePostConstructors(Type type) {
			var postConstructors = new List<MethodInfo>();

			var methods = type.GetMethods(BindingFlags.FlattenHierarchy |
				BindingFlags.Public |
				BindingFlags.NonPublic |
	            BindingFlags.Instance);

			for (int methodIndex = 0; methodIndex < methods.Length; methodIndex++) {
				var method = methods[methodIndex];

				var attributes = method.GetCustomAttributes(typeof(PostConstruct), true);

				if (attributes.Length > 0) {
					postConstructors.Add(method);
				}
			}

			return postConstructors.ToArray();
		}

		/// <summary>
		/// Resolves the properties that can be injected.
		/// </summary>
		/// <param name="type">Type from which reflection will be resolved.</param>
		/// <returns>The properties.</returns>
		protected KeyValuePair<object, PropertyInfo>[] ResolveProperties(Type type) {
			var pairs = new List<KeyValuePair<object, PropertyInfo>>();

			var properties = type.GetProperties(BindingFlags.Instance | 
			    BindingFlags.Static |
			    BindingFlags.NonPublic |
			    BindingFlags.Public);

			for (int propertyIndex = 0; propertyIndex < properties.Length; propertyIndex++) {
				var property = properties[propertyIndex] as PropertyInfo;
				var attributes = property.GetCustomAttributes(typeof(Inject), true);

				if (attributes.Length > 0) {
					//Check if it needs to inject a named instance.
					var attribute = attributes[0] as Inject;

					//Checks if the injection needs to happen by identifier or type.
					object key = (attribute.identifier == null ? 
              			property.PropertyType as object : attribute.identifier);

					pairs.Add(new KeyValuePair<object, PropertyInfo>(key, property));
				}
			}

			return pairs.ToArray();
		}
		
		/// <summary>
		/// Resolves the fields that can be injected.
		/// </summary>
		/// <param name="type">Type from which reflection will be resolved.</param>
		/// <returns>The fields.</returns>
		protected KeyValuePair<object, FieldInfo>[] ResolveFields(Type type) {
			var pairs = new List<KeyValuePair<object, FieldInfo>>();
			
			var fields = type.GetFields(BindingFlags.Instance | 
		        BindingFlags.Static |
		        BindingFlags.NonPublic |
		        BindingFlags.Public);
			
			for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++) {
				var field = fields[fieldIndex] as FieldInfo;
				var attributes = field.GetCustomAttributes(typeof(Inject), true);
				
				if (attributes.Length > 0) {
					//Check if it needs to inject a named instance.
					var attribute = attributes[0] as Inject;
					
					//Checks if the injection needs to happen by name or type.
					object key = (attribute.identifier == null ? 
						field.FieldType as object : attribute.identifier);
					
					pairs.Add(new KeyValuePair<object, FieldInfo>(key, field));
				}
			}
			
			return pairs.ToArray();
		}
	}
}