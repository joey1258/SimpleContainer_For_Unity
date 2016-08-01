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

using System;
using UnityEngine;

namespace uMVVMCS.DIContainer
{
    public class UnityContainer : IContainerAOT
    {
        #region IContainerAOT implementation 

        public void OnRegister(IInjectionContainer container)
        {
            container.beforeAddBinding += OnBeforeAddBinding;
            container.bindingEvaluation += this.OnBindingEvaluation;
        }

        public void OnUnregister(IInjectionContainer container)
        {
            container.beforeAddBinding -= this.OnBeforeAddBinding;
            container.bindingEvaluation -= this.OnBindingEvaluation;
        }

        #endregion

        /// <summary>
        /// 如果当前 binding 的值是类型且是 MonoBehaviour 派生类就抛出异常
        /// </summary>
        protected void OnBeforeAddBinding(IBinder source, ref IBinding binding)
        {
            if (binding.value is Type &&
                TypeUtils.IsAssignable(typeof(MonoBehaviour), binding.value as Type))
            {
                throw new InjectionSystemException(InjectionSystemException.CANNOT_RESOLVE_MONOBEHAVIOUR);
            }
        }

        /// <summary>
        /// 为 ADDRESS 类型 binding 返回实例化并加载好组件的 gameObject(在 Injector 类的 
        /// ResolveBinding 方法中触发)
        /// </summary>
        protected object OnBindingEvaluation(IInjector source, ref IBinding binding)
        {
            UnityEngine.Debug.Log(111);
            if (binding.value is PrefabBinding &&
                binding.bindingType == BindingType.ADDRESS)
            {
                var prefabBinding = (PrefabBinding)binding.value;
                var gameObject = (GameObject)MonoBehaviour.Instantiate(prefabBinding.prefab);

                if (prefabBinding.type.Equals(typeof(GameObject)))
                {
                    return gameObject;
                }
                else
                {
                    var component = gameObject.GetComponent(prefabBinding.type);

                    if (component == null)
                    {
                        component = gameObject.AddComponent(prefabBinding.type);
                    }

                    return component;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
