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
    public class InjectionContainer : Injector, IInjectionContainer {
        /// <summary>Default instance resolution mode.</summary>
        protected const ResolutionMode DEFAULT_RESOLUTION_MODE = ResolutionMode.ALWAYS_RESOLVE;

        /// <summary>Container identifier.</summary>
        public object identifier { get; private set; }

        /// <summary>Indicates whether the container has been initialized.</summary>
        private bool isInitialized;
        /// <summary>Registered container extensions.</summary>
        private List<IContainerExtension> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
        /// </summary>
        /// <remarks>
        /// When passing no parameters to the constructor, default internal objects are created.
        /// </remarks>
        public InjectionContainer()
            : this(GenerateIdentifier(), new ReflectionCache(), new Binder(), DEFAULT_RESOLUTION_MODE) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
        /// </summary>
        /// <remarks>
        /// When passing no parameters to the constructor, default internal objects are created.
        /// </remarks>
        /// <param name="identifier">Container identifier.</param>
        public InjectionContainer(object identifier)
            : this(identifier, new ReflectionCache(), new Binder(), DEFAULT_RESOLUTION_MODE) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
        /// </summary>
        /// <remarks>
        /// <remarks>
        /// Default binder and injector objects are created.
        /// </remarks>
        /// <param name="cache">Reflection cache used to get type info.</param>
        public InjectionContainer(IReflectionCache cache)
            : this(GenerateIdentifier(), cache, new Binder(), DEFAULT_RESOLUTION_MODE) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
        /// </summary>
        /// <remarks>
        /// <param name="resolutionMode">Instance resolution mode.</param>
        public InjectionContainer(ResolutionMode resolutionMode)
            : this(GenerateIdentifier(), new ReflectionCache(), new Binder(), resolutionMode) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
        /// </summary>
        /// <remarks>
        /// <param name="identifier">Container identifier.</param>
        /// <param name="resolutionMode">Instance resolution mode.</param>
        public InjectionContainer(object identifier, ResolutionMode resolutionMode)
            : this(identifier, new ReflectionCache(), new Binder(), resolutionMode) {
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
        public InjectionContainer(object identifier, IReflectionCache cache)
            : this(identifier, cache, new Binder(), DEFAULT_RESOLUTION_MODE) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
        /// </summary>
        /// <remarks>
        /// <remarks>
        /// Default binder and injector objects are created.
        /// </remarks>
        /// <param name="cache">Reflection cache used to get type info.</param>
        /// <param name="resolutionMode">Instance resolution mode.</param>
        public InjectionContainer(IReflectionCache cache, ResolutionMode resolutionMode)
            : this(GenerateIdentifier(), cache, new Binder(), resolutionMode) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
        /// </summary>
        /// <remarks>
        /// <param name="cache">Reflection cache used to get type info.</param>
        /// <param name="binder">Binder to be used on the container.</param>
        public InjectionContainer(IReflectionCache cache, IBinder binder)
            : this(GenerateIdentifier(), cache, binder, DEFAULT_RESOLUTION_MODE) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
        /// </summary>
        /// <remarks>
        /// <param name="identifier">Container identifier.</param>
        /// <param name="cache">Reflection cache used to get type info.</param>
        /// <param name="binder">Binder to be used on the container.</param>
        public InjectionContainer(object identifier, IReflectionCache cache, IBinder binder)
            : this(identifier, cache, binder, DEFAULT_RESOLUTION_MODE) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
        /// </summary>
        /// <remarks>
        /// <param name="identifier">Container identifier.</param>
        /// <param name="cache">Reflection cache used to get type info.</param>
        /// <param name="resolutionMode">Instance resolution mode.</param>
        public InjectionContainer(object identifier, IReflectionCache cache, ResolutionMode resolutionMode)
            : this(identifier, cache, new Binder(), resolutionMode) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.InjectionContainer"/> class.
        /// </summary>
        /// <remarks>
        /// <param name="identifier">Container identifier.</param>
        /// <param name="cache">Reflection cache used to get type info.</param>
        /// <param name="binder">Binder to be used on the container.</param>
        /// <param name="resolutionMode">Instance resolution mode.</param>
        public InjectionContainer(object identifier, IReflectionCache cache, IBinder binder, 
                                  ResolutionMode resolutionMode) : base(cache, binder, resolutionMode) {	
            this.identifier = identifier;
            this.RegisterItself();
        }

        public IInjectionContainer Init() {
            if (this.isInitialized) {
                return this;
            }

            this.cache.CacheFromBinder(this);

            if (extensions != null) {
                foreach (var extension in extensions) {
                    extension.Init(this);
                }
            }

            this.isInitialized = true;

            return this;
        }

        public void Dispose() {
            if (extensions != null) {
                foreach (var extension in extensions) {
                    extension.OnUnregister(this);
                }
                this.extensions.Clear();
                this.extensions = null;
            }

            this.cache = null;
            this.binder = null;
        }

        public IInjectionContainer RegisterExtension<T>() where T : IContainerExtension {
            if (this.extensions == null) {
                this.extensions = new List<IContainerExtension>();
            }

            IContainerExtension extension = this.Resolve<T>();

            // Only adds the extension if it's not already added.
            if (!this.HasExtension(extension.GetType())) {
                this.extensions.Add(extension);
                extension.OnRegister(this);
            }

            return this;
        }

        public IInjectionContainer UnregisterExtension<T>() where T : IContainerExtension {
            var extensionsToUnregister = this.extensions.OfType<T>().ToList();
			
            foreach (var extension in extensionsToUnregister) {
                this.extensions.Remove(extension);
                extension.OnUnregister(this);
            }
			
            return this;
        }

        public T GetExtension<T>() where T : IContainerExtension {
            return (T) GetExtension(typeof(T));
        }

        public IContainerExtension GetExtension(Type type) {
            IContainerExtension found = null;

            if (extensions != null) {
                foreach (var extension in extensions) {
                    if (extension.GetType().Equals(type)) {
                        found = extension;
                        break;
                    }
                }
            }

            return found;
        }

        public bool HasExtension<T>() {
            return HasExtension(typeof(T));
        }

        public bool HasExtension(Type type) {
            return GetExtension(type) != null;
        }

        public void Clear() {
            var bindings = this.binder.GetBindings();

            foreach (var binding in bindings) {
                this.binder.Unbind(binding.type);
            }
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

        public void AddBinding(BindingInfo binding) {
            this.binder.AddBinding(binding);
        }

        public IList<BindingInfo> GetBindings() {
            return this.binder.GetBindings();
        }

        public IList<BindingInfo> GetBindingsFor<T>() {
            return this.binder.GetBindingsFor<T>();
        }

        public IList<BindingInfo> GetBindingsFor(Type type) {
            return this.binder.GetBindingsFor(type);
        }

        public IList<BindingInfo> GetBindingsFor(object identifier) {
            return this.binder.GetBindingsFor(identifier);
        }

        public IList<BindingInfo> GetBindingsTo<T>() {
            return this.binder.GetBindingsTo<T>();
        }

        public IList<BindingInfo> GetBindingsTo(Type type) {
            return this.binder.GetBindingsTo(type);
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

        public void UnbindInstance(object instance) {
            this.binder.UnbindInstance(instance);
        }

        public void UnbindByTag(string tag) {
            this.binder.UnbindByTag(tag);
        }

        /// <summary>
        /// Generates a container identifier.
        /// </summary>
        /// <returns>The container identifier.</returns>
        private static string GenerateIdentifier() {
            return Guid.NewGuid().ToString();
        }
    }
}