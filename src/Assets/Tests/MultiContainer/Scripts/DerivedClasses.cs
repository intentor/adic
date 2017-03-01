namespace Assets.Test.MultiContainer.Example {
    /// <summary>
    /// Main test interface.
    /// </summary>
    public interface ITestInterface {
        /// <summary>Some value. </summary>
        int value { get; }
    }

    /// <summary>
    /// Test class one.
    /// </summary>
    public class TestClassOne : ITestInterface {
        public int value { get { return 666; }}
    }

    /// <summary>
    /// Test class two.
    /// </summary>
    public class TestClassTwo : ITestInterface {
        public int value { get { return 2411; } }
    }
}