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
        public static ConstructorCall CreateConstructor(Type type, ConstructorInfo constructor) {
            #if COMPILE_AOT || ENABLE_IL2CPP || UNITY_IOS || UNITY_WSA || UNITY_WP8 || UNITY_WP8_1 || UNITY_WEBGL || UNITY_XBOXONE

            return () => {
				return constructor.Invoke(null);
			};

            #else
			
            var method = new DynamicMethod(type.Name, type, null, type);
            ILGenerator generator = method.GetILGenerator();
            generator.Emit(OpCodes.Newobj, constructor);
            generator.Emit(OpCodes.Ret);
            return (ConstructorCall) method.CreateDelegate(typeof(ConstructorCall));

            #endif
        }

        /// <summary>
        /// Creates a constructor method with parameters for an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="constructor">Constructor info used to create the function.</param>
        /// <returns>The object constructor.</returns>
        public static ParamsConstructorCall CreateConstructorWithParams(Type type, ConstructorInfo constructor) {
            #if COMPILE_AOT || ENABLE_IL2CPP || UNITY_IOS || UNITY_WSA || UNITY_WP8 || UNITY_WP8_1 || UNITY_WEBGL || UNITY_XBOXONE

            return (object[] parameters) => {
				return constructor.Invoke(parameters);
			};
			
            #else
			
            var parameters = constructor.GetParameters();
			
            Type[] parametersTypes = new Type[] { typeof(object[]) };
            var method = new DynamicMethod(type.Name, type, parametersTypes, type);
            ILGenerator generator = method.GetILGenerator();
			
            // Define parameters.
            for (int paramIndex = 0; paramIndex < parameters.Length; paramIndex++) {
                generator.Emit(OpCodes.Ldarg_0);
				
                // Define parameter position.
                switch (paramIndex) {
                    case 0:
                        generator.Emit(OpCodes.Ldc_I4_0);
                    break;
                    case 1:
                        generator.Emit(OpCodes.Ldc_I4_1);
                    break;
                    case 2:
                        generator.Emit(OpCodes.Ldc_I4_2);
                    break;
                    case 3:
                        generator.Emit(OpCodes.Ldc_I4_3);
                    break;
                    case 4:
                        generator.Emit(OpCodes.Ldc_I4_4);
                    break;
                    case 5:
                        generator.Emit(OpCodes.Ldc_I4_5);
                    break;
                    case 6:
                        generator.Emit(OpCodes.Ldc_I4_6);
                    break;
                    case 7:
                        generator.Emit(OpCodes.Ldc_I4_7);
                    break;
                    case 8:
                        generator.Emit(OpCodes.Ldc_I4_8);
                    break;
                    default:
                        generator.Emit(OpCodes.Ldc_I4, paramIndex);
                    break;
                }
				
                // |Define parameter type.
                generator.Emit(OpCodes.Ldelem_Ref);
                Type paramType = parameters[paramIndex].ParameterType;
                generator.Emit(paramType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, paramType);
            }
			
            generator.Emit(OpCodes.Newobj, constructor);
            generator.Emit(OpCodes.Ret);
            return (ParamsConstructorCall) method.CreateDelegate(typeof(ParamsConstructorCall));
			
            #endif
        }

        /// <summary>
        /// Creates a field setter method.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="fieldInfo">Field info object.</param>
        /// <returns>The field setter.</returns>
        public static SetterCall CreateFieldSetter(Type type, FieldInfo fieldInfo) {
            #if COMPILE_AOT || ENABLE_IL2CPP || UNITY_IOS || UNITY_WSA || UNITY_WP8 || UNITY_WP8_1 || UNITY_WEBGL || UNITY_XBOXONE

            return (object instance, object value) => fieldInfo.SetValue(instance, value);
			
            #else
			
            var parametersTypes = new[] { OBJECT_TYPE, OBJECT_TYPE };
            DynamicMethod setMethod = new DynamicMethod(fieldInfo.Name, typeof(void), parametersTypes, true);
            ILGenerator generator = setMethod.GetILGenerator();
			
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Stfld, fieldInfo);
            generator.Emit(OpCodes.Ret);
			
            return (SetterCall) setMethod.CreateDelegate(typeof(SetterCall));
			
            #endif
        }

        /// <summary>
        /// Creates a field getter method.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="fieldInfo">Field info object.</param>
        /// <returns>The field getter.</returns>
        public static GetterCall CreateFieldGetter(Type type, FieldInfo fieldInfo) {
            return (object instance) => fieldInfo.GetValue(instance);
        }

        /// <summary>
        /// Creates a property setter method.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="propertyInfo">Property info object.</param>
        /// <returns>The property setter.</returns>
        public static SetterCall CreatePropertySetter(Type type, PropertyInfo propertyInfo) {
            #if COMPILE_AOT || ENABLE_IL2CPP || UNITY_IOS || UNITY_WSA || UNITY_WP8 || UNITY_WP8_1 || UNITY_WEBGL || UNITY_XBOXONE

            return (object instance, object value) => propertyInfo.SetValue(instance, value, null);
			
            #else
			
            var propertySetMethod = propertyInfo.GetSetMethod(true);
			
            var parametersTypes = new Type[] { OBJECT_TYPE, OBJECT_TYPE };
            DynamicMethod method = new DynamicMethod(propertyInfo.Name, typeof(void), parametersTypes, true);
            ILGenerator generator = method.GetILGenerator();
			
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Callvirt, propertySetMethod);
            generator.Emit(OpCodes.Ret);
			
            return (SetterCall) method.CreateDelegate(typeof(SetterCall));
			
            #endif
        }

        /// <summary>
        /// Creates a property getter method.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="propertyInfo">Property info object.</param>
        /// <returns>The property getter or null if the property can't be read.</returns>
        public static GetterCall CreatePropertyGetter(Type type, PropertyInfo propertyInfo) {
            if (propertyInfo.CanRead) {
                return (object instance) => propertyInfo.GetValue(instance, null);
            } else {
                return null;
            }
        }

        /// <summary>
        /// Creates method call without parameters.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="methodInfo">Method info object.</param>
        /// <returns>The method caller.</returns>
        public static MethodCall CreateParameterlessMethod(Type type, MethodInfo methodInfo) {
            #if COMPILE_AOT || ENABLE_IL2CPP || UNITY_IOS || UNITY_WSA || UNITY_WP8 || UNITY_WP8_1 || UNITY_WEBGL || UNITY_XBOXONE

            return (object instance) => methodInfo.Invoke(instance, null);
			
            #else
			
            var parametersTypes = new Type[] { OBJECT_TYPE };
            DynamicMethod method = new DynamicMethod(methodInfo.Name, typeof(void), parametersTypes, true);
            ILGenerator generator = method.GetILGenerator();
			
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Callvirt, methodInfo);
            generator.Emit(OpCodes.Ret);
			
            return (MethodCall) method.CreateDelegate(typeof(MethodCall));
			
            #endif
        }

        /// <summary>
        /// Creates method call with parameters.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="methodInfo">Method info object.</param>
        /// <returns>The method caller.</returns>
        public static ParamsMethodCall CreateParameterizedMethod(Type type, MethodInfo methodInfo) {
            return (object instance, object[] parameters) => methodInfo.Invoke(instance, parameters);
        }
    }
}