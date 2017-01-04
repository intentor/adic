namespace Adic {
    /// <summary>
    /// Allows an object that is binded on Adic to receive OnApplicationPause events.
    /// </summary>
    public interface IPausable {
        /// <summary>
        /// Called when the application is pausing.
        /// </summary>
        /// <param name="isPaused">If set to <c>true</c> is paused.</param>
        void OnApplicationPause(bool isPaused);
    }
}