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
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.InjectorBinder"/> class.
		/// </summary>
		/// <param name="cache">Reflection cache used to get type info.</param>
		/// <param name="binder">Binder used to resolved bindings.</param>
		public Injector(IReflectionCache cache, IBinder binder) {
			this.cache = cache;
			this.binder = binder;

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
			return (T)this.Resolve(typeof(T), InjectionMember.None, null, null);
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
			return (T)this.Resolve(typeof(T), InjectionMember.None, null, identifier);
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
			return this.Resolve(type, InjectionMember.None, null, null);
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
			//Given no type will be passed, it'll always resolve an array.
			var instances = (object[])this.Resolve(null, InjectionMember.None, null, identifier);

			if (instances != null && instances.Length > 0) {
				return instances[0];
			} else {
				return instances;
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
			return this.Resolve(type, InjectionMember.None, null, identifier);
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
			var instance = this.Resolve(typeof(T), identifier);
			
			if (instance == null) {
				return null;
			} else if (!instance.GetType().IsArray) {
				var array = Array.CreateInstance(instance.GetType(), 1);
				array.SetValue(instance, 0);
				return (T[])array;
			} else {
				return (T[])instance;
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
				return (object[])array;
			} else {
				return (object[])instance;
			}
		}

		/// <summary>
		/// Resolves an instance for a specified type at a certain member in an instance with a given identifier.
		/// </summary>
		/// <param name="type">Binding type.</param>
		/// <param name="member">Member for which the binding is being resolved.</param>
		/// <param name="parentInstance">Parent object in which the resolve is occuring.</param>
		/// <param name="identifier">The binding identifier to be looked for.</param>
		protected object Resolve(Type type, InjectionMember member, object parentInstance, object identifier) {
			object resolution = null;

			if (this.beforeResolve != null) {
				var delegates = this.beforeResolve.GetInvocationList();
				for (int delegateIndex = 0; delegateIndex < delegates.Length; delegateIndex++) {
					var continueExecution = ((TypeResolutionHandler)delegates[delegateIndex]).Invoke(this, 
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

			//Array is used for multiple injection.
			//So, when the type is an array, the type to be read from the bindings list is the element type.
			Type typeToGet;
			IList<BindingInfo> bindings;
			if (type == null) {
				typeToGet = typeof(object);

				//If no type is provided, look for bindings by identifier.
				bindings = this.binder.GetBindingsFor(identifier);
			} else {
				if (type.IsArray) {
					typeToGet = type.GetElementType();
				} else {
					typeToGet = type;
				}
			
				//If a type is provided, look for bindings by identifier.
				bindings = this.binder.GetBindingsFor(typeToGet);
			}

			IList<object> instances = new List<object>();

			if (bindings == null) {
				instances.Add(this.Instantiate(type as Type));
			} else {
				for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++) {
					var binding = bindings[bindingIndex];
					
					var instance = this.ResolveBinding(binding, type, member, parentInstance, identifier);

					if (instance != null) {
						instances.Add(instance);
					}
				}
			}
			
			if (type != null && !type.IsArray && instances.Count == 1) {
				resolution = instances[0];
			} else if (instances.Count > 0) {
				var array = Array.CreateInstance(typeToGet, instances.Count);
				for (int listIndex = 0; listIndex < instances.Count; listIndex++) {
					array.SetValue(instances[listIndex], listIndex);
				}
				resolution = array;
			}

			if (this.afterResolve != null) {
				var delegates = this.afterResolve.GetInvocationList();
				for (int delegateIndex = 0; delegateIndex < delegates.Length; delegateIndex++) {
					var continueExecution = ((TypeResolutionHandler)delegates[delegateIndex]).Invoke(this, 
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
			return (T)this.Inject(instance, reflectedClass);
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
				for (int fieldIndex = 0; fieldIndex < reflectedClass.fields.Length; fieldIndex++) {
					var field = reflectedClass.fields[fieldIndex];
					var valueToSet = this.Resolve(field.type, InjectionMember.Field, instance, field.identifier);
					field.setter(instance, valueToSet);
				}
			}

			if (reflectedClass.properties.Length > 0) {
				for (int propertyIndex = 0; propertyIndex < reflectedClass.properties.Length; propertyIndex++) {
					var property = reflectedClass.properties[propertyIndex];
					var valueToSet = this.Resolve(property.type, InjectionMember.Property, instance, property.identifier);
					property.setter(instance, valueToSet);
				}
			}
			
			//Call post constructors, if there's any.
			if (reflectedClass.postConstructors.Length > 0) {
				for (int constIndex = 0; constIndex < reflectedClass.postConstructors.Length; constIndex++) {
					var method = reflectedClass.postConstructors[constIndex];
					method(instance);
				}
			}

			if (this.afterInject != null) {
				this.afterInject(this, ref instance, reflectedClass);
			}
			
			return instance;
		}

		/// <summary>
		/// Resolves the binding.
		/// </summary>
		/// <param name="binding">Binding to be resolved.</param>
		/// <param name="type">Binding type.</param>
		/// <param name="member">Member for which the binding is being resolved.</param>
		/// <param name="parentInstance">Parent object in which the resolve is occuring.</param>
		/// <param name="identifier">The binding identifier to be looked for.</param>
		/// <returns>The resolved instance from the binding.</returns>
		protected object ResolveBinding(BindingInfo binding, 
			Type type,
			InjectionMember member,
			object parentInstance,
        	object identifier) {
			//Condition evaluation.
			if (binding.condition != null) {
				var context = new InjectionContext() {
					member = member,
					memberType = type,
					identifier = identifier,
					parentType = (parentInstance != null ? parentInstance.GetType() : null),
					parentInstance = parentInstance,
					injectType = binding.type
				};
				
				if (!binding.condition(context)) {
					return null;
				}
			}

			//Identifier evaluation.
			bool resolveByIdentifier = identifier != null;
			bool bindingHasIdentifier = binding.identifier != null;
			if ((!resolveByIdentifier && bindingHasIdentifier) ||
			    (resolveByIdentifier && !bindingHasIdentifier) ||
			    (resolveByIdentifier && bindingHasIdentifier && !binding.identifier.Equals(identifier))) {
				return null;
			}

			//Instance evaluation.
			object instance = null;

			if (this.bindingEvaluation != null) {
				var delegates = this.bindingEvaluation.GetInvocationList();
				for (int delegateIndex = 0; delegateIndex < delegates.Length; delegateIndex++) {
					instance = ((BindingEvaluationHandler)delegates[delegateIndex]).Invoke(this, ref binding);
				}
			}

			if (instance == null) {
				if (binding.instanceType == BindingInstance.Transient) {
					instance = this.Instantiate(binding.value as Type);
				} else if (binding.instanceType == BindingInstance.Factory) {
					var context = new InjectionContext() {
						member = member,
						memberType = type,
						identifier = identifier,
						parentType = (parentInstance != null ? parentInstance.GetType() : null),
						parentInstance = parentInstance,
						injectType = binding.type
					};

					instance = (binding.value as IFactory).Create(context);
				} else {
					//Binding is a singleton object.
					
					//If the binding value is a type, instantiate it.
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
		/// Instantiate and resolve the dependencies for the specified type.
		/// </summary>
		/// <param name="type">The type to be instantiated.</param>
		protected object Instantiate(Type type) {
			var reflectedClass = this.cache.GetClass(type);
			object instance = null;

			if (reflectedClass.constructor == null && reflectedClass.paramsConstructor == null) {
				throw new InjectorException(string.Format(InjectorException.NO_CONSTRUCTORS, type.ToString()));
			}

			if (reflectedClass.constructorParameters.Length == 0) {
				instance = reflectedClass.constructor();
			} else {
				object[] parameters = new object[reflectedClass.constructorParameters.Length];

				for (int paramIndex = 0; paramIndex < parameters.Length; paramIndex++) {
					var parameterInfo = reflectedClass.constructorParameters[paramIndex];
					
					parameters[paramIndex] = this.Resolve(
						parameterInfo.type,
						InjectionMember.Constructor,
						instance,
						parameterInfo.identifier
             		);
				}

				instance = reflectedClass.paramsConstructor(parameters);
			}

			instance = this.Inject(instance, reflectedClass);

			return instance;
		}
		
		/// <summary>
		/// Handles the before add binding event.
		/// </summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="binding">The binding to be added, by reference.</param>
		protected void OnBeforeAddBinding(IBinder source, ref BindingInfo binding) {
			if (binding.instanceType == BindingInstance.Singleton ||
			    binding.instanceType == BindingInstance.Factory) {
				//Check whether a binding for the same type already exists.
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
						bindingFromBinder.value.GetType().Equals(binding.value);
					var valueInstanceIsTheSame = 
						isSingleton && 
						!bindingIsType && 
						bindingFromBinder.value == binding.value;

					if (valueTypeIsTheSame || valueInstanceIsTheSame) {
						existingBinding = bindingFromBinder;
						//Breaks because if one is found, any other that already exists
						//will already be the same instance.
						break;
					}
				}

				if (existingBinding != null) {
					binding.value = existingBinding.value;
				} else {
					if (bindingIsType) {
						var value = this.Resolve(binding.value as Type);
						binding.value = value;
					} else {
						this.Inject(binding.value);
					}
				}
			}
		}
	}
}