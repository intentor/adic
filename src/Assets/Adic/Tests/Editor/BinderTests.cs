using System;
using Adic;
using NUnit.Framework;

namespace Adic.Tests {
	[TestFixture]
	public class BinderTests {
		[Test]
		public void TestGetAll() {
			var binder = new Binder();

			binder.Bind<IMockInterface>().To<MockClassWithAtrributes>();
			binder.Bind("test").To<MockClassWithAtrributes>();

			var bindings = binder.GetBindings();

			Assert.AreEqual(2, bindings.Length);
		}
		
		[Test]
		public void TestUnbindByType() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().To<MockClassWithAtrributes>();
			binder.Unbind<IMockInterface>();
			
			var bindings = binder.GetBindings();
			
			Assert.AreEqual(0, bindings.Length);
		}
		
		[Test]
		public void TestUnbindByName() {
			var binder = new Binder();
			
			binder.Bind("test").To<MockClassWithAtrributes>();
			binder.Unbind("test");
			
			var bindings = binder.GetBindings();
			
			Assert.AreEqual(0, bindings.Length);
		}
	}
}