using UnityEditor;
using SimpleContainer.Container;
using System;
using System.Collections.Generic;

namespace SimpleContainer.Editors
{
    public abstract class NamespaceCommandEditor<T> : Editor where T : NamespaceCommandBehaviour
    {
        /// <summary>
        /// 需要编辑的组件
        /// </summary>
        protected T component;

        /// <summary>
        /// 可用 command（类型）名
        /// </summary>
        protected Dictionary<string, IList<string>> types;

        /// <summary>
        /// 可用 commands 命名空间
        /// </summary>
        protected string[] namespaceNames;

        protected void OnEnable()
        {
            component = (T)this.target;

            var availableCommands = CommanderUtils.GetAvailableCommands();
            types = CommanderUtils.GetTypesAsString(availableCommands);
            namespaceNames = new List<string>(types.Keys).ToArray();
        }

        public override void OnInspectorGUI()
        {
            // 命名空间
            var namespaceIndex = Array.IndexOf(this.namespaceNames, this.component.commandNamespace);
            if (namespaceIndex == -1) namespaceIndex = 0;
            namespaceIndex = EditorGUILayout.Popup("Namespace", namespaceIndex, this.namespaceNames);
            this.component.commandNamespace = this.namespaceNames[namespaceIndex];

            // Command 
            var commands = this.types[this.component.commandNamespace];
            var commandIndex = commands.IndexOf(this.component.commandName);
            if (commandIndex < 0) commandIndex = 0;
            commandIndex = EditorGUILayout.Popup("Command", commandIndex, new List<string>(commands).ToArray());
            component.commandName = commands[commandIndex];

            // 更新编辑器
            EditorUtility.SetDirty(this.target);
        }
    }
}