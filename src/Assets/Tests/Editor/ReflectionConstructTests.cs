using System;
using System.Reflection;
using Adic;
using Adic.Tests.Util;
using NUnit.Framework;

namespace Adic.Tests {
	/// <summary>
	/// Reflection tests for perfomance evaluation of object construction.
	/// </summary>
	[TestFixture]
	public class ReflectionConstructTests {
		#pragma warning disable 0219
		[Test]
		public void Test01ConstructDirectly() {
			TestUtils.MeasurePerformance(this.ExecuteConstructDirectly,
            	"A million construct directly: {0}ms");
		}
		
		#pragma warning disable 0219
		[Test]
		public void Test02ConstructByReflection() {
			TestUtils.MeasurePerformance(this.ExecuteConstructByReflection,
                "A million construct by reflection: {0}ms");
		}
		
		#pragma warning disable 0219
		[Test]
		public void Test03ConstructByActivator() {
			TestUtils.MeasurePerformance(this.ExecuteConstructByActivator,
                "A million construct by activator: {0}ms");
		}
		
		#pragma warning disable 0219
		[Test]
		public void Test04ConstructByIL() {
			TestUtils.MeasurePerformance(this.ExecuteConstructByIL,
            	"A million construct by IL: {0}ms");
		}
		
		#pragma warning disable 0219
		[Test]
		public void Test05ConstructWithParamsDirectly() {
			TestUtils.MeasurePerformance(this.ExecuteConstructWithParamsDirectly,
                "A million param construct directly: {0}ms");
		}
		
		#pragma warning disable 0219
		[Test]
		public void Test06ConstructWithParamsByReflection() {
			TestUtils.MeasurePerformance(this.ExecuteConstructWithParamsByReflection,
                "A million param construct by reflection: {0}ms");
		}
		
		#pragma warning disable 0219
		[Test]
		public void Test07ConstructWithParamsByIL() {
			TestUtils.MeasurePerformance(this.ExecuteConstructWithParamsByIL,
                "A million param construct by IL: {0}ms");
		}
		
		/// <summary>
		/// Executes construct directly one million times.
		/// </summary>
		public void ExecuteConstructDirectly() {
			MockIClassWithoutAttributes instance = null;
			for (int i = 0; i < 1000000; i++) {
				instance = new MockIClassWithoutAttributes();
			}
		}
		
		/// <summary>
		/// Executes construct by reflection one million times.
		/// </summary>
		public void ExecuteConstructByReflection() {
			MockIClassWithoutAttributes instance = null;
			var constructor = typeof(MockIClassWithoutAttributes).GetConstructors(
				BindingFlags.FlattenHierarchy | 
                BindingFlags.Public |
                BindingFlags.Instance |
				BindingFlags.InvokeMethod)[0];

			for (int i = 0; i < 1000000; i++) {
				instance = (MockIClassWithoutAttributes)constructor.Invoke(null);
			}
		}
		
		/// <summary>
		/// Executes construct by activator one million times.
		/// </summary>
		public void ExecuteConstructByActivator() {
			MockIClassWithoutAttributes instance = null;
			for (int i = 0; i < 1000000; i++) {
				instance = Activator.CreateInstance<MockIClassWithoutAttributes>();
			}
		}
		
		/// <summary>
		/// Executes construct by IL one million times.
		/// </summary>
		public void ExecuteConstructByIL() {
			MockIClassWithoutAttributes instance = null;
			var constructor = ReflectionUtils.CreateConstructor<MockIClassWithoutAttributes>();
			for (int i = 0; i < 1000000; i++) {
				instance = constructor();
			}
		}
		
		/// <summary>
		/// Executes construct directly one million times.
		/// </summary>
		public void ExecuteConstructWithParamsDirectly() {
			MockIClassWithoutAttributes instance = null;
			var dependency = new MockClassToDepend();
			for (int i = 0; i < 1000000; i++) {
				instance = new MockIClassWithoutAttributes(dependency);
			}
		}
		
		/// <summary>
		/// Executes construct by reflection one million times.
		/// </summary>
		public void ExecuteConstructWithParamsByReflection() {
			MockIClassWithoutAttributes instance = null;
			var constructorInfo = typeof(MockIClassWithoutAttributes).GetConstructors(
				BindingFlags.FlattenHierarchy | 
				BindingFlags.Public |
				BindingFlags.Instance |
				BindingFlags.InvokeMethod)[1];
			var dependency = new MockClassToDepend();
			object[] parameters = new object[1] { dependency };
			
			for (int i = 0; i < 1000000; i++) {
				instance = (MockIClassWithoutAttributes)constructorInfo.Invoke(parameters);
			}
		}
		
		/// <summary>
		/// Executes construct by IL one million times.
		/// </summary>
		public void ExecuteConstructWithParamsByIL() {
			MockIClassWithoutAttributes instance = null;
			var constructorInfo = typeof(MockIClassWithoutAttributes).GetConstructors(
				BindingFlags.FlattenHierarchy | 
				BindingFlags.Public |
				BindingFlags.Instance |
				BindingFlags.InvokeMethod)[1];
			var dependency = new MockClassToDepend();
			object[] parameters = new object[1] { dependency };

			var constructor = ReflectionUtils.CreateConstructorWithParams<MockIClassWithoutAttributes>(constructorInfo);
			for (int i = 0; i < 1000000; i++) {
				instance = constructor(parameters);
			}
		}
	}
}