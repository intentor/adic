using System;
using Adic;
using NUnit.Framework;
using UnityEngine;

namespace Adic.Tests {
	[TestFixture]
	public class BindingNegativeTests  {		
		[Test]
		[ExpectedException(typeof(BinderException))]
		public void TestBindToInterface() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().To<IMockInterface>();
		}

		[Test]
		[ExpectedException(typeof(BindingException))]
		public void TestBindNameWithSingletonWithoutType() {
			var binder = new Binder();
			
			binder.Bind("test").AsSingleton();
		}
		
		[Test]
		[ExpectedException(typeof(BindingException))]
		public void TestBindSameKey() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().AsSingleton<MockClassToDepend>();
			binder.Bind<IMockInterface>().To<MockClassToDepend>();
		}
		
		[Test]
		[ExpectedException(typeof(BindingException))]
		public void TestSingletonToNotAssignable() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().AsSingleton<MockClassToDepend>();
		}
		
		[Test]
		[ExpectedException(typeof(BindingException))]
		public void TestSingletonToMonoBehaviour() {
			var binder = new Binder();

			binder.Bind<IMockInterface>().AsSingleton<MonoBehaviour>();
		}
		
		[Test]
		[ExpectedException(typeof(BindingException))]
		public void TestBindToNotAssignable() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().To<MockClassToDepend>();
		}
		
		[Test]
		[ExpectedException(typeof(BindingException))]
		public void TestBindToMonoBehaviour() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().To<MonoBehaviour>();
		}
		
		[Test]
		[ExpectedException(typeof(BindingException))]
		public void TestBindToNotAssignableInstance() {
			var binder = new Binder();

			var instance = new MockClassToDepend();
			binder.Bind<IMockInterface>().To(instance);
		}
		
		[Test]
		[ExpectedException(typeof(BindingException))]
		public void TestBindToWrongFactoryType() {
			var binder = new Binder();
			
			var factory = new MockFactory();
			binder.Bind<MockClassToDepend>().ToFactory(factory);
		}
	}
}