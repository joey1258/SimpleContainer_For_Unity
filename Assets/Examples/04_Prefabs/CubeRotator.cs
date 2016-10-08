using UnityEngine;
using ToluaContainer.Container;

namespace ToluaContainer.Examples.Prefabs
{
	public class CubeRotator : MonoBehaviour
    {
		[Inject("cube")]
		public Transform cube;

		private string messages;

		[Inject]
		protected void MethodInjection() {
			//Setup some messages.
			messages = string.Concat(messages, "MethodInjection called.", System.Environment.NewLine);
			var cubeInjected = (cube == null ? "No..." : "Yes!");
			messages = string.Concat(messages, "Cube injected? " + cubeInjected, System.Environment.NewLine);
		}

		protected void Start() {
			this.Inject();
		}

		protected void Update () {
			cube.Rotate(1.0f, 1.0f, 1.0f);
		}

		protected void OnGUI() {
			GUI.Label(new Rect(10, 10, 300, 100), messages);
		}
	}
}