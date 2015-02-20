using System;
using System.Diagnostics;
using Intentor.Adic;
using NUnit.Framework;

namespace Intentor.Adic.Tests {
	[TestFixture]
	public class SpeedTest {
		[Test]
		public void Test1MillionResolves() {
			var container = new InjectionContainer();
			
			container.Bind<IMockInterface>().To<MockIClass>();

			Stopwatch stopwatch = new Stopwatch();

			stopwatch.Start();
			object instance = null;
			for (int i = 0; i < 1000000; i++) {
				instance = container.Resolve<MockClassVerySimple>();
			}
			stopwatch.Stop();

			UnityEngine.Debug.Log(string.Format("1 million resolves in {0}s", stopwatch.Elapsed));
		}
	}
}