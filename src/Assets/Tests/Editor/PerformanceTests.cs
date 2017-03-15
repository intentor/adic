using System;
using Adic;
using NUnit.Framework;
using Adic.Tests.Util;

namespace Adic.Tests {
    /// <summary>
    /// These tests are just to analyse Adic's performance and present
    /// no real unit testing to the component.
    /// </summary>
    [TestFixture]
    public class PerformanceTests {
        /// <summary>Container used on tests.</summary>
        private InjectionContainer container;

        [SetUp]
        public void Init() {
            this.container = new InjectionContainer();
            this.container.Bind<IMockInterface>().To<MockIClass>();
            this.container.cache.CacheFromBinder(this.container);
        }

        #pragma warning disable 0219
        [Test]
        public void Test01ThousandSimpleResolves() {
            TestUtils.MeasurePerformance(this.ExecuteThousandSimpleResolves, 
                "A thousand simple resolves in {0}ms");
        }

        #pragma warning disable 0219
        [Test]
        public void Test02MillionSimpleResolves() {
            TestUtils.MeasurePerformance(this.ExecuteMillionSimpleResolves, 
                "A million simple resolves in {0}ms");
        }

        #pragma warning disable 0219
        [Test]
        public void Test03ThousandMoreComplexResolves() {
            TestUtils.MeasurePerformance(this.ExecuteThousandMoreComplexResolves, 
                "A thousand more complex resolves in {0}ms");
        }
		
        #pragma warning disable 0219
        [Test]
        public void Test04MillionMoreComplexResolves() {
            TestUtils.MeasurePerformance(this.ExecuteMillionMoreComplexResolves, 
                "A million more complex resolves in {0}ms");
        }

        /// <summary>
        /// Executes one thousand simple resolves.
        /// </summary>
        private void ExecuteThousandSimpleResolves() {
            object instance = null;
            for (int i = 0; i < 1000; i++) {
                instance = this.container.Resolve<IMockInterface>();
            }
        }

        /// <summary>
        /// Executes one million simple resolves.
        /// </summary>
        public void ExecuteMillionSimpleResolves() {
            object instance = null;
            for (int i = 0; i < 1000000; i++) {
                instance = this.container.Resolve<IMockInterface>();
            }
        }

        /// <summary>
        /// Executes one thousand more complex resolves.
        /// </summary>
        public void ExecuteThousandMoreComplexResolves() {
            object instance = null;
            for (int i = 0; i < 1000; i++) {
                instance = this.container.Resolve<MockClassVerySimple>();
            }
        }

        /// <summary>
        /// Executes one million more complex resolves.
        /// </summary>
        public void ExecuteMillionMoreComplexResolves() {
            object instance = null;
            for (int i = 0; i < 1000000; i++) {
                instance = this.container.Resolve<MockClassVerySimple>();
            }
        }
    }
}