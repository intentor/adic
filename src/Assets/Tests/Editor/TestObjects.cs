using System;
using System.Collections.Generic;
using Adic;
using UnityEngine;

namespace Adic.Tests {
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
		public string property1 { get; set; }
		public string property2 { get; set; }
		public string property3 { get; set; }

		public void SomeMethod1() { }
		public void SomeMethod2() { }
		public void SomeMethod3() { }
	}

	/// <summary>
	/// Test class that implements IMockInterface without a Construct attribute.
	/// </summary>
	public class MockIClassWithoutAttributes : IMockInterface {
		public string property1 { get; set; }
		public string property2 { get; set; }
		public string property3 { get; set; }

		public MockIClassWithoutAttributes() { }
		public MockIClassWithoutAttributes(MockClassToDepend dependency) { }
				
		public void SomeMethod1() { }
		public void SomeMethod2() { }
		public void SomeMethod3() { }
	}

	/// <summary>
	/// Test class that implements IMockInterface with a Construct attribute and all the other.
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
		[Construct]
		public MockIClassWithAttributes(MockClassToDepend dependency) { }
		
		public void SomeMethod1() { }		
		[PostConstruct]
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
		[Construct]
		public MockClassWithDependencies(MockClassToDepend dependency) {
			this.dependencyFromConstructor = dependency;
		}
	}

	/// <summary>
	/// Mock class with dependencies.
	/// </summary>
	public class MockClassWithPostConstruct {
		[Inject]
		public IMockInterface propertyMockInterface { get; set; }
		public bool hasCalledPostConstructor { get; set; }

		public MockClassWithPostConstruct() {
			this.hasCalledPostConstructor = false;
		}
		
		[PostConstruct]
		protected void PostConstructor() {
			this.hasCalledPostConstructor = true;
		}
	}

	/// <summary>
	/// Test factory for ClassWithAtrributes.
	/// </summary>
	public class MockFactory : IFactory {
		public Type factoryType { get { return typeof(MockIClassWithAttributes); } }

		public object Create() {
			return new MockIClassWithAttributes();
		}
	}
}