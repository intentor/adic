using System;
using Adic;
using Adic.Binding;
using NUnit.Framework;

namespace Adic.Tests {
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
		public void TestGetBindingsForIdentifier() {
			var binder = new Binder();
			
			binder.Bind<MockClassToDepend>().ToSelf();
			binder.Bind(typeof(MockIClassWithAttributes)).ToSelf();
			binder.Bind<IMockInterface>().To<MockIClass>();
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>().As("Identifier");
			binder.Bind<IMockInterface>().To<MockIClassWithoutAttributes>().As("Identifier");
			
			var bindings = binder.GetBindingsFor("Identifier");
			
			Assert.AreEqual(2, bindings.Count);
			Assert.AreEqual(typeof(IMockInterface), bindings[0].type);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindings[0].value);
			Assert.AreEqual("Identifier", bindings[0].identifier);
			Assert.AreEqual(typeof(IMockInterface), bindings[1].type);
			Assert.AreEqual(typeof(MockIClassWithoutAttributes), bindings[1].value);
			Assert.AreEqual("Identifier", bindings[1].identifier);
		}
		
		[Test]
		public void TestContainsByGenerics() {
			var binder = new Binder();
			
			binder.Bind<MockClassToDepend>().ToSelf();
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>();

			var contains = binder.ContainsBindingFor<IMockInterface>();
			
			Assert.AreEqual(true, contains);
		}
		
		[Test]
		public void TestContainsByType() {
			var binder = new Binder();
			
			binder.Bind<MockClassToDepend>().ToSelf();
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>();

			var contains = binder.ContainsBindingFor(typeof(IMockInterface));
			
			Assert.AreEqual(true, contains);
		}
		
		[Test]
		public void TestContainsByIdentifier() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().To<MockIClass>();
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>().As("Identifier");
			
			var contains = binder.ContainsBindingFor("Identifier");
			
			Assert.AreEqual(true, contains);
		}
		
		[Test]
		public void TestUnbindByGenerics() {
			var binder = new Binder();
			
			binder.Bind<MockClassToDepend>().ToSelf();
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>();
			binder.Unbind<IMockInterface>();
			
			var bindings = binder.GetBindings();
			
			Assert.AreEqual(1, bindings.Count);
			Assert.AreEqual(typeof(MockClassToDepend), bindings[0].type);
		}
		
		[Test]
		public void TestUnbindByType() {
			var binder = new Binder();
			
			binder.Bind<MockClassToDepend>().ToSelf();
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>();
			binder.Unbind(typeof(IMockInterface));
			
			var bindings = binder.GetBindings();

			Assert.AreEqual(1, bindings.Count);
			Assert.AreEqual(typeof(MockClassToDepend), bindings[0].type);
		}
		
		[Test]
		public void TestUnbindByIdentifier() {
			var binder = new Binder();
			
			binder.Bind<MockClassToDepend>().ToSelf();
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>().As("Mock1");
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>().As("Mock2");
			binder.Unbind("Mock1");
			
			var bindings = binder.GetBindings();
			
			Assert.AreEqual(2, bindings.Count);
			Assert.AreEqual(typeof(MockClassToDepend), bindings[0].type);
			Assert.AreEqual(typeof(IMockInterface), bindings[1].type);
			Assert.AreEqual("Mock2", bindings[1].identifier);
		}

		[Test]
		public void TestUnbindSingleton() {
			var binder = new Binder();

			binder.Bind<MockIClassWithAttributes>().ToSingleton();

			binder.Unbind<MockIClassWithAttributes>();

			var bindings = binder.GetBindings();

			Assert.AreEqual(0, bindings.Count);
		}

		[Test]
		public void TestUnbindSingletonFromInterface() {
			var binder = new Binder();

			binder.Bind<IMockInterface>().ToSingleton<MockIClassWithAttributes>();

			binder.Unbind<MockIClassWithAttributes>();

			var bindings = binder.GetBindings();

			Assert.AreEqual(0, bindings.Count);
		}
	}
}