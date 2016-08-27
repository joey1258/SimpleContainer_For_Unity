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

namespace uMVVMCS.Examples.BindingsSetup.Bindings
{
	[Priority]
	public class DataBindings : IBindingsSetup
    {
		public void SetupBindings(IInjectionContainer container)
        {
			container
				//Bind the rotation data for "CubeA".
				.Bind<CubeRotationSpeed>().To(new CubeRotationSpeed(0.5f)).When(
					context => context.parentInstance is MonoBehaviour &&
						((MonoBehaviour)context.parentInstance).name.Contains("CubeA"))
				//Bind the rotation data for "CubeB".
				.Bind<CubeRotationSpeed>().To(new CubeRotationSpeed(2.0f)).When(
					context => context.parentInstance is MonoBehaviour &&
						((MonoBehaviour)context.parentInstance).name.Contains("CubeB"))
				//Bind the rotation data for "CubeC".
				.Bind<CubeRotationSpeed>().To(new CubeRotationSpeed(4.5f)).When(
					context => context.parentInstance is MonoBehaviour &&
						((MonoBehaviour)context.parentInstance).name.Contains("CubeC"));
		}		
	}
}