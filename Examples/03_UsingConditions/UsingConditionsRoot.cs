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
    public class UsingConditionsRoot : ContextRoot
    {
        public override void SetupContainers()
        {
            //Create the container.
            AddContainer<InjectionContainer>()
                //Register any extensions the container may use.
                .RegisterAOT<UnityContainerAOT>()
                //Bind a Transform component to the two cubes on the scene, using a "As"
                //condition to define their identifiers.
                .Bind<Transform>().ToGameObject("LeftCube").As("LeftCube")
                .Bind<Transform>().ToGameObject("RightCube").As("RightCube")
                //Bind the "GameObjectRotator" component to a new game object of the same name.
                //This component will then receive the reference to the "LeftCube", making only
                //this cube rotate.
                .Bind<RotateController03>().ToGameObject();
        }

        public override void Init()
        {
            //Init the game.
        }
    }
}
