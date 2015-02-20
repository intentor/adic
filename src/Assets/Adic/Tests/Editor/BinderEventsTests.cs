using System;
using System.Collections.Generic;
using Intentor.Adic;
using NUnit.Framework;

namespace Intentor.Adic.Tests {
	[TestFixture]
	public class BinderEventsTests {
		[Test]
		public void TestBeforeAddBindingEvent() {
			var eventCalled = false;

			IBinder binder = new Binder();
			binder.beforeAddBinding += delegate(IBinder source, ref Binding binding) {
				Assert.AreEqual(binder, source);
				Assert.AreEqual(typeof(IMockInterface), binding.type);
				Assert.AreEqual(0, binder.GetBindings().Count);

				eventCalled = true;
			};

			binder.Bind<IMockInterface>().To<MockIClass>();

			Assert.IsTrue(eventCalled);
		}
		
		[Test]
		public void TestAfterAddBindingEvent() {
			var eventCalled = false;
			
			IBinder binder = new Binder();
			binder.afterAddBinding += delegate(IBinder source, ref Binding binding) {
				Assert.AreEqual(binder, source);
				Assert.AreEqual(typeof(IMockInterface), binding.type);
				Assert.AreEqual(1, binder.GetBindings().Count);

				eventCalled = true;
			};
			
			binder.Bind<IMockInterface>().To<MockIClass>();
			
			Assert.IsTrue(eventCalled);
		}
		
		[Test]
		public void TestBeforeRemoveBindingEvent() {
			var eventCalled = false;
			
			IBinder binder = new Binder();
			binder.beforeRemoveBinding += delegate(IBinder source, Type type, IList<Binding> bindings) {
				Assert.AreEqual(binder, source);
				Assert.AreEqual(typeof(IMockInterface), type);
				Assert.AreEqual(1, bindings.Count);
				Assert.AreEqual(typeof(MockIClass), bindings[0].value);
				Assert.AreEqual(1, binder.GetBindings().Count);

				eventCalled = true;
			};
			
			binder.Bind<IMockInterface>().To<MockIClass>();
			binder.Unbind<IMockInterface>();
			
			Assert.IsTrue(eventCalled);
		}
		
		[Test]
		public void TestAfterRemoveBindingEvent() {
			var eventCalled = false;
			
			IBinder binder = new Binder();
			binder.afterRemoveBinding += delegate(IBinder source, Type type, IList<Binding> bindings) {
				Assert.AreEqual(binder, source);
				Assert.AreEqual(typeof(IMockInterface), type);
				Assert.AreEqual(1, bindings.Count);
				Assert.AreEqual(typeof(MockIClass), bindings[0].value);
				Assert.AreEqual(0, binder.GetBindings().Count);

				eventCalled = true;
			};
			
			binder.Bind<IMockInterface>().To<MockIClass>();
			binder.Unbind<IMockInterface>();

			Assert.IsTrue(eventCalled);
		}
	}
}