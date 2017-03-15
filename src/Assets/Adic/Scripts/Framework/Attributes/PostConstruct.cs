using System;

namespace Adic {
    /// <summary>
    /// Marks a method to be called immediately after injection.
    /// </summary>
    /// <remarks>
    /// Can be used as a constructor, but with the certainty that all the dependencies have been injected.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method,
        AllowMultiple = false,
        Inherited = true)]
    [Obsolete("As of version 2.20, please use Inject attribute instead.")]
    public class PostConstruct : Attribute {

    }
}