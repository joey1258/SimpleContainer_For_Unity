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
    public class BindingGameObjectsRoot : ContextRoot
    {
        public override void SetupContainers()
        {
            //Create the container.
            AddContainer<InjectionContainer>()
                //Register any extensions the container may use.
                .RegisterExtension<UnityContainer>()
                //Bind a Transform component to the "Cube" game object in the scene.
                .Bind<Transform>().ToGameObject("Cube")
                //Bind the "GameObjectRotator" component to a new ame object of the same name.
                //This component will then receive the reference above so it can rotate the cube.
                .Bind<RotateController>().ToGameObject()/**/;
        }

        public override void Init()
        {
            //Init the game.
        }
    }
}
