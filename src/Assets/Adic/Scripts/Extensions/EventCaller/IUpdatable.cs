namespace Adic {
    /// <summary>
    /// Allows an object that is binded on Adic to receive Update events.
    /// </summary>
    public interface IUpdatable {
        /// <summary>
        /// Called once per frame.
        /// </summary>
        void Update();
    }
}