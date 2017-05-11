using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Adic.Container;

namespace Adic.Tests {
    /// <summary>
    /// Tests for the Commander Extension.
    /// </summary>
    public class CommanderTest {
        /// <summary>Log for command execution.</summary>
        private const string LOG_TEST_EXECUTE = "TestCommand - Execute";
        /// <summary>Log for command dispose.</summary>
        private const string LOG_TEST_DISPOSE = "TestCommand - Dispose";
        /// <summary>Tag for the command.</summary>
        private const string COMMAND_TAG = "Test";

        /// <summary>Main container.</summary>
        private IInjectionContainer container;
        /// <summary>Command dispatcher.</summary>
        private ICommandDispatcher dispatcher;

        /// <summary>
        /// Test command.
        /// </summary>
        private class TestCommand : Command {
            public override void Execute(params object[] parameters) {
                Debug.Log(LOG_TEST_EXECUTE);
                this.Retain();
            }

            public override void Dispose() {
                base.Dispose();
                Debug.Log(LOG_TEST_DISPOSE);
            }
        }

        [SetUp]
        public void Init() {
            this.container = new InjectionContainer()
                .RegisterExtension<CommanderContainerExtension>()
                .RegisterExtension<EventCallerContainerExtension>()
                .RegisterCommand<TestCommand>()
                .Init();

            this.dispatcher = this.container.GetCommandDispatcher();
        }

        [UnityTest]
        public IEnumerator TestDispatch() {
            this.dispatcher.Dispatch<TestCommand>();

            yield return new WaitForEndOfFrame();

            LogAssert.Expect(LogType.Log, LOG_TEST_EXECUTE);
        }

        [UnityTest]
        public IEnumerator TestInvokeDispatch() {
            this.dispatcher.InvokeDispatch<TestCommand>(0.1f);

            yield return new WaitForSeconds(0.15f);

            LogAssert.Expect(LogType.Log, LOG_TEST_EXECUTE);
        }

        [UnityTest]
        public IEnumerator TestDispatchReleaseWithTag() {
            this.dispatcher.Dispatch<TestCommand>().Tag(COMMAND_TAG);

            yield return new WaitForEndOfFrame();

            LogAssert.Expect(LogType.Log, LOG_TEST_EXECUTE);

            this.dispatcher.ReleaseAll(COMMAND_TAG);

            LogAssert.Expect(LogType.Log, LOG_TEST_DISPOSE);
        }

        [UnityTest]
        public IEnumerator TestInvokeDispatchReleaseWithTag() {
            this.dispatcher.InvokeDispatch<TestCommand>(0.1f).Tag(COMMAND_TAG);

            yield return new WaitForSeconds(0.15f);

            LogAssert.Expect(LogType.Log, LOG_TEST_EXECUTE);

            this.dispatcher.ReleaseAll(COMMAND_TAG);

            LogAssert.Expect(LogType.Log, LOG_TEST_DISPOSE);
        }
    }
}