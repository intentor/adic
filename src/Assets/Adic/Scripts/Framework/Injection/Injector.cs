using System;
using System.Collections.Generic;
using Adic.Binding;
using Adic.Cache;
using Adic.Exceptions;

namespace Adic.Injection {
    /// <summary>
    /// An injector that uses a binder to resolve bindings.
    /// </summary>
    public class Injector : IInjector {
        /// <summary>Occurs before a type is resolved.</summary>
        public event TypeResolutionHandler beforeResolve;
        /// <summary>Occurs after a type is resolved.</summary>
        public event TypeResolutionHandler afterResolve;
        /// <summary>Occurs when a binding is available for resolution.</summary>
        public event BindingEvaluationHandler bindingEvaluation;
        /// <summary>Occurs when a binding is resolved to an instance.</summary>
        public event BindingResolutionHandler bindingResolution;
        /// <summary>Occurs before an instance receives injection.</summary>
        public event InstanceInjectionHandler beforeInject;
        /// <summary>Occurs after an instance receives injection.</summary>
        public event InstanceInjectionHandler afterInject;

        /// <summary>Reflection cache used to get type info.</summary>
        public IReflectionCache cache { get; protected set; }

        /// <summary>Binder used to resolved bindings.</summary>
        public IBinder binder { get; protected set; }

        /// <summary>Instance resolution mode.</summary>
        public ResolutionMode resolutionMode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.InjectorBinder"/> class.
        /// </summary>
        /// <param name="cache">Reflection cache used to get type info.</param>
        /// <param name="binder">Binder used to resolved bindings.</param>
        public Injector(IReflectionCache cache, IBinder binder, ResolutionMode resolutionMode) {
            this.cache = cache;
            this.binder = binder;
            this.resolutionMode = resolutionMode;

            this.binder.beforeAddBinding += this.OnBeforeAddBinding;
        }

        /// <summary>
        /// Resolves an instance for a specified type.
        /// </summary>
        /// <remarks>
        /// If the type has multiple instances, please use ResolveAll<T>().
        /// </remarks>
        /// <typeparam name="T">Type to be resolved.</typeparam>
        /// <returns>The instance or NULL.</returns>
        public T Resolve<T>() {
            return (T) this.Resolve(typeof(T), InjectionMember.None, null, null, null, false);
        }

        /// <summary>
        /// Resolves an instance for a specified type with a given identifier.
        /// </summary>
        /// <remarks>
        /// If the type has multiple instances, please use ResolveAll<T>().
        /// </remarks>
        /// <typeparam name="T">Type to be resolved.</typeparam>
        /// <param name="identifier">Identifier to look for.</param>
        /// <returns>The instance or NULL.</returns>
        public T Resolve<T>(object identifier) {
            return (T) this.Resolve(typeof(T), InjectionMember.None, null, null, identifier, false);
        }

        /// <summary>
        /// Resolves an instance for a specified type.
        /// </summary>
        /// <remarks>
        /// If the type has multiple instances, it will return an IList<[type]>.
        /// </remarks>
        /// <param name="type">Type to be resolved.</param>
        /// <returns>The instance or NULL.</returns>
        public object Resolve(Type type) {
            return this.Resolve(type, InjectionMember.None, null, null, null, false);
        }

        /// <summary>
        /// Resolves an instance with a given identifier.
        /// </summary>
        /// <remarks>
        /// If the type has multiple instances, please use ResolveAll().
        /// </remarks>
        /// <param name="identifier">Identifier to look for.</param>
        /// <returns>The instance or NULL.</returns>
        public object Resolve(object identifier) {
            var resolution = this.Resolve(null, InjectionMember.None, null, null, identifier, false);

            if (resolution != null && resolution.GetType().IsArray) {
                var instances = (object[]) resolution;
                return instances.Length > 0 ? instances[0] : null;
            } else if (resolution != null) {
                return resolution;
            } else {
                return null;
            }
        }

        /// <summary>
        /// Resolves an instance for a specified type with a given identifier.
        /// </summary>
        /// <remarks>
        /// If the type has multiple instances, it will return an IList<[type]>.
        /// </remarks>
        /// <param name="type">Type to be resolved.</param>
        /// <param name="identifier">Identifier to look for.</param>
        /// <returns>The instance or NULL.</returns>
        public object Resolve(Type type, object identifier) {
            return this.Resolve(type, InjectionMember.None, null, null, identifier, false);
        }

        /// <summary>
        /// Resolves a list of instances for a specified type.
        /// </summary>
        /// <typeparam name="T">Type to be resolved.</typeparam>
        /// <returns>The list of instances or NULL if there are no instances.</returns>
        public T[] ResolveAll<T>() {
            return this.ResolveAll<T>(null);
        }

        /// <summary>
        /// Resolves a list of instances for a specified type with a given identifier.
        /// </summary>
        /// <typeparam name="T">Type to be resolved.</typeparam>
        /// <param name="identifier">Identifier to look for.</param>
        /// <returns>The list of instances or NULL if there are no instances.</returns>
        public T[] ResolveAll<T>(object identifier) {
            var instance = this.Resolve(typeof(T[]), identifier);
			
            if (instance == null) {
                return null;
            } else if (!instance.GetType().IsArray) {
                var array = Array.CreateInstance(instance.GetType(), 1);
                array.SetValue(instance, 0);
                return (T[]) array;
            } else {
                return (T[]) instance;
            }
        }

        /// <summary>
        /// Resolves a list of instances for a specified type.
        /// </summary>
        /// <param name="type">Type to be resolved.</param>
        /// <returns>The list of instances or NULL if there are no instances.</returns>
        public object[] ResolveAll(Type type) {
            return this.ResolveAll(type, null);
        }

        /// <summary>
        /// Resolves a list of instances with a given identifier.
        /// </summary>
        /// <param name="identifier">Identifier to look for.</param>
        /// <returns>The list of instances or NULL if there are no instances.</returns>
        public object[] ResolveAll(object identifier) {
            return this.ResolveAll(null, identifier);
        }

        /// <summary>
        /// Resolves a list of instances for a specified type with a given identifier.
        /// </summary>
        /// <param name="type">Type to be resolved.</param>
        /// <param name="identifier">Identifier to look for.</param>
        /// <returns>The list of instances or NULL if there are no instances.</returns>
        public object[] ResolveAll(Type type, object identifier) {
            var instance = this.Resolve(type, identifier);
			
            if (instance == null) {
                return null;
            } else if (!instance.GetType().IsArray) {
                var array = Array.CreateInstance(instance.GetType(), 1);
                array.SetValue(instance, 0);
                return (object[]) array;
            } else {
                return (object[]) instance;
            }
        }

        /// <summary>
        /// Resolves an instance for a specified type at a certain member in an instance with a given identifier.
        /// </summary>
        /// <param name="type">Binding type.</param>
        /// <param name="member">Member for which the binding is being resolved.</param>
        /// <param name="memberName">Member name in which the binding is being resolved.</param>
        /// <param name="parentInstance">Parent object in which the resolve is occuring.</param>
        /// <param name="identifier">The binding identifier to be looked for.</param>
        /// <param name="alwaysResolve">Always resolve the type, even when resolution mode is null.</param>
        protected object Resolve(Type type, InjectionMember member, string memberName, object parentInstance,
            object identifier, bool alwaysResolve) {
            object resolution = null;

            if (this.beforeResolve != null) {
                var delegates = this.beforeResolve.GetInvocationList();
                for (int delegateIndex = 0; delegateIndex < delegates.Length; delegateIndex++) {
                    var continueExecution = ((TypeResolutionHandler) delegates[delegateIndex]).Invoke(this, 
                                                type,
                                                member,
                                                parentInstance,
                                                identifier,
                                                ref resolution);

                    if (!continueExecution) {
                        return resolution;
                    }
                }
            }

            // Array is used for multiple injection.
            // So, when the type is an array, the type to be read from the bindings list is the element type.
            Type typeToGet;
            IList<BindingInfo> bindings;
            Boolean typeIsnull = type == null;
            if (typeIsnull) {
                typeToGet = typeof(object);

                // If no type is provided, look for bindings by identifier.
                bindings = this.binder.GetBindingsFor(identifier);
            } else {
                if (type.IsArray) {
                    typeToGet = type.GetElementType();
                } else {
                    typeToGet = type;
                }
			
                // If a type is provided, look for bindings by identifier.
                bindings = this.binder.GetBindingsFor(typeToGet);
            }

            IList<object> instances = new List<object>();

            if (bindings == null) {
                if (alwaysResolve || this.resolutionMode == ResolutionMode.ALWAYS_RESOLVE) {
                    if (!(typeToGet.IsInterface) && !(type.IsArray)) {
                        instances.Add(this.Instantiate(typeToGet));
                    }
                } else {
                    return null;
                }
            } else {
                for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++) {
                    var binding = bindings[bindingIndex];
					
                    var instance = this.ResolveBinding(binding, type, member, memberName, parentInstance, identifier);

                    if (instance != null) {
                        instances.Add(instance);
                    }
                }
            }
			
            if ((typeIsnull && instances.Count == 1) || (!typeIsnull && !type.IsArray && instances.Count == 1)) {
                resolution = instances[0];
            } else if ((typeIsnull && instances.Count > 1) || (!typeIsnull && type.IsArray)) {
                var array = Array.CreateInstance(typeToGet, instances.Count);
                for (int listIndex = 0; listIndex < instances.Count; listIndex++) {
                    array.SetValue(instances[listIndex], listIndex);
                }
                resolution = array;
            }

            if (this.afterResolve != null) {
                var delegates = this.afterResolve.GetInvocationList();
                for (int delegateIndex = 0; delegateIndex < delegates.Length; delegateIndex++) {
                    var continueExecution = ((TypeResolutionHandler) delegates[delegateIndex]).Invoke(this, 
                                                type,
                                                member,
                                                parentInstance,
                                                identifier,
                                                ref resolution);
					
                    if (!continueExecution) {
                        return resolution;
                    }
                }
            }

            return resolution;
        }

        /// <summary>
        /// Injects dependencies on an instance of an object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be resolved.</typeparam>
        /// <param name="instance">Instance to receive injection.</param>
        /// <returns>The instance with all its dependencies injected.</returns>
        public T Inject<T>(T instance) where T : class {
            var reflectedClass = this.cache.GetClass(instance.GetType());
            return (T) this.Inject(instance, reflectedClass);
        }

        /// <summary>
        /// Injects dependencies on an instance of an object.
        /// </summary>
        /// <param name="instance">Instance to receive injection.</param>
        /// <returns>The instance with all its dependencies injected.</returns>
        public object Inject(object instance) {
            var reflectedClass = this.cache.GetClass(instance.GetType());
            return this.Inject(instance, reflectedClass);
        }

        /// <summary>
        /// Injects dependencies on an instance of an object.
        /// </summary>
        /// <param name="instance">The instance to have its dependencies resolved.</param>
        /// <param name="reflectedClass">The reflected class related to the <paramref name="instance"/>.</param>
        protected object Inject(object instance, ReflectedClass reflectedClass) {
            if (this.beforeInject != null) {
                this.beforeInject(this, ref instance, reflectedClass);
            }

            if (reflectedClass.fields.Length > 0) {
                this.InjectFields(instance, reflectedClass.fields);
            }

            if (reflectedClass.properties.Length > 0) {
                this.InjectProperties(instance, reflectedClass.properties);
            }

            if (reflectedClass.methods.Length > 0) {
                this.InjectMethods(instance, reflectedClass.methods);
            }

            if (this.afterInject != null) {
                this.afterInject(this, ref instance, reflectedClass);
            }
			
            return instance;
        }

        /// <summary>
        /// Injects on fields.
        /// </summary>
        /// <remarks>
        /// The value is set only if the field has no value already set.
        /// </remarks>
        /// <param name="instance">The instance to have its dependencies resolved.</param>
        /// <param name="fields">Public fields of the type that can receive injection.</param>
        protected void InjectFields(object instance, AcessorInfo[] fields) {
            for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++) {
                var field = fields[fieldIndex];
                
                var value = field.getter(instance);

                // The Equals(null) comparison is used to ensure correct null evaluation due to the null trick
                // Unity uses for objects derived from UnityEngine.Object.
                if (value == null || value.Equals(null)) {
                    try {
                        var valueToSet = this.Resolve(field.type, InjectionMember.Field, field.name, instance,
                                             field.identifier, false);
                        field.setter(instance, valueToSet);
                    } catch (Exception e) {
                        throw new InjectorException(string.Format("Unable to inject on field {0} at object {1}.\n" +
                                "Caused by: {2}", field.name, instance.GetType(), e.Message), e);
                    }
                }
            }
        }

        /// <summary>
        /// Injects on properties.
        /// </summary>
        /// <remarks>
        /// The value is set only if the property has no value already set.
        /// </remarks>
        /// <param name="instance">The instance to have its dependencies resolved.</param>
        /// <param name="properties">Public properties of the type that can receive injection.</param>
        protected void InjectProperties(object instance, AcessorInfo[] properties) {
            for (int propertyIndex = 0; propertyIndex < properties.Length; propertyIndex++) {
                var property = properties[propertyIndex];

                var value = property.getter == null ? null : property.getter(instance);

                // The Equals(null) comparison is used to ensure correct null evaluation due to the null trick
                // Unity uses for objects derived from UnityEngine.Object.
                if (value == null || value.Equals(null)) {
                    try {
                        var valueToSet = this.Resolve(property.type, InjectionMember.Property, property.name, instance, 
                                             property.identifier, false);
                        property.setter(instance, valueToSet);
                    } catch (Exception e) {
                        throw new InjectorException(string.Format("Unable to inject on property {0} at object {1}.\n" +
                                "Caused by: {2}", property.name, instance.GetType(), e.Message), e);
                    }
                }
            }
        }

        /// <summary>
        /// Injects on methods.
        /// </summary>
        /// <param name="instance">The instance to have its dependencies resolved.</param>
        /// <param name="methods">Methods that have the Inject attribute.</param>
        protected void InjectMethods(object instance, MethodInfo[] methods) {
            for (int constIndex = 0; constIndex < methods.Length; constIndex++) {
                var method = methods[constIndex];

                try {
                    if (method.parameters.Length == 0) {
                        method.method(instance);
                    } else {
                        object[] parameters = this.GetParametersFromInfo(instance, method.parameters, InjectionMember.Method);
                        method.paramsMethod(instance, parameters);
                    }
                } catch (Exception e) {
                    throw new InjectorException(string.Format("Unable to inject on method {0} at object {1}.\n" +
                            "Caused by: {2}", method.name, instance.GetType(), e.Message), e);
                }
            }
        }

        /// <summary>
        /// Resolves the binding.
        /// </summary>
        /// <param name="binding">Binding to be resolved.</param>
        /// <param name="type">Binding type.</param>
        /// <param name="member">Member for which the binding is being resolved.</param>
        /// <param name="memberName">Member name in which the binding is being resolved.</param>
        /// <param name="parentInstance">Parent object in which the resolve is occuring.</param>
        /// <param name="identifier">The binding identifier to be looked for.</param>
        /// <returns>The resolved instance from the binding.</returns>
        protected object ResolveBinding(BindingInfo binding, Type type, InjectionMember member, string memberName,
                                        object parentInstance, object identifier) {
            // Condition evaluation.
            if (binding.condition != null) {
                var context = new InjectionContext() {
                    member = member,
                    memberType = type,
                    memberName = memberName,
                    identifier = identifier,
                    parentType = (parentInstance != null ? parentInstance.GetType() : null),
                    parentInstance = parentInstance,
                    injectType = binding.type
                };
				
                if (!binding.condition(context)) {
                    return null;
                }
            }

            // Identifier evaluation.
            bool resolveByIdentifier = identifier != null;
            bool bindingHasIdentifier = binding.identifier != null;
            if ((!resolveByIdentifier && bindingHasIdentifier) ||
                (resolveByIdentifier && !bindingHasIdentifier) ||
                (resolveByIdentifier && bindingHasIdentifier && !binding.identifier.Equals(identifier))) {
                return null;
            }

            // Instance evaluation.
            object instance = null;

            if (this.bindingEvaluation != null) {
                var delegates = this.bindingEvaluation.GetInvocationList();
                for (int delegateIndex = 0; delegateIndex < delegates.Length; delegateIndex++) {
                    instance = ((BindingEvaluationHandler) delegates[delegateIndex]).Invoke(this, ref binding);
                }
            }

            if (instance == null) {
                if (binding.instanceType == BindingInstance.Transient) {
                    instance = this.Instantiate(binding.value as Type);
                } else if (binding.instanceType == BindingInstance.Factory) {
                    var context = new InjectionContext() {
                        member = member,
                        memberType = type,
                        memberName = memberName,
                        identifier = identifier,
                        parentType = (parentInstance != null ? parentInstance.GetType() : null),
                        parentInstance = parentInstance,
                        injectType = binding.type
                    };

                    instance = (binding.value as IFactory).Create(context);
                } else {
                    // Binding is a singleton object.
					
                    // If the binding value is a type, instantiate it.
                    if (binding.value is Type) {
                        binding.value = this.Instantiate(binding.value as Type);
                    }
					
                    instance = binding.value;
                }
            }

            if (this.bindingResolution != null) {
                this.bindingResolution(this, ref binding, ref instance);
            }

            return instance;
        }

        /// <summary>
        /// Instantiate and resolve dependencies for the specified type.
        /// </summary>
        /// <param name="type">The type to be instantiated.</param>
        protected object Instantiate(Type type) {
            if (type.IsInterface) {
                throw new InjectorException(string.Format(InjectorException.CANNOT_INSTANTIATE_INTERFACE, type.ToString()));
            }

            var reflectedClass = this.cache.GetClass(type);
            object instance = null;

            if (reflectedClass.constructor == null && reflectedClass.paramsConstructor == null) {
                throw new InjectorException(string.Format(InjectorException.NO_CONSTRUCTORS, type.ToString()));
            }

            if (reflectedClass.constructorParameters.Length == 0) {
                instance = reflectedClass.constructor();
            } else {
                object[] parameters = this.GetParametersFromInfo(null, reflectedClass.constructorParameters, InjectionMember.Constructor);
                instance = reflectedClass.paramsConstructor(parameters);
            }

            instance = this.Inject(instance, reflectedClass);

            return instance;
        }

        /// <summary>
        /// Gets parameters from a collection of <see cref="Adic.Cache.ParameterInfo"/>.
        /// </summary>
        /// <param name="instance">The instance to have its dependencies resolved.</param>
        /// <param name="parametersInfo">Parameters info collection.</param>
        /// <param name="injectionMember">The member in which the injection is taking place.</param>
        /// <returns>The parameters.</returns>
        protected object[] GetParametersFromInfo(object instance, ParameterInfo[] parametersInfo, InjectionMember injectionMember) {
            object[] parameters = new object[parametersInfo.Length];
			
            for (int paramIndex = 0; paramIndex < parameters.Length; paramIndex++) {
                var parameterInfo = parametersInfo[paramIndex];
				
                parameters[paramIndex] = this.Resolve(parameterInfo.type, injectionMember, 
                    parameterInfo.name, instance, parameterInfo.identifier, false);
            }

            return parameters;
        }

        /// <summary>
        /// Handles the before add binding event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="binding">The binding to be added, by reference.</param>
        protected void OnBeforeAddBinding(IBinder source, ref BindingInfo binding) {
            if (binding.instanceType == BindingInstance.Singleton ||
                binding.instanceType == BindingInstance.Factory) {
                // Check whether a binding for the same type already exists.
                var bindings = this.binder.GetBindings();
                var bindingIsType = binding.value is Type;
                BindingInfo existingBinding = null;
                for (var bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++) {
                    var bindingFromBinder = bindings[bindingIndex];

                    var isSingleton = 
                        (bindingFromBinder.instanceType == BindingInstance.Singleton ||
                        bindingFromBinder.instanceType == BindingInstance.Factory);
                    var valueTypeIsTheSame = 
                        isSingleton &&
                        bindingIsType &&
                        bindingFromBinder.value != null &&
                        bindingFromBinder.value.GetType().Equals(binding.value);
                    var valueInstanceIsTheSame = 
                        isSingleton &&
                        !bindingIsType &&
                        bindingFromBinder.value == binding.value;

                    if (valueTypeIsTheSame || valueInstanceIsTheSame) {
                        existingBinding = bindingFromBinder;
                        // Break because if one is found, any other that already exists will be the same instance.
                        break;
                    }
                }

                if (existingBinding != null) {
                    binding.value = existingBinding.value;
                } else {
                    if (bindingIsType) {
                        // Force resolution to prevent returning null on ResolutionMode.RETURN_NULL.
                        var value = this.Resolve(binding.value as Type, InjectionMember.None, null, null, null, true);
                        binding.value = value;
                    } else {
                        this.Inject(binding.value);
                    }
                }
            }
        }
    }
}