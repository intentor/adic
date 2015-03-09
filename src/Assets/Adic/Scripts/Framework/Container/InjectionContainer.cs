using System;
using System.Collections.Generic;
using System.Linq;
using Adic.Binding;
using Adic.Cache;
using Adic.Container;
using Adic.Injection;

namespace Adic {
	/// <summary>
	/// A dependency injector container.
	/// 
	/// It's a convenient class that act as binder and injector at the same time
	/// and allows the use of extensions to provide new functionalities.
	/// </summary>
	public class InjectionContainer : IInjectionContainer {
		/// <summary>Reflection cache used to get type info.</summary>
		public IReflectionCache cache { get; private set; }
		/// <summary>Internal binder.</summary>
		public IBinder binder { get; private set; }
		/// <summary>Internal injector.</summary>
		public IInjector injector { get; private set; }

		/// <summary>Registered container extensions.</summary>
		private List<IContainerExtension> extensions;

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
		/// </summary>
		/// <remarks>
		/// When passing no parameters to the constructor, default internal objects are created.
		/// </remarks>
		public InjectionContainer() {
			this.cache = new ReflectionCache();
			this.binder = new Binder();
			this.injector = new Injector(this.cache, this.binder);
			
			this.RegisterItself();
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
		/// </summary>
		/// <remarks>
		/// <remarks>
		/// Default binder and injector objects are created.
		/// </remarks>
		/// <param name="cache">Reflection cache used to get type info.</param>
		public InjectionContainer(IReflectionCache cache) {
			this.cache = cache;
			this.binder = new Binder();
			this.injector = new Injector(this.cache, this.binder);
			
			this.RegisterItself();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
		/// </summary>
		/// <remarks>
		/// <param name="cache">Reflection cache used to get type info.</param>
		/// <param name="binder">Binder to be used on the container.</param>
		/// <param name="injector">Injector to be used on the container.</param>
		public InjectionContainer(IReflectionCache cache, IBinder binder, IInjector injector) {
			this.cache = cache;
			this.binder = binder;
			this.injector = injector;

			this.RegisterItself();
		}

		/// <summary>
		/// Releases all resources used by the <see cref="Adic.InjectionContainer"/> object.
		/// </summary>
		public void Dispose() {
			this.cache = null;
			this.binder = null;
			this.injector = null;
		}

		
		/// <summary>
		/// Registers a container extension.
		/// </summary>
		/// <typeparam name="T">The type of the extension to be registered.</param>
		public void RegisterExtension<T>() where T : IContainerExtension {
			this.RegisterExtension(this.Resolve<T>());
		}

		/// <summary>
		/// Registers a container extension.
		/// </summary>
		/// <param name="extension">The extension to be registered.</param>
		public void RegisterExtension(IContainerExtension extension) {
			if (this.extensions == null) this.extensions = new List<IContainerExtension>();

			this.extensions.Add(extension);
			extension.OnRegister(this);
		}
		
		/// <summary>
		/// Unegisters a container extension.
		/// </summary>
		/// <typeparam name="T">The type of the extension(s) to be unregistered.</param>
		public void UnregisterExtension<T>() where T : IContainerExtension {
			var extensionsToUnregister = this.extensions.OfType<T>().ToList();
			
			foreach (var extension in extensionsToUnregister) {
				this.UnregisterExtension(extension);
			}
		}
		
		/// <summary>
		/// Unegisters a container extension.
		/// </summary>
		/// <param name="extension">The extension to be unregistered.</param>
		public void UnregisterExtension(IContainerExtension extension) {
			if (!this.extensions.Contains(extension)) return;
			
			this.extensions.Remove(extension);
			extension.OnUnregister(this);
		}

		/* Container */

		/// <summary>
		/// Registers the container on itself, so any object that want to receive
		/// a reference to it just reference <see cref="Adic.Container.IInjectionContainer"/>.
		/// </summary>
		protected void RegisterItself() {
			this.Bind<IInjectionContainer>().To<InjectionContainer>(this);
		}

		/* IBinder */
		
		public event BindingAddedHandler beforeAddBinding {
			add { this.binder.beforeAddBinding += value; }
			remove { this.binder.beforeAddBinding -= value; }
		}
		public event BindingAddedHandler afterAddBinding {
			add { this.binder.afterAddBinding += value; }
			remove { this.binder.afterAddBinding -= value; }
		}
		public event BindingRemovedHandler beforeRemoveBinding {
			add { this.binder.beforeRemoveBinding += value; }
			remove { this.binder.beforeRemoveBinding -= value; }
		}
		public event BindingRemovedHandler afterRemoveBinding {
			add { this.binder.afterRemoveBinding += value; }
			remove { this.binder.afterRemoveBinding -= value; }
		}

		public IBindingFactory Bind<T>() {
			return this.binder.Bind<T>();
		}
		
		public IBindingFactory Bind(Type type) {
			return this.binder.Bind(type);
		}

		public void AddBinding(BindingInfo binding){
			this.binder.AddBinding(binding);
		}

		public IList<BindingInfo> GetBindings(){
			return this.binder.GetBindings();
		}

		public IList<BindingInfo> GetBindingsFor<T>(){
			return this.binder.GetBindingsFor<T>();
		}

		public IList<BindingInfo> GetBindingsFor(Type type) {
			return this.binder.GetBindingsFor(type);
		}

		public bool ContainsBindingFor<T>() {
			return this.binder.ContainsBindingFor<T>();
		}

		public bool ContainsBindingFor(Type type) {
			return this.binder.ContainsBindingFor(type);
		}

		public void Unbind<T>() {
			this.binder.Unbind<T>();
		}
		
		public void Unbind(Type type) {
			this.binder.Unbind(type);
		}

		/* IInjector */
		
		public event TypeResolutionHandler beforeResolve {
			add { this.injector.beforeResolve += value; }
			remove { this.injector.beforeResolve -= value; }
		}
		public event TypeResolutionHandler afterResolve {
			add { this.injector.afterResolve += value; }
			remove { this.injector.afterResolve -= value; }
		}
		public event BindingEvaluationHandler bindingEvaluation {
			add { this.injector.bindingEvaluation += value; }
			remove { this.injector.bindingEvaluation -= value; }
		}
		public event BindingResolutionHandler bindingResolution {
			add { this.injector.bindingResolution += value; }
			remove { this.injector.bindingResolution -= value; }
		}
		public event InstanceInjectionHandler beforeInject {
			add { this.injector.beforeInject += value; }
			remove { this.injector.beforeInject -= value; }
		}
		public event InstanceInjectionHandler afterInject {
			add { this.injector.afterInject += value; }
			remove { this.injector.afterInject -= value; }
		}
		
		public T Resolve<T>() {
			return this.injector.Resolve<T>();
		}

		public object Resolve(Type type) {
			return this.injector.Resolve(type);
		}

		public T[] ResolveAll<T>() {
			return this.injector.ResolveAll<T>();
		}
		
		public object[] ResolveAll(Type type) {
			return this.injector.ResolveAll(type);
		}

		public T Inject<T>(T instance) where T : class {
			return this.injector.Inject<T>(instance);
		}

		public object Inject(object instance) {
			return this.injector.Inject(instance);
		}
	}
}