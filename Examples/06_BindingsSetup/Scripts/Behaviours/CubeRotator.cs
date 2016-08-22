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
using System.Collections;
using uMVVMCS.DIContainer;

namespace uMVVMCS.Examples.BindingsSetup
{
	/// <summary>
	/// Cube rotator.
	/// </summary>
	public class CubeRotator : MonoBehaviour {
		/// <summary>Speed data for the cube.</summary>
		[Inject]
		public CubeRotationSpeed speedData;

		/// <summary>The cached transform object.</summary>
		protected Transform cachedTransform;

		protected void Start() {
			cachedTransform = GetComponent<Transform>();

			//Call "Inject" to inject any dependencies in the component.
			//In a production game, it's useful to place this in a base component.
			this.Inject();
        }

		protected void Update() {
			cachedTransform.Rotate(speedData.speed, speedData.speed, speedData.speed);
		}
	}
}