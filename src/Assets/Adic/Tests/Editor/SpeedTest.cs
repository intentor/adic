using System;
using System.Diagnostics;
using Adic;
using NUnit.Framework;

namespace Adic.Tests {
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
				instance = this.container.Resolve<IMockInterface>();
			}
			
			stopwatch.Stop();
			
			UnityEngine.Debug.Log(string.Format("1 thousand simple resolves in {0}s", stopwatch.Elapsed));
		}

		#pragma warning disable 0219
		[Test]
		public void Test1MillionSimpleResolves() {
			var stopwatch = new Stopwatch();

			stopwatch.Start();

			object instance = null;
			for (int i = 0; i < 1000000; i++) {
				instance = this.container.Resolve<IMockInterface>();
			}

			stopwatch.Stop();

			UnityEngine.Debug.Log(string.Format("1 million simple resolves in {0}s", stopwatch.Elapsed));
		}

		#pragma warning disable 0219
		[Test]
		public void Test1ThousandMoreComplexResolves() {
			var stopwatch = new Stopwatch();
			
			stopwatch.Start();
			
			object instance = null;
			for (int i = 0; i < 1000; i++) {
				instance = this.container.Resolve<MockClassVerySimple>();
			}
			
			stopwatch.Stop();
			
			UnityEngine.Debug.Log(string.Format("1 thousand more complex resolves in {0}s", stopwatch.Elapsed));
		}
		
		#pragma warning disable 0219
		[Test]
		public void Test1MillionMoreComplexResolves() {
			var stopwatch = new Stopwatch();
			
			stopwatch.Start();
			
			object instance = null;
			for (int i = 0; i < 1000000; i++) {
				instance = this.container.Resolve<MockClassVerySimple>();
			}
			
			stopwatch.Stop();
			
			UnityEngine.Debug.Log(string.Format("1 million more complex resolves in {0}s", stopwatch.Elapsed));
		}
	}
}