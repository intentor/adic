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

			var bindingsInterface = binder.GetBindingsFor<IMockInterface>();
			Assert.AreEqual(1, bindingsInterface.Count);
			Assert.AreEqual(BindingInstance.Singleton, bindingsInterface[0].instanceType);
			Assert.AreEqual(typeof(IMockInterface), bindingsInterface[0].type);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindingsInterface[0].value);

			var bindingsSingleton = binder.GetBindingsFor<MockIClassWithAttributes>();
			Assert.AreEqual(1, bindingsSingleton.Count);
			Assert.AreEqual(BindingInstance.Singleton, bindingsSingleton[0].instanceType);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindingsSingleton[0].type);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindingsSingleton[0].value);

			Assert.AreEqual(bindingsInterface[0].value, bindingsSingleton[0].value);
		}

		[Test]
		public void TestBindingInterfacesToSameSingleton() {
			var binder = new Binder();

			binder.Bind<IMockInterface1>().ToSingleton<MockClassManyInterfaces>();
			binder.Bind<IMockInterface2>().ToSingleton<MockClassManyInterfaces>();
			binder.Bind<IMockInterface3>().ToSingleton<MockClassManyInterfaces>();

			var bindingsFor = binder.GetBindingsFor<MockClassManyInterfaces>();
			Assert.AreEqual(1, bindingsFor.Count);
			Assert.AreEqual(BindingInstance.Singleton, bindingsFor[0].instanceType);
			Assert.AreEqual(typeof(MockClassManyInterfaces), bindingsFor[0].value);

			var bindingsTo = binder.GetBindingsTo<MockClassManyInterfaces>();
			Assert.AreEqual(4, bindingsTo.Count);
			Assert.AreEqual(BindingInstance.Singleton, bindingsTo[0].instanceType);
			Assert.AreEqual(BindingInstance.Singleton, bindingsTo[1].instanceType);
			Assert.AreEqual(BindingInstance.Singleton, bindingsTo[2].instanceType);
			Assert.AreEqual(BindingInstance.Singleton, bindingsTo[3].instanceType);
			Assert.AreEqual(bindingsTo[0].value, bindingsTo[1].value);
			Assert.AreEqual(bindingsTo[0].value, bindingsTo[2].value);
			Assert.AreEqual(bindingsTo[0].value, bindingsTo[3].value);
			Assert.AreEqual(bindingsTo[1].value, bindingsTo[2].value);
			Assert.AreEqual(bindingsTo[1].value, bindingsTo[3].value);
			Assert.AreEqual(bindingsTo[2].value, bindingsTo[3].value);
		}
		
		[Test]
		public void TestBindingToMultipleSingletonByType() {
			var binder = new Binder();
			
			binder.Bind<MockIClassWithAttributes>().ToSingleton();
			binder.Bind<IMockInterface>().ToSingleton<MockIClassWithAttributes>();

			var bindings1 = binder.GetBindingsFor<MockIClassWithAttributes>();
			var bindings2 = binder.GetBindingsFor<IMockInterface>();
			
			Assert.AreEqual(1, bindings1.Count);
			Assert.AreEqual(1, bindings2.Count);
			Assert.AreEqual(BindingInstance.Singleton, bindings1[0].instanceType);
			Assert.AreEqual(BindingInstance.Singleton, bindings2[0].instanceType);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindings1[0].type);
			Assert.AreEqual(typeof(IMockInterface), bindings2[0].type);
			Assert.AreEqual(bindings1[0].value, bindings1[0].value);
		}
		
		[Test]
		public void TestBindingToMultipleSingletonByInstance() {
			var binder = new Binder();
			
			binder.Bind<MockIClassWithAttributes>().To(new MockIClassWithAttributes());
			binder.Bind<IMockInterface>().ToSingleton<MockIClassWithAttributes>();
			
			var bindings1 = binder.GetBindingsFor<MockIClassWithAttributes>();
			var bindings2 = binder.GetBindingsFor<IMockInterface>();
			
			Assert.AreEqual(1, bindings1.Count);
			Assert.AreEqual(1, bindings2.Count);
			Assert.AreEqual(BindingInstance.Singleton, bindings1[0].instanceType);
			Assert.AreEqual(BindingInstance.Singleton, bindings2[0].instanceType);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindings1[0].type);
			Assert.AreEqual(typeof(IMockInterface), bindings2[0].type);
			Assert.AreEqual(bindings1[0].value, bindings1[0].value);
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
		public void TestBindingToNamespaceTransient() {
			var binder = new Binder();

			binder.Bind<IMockInterface>().ToNamespace("Adic.Tests");
			var bindings = binder.GetBindingsFor<IMockInterface>();

			Assert.AreEqual(3, bindings.Count);
			Assert.AreEqual(BindingInstance.Transient, bindings[0].instanceType);
			Assert.AreEqual(BindingInstance.Transient, bindings[1].instanceType);
			Assert.AreEqual(BindingInstance.Transient, bindings[2].instanceType);
			Assert.AreEqual(typeof(MockIClass), bindings[0].value);
			Assert.AreEqual(typeof(MockIClassWithoutAttributes), bindings[1].value);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindings[2].value);
		}
		
		[Test]
		public void TestBindingToNamespaceSingleton() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().ToNamespaceSingleton("Adic.Tests");
			var bindings = binder.GetBindingsFor<IMockInterface>();

			Assert.AreEqual(3, bindings.Count);
			Assert.AreEqual(BindingInstance.Singleton, bindings[0].instanceType);
			Assert.AreEqual(BindingInstance.Singleton, bindings[1].instanceType);
			Assert.AreEqual(BindingInstance.Singleton, bindings[2].instanceType);
			Assert.AreEqual(typeof(MockIClass), bindings[0].value);
			Assert.AreEqual(typeof(MockIClassWithoutAttributes), bindings[1].value);
			Assert.AreEqual(typeof(MockIClassWithAttributes), bindings[2].value);
		}

		[Test]
		public void TestBindingToFactoryGenericsFromInterface() {
			var binder = new Binder();
			
			var type = typeof(MockFactory);
			binder.Bind<IMockInterface>().ToFactory<MockFactory>();
			var bindings = binder.GetBindingsFor<IMockInterface>();
			
			Assert.AreEqual(1, bindings.Count);
			Assert.AreEqual(BindingInstance.Factory, bindings[0].instanceType);
			Assert.AreEqual(typeof(IMockInterface), bindings[0].type);
			Assert.AreEqual(type, bindings[0].value);
		}
		
		[Test]
		public void TestBindingToFactoryTypeFromInterface() {
			var binder = new Binder();
			
			var type = typeof(MockFactory);
			binder.Bind<IMockInterface>().ToFactory(type);
			var bindings = binder.GetBindingsFor<IMockInterface>();
			
			Assert.AreEqual(1, bindings.Count);
			Assert.AreEqual(BindingInstance.Factory, bindings[0].instanceType);
			Assert.AreEqual(typeof(IMockInterface), bindings[0].type);
			Assert.AreEqual(type, bindings[0].value);
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