using System;
using Adic;
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
			this.injector = new InjectorBinder();
			this.binder = this.injector as IBinder;

			//Binds some objects to use on tests.
			binder.Bind<IMockInterface>().To<MockClassWithoutAtrributes>();
			binder.Bind("singleton").AsSingleton<MockClassWithoutAtrributes>();
		}
		
		[Test]
		public void TestResolveByInterface() {
			var instance = this.injector.Resolve<IMockInterface>();

			Assert.AreEqual(typeof(MockClassWithoutAtrributes), instance.GetType());
		}
		
		[Test]
		public void TestResolveByType() {
			var instance = this.injector.Resolve(typeof(IMockInterface));
			
			Assert.AreEqual(typeof(MockClassWithoutAtrributes), instance.GetType());
		}
		
		[Test]
		public void TestResolveByName() {
			var instance = this.injector.Resolve("singleton");
			
			Assert.AreEqual(typeof(MockClassWithoutAtrributes), instance.GetType());
		}
		
		[Test]
		public void TestResolveFromNoBindedType() {
			var instance = this.injector.Resolve<MockClassWithDependencies>();

			Assert.AreEqual(typeof(MockClassWithDependencies), instance.GetType());
			Assert.NotNull(instance.dependency);
			Assert.AreEqual(instance.propertyMockInterface.GetType(), this.binder.GetBinding<IMockInterface>().value);
			Assert.AreEqual(instance.propertySingleton, this.binder.GetBinding("singleton").value);
			Assert.AreEqual(instance.fieldMockInterface.GetType(), this.binder.GetBinding<IMockInterface>().value);
			Assert.AreEqual(instance.fieldSingleton, this.binder.GetBinding("singleton").value);
		}

		[Test]
		public void TestInjectOnInstance() {
			var instance = new MockClassWithDependencies();

			this.injector.Inject<MockClassWithDependencies>(instance);

			Assert.AreEqual(instance.propertyMockInterface.GetType(), this.binder.GetBinding<IMockInterface>().value);
			Assert.AreEqual(instance.propertySingleton, this.binder.GetBinding("singleton").value);
			Assert.AreEqual(instance.fieldMockInterface.GetType(), this.binder.GetBinding<IMockInterface>().value);
			Assert.AreEqual(instance.fieldSingleton, this.binder.GetBinding("singleton").value);
		}
		
		[Test]
		public void TestCallPostConstruct() {
			var instance = new MockClassWithPostConstruct();

			instance = this.injector.Inject<MockClassWithPostConstruct>(instance);

			Assert.AreEqual(true, instance.hasCalledPostConstructor);
		}
	}
}