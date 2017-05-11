using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Adic {
    /// <summary>
    /// Basic implementation of a command.
    /// </summary>
    public abstract class Command : ICommand, IDisposable {
        public ICommandDispatcher dispatcher { get; set; }
        public string tag { get; set; }
        public bool running { get; set; }
        public bool keepAlive { get; set; }
        public virtual bool singleton { get { return true; } }
        public virtual int preloadPoolSize { get { return 1; } }
        public virtual int maxPoolSize { get { return 10; } }

        /// <summary>Coroutines started on this command.</summary>
        private List<Coroutine> coroutines = new List<Coroutine>(1);

        public abstract void Execute(params object[] parameters);

        public virtual void Retain() {
            this.keepAlive = true;
        }

        public virtual void Release() {
            this.keepAlive = false;

            this.dispatcher.Release(this);
        }

        public virtual void Dispose() {
            while (this.coroutines.Count > 0) {
                this.StopCoroutine(this.coroutines[0]);
            }
        }

        /// <summary>
        /// Invokes the specified method after a specific time in seconds.
        /// </summary>
        /// <param name="method">Method to be called.</param>
        /// <param name="time">Time to call the method (seconds).</param>
        protected void Invoke(Action method, float time) {
            var routine = this.MethodInvoke(method, time);
            this.StartCoroutine(routine);
        }

        /// <summary>
        /// Starts a coroutine.
        /// </summary>
        /// <param name="routine">Routine to be started.</param>
        /// <returns>The coroutine.</returns>
        protected Coroutine StartCoroutine(IEnumerator routine) {
            var coroutine = dispatcher.StartCoroutine(routine);
            this.coroutines.Add(coroutine);
            this.Retain();

            return coroutine;
        }

        /// <summary>
        /// Stops a coroutine.
        /// </summary>
        /// <param name="coroutine">Coroutine to be stopped.</param>
        protected void StopCoroutine(Coroutine coroutine) {
            dispatcher.StopCoroutine(coroutine);
            this.coroutines.Remove(coroutine);
        }

        /// <summary>
        /// Invokes the specified method after a specific time in seconds using a coroutine.
        /// </summary>
        /// <param name="method">Method to be called.</param>
        /// <param name="time">Time to call the method (seconds).</param>
        private IEnumerator MethodInvoke(Action method, float time) {
            yield return new WaitForSeconds(time);
            method();
        }
    }
}