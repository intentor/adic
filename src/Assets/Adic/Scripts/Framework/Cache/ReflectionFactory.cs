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
                    reflectedClass.paramsConstructor = MethodUtils.CreateConstructorWithParams(type, constructor);
                    ;
                }
            }

            reflectedClass.constructorParameters = this.ResolveConstructorParameters(constructor);
            reflectedClass.methods = this.ResolveMethods(type);
            reflectedClass.properties = this.ResolveProperties(type);
            reflectedClass.fields = this.ResolveFields(type);

            return reflectedClass;
        }

        /// <summary>
        /// Selects the constructor marked with <see cref="InjectAttribute"/> or with the minimum amount of parameters.
        /// </summary>
        /// <param name="type">Type from which reflection will be resolved.</param>
        /// <returns>The constructor.</returns>
        #pragma warning disable 0618
		protected ConstructorInfo ResolveConstructor(Type type) {
            var constructors = type.GetConstructors(
                                   BindingFlags.FlattenHierarchy |
                                   BindingFlags.Public |
                                   BindingFlags.NonPublic |
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

                // Construct attribute will be removed on future version.
                var attributesConstruct = constructor.GetCustomAttributes(typeof(Construct), true);
                var attributesInject = constructor.GetCustomAttributes(typeof(Inject), true);

                if (attributesConstruct.Length > 0 || attributesInject.Length > 0) {
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
            if (constructor == null)
                return null;

            var parameters = constructor.GetParameters();			
			
            var constructorParameters = new ParameterInfo[parameters.Length];
            for (int paramIndex = 0; paramIndex < constructorParameters.Length; paramIndex++) {
                object identifier = null;
                var parameter = parameters[paramIndex];

                var attributes = parameter.GetCustomAttributes(typeof(Inject), true);
                if (attributes.Length > 0) {
                    identifier = (attributes[0] as Inject).identifier;
                }

                constructorParameters[paramIndex] = new ParameterInfo(parameter.ParameterType, parameter.Name, identifier);
            }

            return constructorParameters;
        }

        /// <summary>
        /// Resolves the methods that can be injected.
        /// </summary>
        /// <returns>The methods with Inject attributes.</returns>
        #pragma warning disable 0618
		protected MethodInfo[] ResolveMethods(Type type) {
            var methodCalls = new List<MethodInfo>();

            var methods = type.GetMethods(BindingFlags.FlattenHierarchy |
                              BindingFlags.Public |
                              BindingFlags.NonPublic |
                              BindingFlags.Instance);

            for (int methodIndex = 0; methodIndex < methods.Length; methodIndex++) {
                var method = methods[methodIndex];
				
                // PostConstruct attribute will be removed on future version.
                var attributesPostConstruct = method.GetCustomAttributes(typeof(PostConstruct), true);
                var attributesInject = method.GetCustomAttributes(typeof(Inject), true);

                if (attributesPostConstruct.Length > 0 || attributesInject.Length > 0) {
                    var parameters = method.GetParameters();
                    var methodParameters = new ParameterInfo[parameters.Length];
                    for (int paramIndex = 0; paramIndex < methodParameters.Length; paramIndex++) {
                        object identifier = null;
                        var parameter = parameters[paramIndex];
						
                        var parameterAttributes = parameter.GetCustomAttributes(typeof(Inject), true);
                        if (parameterAttributes.Length > 0) {
                            identifier = (parameterAttributes[0] as Inject).identifier;
                        }
						
                        methodParameters[paramIndex] = new ParameterInfo(parameter.ParameterType, parameter.Name, identifier);
                    }

                    var methodCall = new MethodInfo(method.Name, methodParameters);

                    if (methodParameters.Length == 0) {
                        methodCall.method = MethodUtils.CreateParameterlessMethod(type, method);
                    } else {
                        methodCall.paramsMethod = MethodUtils.CreateParameterizedMethod(type, method);
                    }

                    methodCalls.Add(methodCall);
                }
            }

            return methodCalls.ToArray();
        }

        /// <summary>
        /// Resolves the properties that can be injected.
        /// </summary>
        /// <param name="type">Type from which reflection will be resolved.</param>
        /// <returns>The properties.</returns>
        protected AcessorInfo[] ResolveProperties(Type type) {
            var setters = new List<AcessorInfo>();

            var properties = type.GetProperties(BindingFlags.Instance |
                                 BindingFlags.Static |
                                 BindingFlags.Public |
                                 BindingFlags.NonPublic);

            for (int propertyIndex = 0; propertyIndex < properties.Length; propertyIndex++) {
                var property = properties[propertyIndex] as PropertyInfo;
                var attributes = property.GetCustomAttributes(typeof(Inject), true);

                if (attributes.Length > 0) {
                    var attribute = attributes[0] as Inject;
                    var getter = MethodUtils.CreatePropertyGetter(type, property);
                    var setter = MethodUtils.CreatePropertySetter(type, property);
                    var info = new AcessorInfo(property.PropertyType, property.Name, attribute.identifier, getter, 
                                   setter);
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
        protected AcessorInfo[] ResolveFields(Type type) {
            var setters = new List<AcessorInfo>();
			
            var fields = type.GetFields(BindingFlags.Instance |
                             BindingFlags.Static |
                             BindingFlags.Public |
                             BindingFlags.NonPublic);
			
            for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++) {
                var field = fields[fieldIndex] as FieldInfo;
                var attributes = field.GetCustomAttributes(typeof(Inject), true);
				
                if (attributes.Length > 0) {
                    var attribute = attributes[0] as Inject;
                    var getter = MethodUtils.CreateFieldGetter(type, field);
                    var setter = MethodUtils.CreateFieldSetter(type, field);
                    var info = new AcessorInfo(field.FieldType, field.Name, attribute.identifier, getter, setter);
                    setters.Add(info);
                }
            }
			
            return setters.ToArray();
        }
    }
}