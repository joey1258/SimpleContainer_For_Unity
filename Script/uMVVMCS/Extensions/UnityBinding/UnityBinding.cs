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
        #region ToGameObject

        /// <summary>
        /// 将名称与 binding.type 相同的 GameObject 作为单例 binding 的值，如果参数 type 是
        /// Component 类型，就将 Component 作为 binding 的值，同时如果 GameObject 上没有该组件，
        /// 就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObject(this IBinding binding)
        {
            return binding.ToGameObject(binding.type, null);
        }

        /// <summary>
        /// 将名称与参数 type 相同的 GameObject 作为单例 binding 的值，如果参数 type 是 Component 
        /// 类型，就将 Component 作为 binding 的值，同时如果 GameObject 上没有该组件，就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObject<T>(this IBinding binding) where T : Component
        {
            return binding.ToGameObject(typeof(T), null);
        }

        /// <summary>
        /// 将名称与参数 type 相同的 GameObject 作为单例 binding 的值，如果参数 type 是 Component 
        /// 类型，就将 Component 作为 binding 的值，同时如果 GameObject 上没有该组件，就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObject(this IBinding binding, Type type)
        {
            return binding.ToGameObject(type, null);
        }

        /// <summary>
        /// 将一个指定名称的 GameObject 作为单例 binding 的值，如果参数 type 是 Component 类型，
        /// 就将 Component 作为 binding 的值，同时如果 GameObject 上没有该组件，就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObject(this IBinding binding, string name)
        {
            return binding.ToGameObject(binding.type, name);
        }

        /// <summary>
        /// 将一个指定名称的 GameObject 作为单例 binding 的值，如果参数 type 是 Component 类型，
        /// 就将 Component 作为 binding 的值，同时如果 GameObject 上没有该组件，就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObject<T>(this IBinding binding, string name) where T : Component
        {
            return binding.ToGameObject(typeof(T), name);
        }

        /// <summary>
        /// 将一个指定名称的 GameObject 作为单例 binding 的值，如果参数 type 是 Component 类型，
        /// 就将 Component 作为 binding 的值，同时如果 GameObject 上没有该组件，就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObject(this IBinding binding, Type type, string name)
        {
            // 设置 binding 的 bindingType 和 constraint
            if (binding.bindingType != BindingType.SINGLETON)
            {
                binding.SetBindingType(BindingType.SINGLETON);
            }
            if (binding.constraint != ConstraintType.SINGLE)
            {
                binding.SetConstraint(ConstraintType.SINGLE);
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
                binding.SetValue(gameObject);
                binding.binder.Storing(binding);

                return binding;
            }
            // 否则获取 gameObject 上指定类型的组件，将组件作为 binding 的值
            else
            {
                var component = gameObject.GetComponent(type);

                if (component == null)
                {
                    component = gameObject.AddComponent(type);
                }

                binding.SetValue(component);

                return binding;
            }
        }

        #endregion

        #region ToGameObjectWithTag

        /// <summary>
        /// 将一个带有指定 tag 的 GameObject 作为单例 binding 的值，如果带有指定 tag 的 GameObject
        /// 多于1个，将取其中第一个；如果参数 type 是 Component 类型，就将 Component 作为 binding 的
        /// 值，同时如果 GameObject 上没有该组件，就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObjectWithTag(this IBinding binding, string tag)
        {
            return binding.ToGameObjectWithTag(binding.type, tag);
        }

        /// <summary>
        /// 将一个带有指定 tag 的 GameObject 作为单例 binding 的值，如果带有指定 tag 的 GameObject
        /// 多于1个，将取其中第一个；如果参数 type 是 Component 类型，就将 Component 作为 binding 的
        /// 值，同时如果 GameObject 上没有该组件，就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObjectWithTag<T>(this IBinding binding, string tag) where T : Component
        {
            return binding.ToGameObjectWithTag(typeof(T), tag);
        }

        /// <summary>
        /// 将一个带有指定 tag 的 GameObject 作为单例 binding 的值，如果带有指定 tag 的 GameObject
        /// 多于1个，将取其中第一个；如果参数 type 是 Component 类型，就将 Component 作为 binding 的
        /// 值，同时如果 GameObject 上没有该组件，就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObjectWithTag(this IBinding binding, Type type, string tag)
        {
            // 过滤不匹配的类型
            if (!TypeUtils.IsAssignable(binding.type, type))
            {
                throw new BindingSystemException(BindingSystemException.TYPE_NOT_ASSIGNABLE);
            }

            // 过滤既不是 GameObject 也不是 Component 的 type 参数
            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            var isComponent = TypeUtils.IsAssignable(typeof(Component), type);
            if (!isGameObject && !isComponent)
            {
                throw new BindingSystemException(BindingSystemException.TYPE_NOT_COMPONENT);
            }

            // 获取 GameObject
            var gameObject = GameObject.FindWithTag(tag);
            if (gameObject == null)
            {
                throw new BindingSystemException(BindingSystemException.GAMEOBJECT_IS_NULL);
            }

            // 如果参数 type 是 GameObject 类型,就将 gameObject 作为 binding 的值
            if (isGameObject)
            {
                binding.SetValue(gameObject);

                return binding;
            }
            // 否则获取 gameObject 上指定类型的组件，将组件作为 binding 的值
            else
            {
                var component = gameObject.GetComponent(type);

                if (component == null)
                {
                    component = gameObject.AddComponent(type);
                }

                binding.SetValue(component);

                return binding;
            }
        }

        #endregion

        #region ToGameObjectsWithTag

        /// <summary>
        /// 获取所有指定 tag 的 gameobject 并将它们作为多例 binding 的值，如果参数 type 是 Component
        /// 类型，就将 Component 作为 binding 的值，同时如果 GameObject 上没有该组件，就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObjectsWithTag(this IBinding binding, string tag)
        {
            return binding.ToGameObjectsWithTag(binding.type, tag);
        }


        /// <summary>
        /// 获取所有指定 tag 的 gameobject 并将它们作为多例 binding 的值，如果参数 type 是 Component
        /// 类型，就将 Component 作为 binding 的值，同时如果 GameObject 上没有该组件，就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObjectsWithTag<T>(this IBinding binding, string tag) where T : Component
        {
            return binding.ToGameObjectsWithTag(typeof(T), tag);
        }

        /// <summary>
        /// 获取所有指定 tag 的 gameobject 并将它们作为多例 binding 的值，如果参数 type 是 Component
        /// 类型，就将 Component 作为 binding 的值，同时如果 GameObject 上没有该组件，就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObjectsWithTag(this IBinding binding, Type type, string tag)
        {
            if(binding.bindingType == BindingType.FACTORY)
            {
                throw new BindingSystemException(
                    string.Format(
                        BindingSystemException.BINDINGTYPE_NOT_ASSIGNABLE,
                        "ToGameObjectsWithTag",
                        "FACTORY"));
            }

            if (!TypeUtils.IsAssignable(binding.type, type))
            {
                throw new BindingSystemException(BindingSystemException.TYPE_NOT_ASSIGNABLE);
            }

            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            var isComponent = TypeUtils.IsAssignable(typeof(Component), type);
            if (!isGameObject && !isComponent)
            {
                throw new BindingSystemException(BindingSystemException.TYPE_NOT_COMPONENT);
            }

            binding.SetBindingType(BindingType.MULTITON);
            binding.SetConstraint(ConstraintType.MULTIPLE);

            var gameObjects = GameObject.FindGameObjectsWithTag(tag);

            for (int i = 0; i < gameObjects.Length; i++)
            {
                // 如果参数 type 是 GameObject 类型,就将 gameObject 作为 binding 的值
                if (isGameObject)
                {
                    binding.SetValue(gameObjects[i]);
                }
                // 否则获取 gameObject 上指定类型的组件，将组件作为 binding 的值
                else
                {
                    var component = gameObjects[i].GetComponent(type);

                    if (component == null)
                    {
                        component = gameObjects[i].AddComponent(type);
                    }

                    binding.SetValue(component);
                }
            }

            return binding;
        }

        #endregion/**/

        /*#region ToPrefab

        public static IBindingConditionFactory ToPrefab(this IBindingFactory bindingFactory, string name)
        {
            return bindingFactory.ToPrefab(bindingFactory.bindingType, name);
        }

        public static IBindingConditionFactory ToPrefab<T>(this IBindingFactory bindingFactory, string name) where T : Component
        {
            return bindingFactory.ToPrefab(typeof(T), name);
        }

        /// <summary>
        /// Binds the key type to a transient <see cref="UnityEngine.Component"/>
        /// of <paramref name="type"/> on the prefab.
        /// 
        /// If the <see cref="UnityEngine.Component"/> is not found on the prefab
        /// at the moment of the instantiation, it will be added.
        /// </summary>
        /// <remarks>
        /// Every resolution of a transient prefab will generate a new instance. So, even
        /// if the component resolved from the prefab is destroyed, it won't generate any
        /// missing references in the container.
        /// </remarks>
        /// <param name="binding">The original binding factory.</param>
        /// <param name="type">The component type.</param>
        /// <param name="name">Prefab name. It will be loaded using <c>Resources.Load<c/>.</param>
        /// <returns>The binding condition object related to this binding.</returns>
        public static IBinding ToPrefab(this IBinding binding, Type type, string name)
        {
            if (!TypeUtils.IsAssignable(binding.type, type))
            {
                throw new BindingSystemException(BindingSystemException.TYPE_NOT_ASSIGNABLE);
            }

            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            var isComponent = TypeUtils.IsAssignable(typeof(Component), type);
            if (!isGameObject && !isComponent)
            {
                throw new BindingSystemException(BindingSystemException.TYPE_NOT_COMPONENT);
            }

            var prefab = Resources.Load(name);
            if (prefab == null)
            {
                throw new BindingException(PREFAB_IS_NULL);
            }

            var prefabBinding = new PrefabBinding(prefab, type);

            return binding.AddBinding(prefabBinding, BindingInstance.Transient);
        }

        #endregion*/
    }
}
