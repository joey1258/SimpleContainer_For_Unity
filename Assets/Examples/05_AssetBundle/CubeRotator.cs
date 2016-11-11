using UnityEngine;
using SimpleContainer.Container;

namespace SimpleContainer.Examples.AssetBundle
{
    public class CubeRotator : MonoBehaviour
    {
        [Inject("cube")]
        public Transform cube;

        private string messages;

        [Inject]
        protected void MethodInjection()
        {
            messages = string.Concat(messages, "MethodInjection called.", System.Environment.NewLine);
            var cubeInjected = (cube == null ? "No..." : "Yes!");
            messages = string.Concat(messages, "Cube injected? " + cubeInjected, System.Environment.NewLine);
        }

        protected void Start()
        {
            this.Inject();
        }

        protected void Update()
        {
            cube.Rotate(1.0f, 1.0f, 1.0f);
        }

        protected void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 300, 100), messages);
        }
    }
}