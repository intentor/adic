using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Adic.Util {
	/// <summary>
	/// Utility class for dynamic methods creation.
	/// </summary>
	public static class MethodUtils {
		/// <summary>The type "object".</summary>		
		#pragma warning disable 0414
		private static Type OBJECT_TYPE = typeof(object);

		/// <summary>
		/// Creates a constructor method with no parameters for an object.
		/// </summary>
		/// <param name="type">Object type.</param>
		/// <param name="constructor">Constructor info used to create the function.</param>
		/// <returns>The object constructor.</returns>
		public static Constructor CreateConstructor(Type type, ConstructorInfo constructor) {
			#if UNITY_IOS
			
			return () => {
				return constructor.Invoke(null);
			};

			#else
			
			var method = new DynamicMethod(type.Name, type, null, type);
			ILGenerator generator = method.GetILGenerator();
			generator.Emit(OpCodes.Newobj, constructor);
			generator.Emit(OpCodes.Ret);
			return (Constructor)method.CreateDelegate(typeof(Constructor));

			#endif
		}
		
		/// <summary>
		/// Creates a constructor method with parameters for an object of type <typeparamref name="T"/>.
		/// </summary>
		/// <param name="type">Object type.</param>
		/// <param name="constructor">Constructor info used to create the function.</param>
		/// <returns>The object constructor.</returns>
		public static ParamsConstructor CreateConstructorWithParams(Type type, ConstructorInfo constructor) {
			#if UNITY_IOS
			
			return (object[] parameters) => {
				return constructor.Invoke(parameters);
			};
			
			#else
			
			var parameters = constructor.GetParameters();
			
			Type[] parametersTypes = new Type[] { typeof(object[]) };
			var method = new DynamicMethod(type.Name, type, parametersTypes, type);
			ILGenerator generator = method.GetILGenerator();
			
			//Define parameters.
			for (int paramIndex = 0; paramIndex < parameters.Length; paramIndex++) {
				generator.Emit(OpCodes.Ldarg_0);
				
				//Define parameter position.
				switch (paramIndex) {
				case 0: generator.Emit(OpCodes.Ldc_I4_0); break;
				case 1: generator.Emit(OpCodes.Ldc_I4_1); break;
				case 2: generator.Emit(OpCodes.Ldc_I4_2); break;
				case 3: generator.Emit(OpCodes.Ldc_I4_3); break;
				case 4: generator.Emit(OpCodes.Ldc_I4_4); break;
				case 5: generator.Emit(OpCodes.Ldc_I4_5); break;
				case 6: generator.Emit(OpCodes.Ldc_I4_6); break;
				case 7: generator.Emit(OpCodes.Ldc_I4_7); break;
				case 8: generator.Emit(OpCodes.Ldc_I4_8); break;
				default: generator.Emit(OpCodes.Ldc_I4, paramIndex); break;
				}
				
				//Define parameter type.
				generator.Emit(OpCodes.Ldelem_Ref);
				Type paramType = parameters[paramIndex].ParameterType;
				generator.Emit(paramType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, paramType);
			}
			
			generator.Emit(OpCodes.Newobj, constructor);
			generator.Emit(OpCodes.Ret);
			return (ParamsConstructor)method.CreateDelegate(typeof(ParamsConstructor));
			
			#endif
		}
		
		/// <summary>
		/// Creates a field setter method.
		/// </summary>
		/// <param name="type">Object type.</param>
		/// <param name="fieldInfo">Field info object.</param>
		/// <returns>The field setter.</returns>
		public static Setter CreateFieldSetter(Type type, FieldInfo fieldInfo) {
			#if UNITY_IOS
			
			return (object instance, object value) => fieldInfo.SetValue(instance, value);
			
			#else
			
			var parametersTypes = new[] { OBJECT_TYPE, OBJECT_TYPE };
			DynamicMethod setMethod = new DynamicMethod(fieldInfo.Name, typeof(void), parametersTypes, true);
			ILGenerator generator = setMethod.GetILGenerator();
			
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldarg_1);
			generator.Emit(OpCodes.Stfld, fieldInfo);
			generator.Emit(OpCodes.Ret);
			
			return (Setter)setMethod.CreateDelegate(typeof(Setter));
			
			#endif
		}
		
		/// <summary>
		/// Creates a property setter method.
		/// </summary>
		/// <param name="type">Object type.</param>
		/// <param name="propertyInfo">Property info object.</param>
		/// <returns>The property setter.</returns>
		public static Setter CreatePropertySetter(Type type, PropertyInfo propertyInfo) {
			#if UNITY_IOS
			
			return (object instance, object value) => propertyInfo.SetValue(instance, value, null);
			
			#else
			
			var propertySetMethod = propertyInfo.GetSetMethod();
			
			var parametersTypes = new Type[] { OBJECT_TYPE, OBJECT_TYPE };
			DynamicMethod method = new DynamicMethod(propertyInfo.Name, typeof(void), parametersTypes, true);
			ILGenerator generator = method.GetILGenerator();
			
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldarg_1);
			generator.Emit(OpCodes.Callvirt, propertySetMethod);
			generator.Emit(OpCodes.Ret);
			
			return (Setter)method.CreateDelegate(typeof(Setter));
			
			#endif
		}
		
		/// <summary>
		/// Creates method call without parameters.
		/// </summary>
		/// <param name="type">Object type.</param>
		/// <param name="methodInfo">Method info object.</param>
		/// <returns>The method caller.</returns>
		public static PostConstructor CreateMethod(Type type, MethodInfo methodInfo) {
			#if UNITY_IOS
			
			return (object instance) => methodInfo.Invoke(instance, null);
			
			#else
			
			var parametersTypes = new Type[] { OBJECT_TYPE };
			DynamicMethod method = new DynamicMethod(methodInfo.Name, typeof(void), parametersTypes, true);
			ILGenerator generator = method.GetILGenerator();
			
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Callvirt, methodInfo);
			generator.Emit(OpCodes.Ret);
			
			return (PostConstructor)method.CreateDelegate(typeof(PostConstructor));
			
			#endif
		}
	}
}