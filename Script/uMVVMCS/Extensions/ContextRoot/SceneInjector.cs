/*
 * Copyright 2016 Sun Ning（Joey1258）
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *      Unless required by applicable law or agreed to in writing, software
 *      distributed under the License is distributed on an "AS IS" BASIS,
 *      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *      See the License for the specific language governing permissions and
 *      limitations under the License.
 */

using UnityEngine;
using System;

namespace uMVVMCS.DIContainer
{
    [RequireComponent(typeof(ContextRoot))]
    public class SceneInjector : MonoBehaviour
    {
        private void Awake()
        {
            var contextRoot = GetComponent<ContextRoot>();
            var baseType = (contextRoot.baseBehaviourTypeName == "UnityEngine.MonoBehaviour" ?
                typeof(MonoBehaviour) : TypeUtils.GetType(contextRoot.baseBehaviourTypeName));

            switch (contextRoot.injectionType)
            {
                case ContextRoot.MonoBehaviourInjectionType.Children:
                    {
                        InjectOnChildren(baseType);
                    }
                    break;

                case ContextRoot.MonoBehaviourInjectionType.BaseType:
                    {
                        InjectFromBaseType(baseType);
                    }
                    break;
            }
        }

        /// <summary>
        /// 对当前物体的所有子物体注入
        /// </summary>
        public void InjectOnChildren(Type baseType)
        {
            var sceneInjectorType = GetType();
            var components = GetComponent<Transform>().GetComponentsInChildren(baseType, true);
            foreach (var component in components)
            {
                // 如果组件是 ContextRoot 或者是自身则忽略
                var componentType = component.GetType();
                if (componentType == sceneInjectorType ||
                    TypeUtils.IsAssignable(typeof(ContextRoot), componentType)) continue;

                ((MonoBehaviour)component).Inject();
            }
        }

        /// <summary>
        /// 为所有 MonoBehaviour 注入指定类型
        /// </summary>
        public void InjectFromBaseType(Type baseType)
        {
            var components = (MonoBehaviour[])Resources.FindObjectsOfTypeAll(baseType);

            for (var index = 0; index < components.Length; index++)
            {
                components[index].Inject();
            }
        }
    }
}