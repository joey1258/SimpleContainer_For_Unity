/*
 * Copyright 2016 Sun Ning（Joey1258）
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *      Unless required by applicable law or agreed to in writing, software
 *      distributed under the License is distributed on an "AS IS" BASIS,
 *      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *      See the License for the specific language governing permissions and
 *      limitations under the License.
 */

using UnityEditor;
using uMVVMCS.DIContainer;
using System;
using System.Collections.Generic;

namespace uMVVMCS.Editors
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