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
    public static class UnityBinding
    {
        #region exception text

        private const string TYPE_NOT_OBJECT = "The type must be UnityEngine.Object.";
        private const string TYPE_NOT_COMPONENT = "The type must be UnityEngine.Component.";
        private const string GAMEOBJECT_IS_NULL = "GameObject is null";
        //private const string PREFAB_IS_NULL = "prefab is null";
        private const string RESOURCE_IS_NULL = "resource is null";
        private const string VALUE_ISNOT_PREFAB = "The value must be PrefabInfo.";

        #endregion

        #region ToGameObject

        public static IBinding ToGameObject(this IBinding binding)
        {
            return binding.ToGameObject(binding.type, null);
        }

        public static IBinding ToGameObject<T>(this IBinding binding) where T : Component
        {
            return binding.ToGameObject(typeof(T), null);
        }

        public static IBinding ToGameObject(this IBinding binding, Type type)
        {
            return binding.ToGameObject(type, null);
        }

        /// <summary>
        /// 在场景中建立或获取一个指定名称的空物体并将指定类型的组件加载到空物体上，如果指定名称为空就以组件
        /// 的名称来命名空物体，如果类型是 GameObject 就直接用空物体作为 binding 的值，否则用组件作为 
        /// binding 的值，为了保证运作正常该物体生成后不应该被从场景中删除，或将组件添加到会被删除的物体上
        /// </summary>
        public static IBinding ToGameObject(this IBinding binding, string name)
        {
            return binding.ToGameObject(binding.type, name);
        }

        /// <summary>
        /// 在场景中建立或获取一个指定名称的空物体并将指定类型的组件加载到空物体上，如果指定名称为空就以组件
        /// 的名称来命名空物体，如果类型是 GameObject 就直接用空物体作为 binding 的值，否则用组件作为 
        /// binding 的值，为了保证运作正常该物体生成后不应该被从场景中删除，或将组件添加到会被删除的物体上
        /// </summary>
        public static IBinding ToGameObject<T>(this IBinding binding, string name) where T : Component
        {
            return binding.ToGameObject(typeof(T), name);
        }

        /// <summary>
        /// 在场景中建立或获取一个指定名称的空物体并将指定类型的组件加载到空物体上，如果指定名称为空就以组件
        /// 的名称来命名空物体，如果类型是 GameObject 就直接用空物体作为 binding 的值，否则用组件作为 
        /// binding 的值，为了保证运作正常该物体生成后不应该被从场景中删除，或将组件添加到会被删除的物体上
        /// </summary>
        public static IBinding ToGameObject(this IBinding binding, Type type, string name)
        {
            if (binding.bindingType == BindingType.ADDRESS)
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
            SetValueAddComponent(binding, gameObject, type, isGameObject);
            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region ToGameObjects

        /// <summary>
        /// 为多个 GameObject 添加指定类型的组件，如果 binding.type 是 Component 类型，就将 
        /// Component 作为 binding 的值，同时如果 GameObject 上没有该组件，就为其添加该组件，
        /// 为了保证运作正常该物体生成后不应该被从场景中删除，或将组件添加到会被删除的物体上
        /// </summary>
        public static IBinding ToGameObjects(
            this IBinding binding,
            Type type,
            GameObject[] gameObjects)
        {
            if (binding.bindingType == BindingType.ADDRESS)
            {
                binding.SetBindingType(BindingType.MULTITON);
                binding.SetConstraint(ConstraintType.MULTIPLE);
            }

            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            TypeFilter(binding, type, isGameObject);

            for (int i = 0; i < gameObjects.Length; i++)
            {
                // 将 gameObject 设为 binding 的值
                SetValueAddComponent(binding, gameObjects[i], type, isGameObject);
            }

            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region ToGameObjectWithTag

        public static IBinding ToGameObjectWithTag(this IBinding binding, string tag)
        {
            return binding.ToGameObjectWithTag(binding.type, tag);
        }

        public static IBinding ToGameObjectWithTag<T>(this IBinding binding, string tag) where T : Component
        {
            return binding.ToGameObjectWithTag(typeof(T), tag);
        }

        /// <summary>
        /// 获取带有指定 tag 的 GameObject，如果参数 type 是 GameObject 类型，就将 GameObject 作为 
        /// binding 的值；如果参数 type 是 Component 类型，就将 Component 作为 binding 的值，同时如
        /// 果 GameObject 上没有该组件，就为其添加该组件。为了保证运作正常该物体生成后不应该被从场景中删
        /// 除，或将组件添加到会被删除的物体上
        /// </summary>
        public static IBinding ToGameObjectWithTag(this IBinding binding, Type type, string tag)
        {
            if (binding.bindingType == BindingType.ADDRESS)
            {
                binding.SetBindingType(BindingType.SINGLETON);
                binding.SetConstraint(ConstraintType.SINGLE);
            }

            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            TypeFilter(binding, type, isGameObject);

            // 获取 GameObject
            var gameObject = GameObject.FindWithTag(tag);

            // 将 gameObject 设为 binding 的值
            SetValueAddComponent(binding, gameObject, type, isGameObject);
            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region ToGameObjectsWithTag

        public static IBinding ToGameObjectsWithTag(this IBinding binding, string tag)
        {
            return binding.ToGameObjectsWithTag(binding.type, tag);
        }

        public static IBinding ToGameObjectsWithTag<T>(this IBinding binding, string tag) where T : Component
        {
            return binding.ToGameObjectsWithTag(typeof(T), tag);
        }

        /// <summary>
        /// 获取所有带有指定 tag 的 GameObject，如果参数 type 是 GameObject 类型，就将 GameObject 作
        /// 为 binding 的值；如果参数 type 是 Component 类型，就将 Component 作为 binding 的值，同时
        /// 如果 GameObject 上没有该组件，就为其添加该组件。为了保证运作正常该物体生成后不应该被从场景中删
        /// 除，或将组件添加到会被删除的物体上
        /// </summary>
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

            if (binding.bindingType == BindingType.ADDRESS ||
                binding.bindingType == BindingType.SINGLETON)
            {
                binding.SetBindingType(BindingType.MULTITON);
                binding.SetConstraint(ConstraintType.MULTIPLE);
            }

            var gameObjects = GameObject.FindGameObjectsWithTag(tag);

            for (int i = 0; i < gameObjects.Length; i++)
            {
                // 将 gameObject 设为 binding 的值
                SetValueAddComponent(binding, gameObjects[i], type, isGameObject);
            }

            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region ToPrefab

        public static IBinding ToPrefab(this IBinding binding, string path)
        {
            return binding.ToPrefab(binding.type, path);
        }

        public static IBinding ToPrefab<T>(this IBinding binding, string path) where T : Component
        {
            return binding.ToPrefab(typeof(T), path);
        }

        /// <summary>
        /// 如果是 ADDRESS 类型，将 PrefabInfo 作为 binding 的值；否则将 PrefabInfo 的实例
        /// 化结果作为 binding 的值，如果指定的类型不是 GameObject，将会为实例添加指定类型的实例。
        /// 非 ADDRESS 类型的 binding 需要注意实例化的结果（也就是所储存的值）是否被销毁，因为这
        /// 将导致空引用 ToPrefab 方法自身会进行一次实例化，单利类型直接在方法内实例化， ADDRESS 
        /// 类型通过 bindingEvaluation 委托在 ResolveBinding 方法中设置实例化结果
        /// </summary>
        public static IBinding ToPrefab(this IBinding binding, Type type, string path)
        {
            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            TypeFilter(binding, type, isGameObject);

            var prefabInfo = new PrefabInfo(path, type);
            if (prefabInfo.prefab == null)
            {
                throw new BindingSystemException(
                    string.Format(BindingSystemException.RESOURCES_LOAD_FAILURE, path));
            }

            if (binding.bindingType == BindingType.ADDRESS)
            {
                // 将 prefabInfo 设为 binding 的值
                binding.SetValue(prefabInfo);
            }
            else if (binding.bindingType == BindingType.SINGLETON ||
                binding.bindingType == BindingType.MULTITON)
            {
                var gameObject = (GameObject)MonoBehaviour.Instantiate(prefabInfo.prefab);
                prefabInfo.useCount++;

                // 将 gameObject 设为 binding 的值
                SetValueAddComponent(binding, gameObject, type, isGameObject);
            }

            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region ToPrefabAsync

        public static IBinding ToPrefabAsync(this IBinding binding, string path)
        {
            return binding.ToPrefabAsync(binding.type, path, null, null);
        }

        public static IBinding ToPrefabAsync(this IBinding binding, string path, Action<UnityEngine.Object> _loaded)
        {
            return binding.ToPrefabAsync(binding.type, path, _loaded, null);
        }

        public static IBinding ToPrefabAsync(this IBinding binding, string path, Action<float> _progress)
        {
            return binding.ToPrefabAsync(binding.type, path, null, _progress);
        }

        public static IBinding ToPrefabAsync<T>(this IBinding binding, string path) where T : Component
        {
            return binding.ToPrefabAsync(typeof(T), path, null, null);
        }

        public static IBinding ToPrefabAsync<T>(this IBinding binding, string path, Action<UnityEngine.Object> _loaded) where T : Component
        {
            return binding.ToPrefabAsync(typeof(T), path, _loaded, null);
        }

        public static IBinding ToPrefabAsync<T>(this IBinding binding, string path, Action<float> _progress) where T : Component
        {
            return binding.ToPrefabAsync(typeof(T), path, null, _progress);
        }

        public static IBinding ToPrefabAsync<T>(this IBinding binding, string path, Action<UnityEngine.Object> _loaded, Action<float> _progress) where T : Component
        {
            return binding.ToPrefabAsync(typeof(T), path, _loaded, _progress);
        }

        /// <summary>
        /// 如果是 ADDRESS 类型，将 PrefabInfo 作为 binding 的值；否则将 PrefabInfo 的实例
        /// 化结果作为 binding 的值，如果指定的类型不是 GameObject，将会为实例添加指定类型的实例。
        /// 非 ADDRESS 类型的 binding 需要注意实例化的结果（也就是所储存的值）是否被销毁，因为这
        /// 将导致空引用 ToPrefab 方法自身会进行一次实例化，单利类型直接在方法内实例化， ADDRESS 
        /// 类型通过 bindingEvaluation 委托在 ResolveBinding 方法中设置实例化结果
        /// </summary>
        public static IBinding ToPrefabAsync(
            this IBinding binding, 
            Type type, 
            string path,
            Action<UnityEngine.Object> _loaded, 
            Action<float> _progress)
        {
            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            TypeFilter(binding, type, isGameObject);

            var prefabInfo = new PrefabInfo(path, type);
            prefabInfo.GetAsyncObject(_loaded, _progress);

            if (prefabInfo.prefab == null)
            {
                throw new BindingSystemException(
                    string.Format(BindingSystemException.RESOURCES_LOAD_FAILURE, path));
            }

            if (binding.bindingType == BindingType.ADDRESS)
            {
                // 将 prefabInfo 设为 binding 的值
                binding.SetValue(prefabInfo);
            }
            else if (binding.bindingType == BindingType.SINGLETON ||
                binding.bindingType == BindingType.MULTITON)
            {
                var gameObject = (GameObject)MonoBehaviour.Instantiate(prefabInfo.prefab);

                // 将 gameObject 设为 binding 的值
                SetValueAddComponent(binding, gameObject, type, isGameObject);
            }

            binding.binder.Storing(binding);

            return binding;
        }

        #endregion
        
        #region ToPrefabCoroutine

        public static IBinding ToPrefabCoroutine(this IBinding binding, string path)
        {
            return binding.ToPrefabCoroutine(binding.type, path, null);
        }

        public static IBinding ToPrefabCoroutine(this IBinding binding, string path, Action<UnityEngine.Object> _loaded)
        {
            return binding.ToPrefabCoroutine(binding.type, path, _loaded);
        }

        public static IBinding ToPrefabCoroutine<T>(this IBinding binding, string path) where T : Component
        {
            return binding.ToPrefabCoroutine(typeof(T), path, null);
        }

        public static IBinding ToPrefabCoroutine<T>(this IBinding binding, string path, Action<UnityEngine.Object> _loaded) where T : Component
        {
            return binding.ToPrefabCoroutine(typeof(T), path, _loaded);
        }

        /// <summary>
        /// 如果是 ADDRESS 类型，将 PrefabInfo 作为 binding 的值；否则将 PrefabInfo 的实例
        /// 化结果作为 binding 的值，如果指定的类型不是 GameObject，将会为实例添加指定类型的实例。
        /// 非 ADDRESS 类型的 binding 需要注意实例化的结果（也就是所储存的值）是否被销毁，因为这
        /// 将导致空引用 ToPrefab 方法自身会进行一次实例化，单利类型直接在方法内实例化， ADDRESS 
        /// 类型通过 bindingEvaluation 委托在 ResolveBinding 方法中设置实例化结果
        /// </summary>
        public static IBinding ToPrefabCoroutine(
            this IBinding binding,
            Type type,
            string path,
            Action<UnityEngine.Object> _loaded)
        {
            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            TypeFilter(binding, type, isGameObject);

            var prefabInfo = new PrefabInfo(path, type);
            prefabInfo.GetCoroutineObject(_loaded);

            if (prefabInfo.prefab == null)
            {
                throw new BindingSystemException(
                    string.Format(BindingSystemException.RESOURCES_LOAD_FAILURE, path));
            }

            if (binding.bindingType == BindingType.ADDRESS)
            {
                // 将 prefabInfo 设为 binding 的值
                binding.SetValue(prefabInfo);
            }
            else if (binding.bindingType == BindingType.SINGLETON ||
                binding.bindingType == BindingType.MULTITON)
            {
                var gameObject = (GameObject)MonoBehaviour.Instantiate(prefabInfo.prefab);

                // 将 gameObject 设为 binding 的值
                SetValueAddComponent(binding, gameObject, type, isGameObject);
            }

            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region Instantiate

        public static IBinding Instantiate(this IBinding binding)
        {
            return Instantiate(binding, null, 1);
        }

        public static IBinding Instantiate(
            this IBinding binding, 
            Action<object> handle)
        {
            return Instantiate(binding, handle, 1);
        }

        /// <summary>
        /// 实例化 binding 的 PrefabInfo 类型值指定次数，并提供一个可供操作实例化结果的委托
        /// 需注意 ToPrefab 方法自身会进行一次实例化，在其后再调用 Instantiate 方法会实例化出多个物体
        /// </summary>
        public static IBinding Instantiate(
            this IBinding binding, 
            Action<object> handle, 
            int times)
        {
            if (!(binding.value is PrefabInfo)){ throw new Exception(VALUE_ISNOT_PREFAB); }

            var prefabInfo = (PrefabInfo)binding.value;
            if (prefabInfo.prefab == null)
            {
                throw new BindingSystemException(
                    string.Format(BindingSystemException.RESOURCES_LOAD_FAILURE));
            }

            prefabInfo.useCount++;

            if (prefabInfo.type.Equals(typeof(GameObject)))
            {
                GameObject[] gameObjectList = new GameObject[times];

                for (int i = 0; i < times; i++)
                {
                    var gameObject = (GameObject)MonoBehaviour.Instantiate(prefabInfo.prefab);
                    gameObjectList[i] = gameObject;
                }

                if (handle != null) { handle(gameObjectList); }
            }
            else
            {
                Component[] componentList = new Component[times];

                for (int i = 0; i < times; i++)
                {
                    var gameObject = (GameObject)MonoBehaviour.Instantiate(prefabInfo.prefab);
                    var component = gameObject.GetComponent(prefabInfo.type);

                    if (component == null)
                    {
                        component = gameObject.AddComponent(prefabInfo.type);
                        componentList[i] = component;
                    }
                }

                if (handle != null) { handle(componentList); }
            }

            return binding;
        }

        #endregion

        #region ToUnload

        /// <summary>
        /// 从内存和 binding 中移除指定资源
        /// </summary>
        public static IBinding ToUnload(this IBinding binding, object value)
        {
            var prefabInfo = (PrefabInfo)value;
            Resources.UnloadAsset(prefabInfo.prefab);

            binding.RemoveValue(value);
            //TODO:: AssetBundle unload

            return binding;
        }

        #endregion

        private static void SetValueAddComponent(IBinding binding,
            GameObject gameObject,
            Type type,
            bool typeIsGameObject)
        {
            // 如果参数 gameObject 为空就抛出异常
            if (gameObject == null) { throw new Exception(GAMEOBJECT_IS_NULL); }

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

            if (!isGameObject && !isComponent) { throw new Exception(TYPE_NOT_COMPONENT); }
        }
    }
}
