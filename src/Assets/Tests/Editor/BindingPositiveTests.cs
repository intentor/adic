using System;
using Adic;
using Adic.Binding;
using NUnit.Framework;

namespace Adic.Tests {
	[TestFixture]
	public class BindingPositiveTests  {
		[Test]
		public void TestBindingToSelf() {
			var binder = new Binder();
			
			binder.Bind<MockIClassWithAttributes>().ToSelf();
			var bindings = binder.GetBindingsFor<MockIClassWithAttributes>();
			
			Assert.AreEqual(1, bindings.Count);
			Assert.AreEqual(BindingInstance.Transient, bindings[0].instanceType);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindings[0].type);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindings[0].value);
		}

		[Test]
		public void TestBindingToSingleton() {
			var binder = new Binder();

			binder.Bind<MockIClassWithAttributes>().ToSingleton();
			var bindings = binder.GetBindingsFor<MockIClassWithAttributes>();
			
			Assert.AreEqual(1, bindings.Count);
			Assert.AreEqual(BindingInstance.Singleton, bindings[0].instanceType);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindings[0].type);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindings[0].value);
		}

		[Test]
		public void TestBindingToSingletonFromInterface() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().ToSingleton<MockIClassWithAttributes>();
			var bindings = binder.GetBindingsFor<IMockInterface>();
			
			Assert.AreEqual(1, bindings.Count);
			Assert.AreEqual(BindingInstance.Singleton, bindings[0].instanceType);
			Assert.AreEqual(typeof(IMockInterface), bindings[0].type);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindings[0].value);
		}
		
		[Test]
		public void TestBindingToTransientFromInterface() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>();
			var bindings = binder.GetBindingsFor<IMockInterface>();
			
			Assert.AreEqual(1, bindings.Count);
			Assert.AreEqual(BindingInstance.Transient, bindings[0].instanceType);
			Assert.AreEqual(typeof(IMockInterface), bindings[0].type);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindings[0].value);
		}
		
		[Test]
		public void TestBindingToInstanceFromInterface() {
			var binder = new Binder();

			var instance = new MockIClassWithAttributes();
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>(instance);
			var bindings = binder.GetBindingsFor<IMockInterface>();
			
			Assert.AreEqual(1, bindings.Count);
			Assert.AreEqual(BindingInstance.Singleton, bindings[0].instanceType);
			Assert.AreEqual(typeof(IMockInterface), bindings[0].type);
			Assert.AreEqual(instance, bindings[0].value);
		}
		
		[Test]
		public void TestBindingToFactoryInstanceFromInterface() {
			var binder = new Binder();
			
			var factory = new MockFactory();
			binder.Bind<IMockInterface>().ToFactory(factory);
			var bindings = binder.GetBindingsFor<IMockInterface>();
			
			Assert.AreEqual(1, bindings.Count);
			Assert.AreEqual(BindingInstance.Factory, bindings[0].instanceType);
			Assert.AreEqual(typeof(IMockInterface), bindings[0].type);
			Assert.AreEqual(factory, bindings[0].value);
		}
	}
}