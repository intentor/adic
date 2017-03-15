using UnityEditor;
using System;

namespace Adic.Extensions.ContextRoots.Utils {
    /// <summary>
    /// Execution order utils.
    /// </summary>
    public static class ExecutionOrderUtils {
        /// <summary>
        /// Sets the script execution order ensuring unique order values.
        /// </summary>
        /// <param name="type">Type to be set.</param>
        /// <param name="order">Order to be set.</param>
        public static int SetScriptExecutionOrder(Type type, int order) {
            return SetScriptExecutionOrder(type, order, true);
        }

        /// <summary>
        /// Sets the script execution order.
        /// </summary>
        /// <param name="type">Type to be set.</param>
        /// <param name="order">Order to be set.</param>
        /// <param name="unique">Indicates whether the execution order should be unique.</param>
        public static int SetScriptExecutionOrder(Type type, int order, bool unique) {
            var executionOrder = order;
            MonoScript selectedScript = null;

            // Get the first available execution order.
            var available = false;
            while (!available) {
                available = true;

                foreach (var script in MonoImporter.GetAllRuntimeMonoScripts()) {
                    if (selectedScript == null && script.GetClass() == type) {
                        selectedScript = script;
                        if (!unique)
                            break;
                    }

                    if (script.GetClass() != type && MonoImporter.GetExecutionOrder(script) == executionOrder) {
                        executionOrder += order;
                        available = false;
                        continue;
                    }
                }
            }
			
            MonoImporter.SetExecutionOrder(selectedScript, executionOrder);

            return executionOrder;
        }
    }
}