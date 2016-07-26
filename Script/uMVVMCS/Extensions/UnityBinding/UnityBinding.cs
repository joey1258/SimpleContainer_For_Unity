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
        /// 用 type 的字符串名称新建的 GameObject 作为单例 binding 的值，如果参数 type 是 Component 
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
        /// 将一个指定名称的 GameObject 作为单例 binding 的值，如果 binding.type 是 Component 类型，
        /// 就将 Component 作为 binding 的值，同时如果 GameObject 上没有该组件，就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObject(this IBinding binding, Type type, string name)
        {
            if (binding.bindingType == BindingType.TEMP)
            {
                binding.SetBindingType(BindingType.SINGLETON);
                binding.SetConstraint(ConstraintType.SINGLE);
            }

            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            TypeFilter(binding, type, isGameObject);

            // 如果参数 name 为空，使用参数 type 的字符串名称来命名新创建的 GameObject
            GameObject gameObject = null;
            if (string.IsNullOrEmpty(name))
            {
                gameObject = new GameObject(type.Name);
            }
            // 否则用参数 name 查找 GameObject
            else { gameObject = GameObject.Find(name); }

            // 将 gameObject 设为 binding 的值
            SetBindingValue(binding, gameObject, type, isGameObject);
            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region ToGameObjects

        /// <summary>
        /// 将多个 GameObject 作为单例 binding 的值，如果 binding.type 是 Component 类型，
        /// 就将 Component 作为 binding 的值，同时如果 GameObject 上没有该组件，就为其添加该组件
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToGameObjects(
            this IBinding binding, 
            Type type, 
            GameObject[] gameObjects)
        {
            if (binding.bindingType == BindingType.TEMP)
            {
                binding.SetBindingType(BindingType.MULTITON);
                binding.SetConstraint(ConstraintType.MULTIPLE);
            }

            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            TypeFilter(binding, type, isGameObject);

            for (int i = 0; i < gameObjects.Length; i++)
            {
                // 将 gameObject 设为 binding 的值
                SetBindingValue(binding, gameObjects[i], type, isGameObject);
            }

            binding.binder.Storing(binding);

            return binding;
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
            if (binding.bindingType == BindingType.TEMP)
            {
                binding.SetBindingType(BindingType.SINGLETON);
                binding.SetConstraint(ConstraintType.SINGLE);
            }

            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            TypeFilter(binding, type, isGameObject);

            // 获取 GameObject
            var gameObject = GameObject.FindWithTag(tag);

            // 将 gameObject 设为 binding 的值
            SetBindingValue(binding, gameObject, type, isGameObject);
            binding.binder.Storing(binding);

            return binding;
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

            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            TypeFilter(binding, type, isGameObject);

            if (binding.bindingType == BindingType.TEMP ||
                binding.bindingType == BindingType.SINGLETON)
            {
                binding.SetBindingType(BindingType.MULTITON);
                binding.SetConstraint(ConstraintType.MULTIPLE);
            }

            var gameObjects = GameObject.FindGameObjectsWithTag(tag);

            for (int i = 0; i < gameObjects.Length; i++)
            {
                // 将 gameObject 设为 binding 的值
                SetBindingValue(binding, gameObjects[i], type, isGameObject);
            }

            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region ToPrefab

        /// <summary>
        /// 将带有参数 type 类型组件的 prefab 作为 binding 的值，如果实例化时 prefab 上没有组件，
        /// 实例化时将会进行添加
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToPrefab(this IBinding binding, string path)
        {
            return binding.ToPrefab(binding.type, path);
        }

        /// <summary>
        /// 将带有参数 type 类型组件的 prefab 作为 binding 的值，如果实例化时 prefab 上没有组件，
        /// 实例化时将会进行添加
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToPrefab<T>(this IBinding binding, string path) where T : Component
        {
            return binding.ToPrefab(typeof(T), path);
        }

        /// <summary>
        /// 将带有参数 type 类型组件的 prefab 作为 binding 的值，如果实例化时 prefab 上没有组件，
        /// 实例化时将会进行添加
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToPrefab(this IBinding binding, Type type, string path)
        {
            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            TypeFilter(binding, type, isGameObject);

            // 此处需要思考如何与 ResManager 结合
            var prefab = Resources.Load(path);
            if (prefab == null)
            {
                throw new BindingSystemException(BindingSystemException.PREFAB_IS_NULL);
            }

            var prefabBinding = new PrefabBinding(prefab, type);

            // 将 gameObject 设为 binding 的值
            binding.SetValue(prefabBinding);
            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region ToResource

        /// <summary>
        /// 将从指定路径中读取的资源作为 binding 的值
        /// </summary>
        /// <remarks>
        /// 只有绑定场景中不会被销毁的游戏物体才可以防止对已销毁的对象的引用
        /// </remarks>
        public static IBinding ToResource(this IBinding binding, string path)
        {
            if (!TypeUtils.IsAssignable(typeof(UnityEngine.Object), binding.type))
            {
                throw new BindingSystemException(BindingSystemException.TYPE_NOT_OBJECT);
            }

            var resource = Resources.Load(path);
            if (resource == null)
            {
                throw new BindingSystemException(BindingSystemException.RESOURCE_IS_NULL);
            }

            // 将 gameObject 设为 binding 的值
            binding.SetValue(resource);
            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        private static void SetBindingValue(IBinding binding,
            GameObject gameObject,
            Type type,
            bool typeIsGameObject)
        {
            // 如果参数 gameObject 为空就抛出异常
            if (gameObject == null)
            {
                throw new BindingSystemException(BindingSystemException.GAMEOBJECT_IS_NULL);
            }

            // 如果参数 type 是 GameObject 类型,就将 gameObject 作为 binding 的值
            if (typeIsGameObject)
            {
                binding.SetValue(gameObject);
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
            }
        }

        private static void TypeFilter(IBinding binding, Type type, bool isGameObject)
        {
            // 过滤不匹配的类型
            if (!TypeUtils.IsAssignable(binding.type, type))
            {
                throw new BindingSystemException(BindingSystemException.TYPE_NOT_ASSIGNABLE);
            }

            // 过滤既不是 GameObject 也不是 Component 的 type 参数
            var isComponent = TypeUtils.IsAssignable(typeof(Component), type);
            if (!isGameObject && !isComponent)
            {
                throw new BindingSystemException(BindingSystemException.TYPE_NOT_COMPONENT);
            }
        }
    }
}
