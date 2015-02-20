using System;
using System.Diagnostics;
using Intentor.Adic;
using NUnit.Framework;

namespace Intentor.Adic.Tests {
	/// <summary>
	/// These tests are just to analyse Adic's performance and present
	/// no real unit testing to the component.
	/// </summary>
	[TestFixture]
	public class SpeedTest {
		/// <summary>Container used on tests.</summary>
		private InjectionContainer container;
		
		[SetUp]
		public void Init() {
			this.container = new InjectionContainer();
			this.container.Bind<IMockInterface>().To<MockIClass>();
			this.container.cache.CacheFromBinder(this.container);
		}

		#pragma warning disable 0219
		[Test]
		public void Test1ThousandSimpleResolves() {
			var stopwatch = new Stopwatch();
			
			stopwatch.Start();
			
			object instance = null;
			for (int i = 0; i < 1000; i++) {
				instance = this.container.Resolve<MockClassVerySimple>();
			}
			
			stopwatch.Stop();
			
			UnityEngine.Debug.Log(string.Format("1 thousand resolves in {0}s", stopwatch.Elapsed));
		}

		#pragma warning disable 0219
		[Test]
		public void Test1MillionSimpleResolves() {
			var stopwatch = new Stopwatch();

			stopwatch.Start();

			object instance = null;
			for (int i = 0; i < 1000000; i++) {
				instance = this.container.Resolve<MockClassVerySimple>();
			}

			stopwatch.Stop();

			UnityEngine.Debug.Log(string.Format("1 million resolves in {0}s", stopwatch.Elapsed));
		}
	}
}