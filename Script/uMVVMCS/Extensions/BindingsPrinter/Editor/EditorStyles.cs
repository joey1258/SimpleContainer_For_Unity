/*
 * Copyright 2016 Sun Ning（Joey1258）
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

using UnityEngine;

namespace uMVVMCS.DIContainer.Extensions
{
    public static class EditorStyles
    {
        /// <summary>
        /// Message style
        /// </summary>
        public static GUIStyle message
        {
            get
            {
                var style = new GUIStyle();
                style.fontSize = 20;
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleCenter;
                return style;
            }
        }

        /// <summary>
        /// Styles for titles
        /// </summary>
        public static GUIStyle title
        {
            get
            {
                var style = new GUIStyle();
                style.fontSize = 18;
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleLeft;
                return style;
            }
        }

        /// <summary>
        /// Styles for container's info
        /// </summary>
        public static GUIStyle containerInfo
        {
            get
            {
                var style = new GUIStyle();
                style.fontSize = 12;
                style.alignment = TextAnchor.MiddleLeft;
                return style;
            }
        }

        /// <summary>
        /// Styles for container's names
        /// </summary>
        public static GUIStyle containerName
        {
            get
            {
                var style = new GUIStyle();
                style.fontSize = 16;
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.UpperLeft;
                return style;
            }
        }

        /// <summary>
        /// Styles for binding's data
        /// </summary>
        public static GUIStyle bindinds
        {
            get
            {
                var style = new GUIStyle();
                style.fontSize = 16;
                style.alignment = TextAnchor.UpperLeft;
                return style;
            }
        }
    }
}