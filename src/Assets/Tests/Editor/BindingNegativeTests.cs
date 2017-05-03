using System;
using Adic;
using Adic.Binding;
using Adic.Exceptions;
using NUnit.Framework;
using UnityEngine;

namespace Adic.Tests {
    [TestFixture]
    public class BindingNegativeTests {
        [Test]
        public void TestBindToInterfaceSelf() {
            var binder = new Binder();

            Assert.Throws(typeof(BinderException), delegate {
                binder.Bind<IMockInterface>().ToSelf();
            });
        }

        [Test]
        public void TestBindToInterfaceSingleton() {
            var binder = new Binder();

            Assert.Throws(typeof(BinderException), delegate {
                binder.Bind<IMockInterface>().ToSingleton();
            });
        }

        [Test]
        public void TestBindToInterfaceTransient() {
            var binder = new Binder();

            Assert.Throws(typeof(BinderException), delegate {
                binder.Bind<IMockInterface>().To<IMockInterface>();
            });
        }

        [Test]
        public void TestBindNotAssignableKeyTypeToSingleton() {
            var binder = new Binder();

            Assert.Throws(typeof(BindingException), delegate {
                binder.Bind<IMockInterface>().ToSingleton<MockClassToDepend>();
            });
        }

        [Test]
        public void TestBindNotAssignableKeyTypeToTransient() {
            var binder = new Binder();

            Assert.Throws(typeof(BindingException), delegate {
                binder.Bind<IMockInterface>().To<MockClassToDepend>();
            });
        }

        [Test]
        public void TestBindNotAssignableKeyTypeToInstance() {
            var binder = new Binder();

            var instance = new MockClassToDepend();

            Assert.Throws(typeof(BindingException), delegate {
                binder.Bind<IMockInterface>().To<MockClassToDepend>(instance);
            });
        }

        [Test]
        public void TestBindNotAssignableInstanceTypeToInstance() {
            var binder = new Binder();

            var instance = new MockClassToDepend();

            Assert.Throws(typeof(BindingException), delegate {
                binder.Bind<MockClassToDepend>().To(typeof(MockClassVerySimple), instance);
            });
        }

        [Test]
        public void TestBindFactoryToNotFactory() {
            var binder = new Binder();

            Assert.Throws(typeof(BindingException), delegate {
                binder.Bind<MockClassToDepend>().ToFactory(typeof(MockClassToDepend));
            });
        }
    }
}