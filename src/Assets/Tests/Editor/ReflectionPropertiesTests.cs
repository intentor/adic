using System;
using Adic;
using Adic.Tests.Util;
using NUnit.Framework;

namespace Adic.Tests {
	/// <summary>
	/// Reflection tests for perfomance evaluation of property getters and setters.
	/// </summary>
	[TestFixture]
	public class ReflectionPropertiesTests {
		/// <summary>The value to use in setter tests.</summary>
		private const string VALUE_TO_SET = "Test Value";
		/// <summary>Property name from MockIClass used in tests.</summary>
		private const string CLASS_PROPERTY = "property1";
		/// <summary>The test class for property call.</summary>
		private MockIClass testClass;
		
		[SetUp]
		public void Init() {			
			this.testClass = new MockIClass() {
				field1 = VALUE_TO_SET,
				property1 = VALUE_TO_SET
			};
		}
		
		#pragma warning disable 0219
		[Test]
		public void Test01GetPropertyDirectly() {
			TestUtils.MeasurePerformance(this.ExecuteGetPropertyDirectly,
            	"A million get property directly: {0}ms");
		}
		
		[Test]
		public void Test02SetPropertyDirectly() {
			TestUtils.MeasurePerformance(this.ExecuteSetPropertyDirectly,
            	"A million set property directly: {0}ms");
		}
		
		#pragma warning disable 0219
		[Test]
		public void Test03GetPropertyByReflection() {
			TestUtils.MeasurePerformance(this.ExecuteGetPropertyByReflection,
            	"A million get property by reflection: {0}ms");
		}
		
		[Test]
		public void Test04SetPropertyByReflection() {
			TestUtils.MeasurePerformance(this.ExecuteSetPropertyByReflection,
            	"A million set property by reflection: {0}ms");
		}
		
		#pragma warning disable 0219
		[Test]
		public void Test05GetPropertyByIL() {
			TestUtils.MeasurePerformance(this.ExecuteGetPropertyByIL,
            	"A million get property by IL method: {0}ms");
		}
		
		[Test]
		public void Test06SetPropertyByIL() {
			TestUtils.MeasurePerformance(this.ExecuteSetPropertyByIL,
           		"A million set property by IL method: {0}ms");
		}
		
		/// <summary>
		/// Executes get property directly one million times.
		/// </summary>
		public void ExecuteGetPropertyDirectly() {
			for (int i = 0; i < 1000000; i++) {
				string value = this.testClass.property1;			
			}
		}
		
		/// <summary>
		/// Executes set property directly one million times.
		/// </summary>
		public void ExecuteSetPropertyDirectly() {
			for (int i = 0; i < 1000000; i++) {
				this.testClass.property1 = VALUE_TO_SET;			
			}
		}
		
		/// <summary>
		/// Executes get property by relfection one million times.
		/// </summary>
		public void ExecuteGetPropertyByReflection() {
			var property = this.testClass.GetType().GetProperty(CLASS_PROPERTY);
			for (int i = 0; i < 1000000; i++) {
				string value = (string)property.GetValue(this.testClass, null);
			}
		}
		
		/// <summary>
		/// Executes set property by relfection one million times.
		/// </summary>
		public void ExecuteSetPropertyByReflection() {
			var property = this.testClass.GetType().GetProperty(CLASS_PROPERTY);
			for (int i = 0; i < 1000000; i++) {
				property.SetValue(this.testClass, CLASS_PROPERTY, null);
			}
		}
		
		/// <summary>
		/// Executes get property by IL method one million times.
		/// </summary>
		public void ExecuteGetPropertyByIL() {
			var getter = ReflectionUtils.CreatePropertyGetter<MockIClass, string>(CLASS_PROPERTY);
			for (int i = 0; i < 1000000; i++) {
				string value = getter(this.testClass);
			}
		}
		
		/// <summary>
		/// Executes set property by IL method one million times.
		/// </summary>
		public void ExecuteSetPropertyByIL() {
			var setter = ReflectionUtils.CreatePropertySetter<MockIClass, string>(CLASS_PROPERTY);
			for (int i = 0; i < 1000000; i++) {
				setter(this.testClass, VALUE_TO_SET);
			}
		}
	}
}