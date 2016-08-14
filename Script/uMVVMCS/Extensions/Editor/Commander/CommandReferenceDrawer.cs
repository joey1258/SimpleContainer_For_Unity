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

using UnityEngine;
using UnityEditor;
using uMVVMCS.DIContainer;

namespace uMVVMCS.Editor
{
    [CustomPropertyDrawer(typeof(CommandReference))]
    public class CommandReferenceDrawer : PropertyDrawer
    {
        /// <summary>Default line height.</summary>
        private const int LINE_HEIGHT = 18;

        /// <summary>The available commands' names, ordered by namespace.</summary>
        private Dictionary<string, IList<string>> types;
        /// <summary>The available commands' namespace names.</summary>
        private string[] namespaceNames;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (this.types == null || this.types.Count == 0)
            {
                var availableCommands = CommanderUtils.GetAvailableCommands();
                this.types = CommanderUtils.GetTypesAsString(availableCommands);
                this.namespaceNames = this.types.Keys.ToArray();
            }

            EditorGUI.BeginProperty(position, label, property);

            //Label.
            EditorGUI.LabelField(position, label);

            //Don't make child fields be indented.
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            //Namespace.
            var namespaceRect = new Rect(position.x, position.y + LINE_HEIGHT, position.width, LINE_HEIGHT);
            var propertyNamespace = property.FindPropertyRelative("commandNamespace");
            var namespaceIndex = Array.IndexOf(this.namespaceNames, propertyNamespace.stringValue);
            if (namespaceIndex < 0) namespaceIndex = 0;

            EditorGUI.BeginChangeCheck();
            namespaceIndex = EditorGUI.Popup(namespaceRect, "Namespace", namespaceIndex, this.namespaceNames);
            if (EditorGUI.EndChangeCheck() || string.IsNullOrEmpty(propertyNamespace.stringValue))
            {
                propertyNamespace.stringValue = this.namespaceNames[namespaceIndex];
            }

            //Command.
            var commandRect = new Rect(position.x, position.y + LINE_HEIGHT * 2, position.width, LINE_HEIGHT);
            var propertyCommand = property.FindPropertyRelative("commandName");
            var commands = this.types[propertyNamespace.stringValue];
            var commandIndex = commands.IndexOf(propertyCommand.stringValue);
            if (commandIndex < 0) commandIndex = 0;

            EditorGUI.BeginChangeCheck();
            commandIndex = EditorGUI.Popup(commandRect, "Command", commandIndex, commands.ToArray());
            if (EditorGUI.EndChangeCheck() || string.IsNullOrEmpty(propertyCommand.stringValue))
            {
                propertyCommand.stringValue = commands[commandIndex];
            }

            //Set indent back to what it was.
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return LINE_HEIGHT * 3;
        }
    }
}