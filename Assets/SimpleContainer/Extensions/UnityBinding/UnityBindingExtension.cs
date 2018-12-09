/**
 * This file is part of SimpleContainer.
 *
 * Licensed under The MIT License
 * For full copyright and license information, please see the MIT-LICENSE.txt
 * Redistributions of files must retain the above copyright notice.
 *
 * @copyright Joey1258
 * @link https://github.com/joey1258/SimpleContainer_For_Unity
 * @license http://www.opensource.org/licenses/mit-license.php MIT License
 */

using System;
using UnityEngine;
using Utils;

namespace SimpleContainer.Container
{
    public static class UnityBindingExtension
    {
        #region ToGameObject & ToGameObjectDDOL

        public static IBinding ToGameObject(this IBinding binding)
        {
            return binding.ToGameObject(false, binding.type, null);
        }

        public static IBinding ToGameObject<T>(this IBinding binding) where T : Component
        {
            return binding.ToGameObject(false, typeof(T), null);
        }

        public static IBinding ToGameObject(this IBinding binding, Type type)
        {
            return binding.ToGameObject(false, type, null);
        }

        /// <summary>
        /// 在场景中建立或获取一个指定名称的空物体并将指定类型的组件加载到空物体上，如果指定名称为空就以组件
        /// 的名称来命名空物体，如果类型是 GameObject 就直接用空物体作为 binding 的值，否则用组件作为 
        /// binding 的值，为了保证运作正常该物体生成后不应该被从场景中删除，或将组件添加到会被删除的物体上
        /// </summary>
        public static IBinding ToGameObject(this IBinding binding, string name)
        {
            return binding.ToGameObject(false, binding.type, name);
        }

        /// <summary>
        /// 在场景中建立或获取一个指定名称的空物体并将指定类型的组件加载到空物体上，如果指定名称为空就以组件
        /// 的名称来命名空物体，如果类型是 GameObject 就直接用空物体作为 binding 的值，否则用组件作为 
        /// binding 的值，为了保证运作正常该物体生成后不应该被从场景中删除，或将组件添加到会被删除的物体上
        /// </summary>
        public static IBinding ToGameObject<T>(this IBinding binding, string name) where T : Component
        {
            return binding.ToGameObject(false, typeof(T), name);
        }

        public static IBinding ToGameObjectDDOL(this IBinding binding)
        {
            return binding.ToGameObject(true, binding.type, null);
        }

        public static IBinding ToGameObjectDDOL<T>(this IBinding binding) where T : Component
        {
            return binding.ToGameObject(true, typeof(T), null);
        }

        public static IBinding ToGameObjectDDOL(this IBinding binding, Type type)
        {
            return binding.ToGameObject(true, type, null);
        }

        /// <summary>
        /// 在场景中建立或获取一个指定名称的空物体并将指定类型的组件加载到空物体上，如果指定名称为空就以组件
        /// 的名称来命名空物体，如果类型是 GameObject 就直接用空物体作为 binding 的值，否则用组件作为 
        /// binding 的值，为了保证运作正常该物体生成后不应该被从场景中删除，或将组件添加到会被删除的物体上
        /// </summary>
        public static IBinding ToGameObjectDDOL(this IBinding binding, string name)
        {
            return binding.ToGameObject(true, binding.type, name);
        }

        /// <summary>
        /// 在场景中建立或获取一个指定名称的空物体并将指定类型的组件加载到空物体上，如果指定名称为空就以组件
        /// 的名称来命名空物体，如果类型是 GameObject 就直接用空物体作为 binding 的值，否则用组件作为 
        /// binding 的值，为了保证运作正常该物体生成后不应该被从场景中删除，或将组件添加到会被删除的物体上
        /// </summary>
        public static IBinding ToGameObjectDDOL<T>(this IBinding binding, string name) where T : Component
        {
            return binding.ToGameObject(true, typeof(T), name);
        }

        /// <summary>
        /// 在场景中建立或获取一个指定名称的空物体并将指定类型的组件加载到空物体上，如果指定名称为空就以组件
        /// 的名称来命名空物体，如果类型是 GameObject 就直接用空物体作为 binding 的值，否则用组件作为 
        /// binding 的值，为了保证运作正常该物体生成后不应该被从场景中删除，或将组件添加到会被删除的物体上
        /// </summary>
        public static IBinding ToGameObject(this IBinding binding, bool DDOL, Type type, string name)
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
            else
            {
                gameObject = GameObject.Find(name);
            }

            // 根据 DDOL 参数的值来决定是否设置切换场景时不销毁 gameObject
            if (DDOL) { MonoBehaviour.DontDestroyOnLoad(gameObject); }

            // 将 gameObject 设为 binding 的值
            SetValueAddComponent(binding, gameObject, type, isGameObject);
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
        /// 类型通过 beforeDefaultInstantiate 委托在 ResolveBinding 方法中设置实例化结果
        /// </summary>
        public static IBinding ToPrefab(this IBinding binding, Type type, string path)
        {
            var isGameObject = TypeUtils.IsAssignable(typeof(GameObject), type);
            TypeFilter(binding, type, isGameObject);

            var prefabInfo = new PrefabInfo(path, type);
            if (prefabInfo.prefab == null)
            {
                throw new Exceptions(
                    string.Format(Exceptions.RESOURCES_LOAD_FAILURE, path));
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

        #region ToAssetBundleFromFile

        /// <summary>
        /// 绑定 AssetBundle 资源，将 AssetBundleInfo 作为 binding 的值.
        /// </summary>
        public static IBinding ToAssetBundleFromFile(this IBinding binding, string url)
        {
            var assetBundleInfo = new AssetBundleInfo(url);
            assetBundleInfo.LoadFromFile();

            if (binding.bindingType != BindingType.ADDRESS)
            {
                binding.SetBindingType(BindingType.SINGLETON);
            }

            binding.SetValue(assetBundleInfo);

            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region ToAssetBundleNewWWW

        /// <summary>
        /// 通过新建 www 绑定 AssetBundle 资源，将 AssetBundleInfo 作为 binding 的值.
        /// </summary>
        public static IBinding ToAssetBundleFromNewWWW(this IBinding binding, string url)
        {
            var assetBundleInfo = new AssetBundleInfo(url);
            assetBundleInfo.LoadFromMemory_NW();

            if (binding.bindingType != BindingType.ADDRESS)
            {
                binding.SetBindingType(BindingType.SINGLETON);
            }

            binding.SetValue(assetBundleInfo);

            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region ToAssetBundleFromCacheOrDownload

        /// <summary>
        /// 通过 www.LoadFromCacheOrDownload 绑定 AssetBundle 资源，将 AssetBundleInfo 作为
        /// binding 的值.
        /// </summary>
        public static IBinding ToAssetBundleFromCacheOrDownload(this IBinding binding, string url)
        {
            var assetBundleInfo = new AssetBundleInfo(url);
            assetBundleInfo.LoadFromCacheOrDownload();

            if (binding.bindingType != BindingType.ADDRESS)
            {
                binding.SetBindingType(BindingType.SINGLETON);
            }

            binding.SetValue(assetBundleInfo);

            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region ToResource

        /// <summary>
        /// 绑定一个单例的资源但并不进行实例化（用于不需要立即实例化的场景或声音等非 prefab 资源）
        /// </summary>
        public static IBinding ToResource(this IBinding binding, string name)
        {
            if (!TypeUtils.IsAssignable(typeof(UnityEngine.Object), binding.type))
            {
                throw new Exceptions(
                    string.Format(Exceptions.NON_SPECIFIED_TYPE, "UnityEngine.Object"));
            }

            var resource = Resources.Load(name);
            if (resource == null)
            {
                throw new Exceptions(
                    string.Format(Exceptions.RESOURCE_LOAD_FAILURE, name));
            }

            binding.SetBindingType(BindingType.SINGLETON);
            binding.SetValue(resource);

            binding.binder.Storing(binding);

            return binding;
        }

        /// <summary>
        /// 绑定一个指定类型的单例的资源但并不进行实例化（用于不需要立即实例化的场景或声音等非 prefab 资源）
        /// </summary>
        public static IBinding ToResource(this IBinding binding, string name, Type type)
        {
            if (!TypeUtils.IsAssignable(typeof(UnityEngine.Object), binding.type))
            {
                throw new Exceptions(
                    string.Format(Exceptions.NON_SPECIFIED_TYPE, "UnityEngine.Object"));
            }

            var resource = Resources.Load(name, type);
            if (resource == null)
            {
                throw new Exceptions(
                    string.Format(Exceptions.RESOURCE_LOAD_FAILURE, name));
            }

            binding.SetBindingType(BindingType.SINGLETON);
            binding.SetValue(resource);

            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region Instantiate

        /// <summary>
        /// 实例化符合条件的各类型 binding 的值，并可从委托中操作实例化的结果；
        /// 参数 handle 委托的第一个参数为实例化后的 GameObject，第二个参数为实例化后的 Component;
        /// 参数 gameObjectName 为生成后 gameobject 的名称；
        /// AssetBundle 与 Resource 资源实例化之后无法挂载 Component，在这种情况下请自行选择合适的方式进行挂载；
        /// （需注意，如在 ToPrefab 方法之后执行，ToPrefab 方法自身会进行一次实例化，
        /// 因此 Instantiate 方法实际上进行的是第二次的实例化，所以场景中会产生两个实例）。
        /// </summary>
        public static IBinding Instantiate(this IBinding binding)
        {
            return Instantiate(binding, null, null);
        }

        /// <summary>
        /// 实例化符合条件的各类型 binding 的值，并可从委托中操作实例化的结果；
        /// 参数 handle 委托的第一个参数为实例化后的 GameObject，第二个参数为实例化后的 Component;
        /// 参数 gameObjectName 为生成后 gameobject 的名称；
        /// AssetBundle 与 Resource 资源实例化之后无法挂载 Component，在这种情况下请自行选择合适的方式进行挂载；
        /// （需注意，如在 ToPrefab 方法之后执行，ToPrefab 方法自身会进行一次实例化，
        /// 因此 Instantiate 方法实际上进行的是第二次的实例化，所以场景中会产生两个实例）。
        /// </summary>
        public static IBinding Instantiate(
            this IBinding binding,
            string gameObjectName)
        {
            return Instantiate(binding, null, gameObjectName);
        }

        /// <summary>
        /// 实例化符合条件的各类型 binding 的值，并可从委托中操作实例化的结果；
        /// 参数 handle 委托的第一个参数为实例化后的 GameObject，第二个参数为实例化后的 Component;
        /// 参数 gameObjectName 为生成后 gameobject 的名称；
        /// AssetBundle 与 Resource 资源实例化之后无法挂载 Component，在这种情况下请自行选择合适的方式进行挂载；
        /// （需注意，如在 ToPrefab 方法之后执行，ToPrefab 方法自身会进行一次实例化，
        /// 因此 Instantiate 方法实际上进行的是第二次的实例化，所以场景中会产生两个实例）。
        /// </summary>
        public static IBinding Instantiate(
            this IBinding binding,
            Action<object, object> handle)
        {
            return Instantiate(binding, handle, null);
        }

        /// <summary>
        /// 实例化符合条件的各类型 binding 的值，并可从委托中操作实例化的结果；
        /// 参数 handle 委托的第一个参数为实例化后的 GameObject，第二个参数为实例化后的 Component;
        /// 参数 gameObjectName 为生成后 gameobject 的名称；
        /// AssetBundle 与 Resource 资源实例化之后无法挂载 Component，在这种情况下请自行选择合适的方式进行挂载；
        /// （需注意，如在 ToPrefab 方法之后执行，ToPrefab 方法自身会进行一次实例化，
        /// 因此 Instantiate 方法实际上进行的是第二次的实例化，所以场景中会产生两个实例）。
        /// </summary>
        public static IBinding Instantiate(
            this IBinding binding,
            Action<object, object> handle,
            string gameObjectName)
        {
            ResolveResources(binding, handle, gameObjectName);
            return binding;
        }

        #endregion

        #region Unload

        /// <summary>
        /// 从内存和 binding 中移除指定资源
        /// </summary>
        public static IBinding Unload(this IBinding binding, object value)
        {
            var prefabInfo = (PrefabInfo)value;
            Resources.UnloadAsset(prefabInfo.prefab);

            binding.RemoveValue(value);

            return binding;
        }

        #endregion

        #region UnloadAssetBundle

        /// <summary>
        /// 释放资源(当参数为假时，asset 内的数据将被清除，之后无法再从 asset 进行任何加载，但已经加载的保持不变，为真时则清除所有，即使已经加载了的一并清除，对它们的引用将会失效)
        /// </summary>
        public static IBinding UnloadAssetBundle(this IBinding binding, bool unloadAllLoadedObjects)
        {
            if (binding.value.GetType() != typeof(AssetBundleInfo))
            {
                throw new Exceptions(
                    string.Format(Exceptions.NON_SPECIFIED_TYPE, "AssetBundleInfo"));
            }

            ((AssetBundleInfo)binding.value).Dispose(unloadAllLoadedObjects);

            return binding;
        }

        #endregion

        #region CleanCache

        /// <summary>
        /// 释放当前程序中所有 www 对象缓存到本地的资源
        /// </summary>
        public static void CleanCache()
        {
            Caching.ClearCache();
        }

        #endregion

        #region private functions

        private static void SetValueAddComponent(IBinding binding,
            GameObject gameObject,
            Type type,
            bool typeIsGameObject)
        {
            // 如果参数 gameObject 为空就抛出异常
            if (gameObject == null) { throw new Exceptions(Exceptions.GAMEOBJECT_IS_NULL); }

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
                throw new Exceptions(Exceptions.TYPE_NOT_ASSIGNABLE);
            }

            // 过滤既不是 GameObject 也不是 Component 的 type 参数
            var isComponent = TypeUtils.IsAssignable(typeof(Component), type);

            if (!isGameObject && !isComponent)
            {
                throw new Exceptions(
                    string.Format(Exceptions.NON_SPECIFIED_TYPE, "UnityEngine.Component"));
            }
        }

        #region Instantiate assist

        private static void ResolveResources(
            this IBinding binding,
            Action<object, object> handle,
            string gameObjectName)
        {
            GameObject gameObject = InstantiateGameObject(binding, gameObjectName);

            Component component = null;

            if (!(binding.value is AssetBundleInfo))
            {
                // 如果没有获取到符合类型的 Component 就忽略，留给外部方法在委托中判断。
                component = gameObject.GetComponent(binding.type);
            }

            if (!String.IsNullOrEmpty(gameObjectName))
            {
                gameObject.name = gameObjectName;
            }

            if (handle != null) { handle(gameObject, component); }
        }

        private static GameObject InstantiateGameObject(IBinding binding, string gameObjectName)
        {
            GameObject gameObject = null;
            if (binding.value is PrefabInfo)
            {
                var prefabInfo = (PrefabInfo)binding.value;
                prefabInfo.useCount++;
                gameObject = (GameObject)MonoBehaviour.Instantiate(prefabInfo.prefab);
            }
            else if (binding.value is AssetBundleInfo)
            {
                if (String.IsNullOrEmpty(gameObjectName))
                {
                    throw new Exceptions("asetBundle ObjectName can not be Null!");
                }
                AssetBundleInfo asi = (AssetBundleInfo)binding.value;
                gameObject = (GameObject)MonoBehaviour.Instantiate(asi.asetBundle.LoadAsset(gameObjectName));
            }
            else
            {
                if (binding.value is UnityEngine.Object)
                {
                    gameObject = (GameObject)MonoBehaviour.Instantiate((UnityEngine.Object)binding.value);
                }
                else
                {
                    throw new Exceptions(
                        string.Format(Exceptions.NON_SPECIFIED_TYPE, "UnityEngine.Object"));
                }
            }

            if (gameObject == null)
            {
                throw new Exceptions("Instantiate Failed!");
            }

            return gameObject;
        }

        #endregion

        #endregion
    }
}
