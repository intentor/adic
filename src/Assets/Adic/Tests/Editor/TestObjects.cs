using System;
using Adic;

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
	/// Test class that ClassWithoutAtrributes and ClassWithAtrributes depend on.
	/// </summary>
	public class MockClassToDepend { }

	/// <summary>
	/// Test class without a Construct attribute.
	/// </summary>
	public class MockClassWithoutAtrributes : IMockInterface {
		public string property1 { get; set; }
		public string property2 { get; set; }
		public string property3 { get; set; }

		public MockClassWithoutAtrributes() { }
		public MockClassWithoutAtrributes(MockClassToDepend dependency) { }
				
		public void SomeMethod1() { }
		public void SomeMethod2() { }
		public void SomeMethod3() { }
	}

	/// <summary>
	/// Test class with a Construct attribute and all the other.
	/// </summary>
	public class MockClassWithAtrributes : IMockInterface {
		public string property1 { get; set; }
		[Inject]
		public string property2 { get; set; }
		public string property3 { get; set; }
		
		public string field1;
		[Inject]
		public string field2;
		public string field3;
		
		public MockClassWithAtrributes() { }
		[Construct]
		public MockClassWithAtrributes(MockClassToDepend dependency) { }
		
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
		public IMockInterface propertyMockInterface { get; set; }
		[Inject("singleton")]
		public MockClassWithoutAtrributes propertySingleton { get; set; }
		public MockClassToDepend dependency { get; set; }

		[Inject]
		public IMockInterface fieldMockInterface;
		[Inject("singleton")]
		public MockClassWithoutAtrributes fieldSingleton;

		public MockClassWithDependencies() { }
		[Construct]
		public MockClassWithDependencies(MockClassToDepend dependency) {
			this.dependency = dependency;
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
		public Type factoryType { get { return typeof(MockClassWithAtrributes); } }

		public object Create() {
			return new MockClassWithAtrributes();
		}
	}
}