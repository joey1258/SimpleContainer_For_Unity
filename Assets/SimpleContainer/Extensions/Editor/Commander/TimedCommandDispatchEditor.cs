using UnityEditor;
using ToluaContainer.Container;

namespace ToluaContainer.Editors
{
    [CustomEditor(typeof(TimedCommandDispatch))]
    public class TimedCommandDispatchEditor : NamespaceCommandEditor<TimedCommandDispatch>
    {
        public override void OnInspectorGUI()
        {
            component.timer = EditorGUILayout.FloatField("Timer (seconds)", component.timer);

            base.OnInspectorGUI();
        }
    }
}