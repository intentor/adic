using System;
using Adic;
using Adic.Binding;
using Adic.Injection;
using NUnit.Framework;
using Adic.Container;

namespace Adic.Tests {
	[TestFixture]
	public class InjectorTests {
		/// <summary>Injector used on tests.</summary>
		private IInjector injector;
		/// <summary>Binder used on tests.</summary>
		private IBinder binder;
		/// <summary>Injection container for identifier tests.</summary>
		private IInjectionContainer containerIdentifierTests;

		[SetUp]
		public void Init() {
			this.injector = new InjectionContainer();
			this.binder = this.injector as IBinder;

			//Binds some objects to use on tests.
			binder.Bind<IMockInterface>().To<MockIClassWithAttributes>();
			binder.Bind<MockIClassWithoutAttributes>().ToSingleton().As("singleton");
			binder.Bind<MockIClass>().ToSingleton();

			this.containerIdentifierTests = new InjectionContainer();
			var mockClass1 = new MockIClass() { property1 = "MockClass1" };
			var mockClass2 = new MockIClassWithoutAttributes() { property1 = "MockClass2" };
			var mockClass3 = new MockIClassWithAttributes() { property1 = "MockClass3" };
			this.containerIdentifierTests.Bind<MockIClass>().To(mockClass1).As(TestIdentifier.MockClass);
			this.containerIdentifierTests.Bind<MockIClassWithoutAttributes>().To(mockClass2).As(TestIdentifier.MockClass);
			this.containerIdentifierTests.Bind<MockIClassWithAttributes>().To(mockClass3).As(TestIdentifier.MockClass);
			this.containerIdentifierTests.Bind<IMockInterface>().To(mockClass1).As(TestIdentifier.MockClass1);
			this.containerIdentifierTests.Bind<IMockInterface>().To(mockClass2).As(TestIdentifier.MockClass2);
			this.containerIdentifierTests.Bind<IMockInterface>().To(mockClass3).As(TestIdentifier.MockClass3);
			this.containerIdentifierTests.Bind<IMockInterface>().To<MockIClass>().As(TestIdentifier.MockClassSingle);
		}

		[Test]
		public void TestResolveWithConstructorInject() {
			var instance = this.containerIdentifierTests.Resolve<MockClassSimpleConstructInject>();

			Assert.AreEqual(typeof(MockIClassWithAttributes), instance.mock.GetType());
			Assert.AreEqual("MockClass3", instance.mock.property1);
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
		public void TestResolveFromIdentifier() {
			var instance = this.containerIdentifierTests.Resolve(TestIdentifier.MockClassSingle);
			
			Assert.AreEqual(typeof(MockIClass), instance.GetType());
		}
		
		[Test]
		public void TestResolveFromIdentifierByGenerics() {
			var instance1 = this.containerIdentifierTests.Resolve<IMockInterface>(TestIdentifier.MockClass1);
			var instance2 = this.containerIdentifierTests.Resolve<IMockInterface>(TestIdentifier.MockClass2);
			var instance3 = this.containerIdentifierTests.Resolve<IMockInterface>(TestIdentifier.MockClass3);
			
			Assert.AreEqual(typeof(MockIClass), instance1.GetType());
			Assert.AreEqual(typeof(MockIClassWithoutAttributes), instance2.GetType());
			Assert.AreEqual(typeof(MockIClassWithAttributes), instance3.GetType());
			
		}
		
		[Test]
		public void TestResolveFromIdentifierByType() {			
			var instance1 = this.containerIdentifierTests.Resolve(typeof(IMockInterface), TestIdentifier.MockClass1);
			var instance2 = this.containerIdentifierTests.Resolve(typeof(IMockInterface), TestIdentifier.MockClass2);
			var instance3 = this.containerIdentifierTests.Resolve(typeof(IMockInterface), TestIdentifier.MockClass3);
			
			Assert.AreEqual(typeof(MockIClass), instance1.GetType());
			Assert.AreEqual(typeof(MockIClassWithoutAttributes), instance2.GetType());
			Assert.AreEqual(typeof(MockIClassWithAttributes), instance3.GetType());
		}
		
		[Test]
		public void TestResolveAllFromIdentifier() {
			var instances = this.containerIdentifierTests.ResolveAll(TestIdentifier.MockClass);

			Assert.AreEqual(3, instances.Length);
			Assert.AreEqual(typeof(MockIClass), instances[0].GetType());
			Assert.AreEqual(typeof(MockIClassWithoutAttributes), instances[1].GetType());
			Assert.AreEqual(typeof(MockIClassWithAttributes), instances[2].GetType());
		}
		
		[Test]
		public void TestResolveAllFromIdentifierByGenerics() {
			var instances = this.containerIdentifierTests.ResolveAll<MockIClass>(TestIdentifier.MockClass);

			Assert.AreEqual(1, instances.Length);
			Assert.AreEqual(typeof(MockIClass), instances[0].GetType());
		}
		
		[Test]
		public void TestResolveAllFromIdentifierByType() {
			var instances = this.containerIdentifierTests.ResolveAll(typeof(MockIClass), TestIdentifier.MockClass);

			Assert.AreEqual(1, instances.Length);
			Assert.AreEqual(typeof(MockIClass), instances[0].GetType());
		}
		
		[Test]
		public void TestResolveFromFactoryGenerics() {
			var container = new InjectionContainer();
			
			container.Bind<MockIClassWithAttributes>().ToFactory<MockFactory>();
			var instance = container.Resolve<MockIClassWithAttributes>();
			
			Assert.NotNull(instance);
			Assert.AreEqual("Created from a Factory", instance.field1);
			Assert.AreEqual("Created from a Factory", instance.field2);
			Assert.AreEqual("Created from a Factory", instance.field3);
		}
		
		[Test]
		public void TestResolveFromFactoryType() {
			var container = new InjectionContainer();
			
			container.Bind<MockIClassWithAttributes>().ToFactory(typeof(MockFactory));
			var instance = container.Resolve<MockIClassWithAttributes>();

			Assert.NotNull(instance);
			Assert.AreEqual("Created from a Factory", instance.field1);
			Assert.AreEqual("Created from a Factory", instance.field2);
			Assert.AreEqual("Created from a Factory", instance.field3);
		}
		
		[Test]
		public void TestResolveFromFactoryInstance() {
			var container = new InjectionContainer();
			
			container.Bind<MockIClassWithAttributes>().ToFactory(new MockFactory());
			var instance = container.Resolve<MockIClassWithAttributes>();
			
			Assert.NotNull(instance);
			Assert.AreEqual("Created from a Factory", instance.field1);
			Assert.AreEqual("Created from a Factory", instance.field2);
			Assert.AreEqual("Created from a Factory", instance.field3);
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
		public void TestCallMethodInject() {
			var instance = new MockClassParameterlessMethodInject();

			instance = this.injector.Inject<MockClassParameterlessMethodInject>(instance);
			
			Assert.IsTrue(instance.hasCalledMethod);
		}

		[Test]
		public void TestInjectMethodInject() {
			var instance = new MockClassParametersMethodInject();
			
			instance = this.containerIdentifierTests.Inject<MockClassParametersMethodInject>(instance);
			
			Assert.AreEqual(typeof(MockIClassWithAttributes), instance.propertyMock1.GetType());
			Assert.AreEqual(typeof(MockClassToDepend), instance.propertyMock2.GetType());
			Assert.AreEqual("MockClass3", instance.propertyMock1.property1);
		}
		
		[Test]
		public void TestResolveMethodInject() {
			var instance = this.containerIdentifierTests.Resolve<MockClassParametersMethodInject>();
			
			Assert.AreEqual(typeof(MockIClassWithAttributes), instance.propertyMock1.GetType());
			Assert.AreEqual(typeof(MockClassToDepend), instance.propertyMock2.GetType());
			Assert.AreEqual("MockClass3", instance.propertyMock1.property1);
		}

		[Test]
		public void TestCallMultipleMethodInject() {
			var instance = new MockClassMultipleMethodInject();

			instance = this.injector.Inject<MockClassMultipleMethodInject>(instance);

			Assert.IsTrue(instance.calledMethod1);
			Assert.IsTrue(instance.calledMethod2);
			Assert.IsTrue(instance.calledMethod3);
		}

		[Test]
		public void TestInjectMultipleMethodInject() {
			var instance = new MockClassMultipleMethodInject();

			instance = this.containerIdentifierTests.Inject<MockClassMultipleMethodInject>(instance);

			Assert.IsTrue(instance.calledMethod1);
			Assert.IsTrue(instance.calledMethod2);
			Assert.IsTrue(instance.calledMethod3);
		}

		[Test]
		public void TestResolveMultipleMethodInject() {
			var instance = this.containerIdentifierTests.Resolve<MockClassMultipleMethodInject>();

			Assert.IsTrue(instance.calledMethod1);
			Assert.IsTrue(instance.calledMethod2);
			Assert.IsTrue(instance.calledMethod3);
		}

		[Test]
		public void TestResolutionModeReturnNullBound() {
			var container = new InjectionContainer(ResolutionMode.ALWAYS_RESOLVE);
			container.Bind<IMockInterface1>().ToSingleton<MockClassManyInterfaces>();

			var instance = container.Resolve<IMockInterface1>();

			Assert.NotNull(instance);
		}

		[Test]
		public void TestResolutionModeReturnNullNotBound() {
			var container = new InjectionContainer(ResolutionMode.RETURN_NULL);

			var instance = container.Resolve<MockClassManyInterfaces>();

			Assert.IsNull(instance);
		}

		[Test]
		public void TestResolutionModeReturnNullSingleton() {
			var container = new InjectionContainer(ResolutionMode.RETURN_NULL);
			container.Bind<IMockInterface>().ToSingleton<MockIClassWithAttributes>();
			
			var instance = container.Resolve<IMockInterface>();

			Assert.IsNotNull(instance);
			Assert.AreEqual(typeof(MockIClassWithAttributes), instance.GetType());
			Assert.Null(((MockIClassWithAttributes)instance).field4);
			Assert.Null(((MockIClassWithAttributes)instance).property4);
		}
	}
}