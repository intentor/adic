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
        /// <summary>Dispatched command. Can be null for non alive commands.</summary>
        private ICommand command;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.DispatcherOptions"/> class.
        /// </summary>
        /// <param name="dispatcher">Command dispatcher.</param>
        public DispatcherOptions(ICommandDispatcher dispatcher) : this(dispatcher, null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Adic.DispatcherOptions"/> class.
        /// </summary>
        /// <param name="dispatcher">Command dispatcher.</param>
        /// <param name="command">Dispatched command.</param>
        public DispatcherOptions(ICommandDispatcher dispatcher, ICommand command) {
            this.dispatcher = dispatcher;
            this.command = command;
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
            if (this.command != null) {
                this.command.tag = tag;
            }

            return this.dispatcher;
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

        public void InvokeDispatch<T>(float time, params object[] parameters) where T : ICommand {
            this.dispatcher.InvokeDispatch<T>(time, parameters);
        }

        public void InvokeDispatch(Type type, float time, params object[] parameters) {
            this.dispatcher.InvokeDispatch(type, time, parameters);
        }

        public void Release(ICommand command) {
            this.dispatcher.Release(command);
        }

        public void ReleaseAll() {
            this.dispatcher.ReleaseAll();
        }

        public void ReleaseAll<T>() where T : ICommand {
            this.dispatcher.ReleaseAll<T>();
        }

        public void ReleaseAll(Type type) {
            this.dispatcher.ReleaseAll(type);
        }

        public void ReleaseAll(String tag) {
            this.dispatcher.ReleaseAll(tag);
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