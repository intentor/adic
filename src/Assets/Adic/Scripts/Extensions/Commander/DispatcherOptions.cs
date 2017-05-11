using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Adic {
    /// <summary>
    /// Options for dispatched commands.
    /// </summary>
    public class DispatcherOptions : ICommandDispatcher {
        /// <summary>Dispatcher that dispatched the command.</summary>
        private ICommandDispatcher dispatcher;
        /// <summary>Command tag.</summary>
        private string tag;
        /// <summary>Command related to the dispatcher options.</summary>
        private ICommand internalCommand;

        /// <summary>Command related to the dispatcher options.</summary>
        public ICommand command {
            get {
                return internalCommand;
            }
            set {
                internalCommand = value;

                if (internalCommand != null) {
                    ApplyTag(internalCommand);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.DispatcherOptions"/> class.
        /// </summary>
        /// <param name="dispatcher">Command dispatcher.</param>
        public DispatcherOptions(ICommandDispatcher dispatcher) {
            this.dispatcher = dispatcher;
        }

        /// <summary>
        /// Tags the dispatched command.
        /// 
        /// Tags are only added to singleton or alive commands.
        /// 
        /// When tagging singleton commands, any previous tags will be replaced.
        /// </summary>
        /// <param name="tag">Tag.</param>
        public ICommandDispatcher Tag(string tag) {
            this.tag = tag;

            if (this.command != null) {
                this.ApplyTag(this.command);
            }

            return this.dispatcher;
        }

        /// <summary>
        /// Applies to tag to a command.
        /// </summary>
        /// <param name="commandToApply">Command to apply.</param>
        private void ApplyTag(ICommand commandToApply) {
            if (!string.IsNullOrEmpty(this.tag)) {
                command.tag = tag;
            }
        }

        public void Init() {
            this.dispatcher.Init();
        }

        public DispatcherOptions Dispatch<T>(params object[] parameters) where T : ICommand {
            return this.dispatcher.Dispatch<T>(parameters);
        }

        public DispatcherOptions Dispatch(Type type, params object[] parameters) {
            return this.dispatcher.Dispatch(type, parameters);
        }

        public DispatcherOptions InvokeDispatch<T>(float time, params object[] parameters) where T : ICommand {
            return this.dispatcher.InvokeDispatch<T>(time, parameters);
        }

        public DispatcherOptions InvokeDispatch(Type type, float time, params object[] parameters) {
            return this.dispatcher.InvokeDispatch(type, time, parameters);
        }

        public ICommandDispatcher Release(ICommand command) {
            return this.dispatcher.Release(command);
        }

        public ICommandDispatcher ReleaseAll() {
            return this.dispatcher.ReleaseAll();
        }

        public ICommandDispatcher ReleaseAll<T>() where T : ICommand {
            return this.dispatcher.ReleaseAll<T>();
        }

        public ICommandDispatcher ReleaseAll(Type type) {
            return this.dispatcher.ReleaseAll(type);
        }

        public ICommandDispatcher ReleaseAll(String tag) {
            return this.dispatcher.ReleaseAll(tag);
        }

        public bool ContainsRegistration<T>() where T : ICommand {
            return this.dispatcher.ContainsRegistration<T>();
        }

        public bool ContainsRegistration(Type type) {
            return this.dispatcher.ContainsRegistration(type);
        }

        public Type[] GetAllRegistrations() {
            return this.dispatcher.GetAllRegistrations();
        }

        public Coroutine StartCoroutine(IEnumerator routine) {
            return this.dispatcher.StartCoroutine(routine);
        }

        public void StopCoroutine(Coroutine coroutine) {
            this.dispatcher.StopCoroutine(coroutine);
        }
    }
}