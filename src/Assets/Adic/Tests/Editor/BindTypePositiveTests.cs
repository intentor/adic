using System;
using Adic;
using NUnit.Framework;

namespace Adic.Tests {
	[TestFixture]
	public class BindTypePositiveTests  {
		[Test]
		public void TestBindingAsSingleton() {
			var binder = new Binder();

			binder.Bind<MockClassWithAtrributes>().AsSingleton();
			var binding = binder.GetBinding<MockClassWithAtrributes>();
			
			Assert.AreEqual(BindingType.Singleton, binding.bindingType);
			Assert.AreEqual(typeof(MockClassWithAtrributes), binding.key);
			Assert.AreEqual(typeof(MockClassWithAtrributes), binding.value);
		}

		[Test]
		public void TestBindingAsSingletonFromInterface() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().AsSingleton<MockClassWithAtrributes>();
			var binding = binder.GetBinding<IMockInterface>();
			
			Assert.AreEqual(BindingType.Singleton, binding.bindingType);
			Assert.AreEqual(typeof(IMockInterface), binding.key);
			Assert.AreEqual(typeof(MockClassWithAtrributes), binding.value);
		}
		
		[Test]
		public void TestBindingToConcreteFromInterface() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().To<MockClassWithAtrributes>();
			var binding = binder.GetBinding<IMockInterface>();

			Assert.AreEqual(BindingType.Default, binding.bindingType);
			Assert.AreEqual(typeof(IMockInterface), binding.key);
			Assert.AreEqual(typeof(MockClassWithAtrributes), binding.value);
		}
		
		[Test]
		public void TestBindingToInstanceFromInterface() {
			var binder = new Binder();

			var instance = new MockClassWithAtrributes();
			binder.Bind<IMockInterface>().To(instance);
			var binding = binder.GetBinding<IMockInterface>();
			
			Assert.AreEqual(BindingType.Singleton, binding.bindingType);
			Assert.AreEqual(typeof(IMockInterface), binding.key);
			Assert.AreEqual(instance, binding.value);
		}
		
		[Test]
		public void TestBindingFactoryFromInterface() {
			var binder = new Binder();
			
			var factory = new MockFactory();
			binder.Bind<IMockInterface>().ToFactory(factory);
			var binding = binder.GetBinding<IMockInterface>();
			
			Assert.AreEqual(BindingType.Factory, binding.bindingType);
			Assert.AreEqual(typeof(IMockInterface), binding.key);
			Assert.AreEqual(factory, binding.value);
		}
	}
}