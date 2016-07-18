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

namespace uMVVMCS.DIContainer.Extensions
{
    public static class UnityBinding
    {
        #region To

        /// <summary>
        /// Binds the key type to a singleton  <paramref name="type"/> on a GameObject 
        /// of a given <paramref name="name"/>.
        /// 
        /// If <paramref name="type"/> is <see cref="UnityEngine.GameObject"/>, binds the
        /// key to the GameObject itself.
        /// 
        /// If <paramref name="type"/> is see cref="UnityEngine.Component"/>, binds the key
        /// to the the instance of the component.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the GameObject, it will be added.
        /// </summary>
        /// <remarks>
        /// To prevent references to destroyed objects, only bind to game objects that won't 
        /// be destroyed in the scene.
        /// </remarks>
        /// <param name="bindingFactory">The original binding factory.</param>
        /// <param name="type">The component type.</param>
        /// <param name="name">The GameObject name.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static IBinding ToGameObject(this IBinding binding, Type type, string name)
        {
            if (binding.bindingType != BindingType.SINGLETON)
            {
                binding
                    .SetBindingType(BindingType.SINGLETON)
                    .SetConstraint(ConstraintType.SINGLE);
            }

            // binding.type 和 type 不兼容就抛出异常
            if (!TypeUtils.IsAssignable(binding.type, type))
            {
                throw new BindingSystemException(BindingSystemException.TYPE_NOT_ASSIGNABLE);
            }

            // 如果参数 type 既不是 GameObject 也不是 Component 同样抛出异常
            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            var isComponent = TypeUtils.IsAssignable(typeof(Component), type);
            if (!isGameObject && !isComponent)
            {
                throw new BindingSystemException(BindingSystemException.TYPE_NOT_COMPONENT);
            }

            // 如果参数 name 为空，使用参数 type 的字符串名称来命名新创建的 GameObject
            GameObject gameObject = null;
            if (string.IsNullOrEmpty(name))
            {
                gameObject = new GameObject(type.Name);
            }
            // 否则用参数 name 查找 GameObject
            else { gameObject = GameObject.Find(name); }
            // 如果没有获取到 gameObject 就抛出异常
            if (gameObject == null)
            {
                throw new BindingSystemException(BindingSystemException.GAMEOBJECT_IS_NULL);
            }

            // 如果参数 type 是 GameObject 类型,就将 gameObject 作为 binding 的值
            if (isGameObject)
            {
                binding.value = gameObject;

                if (storing != null) { storing(this); }

                return this;
            }
            // 否则获取 gameObject 上指定类型的组件，将组件作为 binding 的值
            else
            {
                var component = gameObject.GetComponent(type);

                if (component == null)
                {
                    component = gameObject.AddComponent(type);
                }

                _value = component;

                if (storing != null) { storing(this); }

                return this;
            }
        }

        #endregion
    }
}
