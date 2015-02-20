using System;
using Intentor.Adic;
using NUnit.Framework;

namespace Intentor.Adic.Tests {
	[TestFixture]
	public class BinderTests {

		[Test]
		public void TestGetAll() {
			var binder = new Binder();

			binder.Bind(typeof(IMockInterface)).To<MockIClassWithAttributes>();
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>().As("test");

			var bindings = binder.GetBindings();

			Assert.AreEqual(2, bindings.Count);
		}

		[Test]
		public void TestGetBindingsForGenerics() {
			var binder = new Binder();
			
			binder.Bind<MockClassToDepend>().ToSelf();
			binder.Bind(typeof(IMockInterface)).To<MockIClassWithAttributes>();
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>().As("test");
			
			var bindings = binder.GetBindingsFor<IMockInterface>();
			
			Assert.AreEqual(2, bindings.Count);
			Assert.AreEqual(typeof(IMockInterface), bindings[0].type);
			Assert.IsNull(bindings[0].identifier);
			Assert.AreEqual(typeof(IMockInterface), bindings[1].type);
			Assert.AreEqual("test", bindings[1].identifier);
		}

		[Test]
		public void TestGetBindingsForType() {
			var binder = new Binder();
			
			binder.Bind<MockClassToDepend>().ToSelf();
			binder.Bind(typeof(IMockInterface)).To<MockIClassWithAttributes>();
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>().As("test");
			
			var bindings = binder.GetBindingsFor(typeof(IMockInterface));
			
			Assert.AreEqual(2, bindings.Count);
			Assert.AreEqual(typeof(IMockInterface), bindings[0].type);
			Assert.IsNull(bindings[0].identifier);
			Assert.AreEqual(typeof(IMockInterface), bindings[1].type);
			Assert.AreEqual("test", bindings[1].identifier);
		}
		
		[Test]
		public void TestUnbindByGenerics() {
			var binder = new Binder();
			
			binder.Bind<MockClassToDepend>().ToSelf();
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>();
			binder.Unbind<IMockInterface>();
			
			var bindings = binder.GetBindings();
			
			Assert.AreEqual(1, bindings.Count);
		}
		
		[Test]
		public void TestUnbindByType() {
			var binder = new Binder();
			
			binder.Bind<MockClassToDepend>().ToSelf();
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>();
			binder.Unbind(typeof(IMockInterface));
			
			var bindings = binder.GetBindings();
			
			Assert.AreEqual(1, bindings.Count);
		}
	}
}