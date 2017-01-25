namespace Adic {
    /// <summary>
    /// Allows an object that is binded on Adic to receive LateUpdate events.
    /// </summary>
    public interface ILateUpdatable {
        /// <summary>
        /// Called once per frame after Update has finished.
        /// </summary>
        void LateUpdate();
    }
}