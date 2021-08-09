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
            return () => {
				return constructor.Invoke(null);
			};
        }

        /// <summary>
        /// Creates a constructor method with parameters for an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="constructor">Constructor info used to create the function.</param>
        /// <returns>The object constructor.</returns>
        public static ParamsConstructorCall CreateConstructorWithParams(Type type, ConstructorInfo constructor) {
            return (object[] parameters) => {
                return constructor.Invoke(parameters);
            };
        }

        /// <summary>
        /// Creates a field setter method.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="fieldInfo">Field info object.</param>
        /// <returns>The field setter.</returns>
        public static SetterCall CreateFieldSetter(Type type, FieldInfo fieldInfo) {
            return (object instance, object value) => fieldInfo.SetValue(instance, value);
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
            return (object instance, object value) => propertyInfo.SetValue(instance, value, null);
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
            return (object instance) => methodInfo.Invoke(instance, null);
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