using System;
using Intentor.Adic;
using NUnit.Framework;
using UnityEngine;

namespace Intentor.Adic.Tests {
	[TestFixture]
	public class BindingNegativeTests  {
		[Test]
		[ExpectedException(typeof(BinderException))]
		public void TestBindToInterfaceSelf() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().ToSelf();
		}

		[Test]
		[ExpectedException(typeof(BinderException))]
		public void TestBindToInterfaceSingleton() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().ToSingleton();
		}

		[Test]
		[ExpectedException(typeof(BinderException))]
		public void TestBindToInterfaceTransient() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().To<IMockInterface>();
		}
		
		[Test]
		[ExpectedException(typeof(BindingException))]
		public void TestBindNotAssignableKeyTypeToSingleton() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().ToSingleton<MockClassToDepend>();
		}
		
		[Test]
		[ExpectedException(typeof(BindingException))]
		public void TestBindNotAssignableKeyTypeToTransient() {
			var binder = new Binder();
			
			binder.Bind<IMockInterface>().To<MockClassToDepend>();
		}
		
		[Test]
		[ExpectedException(typeof(BindingException))]
		public void TestBindNotAssignableKeyTypeToInstance() {
			var binder = new Binder();

			var instance = new MockClassToDepend();
			binder.Bind<IMockInterface>().To<MockClassToDepend>(instance);
		}
		
		[Test]
		[ExpectedException(typeof(BindingException))]
		public void TestBindNotAssignableInstanceTypeToInstance() {
			var binder = new Binder();

			var instance = new MockClassToDepend();
			binder.Bind<MockClassToDepend>().To(typeof(MockClassVerySimple), instance);
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