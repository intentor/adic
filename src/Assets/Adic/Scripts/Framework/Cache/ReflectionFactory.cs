using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Adic.Util;

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

			var constructor = this.ResolveConstructor(type);
			if (constructor != null) {
				if (constructor.GetParameters().Length == 0) {
					reflectedClass.constructor = MethodUtils.CreateConstructor(type, constructor);
				} else {
					reflectedClass.paramsConstructor = MethodUtils.CreateConstructorWithParams(type, constructor);;
				}
			}

			reflectedClass.constructorParameters = this.ResolveConstructorParameters(constructor);
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
			var constructors = type.GetConstructors(
				BindingFlags.FlattenHierarchy | 
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
		protected ParameterInfo[] ResolveConstructorParameters(ConstructorInfo constructor) {
			if (constructor == null) return null;

			var parameters = constructor.GetParameters();			
			
			var constructorParameters = new ParameterInfo[parameters.Length];
			for (int paramIndex = 0; paramIndex < constructorParameters.Length; paramIndex++) {
				object identifier = null;
				var parameter = parameters[paramIndex];

				var attributes = parameter.GetCustomAttributes(typeof(Inject), true);
				if (attributes.Length > 0) {
					identifier = (attributes[0] as Inject).identifier;
				}

				constructorParameters[paramIndex] = new ParameterInfo(parameter.ParameterType, identifier);
			}

			return constructorParameters;
		}

		/// <summary>
		/// Resolves the post constructors for the type.
		/// </summary>
		/// <returns>The post constructors.</returns>
		protected PostConstructor[] ResolvePostConstructors(Type type) {
			var postConstructors = new List<PostConstructor>();

			var methods = type.GetMethods(BindingFlags.FlattenHierarchy |
				BindingFlags.Public |
				BindingFlags.NonPublic |
	            BindingFlags.Instance);

			for (int methodIndex = 0; methodIndex < methods.Length; methodIndex++) {
				var method = methods[methodIndex];

				var attributes = method.GetCustomAttributes(typeof(PostConstruct), true);

				if (attributes.Length > 0) {
					postConstructors.Add((PostConstructor)MethodUtils.CreateMethod(type, method));
				}
			}

			return postConstructors.ToArray();
		}

		/// <summary>
		/// Resolves the properties that can be injected.
		/// </summary>
		/// <param name="type">Type from which reflection will be resolved.</param>
		/// <returns>The properties.</returns>
		protected SetterInfo[] ResolveProperties(Type type) {
			var setters = new List<SetterInfo>();

			var properties = type.GetProperties(BindingFlags.Instance | 
			    BindingFlags.Static |
			    BindingFlags.NonPublic |
			    BindingFlags.Public);

			for (int propertyIndex = 0; propertyIndex < properties.Length; propertyIndex++) {
				var property = properties[propertyIndex] as PropertyInfo;
				var attributes = property.GetCustomAttributes(typeof(Inject), true);

				if (attributes.Length > 0) {
					var attribute = attributes[0] as Inject;
					var method = MethodUtils.CreatePropertySetter(type, property);
					var info = new SetterInfo(property.PropertyType, attribute.identifier, method);
					setters.Add(info);
				}
			}

			return setters.ToArray();
		}
		
		/// <summary>
		/// Resolves the fields that can be injected.
		/// </summary>
		/// <param name="type">Type from which reflection will be resolved.</param>
		/// <returns>The fields.</returns>
		protected SetterInfo[] ResolveFields(Type type) {
			var setters = new List<SetterInfo>();
			
			var fields = type.GetFields(BindingFlags.Instance | 
		        BindingFlags.Static |
		        BindingFlags.NonPublic |
		        BindingFlags.Public);
			
			for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++) {
				var field = fields[fieldIndex] as FieldInfo;
				var attributes = field.GetCustomAttributes(typeof(Inject), true);
				
				if (attributes.Length > 0) {
					var attribute = attributes[0] as Inject;
					var method = MethodUtils.CreateFieldSetter(type, field);
					var info = new SetterInfo(field.FieldType, attribute.identifier, method);
					setters.Add(info);
				}
			}
			
			return setters.ToArray();
		}
	}
}