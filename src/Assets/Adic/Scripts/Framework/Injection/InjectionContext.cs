using System;

namespace Adic.Injection {
    /// <summary>
    /// Injection context.
    /// </summary>
    public class InjectionContext {
        /// <summary>The class member in which the injection is occuring.</summary>
        public InjectionMember member;
        /// <summary>The type of the member in which the injection is occuring.</summary>
        public Type memberType;
        /// <summary>The name of the member in which the injection is occuring.</summary>
        public string memberName;
        /// <summary>The identifier of the member in which the injection is occuring (from InjectAttribute).</summary>
        public object identifier;
        /// <summary>The type of the object in which the injection is occuring.</summary>
        public Type parentType;
        /// <summary>The instance of the object in which the injection is occuring.</summary>
        public object parentInstance;
        /// <summary>The type of the object being injected.</summary>
        public Type injectType;
    }
}