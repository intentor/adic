using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adic.Commander.Behaviours.Editors {
    /// <summary>
    /// Inspector editor for <see cref="Adic.Commander.Behaviours.CommandDispatch"/>.
    /// </summary>
    [CustomEditor(typeof(CommandDispatch))]
    public class CommandDispatchEditor : NamespaceCommandEditor<CommandDispatch> {

    }
}