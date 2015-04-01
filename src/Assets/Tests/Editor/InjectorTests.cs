using System;
using Adic;
using Adic.Binding;
using Adic.Injection;
using NUnit.Framework;

namespace Adic.Tests {
	[TestFixture]
	public class InjectorTests {
		/// <summary>Injector used on tests.</summary>
		private IInjector injector;
		/// <summary>Binder used on tests.</summary>
		private IBinder binder;

		[SetUp]
		public void Init() {
			this.injector = new InjectionContainer();
			this.binder = this.injector as IBinder;

			//Binds some objects to use on tests.
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>();
			binder.Bind<MockIClassWithoutAttributes>().ToSingleton().As("singleton");
			binder.Bind<MockIClass>().ToSingleton();
		}
		
		[Test]
		public void TestResolveByGenerics() {
			var instance = this.injector.Resolve<IMockInterface>();

			Assert.AreEqual(typeof(MockIClassWithAttributes), instance.GetType());
		}
		
		[Test]
		public void TestResolveByType() {
			var instance = this.injector.Resolve(typeof(IMockInterface));
			
			Assert.AreEqual(typeof(MockIClassWithAttributes), instance.GetType());
		}
		
		[Test]
		public void TestResolveSingleton() {
			var singleton = this.binder.GetBindingsFor<MockIClass>()[0].value;
			var instance = this.injector.Resolve<MockIClass>();
			
			Assert.AreEqual(singleton, instance);
		}
		
		[Test]
		public void TestResolveMultiple() {
			var container = new InjectionContainer();

			container.Bind<IMockInterface>().To<MockIClass>();
			container.Bind<IMockInterface>().To<MockIClassWithAttributes>();
			container.Bind<IMockInterface>().To<MockIClassWithoutAttributes>();

			var instance = container.Resolve<MockClassMultiple>();
			
			Assert.AreEqual(3, instance.list.Length);
			Assert.AreEqual(typeof(MockIClass), instance.list[0].GetType());
			Assert.AreEqual(typeof(MockIClassWithAttributes), instance.list[1].GetType());
			Assert.AreEqual(typeof(MockIClassWithoutAttributes), instance.list[2].GetType());
		}

		[Test]
		public void TestResolveToNamespaceTransient() {
			var container = new InjectionContainer();
			
			container.Bind<IMockInterface>().ToNamespace("Adic.Tests");
			var instance = container.ResolveAll<IMockInterface>();

			Assert.AreEqual(3, instance.Length);
			Assert.IsTrue(instance[0] is MockIClass);
			Assert.IsTrue(instance[1] is MockIClassWithoutAttributes);
			Assert.IsTrue(instance[2] is MockIClassWithAttributes);
		}
		
		[Test]
		public void TestResolveToNamespaceSingleton() {
			var container = new InjectionContainer();
			
			container.Bind<IMockInterface>().ToNamespaceSingleton("Adic.Tests");
			var bindings = container.GetBindingsFor<IMockInterface>();
			var instance = container.ResolveAll<IMockInterface>();
			
			Assert.AreEqual(3, instance.Length);
			Assert.IsTrue(instance[0] is MockIClass);
			Assert.IsTrue(instance[1] is MockIClassWithoutAttributes);
			Assert.IsTrue(instance[2] is MockIClassWithAttributes);
			Assert.IsTrue(instance[0] == bindings[0].value);
			Assert.IsTrue(instance[1] == bindings[1].value);
			Assert.IsTrue(instance[2] == bindings[2].value);
		}
		
		[Test]
		public void TestResolveFromNoBoundType() {
			var mockInterface = this.binder.GetBindingsFor<IMockInterface>()[0].value;
			var singleton = this.binder.GetBindingsFor<MockIClassWithoutAttributes>()[0].value;

			var instance = this.injector.Resolve<MockClassWithDependencies>();

			Assert.AreEqual(typeof(MockClassWithDependencies), instance.GetType());
			Assert.NotNull(instance.dependencyFromConstructor);
			Assert.AreEqual(instance.fieldMockInterface.GetType(), mockInterface);
			Assert.AreEqual(instance.fieldSingleton, singleton);
			Assert.AreEqual(instance.propertyMockInterface.GetType(), mockInterface);
			Assert.AreEqual(instance.propertySingleton, singleton);
		}

		[Test]
		public void TestInjectOnInstance() {
			var mockInterface = this.binder.GetBindingsFor<IMockInterface>()[0].value;
			var singleton = this.binder.GetBindingsFor<MockIClassWithoutAttributes>()[0].value;

			var instance = new MockClassWithDependencies();
			this.injector.Inject<MockClassWithDependencies>(instance);

			Assert.AreEqual(instance.fieldMockInterface.GetType(), mockInterface);
			Assert.AreEqual(instance.fieldSingleton, singleton);
			Assert.AreEqual(instance.propertyMockInterface.GetType(), mockInterface);
			Assert.AreEqual(instance.propertySingleton, singleton);
		}
		
		[Test]
		public void TestCallPostConstruct() {
			var instance = new MockClassWithPostConstruct();

			instance = this.injector.Inject<MockClassWithPostConstruct>(instance);
			
			Assert.IsTrue(instance.hasCalledPostConstructor);
		}
	}
}