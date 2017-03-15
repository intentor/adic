using System;
using Adic;
using Adic.Binding;
using Adic.Cache;
using Adic.Injection;
using NUnit.Framework;

namespace Adic.Tests {
    [TestFixture]
    public class InjectorEventsTests {
        [Test]
        public void TestBeforeResolveContinue() {
            var eventCalled = false;

            IReflectionCache cache = new ReflectionCache();
            IBinder binder = new Binder();
            IInjector injector = new Injector(cache, binder, ResolutionMode.ALWAYS_RESOLVE);

            injector.beforeResolve += delegate(
                IInjector source,
                Type type,
                InjectionMember member,
                object parentInstance,
                object identifier,
                ref object resolutionInstance) {
                Assert.AreEqual(injector, source);
                Assert.AreEqual(typeof(IMockInterface), type);
                Assert.AreEqual(InjectionMember.None, member);
                Assert.IsNull(parentInstance);
                Assert.IsNull(identifier);
                Assert.IsNull(resolutionInstance);

                eventCalled = true;
				
                return true;
            };
			
            binder.Bind<IMockInterface>().To<MockIClass>();
            var instance = injector.Resolve<IMockInterface>();
			
            Assert.IsTrue(eventCalled);
            Assert.AreEqual(typeof(MockIClass), instance.GetType());
        }

        [Test]
        public void TestBeforeResolveStop() {
            var eventCalled = false;
			
            IReflectionCache cache = new ReflectionCache();
            IBinder binder = new Binder();
            IInjector injector = new Injector(cache, binder, ResolutionMode.ALWAYS_RESOLVE);
			
            injector.beforeResolve += delegate(
                IInjector source,
                Type type,
                InjectionMember member,
                object parentInstance,
                object identifier,
                ref object resolutionInstance) {
                Assert.AreEqual(injector, source);
                Assert.AreEqual(typeof(IMockInterface), type);
                Assert.AreEqual(InjectionMember.None, member);
                Assert.IsNull(parentInstance);
                Assert.IsNull(identifier);
                Assert.IsNull(resolutionInstance);

                resolutionInstance = new MockIClassWithoutAttributes();
				
                eventCalled = true;
				
                return false;
            };
			
            binder.Bind<IMockInterface>().To<MockIClass>();
            var instance = injector.Resolve<IMockInterface>();
			
            Assert.IsTrue(eventCalled);
            Assert.AreEqual(typeof(MockIClassWithoutAttributes), instance.GetType());
        }

        [Test]
        public void TestAfterResolve() {
            var eventCalled = false;
			
            IReflectionCache cache = new ReflectionCache();
            IBinder binder = new Binder();
            IInjector injector = new Injector(cache, binder, ResolutionMode.ALWAYS_RESOLVE);
            IMockInterface resolvedInstance = null;
			
            injector.afterResolve += delegate(IInjector source,
                                              Type type,
                                              InjectionMember member,
                                              object parentInstance,
                                              object identifier,
                                              ref object resolutionInstance) {
                Assert.AreEqual(injector, source);
                Assert.AreEqual(typeof(IMockInterface), type);
                Assert.AreEqual(InjectionMember.None, member);
                Assert.IsNull(parentInstance);
                Assert.IsNull(identifier);
                Assert.IsNotNull(resolutionInstance);

                resolvedInstance = (IMockInterface) resolutionInstance;
                eventCalled = true;

                return false;
            };
			
            binder.Bind<IMockInterface>().To<MockIClass>();
            var instance = injector.Resolve<IMockInterface>();
			
            Assert.IsTrue(eventCalled);
            Assert.AreEqual(typeof(MockIClass), instance.GetType());
            Assert.AreEqual(resolvedInstance, instance);
        }

        [Test]
        public void TestBindingEvaluation() {
            var eventCalled = false;
			
            IReflectionCache cache = new ReflectionCache();
            IBinder binder = new Binder();
            IInjector injector = new Injector(cache, binder, ResolutionMode.ALWAYS_RESOLVE);

            injector.bindingEvaluation += delegate(IInjector source, ref BindingInfo binding) {
                Assert.AreEqual(injector, source);
                Assert.NotNull(binding);

                eventCalled = true;
				
                return new MockIClassWithoutAttributes();
            };
			
            binder.Bind<IMockInterface>().To<MockIClass>();
            var instance = injector.Resolve<IMockInterface>();
			
            Assert.IsTrue(eventCalled);
            Assert.AreEqual(typeof(MockIClassWithoutAttributes), instance.GetType());
        }

        [Test]
        public void TestBeforeInject() {
            var eventCalled = false;
			
            IReflectionCache cache = new ReflectionCache();
            IBinder binder = new Binder();
            IInjector injector = new Injector(cache, binder, ResolutionMode.ALWAYS_RESOLVE);
            var instanceToInject = new MockClassVerySimple();

            injector.beforeInject += delegate(IInjector source, ref object instance, ReflectedClass reflectedClass) {
                //The if below is just to avoid checking when injecting on MockIClass.
                if (reflectedClass.type != typeof(MockClassVerySimple))
                    return;

                Assert.AreEqual(injector, source);
                Assert.AreEqual(instanceToInject, instance);
                Assert.IsNull(instanceToInject.field);

                eventCalled = true;
            };
			
            binder.Bind<IMockInterface>().To<MockIClass>();
            injector.Inject(instanceToInject);
			
            Assert.IsTrue(eventCalled);
            Assert.AreEqual(typeof(MockIClass), instanceToInject.field.GetType());
        }

        [Test]
        public void TestAfterInject() {
            var eventCalled = false;
			
            IReflectionCache cache = new ReflectionCache();
            IBinder binder = new Binder();
            IInjector injector = new Injector(cache, binder, ResolutionMode.ALWAYS_RESOLVE);
            var instanceToInject = new MockClassVerySimple();
			
            injector.afterInject += delegate(IInjector source, ref object instance, ReflectedClass reflectedClass) {
                //The if below is just to avoid checking when injecting on MockIClass.
                if (reflectedClass.type != typeof(MockClassVerySimple))
                    return;
				
                Assert.AreEqual(injector, source);
                Assert.AreEqual(instanceToInject, instance);
                Assert.AreEqual(typeof(MockIClass), instanceToInject.field.GetType());
				
                eventCalled = true;
            };
			
            binder.Bind<IMockInterface>().To<MockIClass>();
            injector.Inject(instanceToInject);
			
            Assert.IsTrue(eventCalled);
        }
    }
}