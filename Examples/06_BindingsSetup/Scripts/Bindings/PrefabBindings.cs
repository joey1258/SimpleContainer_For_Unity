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

namespace uMVVMCS.Examples.BindingsSetup.Bindings {
	/// <summary>
	/// Bindings for prefabs.
	/// </summary>
	public class PrefabBindings : IBindingsSetup {
		public void SetupBindings(IInjectionContainer container) {
			container
				//Bind the "CubeA" prefab.
				.BindSingleton<Transform>().ToPrefab("06_BindingsSetup/CubeA")
				//Bind the "CubeB" prefab.
				.BindSingleton<Transform>().ToPrefab("06_BindingsSetup/CubeB")
				//Bind the "CubeC" prefab.
				.BindSingleton<Transform>().ToPrefab("06_BindingsSetup/CubeC");
		}		
	}
}