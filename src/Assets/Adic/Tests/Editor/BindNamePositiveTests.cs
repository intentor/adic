using System;
using Adic;
using NUnit.Framework;

namespace Adic.Tests {
	[TestFixture]
	public class BindNamePositiveTests  {		
		[Test]
		public void TestBindingAsSingleton() {
			var binder = new Binder();
			
			binder.Bind("test").AsSingleton<MockClassWithAtrributes>();
			var binding = binder.GetBinding("test");
			
			Assert.AreEqual(BindingType.Singleton, binding.bindingType);
			Assert.AreEqual("test", binding.key);
			Assert.AreEqual(typeof(MockClassWithAtrributes), binding.value);
		}
		
		[Test]
		public void TestBindingToConcreteClass() {
			var binder = new Binder();
			
			binder.Bind("test").To<MockClassWithAtrributes>();
			var binding = binder.GetBinding("test");
			
			Assert.AreEqual(BindingType.Default, binding.bindingType);
			Assert.AreEqual("test", binding.key);
			Assert.AreEqual(typeof(MockClassWithAtrributes), binding.value);
		}
		
		[Test]
		public void TestBindingToInstance() {
			var binder = new Binder();
			
			var instance = new MockClassWithAtrributes();
			binder.Bind("test").To(instance);
			var binding = binder.GetBinding("test");
			
			Assert.AreEqual(BindingType.Singleton, binding.bindingType);
			Assert.AreEqual("test", binding.key);
			Assert.AreEqual(instance, binding.value);
		}
		
		[Test]
		public void TestBindingToFactory() {
			var binder = new Binder();
			
			var factory = new MockFactory();
			binder.Bind("test").ToFactory(factory);
			var binding = binder.GetBinding("test");
			
			Assert.AreEqual(BindingType.Factory, binding.bindingType);
			Assert.AreEqual("test", binding.key);
			Assert.AreEqual(factory, binding.value);
		}
	}
}