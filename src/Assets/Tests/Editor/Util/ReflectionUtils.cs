using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
	
namespace Adic.Tests.Util {
	/// <summary>
	/// Reflection utils for tests.
	/// </summary>
	public static class ReflectionUtils {
		/// <summary>
		/// Delegate for a constructor call with parameters.
		/// </summary>
		/// <typeparam name="T">Constructor's object type.</typeparam>
		/// <param name="parameters">Constructor parameters.</param>
		public delegate T ParamsConstructorDelegate<T>(params object[] parameters);

		/// <summary>
		/// Creates a constructor method with no parameters for an object of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">Instance's type to be created.</typeparam>
		/// <returns>The object constructor.</returns>
		public static Func<T> CreateConstructor<T>() {
			var type = typeof(T);

			var method = new DynamicMethod(type.Name, type, null, type);
			ILGenerator generator = method.GetILGenerator();
			generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
			generator.Emit(OpCodes.Ret);
			return (Func<T>)method.CreateDelegate(typeof(Func<T>));

			/* Below is another way of creating the constructor, which is slightly slow.
			return Expression.Lambda<Func<T>>(Expression.New(type)).Compile();*/
		}

		/// <summary>
		/// Creates a constructor method with parameters for an object of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">Instance's type to be created.</typeparam>
		/// <param name="constructor">Constructor info used to create the function.</param>
		/// <returns>The object constructor.</returns>
		public static ParamsConstructorDelegate<T> CreateConstructorWithParams<T>(ConstructorInfo constructor) {
			var type = typeof(T);
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
			return (ParamsConstructorDelegate<T>)method.CreateDelegate(typeof(ParamsConstructorDelegate<T>));
		}

		/// <summary>
		/// Creates a field getter method.
		/// </summary>
		/// <typeparam name="TClass">Class' type the field belongs to.</typeparam>
		/// <typeparam name="TFieldType">Field type.</typeparam>
		/// <param name="fieldName">Field name.</param>
		/// <returns>The field getter.</returns>
		public static Func<TClass, TFieldType> CreateFieldGetter<TClass, TFieldType>(string fieldName) {
			var classType = typeof(TClass);
			var field = classType.GetField(fieldName);
			
			var parametersTypes = new Type[] { classType };
			DynamicMethod method = new DynamicMethod(fieldName, typeof(TFieldType), parametersTypes, true);
			ILGenerator generator = method.GetILGenerator();
			
			generator.Emit(OpCodes.Ldarg_0);			
			generator.Emit(OpCodes.Castclass, classType);
			generator.Emit(OpCodes.Ldfld, field);
			generator.Emit(OpCodes.Ret);
			
			return (Func<TClass, TFieldType>)method.CreateDelegate(typeof(Func<TClass, TFieldType>));

			/* Below is another way of creating the getter, which is slightly slow.
			ParameterExpression arg = Expression.Parameter(typeof(TClass), "value");
			Expression expr = Expression.Field(arg, fieldName);
			var getter = Expression.Lambda<Func<TClass, TFieldType>>(expr, arg).Compile();
			
			return getter;*/
		}
		
		/// <summary>
		/// Creates a field setter method.
		/// </summary>
		/// <typeparam name="TClass">Class' type the field belongs to.</typeparam>
		/// <typeparam name="TFieldType">Field type.</typeparam>
		/// <param name="fieldName">Field name.</param>
		/// <returns>The field setter.</returns>
		public static Action<TClass, TFieldType> CreateFieldSetter<TClass, TFieldType>(string fieldName) {
			var classType = typeof(TClass);
			var field = classType.GetField(fieldName);
			
			var parametersTypes = new[] { classType, typeof(TFieldType) };
			DynamicMethod setMethod = new DynamicMethod(field.Name, typeof(void), parametersTypes, true);
			ILGenerator generator = setMethod.GetILGenerator();
			
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldarg_1);
			generator.Emit(OpCodes.Stfld, field);
			generator.Emit(OpCodes.Ret);
			
			return (Action<TClass, TFieldType>)setMethod.CreateDelegate(typeof(Action<TClass, TFieldType>));
		}
		
		/// <summary>
		/// Creates a property getter method.
		/// </summary>
		/// <typeparam name="TClass">Class' type the property belongs to.</typeparam>
		/// <typeparam name="TPropertyType">Property type.</typeparam>
		/// <param name="propertyName">Property name.</param>
		/// <returns>The property getter.</returns>
		public static Func<TClass, TPropertyType> CreatePropertyGetter<TClass, TPropertyType>(string propertyName) {
			var classType = typeof(TClass);
			var propertyGetMethod = classType.GetProperty(propertyName).GetGetMethod();
			
			var parametersTypes = new Type[] { classType };
			DynamicMethod method = new DynamicMethod(propertyName, typeof(TPropertyType), parametersTypes, true);
			ILGenerator generator = method.GetILGenerator();
			
			generator.Emit(OpCodes.Ldarg_0);			
			generator.Emit(OpCodes.Castclass, classType);
			generator.Emit(OpCodes.Callvirt, propertyGetMethod);
			generator.Emit(OpCodes.Ret);
			
			return (Func<TClass, TPropertyType>)method.CreateDelegate(typeof(Func<TClass, TPropertyType>));

			/* Below is another way of creating the getter, which is slightly slow.
			ParameterExpression arg = Expression.Parameter(typeof(TClass), "value");
			Expression expr = Expression.Property(arg, propertyName);
			var getter = Expression.Lambda<Func<TClass, TPropertyType>>(expr, arg).Compile();
			
			return getter;*/
		}
		
		/// <summary>
		/// Creates a property setter method.
		/// </summary>
		/// <typeparam name="TClass">Class' type the property belongs to.</typeparam>
		/// <typeparam name="TPropertyType">Property type.</typeparam>
		/// <param name="propertyName">Property name.</param>
		/// <returns>The property setter.</returns>
		public static Action<TClass, TPropertyType> CreatePropertySetter<TClass, TPropertyType>(string propertyName) {
			var classType = typeof(TClass);
			var propertySetMethod = classType.GetProperty(propertyName).GetSetMethod();
			
			var parametersTypes = new Type[] { classType, typeof(TPropertyType) };
			DynamicMethod method = new DynamicMethod(propertyName, typeof(void), parametersTypes, true);
			ILGenerator generator = method.GetILGenerator();

			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldarg_1);
			generator.Emit(OpCodes.Callvirt, propertySetMethod);
			generator.Emit(OpCodes.Ret);
			
			return (Action<TClass, TPropertyType>)method.CreateDelegate(typeof(Action<TClass, TPropertyType>));
		}

		/// <summary>
		/// Creates method call without parameters.
		/// </summary>
		/// <typeparam name="T">Class' type the method belongs to.</typeparam>
		/// <param name="propertyName">Property name.</param>
		/// <returns>The property getter.</returns>
		public static Action<T> CreateMethod<T>(string methodName) {
			var classType = typeof(T);
			var methodInfo = classType.GetMethod(methodName);

			var parametersTypes = new Type[] { classType };
			DynamicMethod method = new DynamicMethod(methodName, typeof(void), parametersTypes, true);
			ILGenerator generator = method.GetILGenerator();
			
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Callvirt, methodInfo);
			generator.Emit(OpCodes.Ret);
			
			return (Action<T>)method.CreateDelegate(typeof(Action<T>));
		}
	}
}