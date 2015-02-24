using System;
using Adic;
using Adic.Cache;
using NUnit.Framework;

namespace Adic.Tests {
	[TestFixture]
	public class ReflectionFactoryTests {
		[Test]
		public void TestReflectedClassCreation() {
			var factory = new ReflectionFactory();
			var reflectedClass = factory.Create(typeof(MockIClassWithoutAttributes));
			
			Assert.NotNull(reflectedClass);
		}

		[Test]
		public void TestConstructorWhenNoConstruct() {
			var factory = new ReflectionFactory();
			var reflectedClass = factory.Create(typeof(MockIClassWithoutAttributes));

			Assert.NotNull(reflectedClass.constructor);
			Assert.AreEqual(0, reflectedClass.constructorParameters.Length);
		}

		[Test]
		public void TestConstructorWithConstruct() {
			var factory = new ReflectionFactory();			
			var reflectedClass = factory.Create(typeof(MockIClassWithAttributes));

			Assert.NotNull(reflectedClass.constructor);
			Assert.AreEqual(1, reflectedClass.constructorParameters.Length);
			Assert.AreEqual(typeof(MockClassToDepend), reflectedClass.constructorParameters[0]);
		}
		
		[Test]
		public void TestPostConstructor() {
			var factory = new ReflectionFactory();			
			var reflectedClass = factory.Create(typeof(MockIClassWithAttributes));

			Assert.AreEqual(1, reflectedClass.postConstructors.Length);
			Assert.AreEqual("SomeMethod2", reflectedClass.postConstructors[0].Name);
		}
		
		[Test]
		public void TestInjectProperty() {
			var factory = new ReflectionFactory();			
			var reflectedClass = factory.Create(typeof(MockIClassWithAttributes));
			
			Assert.AreEqual(1, reflectedClass.properties.Length);
			Assert.AreEqual("property4", reflectedClass.properties[0].Value.Name);
		}
		
		[Test]
		public void TestInjectField() {
			var factory = new ReflectionFactory();			
			var reflectedClass = factory.Create(typeof(MockIClassWithAttributes));
			
			Assert.AreEqual(1, reflectedClass.fields.Length);
			Assert.AreEqual("field4", reflectedClass.fields[0].Value.Name);
		}
	}
}