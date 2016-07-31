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
using uMVVMCS.DIContainer;

namespace uMVVMCS.Examples
{
	/// <summary>
	/// Cube rotator script.
	/// </summary>
	public class CubeRotator : MonoBehaviour {
		/// <summary>
        /// Cube to rotate. It will only rotate the cube with identifier "cube".
        /// </summary>
		[Inject("cube")]
		public Transform cube;

		/// <summary>Messages to show on screen.</summary>
		private string messages;

		/// <summary>
		/// Called after all injections.
		/// </summary>
		[Inject]
		protected void MethodInjection() {
			//Setup some messages.
			messages = string.Concat(messages, "MethodInjection called.", System.Environment.NewLine);
			var cubeInjected = (cube == null ? "No..." : "Yes!");
			messages = string.Concat(messages, "Cube injected? " + cubeInjected, System.Environment.NewLine);
		}

		/// <summary>
		/// Start is called after PostConstruct.
		/// </summary>
		protected void Start() {
			//Calls "Inject" to inject any dependencies in the component.
			//In a production game, it's useful to place this in a base component.
			//this.Inject();
		}

		protected void Update () {
			cube.Rotate(1.0f, 1.0f, 1.0f);
		}

		protected void OnGUI() {
			GUI.Label(new Rect(10, 10, 300, 100), messages);
		}
	}
}