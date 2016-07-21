using UnityEngine;
using System;
using System.Collections.Generic;
using Adic;
using Adic.Injection;

namespace Adic.Tests {
	/// <summary>
	/// Identifiers for tests.
	/// </summary>
	public enum TestIdentifier {
		MockClass,
		MockClass1,
		MockClass2,
		MockClass3,
		MockClassSingle
	}

	/// <summary>
	/// Interface for tests.
	/// </summary>
	public interface IMockInterface {
		string property1 { get; set; }
		string property2 { get; set; }
		string property3 { get; set; }
		
		void SomeMethod1();
		void SomeMethod2();
		void SomeMethod3();
	}

	/// <summary>
	/// Interface 1 for tests with a single method.
	/// </summary>
	public interface IMockInterface1 {
		void Method1();
	}

	/// <summary>
	/// Interface 2 for tests with a single method.
	/// </summary>
	public interface IMockInterface2 {
		void Method2();
	}

	/// <summary>
	/// Interface 3 for tests with a single method.
	/// </summary>
	public interface IMockInterface3 {
		void Method3();
	}

	/// <summary>
	/// Mock class implemeting many interfaces.
	/// </summary>
	public class MockClassManyInterfaces : IMockInterface1, IMockInterface2, IMockInterface3 {
		public void Method1() { }
		public void Method2() { }
		public void Method3() { }
	}
	
	/// <summary>
	/// Test class that is a MonoBehaviour.
	/// </summary>
	public class MockMonoBehaviour : MonoBehaviour { }

	/// <summary>
	/// Test class that ClassWithoutAtrributes and ClassWithAtrributes depend on.
	/// </summary>
	public class MockClassToDepend { }
	
	/// <summary>
	/// Very simple mock class with a basic dependency.
	/// </summary>
	public class MockClassVerySimple {
		[Inject]
		public IMockInterface field;
	}
	
	/// <summary>
	/// Simple mock class with a named and a basic dependency.
	/// </summary>
	public class MockClassSimple {
		[Inject]
		public IMockInterface field;
		
		[Inject("singleton")]
		public IMockInterface property { get; set; }
	}
	
	/// <summary>
	/// Simple mock class with a constructor parameter with Inject attribute.
	/// </summary>
	public class MockClassSimpleConstructInject {
		public IMockInterface mock;

		public MockClassSimpleConstructInject([Inject(TestIdentifier.MockClass3)] IMockInterface mock) {
			this.mock = mock;
       	}
	}
	
	/// <summary>
	/// Mock class with a multiple dependency.
	/// </summary>
	public class MockClassMultiple {
		[Inject]
		public IMockInterface[] list;
	}

	/// <summary>
	/// Test class that implements IMockInterface.
	/// </summary>
	public class MockIClass : IMockInterface {
		public string field1;
		public string field2;
		public string field3;

		public string property1 { get; set; }
		public string property2 { get; set; }
		public string property3 { get; set; }

		public void SomeMethod1() { }
		public void SomeMethod2() { }
		public void SomeMethod3() { }
	}

	/// <summary>
	/// Test class that implements IMockInterface without Inject attributes.
	/// </summary>
	public class MockIClassWithoutAttributes : IMockInterface {
		public string property1 { get; set; }
		public string property2 { get; set; }
		public string property3 { get; set; }
		public MockClassToDepend dependency { get; set; }

		public MockIClassWithoutAttributes() { }
		public MockIClassWithoutAttributes(MockClassToDepend dependency) {
			this.dependency = dependency;
		}
				
		public void SomeMethod1() { }
		public void SomeMethod2() { }
		public void SomeMethod3() { }
	}

	/// <summary>
	/// Test class that implements IMockInterface with Inject attributes.
	/// </summary>
	public class MockIClassWithAttributes : IMockInterface {
		public string field1;
		public string field2;
		public string field3;
		[Inject]
		public MockClassToDepend field4;

		public string property1 { get; set; }
		public string property2 { get; set; }
		public string property3 { get; set; }
		[Inject]
		public MockClassToDepend property4 { get; set; }
		
		public MockIClassWithAttributes() { }
		[Inject]
		public MockIClassWithAttributes(MockClassToDepend dependency) { }

		public void SomeMethod1() { }		
		[Inject]
		public void SomeMethod2() { }
		public void SomeMethod3() { }
	}

	/// <summary>
	/// Mock class with dependencies.
	/// </summary>
	public class MockClassWithDependencies {
		[Inject]
		public IMockInterface fieldMockInterface;
		[Inject("singleton")]
		public MockIClassWithoutAttributes fieldSingleton;

		[Inject]
		public IMockInterface propertyMockInterface { get; set; }
		[Inject("singleton")]
		public MockIClassWithoutAttributes propertySingleton { get; set; }
		public MockClassToDepend dependencyFromConstructor { get; set; }

		public MockClassWithDependencies() { }
		[Inject]
		public MockClassWithDependencies(MockClassToDepend dependency) {
			this.dependencyFromConstructor = dependency;
		}
	}

	/// <summary>
	/// Mock class with a parameterless method inject.
	/// </summary>
	public class MockClassParameterlessMethodInject {
		[Inject]
		public IMockInterface propertyMockInterface { get; set; }
		public bool hasCalledMethod { get; set; }

		public MockClassParameterlessMethodInject() {
			this.hasCalledMethod = false;
		}
		
		[Inject]
		protected void PostConstructor() {
			this.hasCalledMethod = true;
		}
	}
	
	/// <summary>
	/// Mock class with a parameterized method inject.
	/// </summary>
	public class MockClassParametersMethodInject {
		public IMockInterface propertyMock1 { get; set; }
		public MockClassToDepend propertyMock2 { get; set; }
		
		[Inject]
		protected void PostConstructor([Inject(TestIdentifier.MockClass3)] IMockInterface mock1, MockClassToDepend mock2) {
			this.propertyMock1 = mock1;
			this.propertyMock2 = mock2;
		}
	}

	/// <summary>
	/// Mock class with multiple method injection.
	/// </summary>
	public class MockClassMultipleMethodInject {
		public bool calledMethod1 { get; set; }
		public bool calledMethod2 { get; set; }
		public bool calledMethod3 { get; set; }

		[Inject]
		protected void Method1() {
			this.calledMethod1 = true;
		}

		[Inject]
		protected void Method2() {
			this.calledMethod2 = true;
		}

		[Inject]
		protected void Method3() {
			this.calledMethod3 = true;
		}
	}

	/// <summary>
	/// Test factory for ClassWithAtrributes.
	/// </summary>
	public class MockFactory : IFactory {
		public object Create(InjectionContext context) {
			var obj = new MockIClassWithAttributes();
			obj.field1 = 
				obj.field2 = 
					obj.field3 = "Created from a Factory";

			return obj;
		}
	}
}