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
	public class InjectionContainer : Injector, IInjectionContainer  {
		/// <summary>Container identifier.</summary>
		public object identifier { get; private set; }

		/// <summary>Registered container extensions.</summary>
		private List<IContainerExtension> extensions;

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
		/// </summary>
		/// <remarks>
		/// When passing no parameters to the constructor, default internal objects are created.
		/// </remarks>
		public InjectionContainer() : base(new ReflectionCache(), new Binder()) {
			this.RegisterItself();
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
		/// </summary>
		/// <remarks>
		/// When passing no parameters to the constructor, default internal objects are created.
		/// </remarks>
		/// <param name="identifier">Container identifier.</param>
		public InjectionContainer(object identifier) : base(new ReflectionCache(), new Binder()) {
			this.identifier = identifier;
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
		public InjectionContainer(IReflectionCache cache) : base(cache, new Binder()) {			
			this.RegisterItself();
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
		/// </summary>
		/// <remarks>
		/// <remarks>
		/// Default binder and injector objects are created.
		/// </remarks>
		/// <param name="identifier">Container identifier.</param>
		/// <param name="cache">Reflection cache used to get type info.</param>
		public InjectionContainer(object identifier, IReflectionCache cache) : base(cache, new Binder()) {	
			this.identifier = identifier;
			this.RegisterItself();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
		/// </summary>
		/// <remarks>
		/// <param name="cache">Reflection cache used to get type info.</param>
		/// <param name="binder">Binder to be used on the container.</param>
		public InjectionContainer(IReflectionCache cache, IBinder binder) : base(cache, binder) {
			this.RegisterItself();
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
		/// </summary>
		/// <remarks>
		/// <param name="identifier">Container identifier.</param>
		/// <param name="cache">Reflection cache used to get type info.</param>
		/// <param name="binder">Binder to be used on the container.</param>
		public InjectionContainer(object identifier, IReflectionCache cache, IBinder binder) : base(cache, binder) {	
			this.identifier = identifier;
			this.RegisterItself();
		}

		/// <summary>
		/// Releases all resources used by the <see cref="Adic.InjectionContainer"/> object.
		/// </summary>
		public void Dispose() {
			this.cache = null;
			this.binder = null;
		}
				
		/// <summary>
		/// Registers a container extension.
		/// </summary>
		/// <typeparam name="T">The type of the extension to be registered.</param>
		/// <returns>The injection container for chaining.</returns>
		public IInjectionContainer RegisterExtension<T>() where T : IContainerExtension {
			this.RegisterExtension(this.Resolve<T>());

			return this;
		}

		/// <summary>
		/// Registers a container extension.
		/// </summary>
		/// <param name="extension">The extension to be registered.</param>
		/// <returns>The injection container for chaining.</returns>
		public IInjectionContainer RegisterExtension(IContainerExtension extension) {
			if (this.extensions == null) this.extensions = new List<IContainerExtension>();

			this.extensions.Add(extension);
			extension.OnRegister(this);
			
			return this;
		}
		
		/// <summary>
		/// Unegisters a container extension.
		/// </summary>
		/// <typeparam name="T">The type of the extension(s) to be unregistered.</param>
		/// <returns>The injection container for chaining.</returns>
		public IInjectionContainer UnregisterExtension<T>() where T : IContainerExtension {
			var extensionsToUnregister = this.extensions.OfType<T>().ToList();
			
			foreach (var extension in extensionsToUnregister) {
				this.UnregisterExtension(extension);
			}
			
			return this;
		}
		
		/// <summary>
		/// Unegisters a container extension.
		/// </summary>
		/// <param name="extension">The extension to be unregistered.</param>
		/// <returns>The injection container for chaining.</returns>
		public IInjectionContainer UnregisterExtension(IContainerExtension extension) {
			if (!this.extensions.Contains(extension)) return this;
			
			this.extensions.Remove(extension);
			extension.OnUnregister(this);
			
			return this;
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
		
		public IList<BindingInfo> GetBindingsFor(object identifier) {
			return this.binder.GetBindingsFor(identifier);
		}

		public bool ContainsBindingFor<T>() {
			return this.binder.ContainsBindingFor<T>();
		}

		public bool ContainsBindingFor(Type type) {
			return this.binder.ContainsBindingFor(type);
		}
		
		public bool ContainsBindingFor(object identifier) {
			return this.binder.ContainsBindingFor(identifier);
		}

		public void Unbind<T>() {
			this.binder.Unbind<T>();
		}
		
		public void Unbind(Type type) {
			this.binder.Unbind(type);
		}

		public void Unbind(object identifier) {
			this.binder.Unbind(identifier);
		}
	}
}