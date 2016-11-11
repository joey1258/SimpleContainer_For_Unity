using UnityEngine;

namespace SimpleContainer.Editors
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
        public static GUIStyle bindings
        {
            get
            {
                var style = new GUIStyle();
                style.fontSize = 13;
                style.alignment = TextAnchor.UpperLeft;
                return style;
            }
        }
    }
}