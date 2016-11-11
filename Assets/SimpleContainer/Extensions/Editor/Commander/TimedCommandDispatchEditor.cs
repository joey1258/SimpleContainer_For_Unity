using UnityEditor;
using SimpleContainer.Container;

namespace SimpleContainer.Editors
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