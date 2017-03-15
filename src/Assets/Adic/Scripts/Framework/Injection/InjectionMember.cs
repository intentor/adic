using System;

namespace Adic.Injection {
    /// <summary>
    /// Class members in which injection can occur.
    /// </summary>
    public enum InjectionMember {
        None,
        Constructor,
        Method,
        Field,
        Property
    }
}