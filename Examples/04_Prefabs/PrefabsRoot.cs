/*
 * Copyright 2016 Sun Ning£¨Joey1258£©
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
	/// Game context root.
	/// </summary>
	public class PrefabsRoot : ContextRoot {
		public override void SetupContainers() {
			//Create the container.
			AddContainer<InjectionContainer>()
				//Register any extensions the container may use.
				.RegisterAOT<UnityContainer>()
				//Bind the "Cube" prefab. It will be injected in CubeRotator.
				.Bind<Transform>().ToPrefab("04_Prefabs/Cube").As("cube").Instantiate()
				//Bind the "Plane" prefab. It exists just to make the scene less empty.
				.BindSingleton<GameObject>().ToPrefab("04_Prefabs/Plane");
		}
		
		public override void Init() {
			//Init the game.
		}
	}
}