namespace Adic {
    /// <summary>
    /// Allows an object that is binded on Adic to receive LateUpdate events.
    /// </summary>
    public interface IFixedUpdatable {
        /// <summary>
        /// Called on a reliable time. Can be called more frequently than Update.
        /// </summary>
        void FixedUpdate();
    }
}