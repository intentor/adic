namespace Adic {
    /// <summary>
    /// Allows an object that is binded on Adic to receive OnApplicationQuit events.
    /// </summary>
    public interface IQuitable {
        /// <summary>
        /// Called when the application is quitting.
        /// </summary>
        void OnApplicationQuit();
    }
}