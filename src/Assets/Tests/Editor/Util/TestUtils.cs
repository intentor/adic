using System;
using System.Diagnostics;

namespace Adic.Tests.Util {
	/// <summary>
	/// Utility methods for tests.
	/// </summary>
	public static class TestUtils {
		/// <summary>
		/// Measures the performance of an action.
		/// </summary>
		/// <param name="action">Action to be measured.</param>
		/// <param name="message">Message to be displayed after the measurement.</param>
		public static void MeasurePerformance(Action action, string message) {
			Stopwatch watch = new Stopwatch();
			watch.Start();
			
			action();
			
			watch.Stop();
			UnityEngine.Debug.Log(string.Format(message, watch.ElapsedMilliseconds));
		}
	}
}