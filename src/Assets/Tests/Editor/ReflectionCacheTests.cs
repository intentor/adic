using System;
using Adic;
using Adic.Binding;
using Adic.Cache;
using NUnit.Framework;

namespace Adic.Tests {
	[TestFixture]
	public class ReflectionCacheTests {
		/// <summary>Binder used on tests.</summary>
		private IBinder binder;

		[SetUp]
		public void Init() {
			this.binder = new Binder();
			
			//Binds some objects to use on tests.
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>();
			binder.Bind(typeof(MockClassWithDependencies)).ToSelf();
		}

		[Test]
		public void TestAddType() {
			var cache = new ReflectionCache();
			var type = typeof(MockIClassWithoutAttributes);

			cache.Add(type);
			
			Assert.True(cache.Contains(type));
		}
		
		[Test]
		public void TestRemoveType() {
			var cache = new ReflectionCache();
			var type = typeof(MockIClassWithoutAttributes);
			
			cache.Add(type);
			cache.Remove(type);
			
			Assert.False(cache.Contains(type));
		}
		
		[Test]
		public void TestCacheAll() {
			var cache = new ReflectionCache();

			cache.CacheFromBinder(this.binder);
			
			Assert.True(cache.Contains(typeof(MockIClassWithAttributes)));
			Assert.True(cache.Contains(typeof(MockClassWithDependencies)));
		}
	}
}
