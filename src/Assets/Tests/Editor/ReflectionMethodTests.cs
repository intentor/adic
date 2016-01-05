using System;
using System.Reflection;
using Adic;
using Adic.Tests.Util;
using NUnit.Framework;

namespace Adic.Tests {
	/// <summary>
	/// Reflection tests for perfomance evaluation of object methods.
	/// </summary>
	[TestFixture]
	public class ReflectionMethodTests {
		/// <summary>Method name from MockIClassWithoutAttributes used in tests.</summary>
		private const string CLASS_METHOD = "SomeMethod1";

		#pragma warning disable 0219
		[Test]
		public void Test01CallMethodDirectly() {
			TestUtils.MeasurePerformance(this.ExecuteCallMethodDirectly,
            	"A million method calls directly: {0}ms");
		}
		
		#pragma warning disable 0219
		[Test]
		public void Test02CallMethodByReflection() {
			TestUtils.MeasurePerformance(this.ExecuteCallMethodByReflection,
                "A million method calls by reflection: {0}ms");
		}
		
		#pragma warning disable 0219
		[Test]
		public void Test04CallMethodByIL() {
			TestUtils.MeasurePerformance(this.ExecuteCallMethodByIL,
            	"A million method calls by IL: {0}ms");
		}
		
		/// <summary>
		/// Executes a directly method call one million times.
		/// </summary>
		public void ExecuteCallMethodDirectly() {
			var instance = new MockIClassWithoutAttributes();
			for (int i = 0; i < 1000000; i++) {
				instance.SomeMethod1();
			}
		}
		
		/// <summary>
		/// Executes method call by reflection one million times.
		/// </summary>
		public void ExecuteCallMethodByReflection() {
			var instance = new MockIClassWithoutAttributes();
			var method = instance.GetType().GetMethod(CLASS_METHOD);
			for (int i = 0; i < 1000000; i++) {
				method.Invoke(instance, null);
			}
		}
		
		/// <summary>
		/// Executes a method call by IL one million times.
		/// </summary>
		public void ExecuteCallMethodByIL() {
			var instance = new MockIClassWithoutAttributes();
			var method = ReflectionUtils.CreateMethod<MockIClassWithoutAttributes>(CLASS_METHOD);
			for (int i = 0; i < 1000000; i++) {
				method(instance);
			}
		}
	}
}