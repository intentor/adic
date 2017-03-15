using System;
using Adic;
using Adic.Injection;
using NUnit.Framework;

namespace Adic.Tests {
    [TestFixture]
    public class ConditionsTests {
        [Test]
        public void TestBindAsIdentifier() {
            var container = new InjectionContainer();

            container.Bind<IMockInterface>().To<MockIClassWithAttributes>();
            container.Bind<IMockInterface>().To<MockIClassWithoutAttributes>().As("singleton");
            container.Bind<IMockInterface>().To<MockIClass>().As("test");

            var instance = container.Resolve<MockClassSimple>();

            Assert.AreEqual(typeof(MockIClassWithAttributes), instance.field.GetType());
            Assert.AreEqual(typeof(MockIClassWithoutAttributes), instance.property.GetType());
        }

        [Test]
        public void TestBindWhenIntoByGenerics() {
            var container = new InjectionContainer();

            container.Bind<IMockInterface>().To<MockIClassWithAttributes>().WhenInto<MockClassVerySimple>();
            container.Bind<IMockInterface>().To<MockIClassWithoutAttributes>().WhenInto<MockClassSimple>();

            var instance1 = container.Resolve<MockClassVerySimple>();
            var instance2 = container.Resolve<MockClassSimple>();

            Assert.AreEqual(typeof(MockIClassWithAttributes), instance1.field.GetType());
            Assert.AreEqual(typeof(MockIClassWithoutAttributes), instance2.field.GetType());
        }

        [Test]
        public void TestBindWhenIntoByType() {
            var container = new InjectionContainer();

            container.Bind<IMockInterface>().To<MockIClassWithAttributes>().WhenInto(typeof(MockClassVerySimple));
            container.Bind<IMockInterface>().To<MockIClassWithoutAttributes>().WhenInto(typeof(MockClassSimple));
			
            var instance1 = container.Resolve<MockClassVerySimple>();
            var instance2 = container.Resolve<MockClassSimple>();

            Assert.AreEqual(typeof(MockIClassWithAttributes), instance1.field.GetType());
            Assert.AreEqual(typeof(MockIClassWithoutAttributes), instance2.field.GetType());
        }

        [Test]
        public void TestBindWhenIntoByInstance() {
            var instance1 = new MockClassSimple();
            var instance2 = new MockClassVerySimple();

            var container = new InjectionContainer();

            container.Bind<IMockInterface>().To<MockIClassWithAttributes>().WhenIntoInstance(instance1);
            container.Bind<IMockInterface>().To<MockIClassWithoutAttributes>().WhenIntoInstance(instance2);

            container.Inject(instance1);
            container.Inject(instance2);

            Assert.AreEqual(typeof(MockIClassWithAttributes), instance1.field.GetType());
            Assert.AreEqual(typeof(MockIClassWithoutAttributes), instance2.field.GetType());
        }

        [Test]
        public void TestBindAsIdentifierWhenIntoByGenerics() {
            var container = new InjectionContainer();
			
            container.Bind<IMockInterface>()
				.To<MockIClassWithAttributes>().WhenInto<MockClassVerySimple>();
            container.Bind<IMockInterface>()
				.To<MockIClassWithoutAttributes>().WhenInto<MockClassSimple>().As("singleton");
            container.Bind<IMockInterface>()
				.To<MockIClass>().WhenInto<MockClassSimple>().As("test");
			
            var instance1 = container.Resolve<MockClassVerySimple>();
            var instance2 = container.Resolve<MockClassSimple>();
			
            Assert.AreEqual(typeof(MockIClassWithAttributes), instance1.field.GetType());
            Assert.IsNull(instance2.field);
            Assert.AreEqual(typeof(MockIClassWithoutAttributes), instance2.property.GetType());
        }

        [Test]
        public void TestBindAsIdentifierWhenIntoByType() {
            var container = new InjectionContainer();
			
            container.Bind<IMockInterface>()
				.To<MockIClassWithAttributes>().WhenInto(typeof(MockClassVerySimple));
            container.Bind<IMockInterface>()
				.To<MockIClassWithoutAttributes>().WhenInto(typeof(MockClassSimple)).As("singleton");
            container.Bind<IMockInterface>()
				.To<MockIClass>().WhenInto(typeof(MockClassSimple)).As("test");
			
            var instance1 = container.Resolve<MockClassVerySimple>();
            var instance2 = container.Resolve<MockClassSimple>();
			
            Assert.AreEqual(typeof(MockIClassWithAttributes), instance1.field.GetType());
            Assert.IsNull(instance2.field);
            Assert.AreEqual(typeof(MockIClassWithoutAttributes), instance2.property.GetType());
        }

        [Test]
        public void TestBindAsIdentifierWhenIntoByInstance() {
            var instance1 = new MockClassVerySimple();
            var instance2 = new MockClassSimple();
			
            var container = new InjectionContainer();
			
            container.Bind<IMockInterface>()
				.To<MockIClassWithAttributes>().WhenIntoInstance(instance1);
            container.Bind<IMockInterface>()
				.To<MockIClassWithoutAttributes>().WhenIntoInstance(instance2).As("singleton");
            container.Bind<IMockInterface>()
				.To<MockIClass>().WhenInto<MockClassSimple>().WhenIntoInstance(instance2).As("test");
			
            container.Inject(instance1);
            container.Inject(instance2);
			
            Assert.AreEqual(typeof(MockIClassWithAttributes), instance1.field.GetType());
            Assert.IsNull(instance2.field);
            Assert.AreEqual(typeof(MockIClassWithoutAttributes), instance2.property.GetType());
        }

        [Test]
        public void TestBindWhenComplexConditionField() {
            var instance1 = new MockClassVerySimple();
            var instance2 = new MockClassSimple();
            var instance3 = new MockClassSimple();
			
            var container = new InjectionContainer();

            container.Bind<IMockInterface>().To<MockIClass>().When(context =>
	        		context.member.Equals(InjectionMember.Field)
                && context.parentInstance.Equals(instance3)
            );
			
            container.Inject(instance1);
            container.Inject(instance2);
            container.Inject(instance3);

            Assert.IsNull(instance1.field);
            Assert.IsNull(instance2.field);
            Assert.IsNull(instance2.property);
            Assert.AreEqual(typeof(MockIClass), instance3.field.GetType());
            Assert.IsNull(instance3.property);
        }

        [Test]
        public void TestBindWhenComplexConditionByMemberName() {
            var container = new InjectionContainer(ResolutionMode.RETURN_NULL);

            container.Bind<IMockInterface>().To<MockIClass>().When(context =>
                context.memberName.Equals("property2")
            );
            container.Bind<IMockInterface>().To<MockIClass>().When(context =>
                context.memberName.Equals("field2")
            );
            container.Bind<MockClassToDepend>().ToSelf().When(context =>
                context.member.Equals(InjectionMember.Method)
                && context.memberName.Equals("field2")
            );
            container.Bind<MockClassInjectAll>().ToSingleton();

            var instance = container.Resolve<MockClassInjectAll>();

            Assert.IsNull(instance.property1);
            Assert.AreEqual(typeof(MockIClass), instance.property2.GetType());
            Assert.IsNull(instance.property3);

            Assert.IsNull(instance.field1);
            Assert.AreEqual(typeof(MockIClass), instance.field2.GetType());
            Assert.IsNull(instance.field3);

            Assert.AreNotEqual(instance.property2, instance.field2);

            Assert.IsNull(instance.fieldFromConstructor1);
            Assert.IsNull(instance.fieldFromConstructor2);

            Assert.IsNull(instance.fieldFromMethod1);
            Assert.IsNotNull(instance.fieldFromMethod2);
        }
    }
}