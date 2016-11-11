using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using SimpleContainer.Container;

namespace SimpleContainer.Editors
{
    [CustomPropertyDrawer(typeof(CommandReference))]
    public class CommandReferenceDrawer : PropertyDrawer
    {
        /// <summary>
        /// 默认行高
        /// </summary>
        private const int LINE_HEIGHT = 18;

        /// <summary>
        /// 可用 commands(类型)名字典，以命名空间为 key
        /// a</summary>
        private Dictionary<string, IList<string>> types;

        /// <summary>
        /// 可用 commands 命名空间
        /// </summary>
        private string[] namespaceNames;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (types == null || types.Count == 0)
            {
                var availableCommands = CommanderUtils.GetAvailableCommands();
                types = CommanderUtils.GetTypesAsString(availableCommands);
                namespaceNames = new List<string>(types.Keys).ToArray();
            }

            EditorGUI.BeginProperty(position, label, property);

            // Label.
            EditorGUI.LabelField(position, label);

            // 子段落不缩进
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // 命名空间
            var namespaceRect = new Rect(position.x, position.y + LINE_HEIGHT, position.width, LINE_HEIGHT);
            var propertyNamespace = property.FindPropertyRelative("commandNamespace");
            var i = Array.IndexOf(namespaceNames, propertyNamespace.stringValue);
            if (i < 0) { i = 0; }

            EditorGUI.BeginChangeCheck();
            i = EditorGUI.Popup(namespaceRect, "Namespace", i, namespaceNames);
            if (EditorGUI.EndChangeCheck() || string.IsNullOrEmpty(propertyNamespace.stringValue))
            {
                propertyNamespace.stringValue = namespaceNames[i];
            }

            // Command.
            var commandRect = new Rect(position.x, position.y + LINE_HEIGHT * 2, position.width, LINE_HEIGHT);
            var propertyCommand = property.FindPropertyRelative("commandName");
            var commands = types[propertyNamespace.stringValue];
            var commandIndex = commands.IndexOf(propertyCommand.stringValue);
            if (commandIndex < 0) commandIndex = 0;

            EditorGUI.BeginChangeCheck();
            commandIndex = EditorGUI.Popup(commandRect, "Command", commandIndex, new List<string>(commands).ToArray());
            if (EditorGUI.EndChangeCheck() || string.IsNullOrEmpty(propertyCommand.stringValue))
            {
                propertyCommand.stringValue = commands[commandIndex];
            }

            // 设置缩进
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return LINE_HEIGHT * 3;
        }
    }
}