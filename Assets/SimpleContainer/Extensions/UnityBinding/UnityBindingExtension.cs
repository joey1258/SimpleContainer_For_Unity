/**
 * This file is part of SimpleContainer.
 *
 * Licensed under The MIT License
 * For full copyright and license information, please see the MIT-LICENSE.txt
 * Redistributions of files must retain the above copyright notice.
 *
 * @copyright Joey1258
 * @link https://github.com/joey1258/SimpleContainer
 * @license http://www.opensource.org/licenses/mit-license.php MIT License
 */

using System;
using UnityEngine;
using Utils;

namespace SimpleContainer.Container
{
    public static class UnityBindingExtension
    {
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

        #region ToGameObjectDDOL

        public static IBinding ToGameObjectDDOL(this IBinding binding)
        {
            return binding.ToGameObjectDDOL(binding.type, null);
        }

        public static IBinding ToGameObjectDDOL<T>(this IBinding binding) where T : Component
        {
            return binding.ToGameObjectDDOL(typeof(T), null);
        }

        public static IBinding ToGameObjectDDOL(this IBinding binding, Type type)
        {
            return binding.ToGameObjectDDOL(type, null);
        }

        /// <summary>
        /// 在场景中建立或获取一个指定名称的空物体并将指定类型的组件加载到空物体上，如果指定名称为空就以组件
        /// 的名称来命名空物体，如果类型是 GameObject 就直接用空物体作为 binding 的值，否则用组件作为 
        /// binding 的值，为了保证运作正常该物体生成后不应该被从场景中删除，或将组件添加到会被删除的物体上
        /// </summary>
        public static IBinding ToGameObjectDDOL(this IBinding binding, string name)
        {
            return binding.ToGameObjectDDOL(binding.type, name);
        }

        /// <summary>
        /// 在场景中建立或获取一个指定名称的空物体并将指定类型的组件加载到空物体上，如果指定名称为空就以组件
        /// 的名称来命名空物体，如果类型是 GameObject 就直接用空物体作为 binding 的值，否则用组件作为 
        /// binding 的值，为了保证运作正常该物体生成后不应该被从场景中删除，或将组件添加到会被删除的物体上
        /// </summary>
        public static IBinding ToGameObjectDDOL<T>(this IBinding binding, string name) where T : Component
        {
            return binding.ToGameObjectDDOL(typeof(T), name);
        }

        /// <summary>
        /// 在场景中建立或获取一个指定名称的空物体并将指定类型的组件加载到空物体上，如果指定名称为空就以组件
        /// 的名称来命名空物体，如果类型是 GameObject 就直接用空物体作为 binding 的值，否则用组件作为 
        /// binding 的值，为了保证运作正常该物体生成后不应该被从场景中删除，或将组件添加到会被删除的物体上
        /// </summary>
        public static IBinding ToGameObjectDDOL(this IBinding binding, Type type, string name)
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
                MonoBehaviour.DontDestroyOnLoad(gameObject);
            }
            // 否则用参数 name 查找 GameObject
            else {
                gameObject = GameObject.Find(name);
                MonoBehaviour.DontDestroyOnLoad(gameObject);
            }

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
                throw new Exceptions(
                    string.Format(
                        Exceptions.BINDINGTYPE_NOT_ASSIGNABLE,
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
        /// 类型通过 beforeDefaultInstantiate 委托在 ResolveBinding 方法中设置实例化结果
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
            CoroutineUtils.Instance.StartCoroutine(prefabInfo.GetAsyncObject(_loaded, _progress));

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
        /// 类型通过 beforeDefaultInstantiate 委托在 ResolveBinding 方法中设置实例化结果
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
            CoroutineUtils.Instance.StartCoroutine(prefabInfo.GetCoroutineObject(_loaded));

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

        #region ToAssetBundleAsyncFromFile

        public static IBinding ToAssetBundleAsyncFromFile(this IBinding binding, string url)
        {
            return binding.ToAssetBundleAsyncFromFile(url, null, null);
        }

        public static IBinding ToAssetBundleAsyncFromFile(this IBinding binding, string url, Action<UnityEngine.Object> _loaded)
        {
            return binding.ToAssetBundleAsyncFromFile(url, _loaded, null);
        }

        public static IBinding ToAssetBundleAsyncFromFile(this IBinding binding, string url, Action<float> _progress)
        {
            return binding.ToAssetBundleAsyncFromFile(url, null, _progress);
        }

        /// <summary>
        /// 异步绑定 AssetBundle 资源，将 AssetBundleInfo 作为 binding 的值
        /// </summary>
        public static IBinding ToAssetBundleAsyncFromFile(
            this IBinding binding,
            string url,
            Action<UnityEngine.Object> _loaded,
            Action<float> _progress)
        {
            var assetBundleInfo = new AssetBundleInfo(url);
            CoroutineUtils.Instance.StartCoroutine(assetBundleInfo.GetAsyncFromFile(_loaded, _progress));

            if (binding.bindingType != BindingType.ADDRESS)
            {
                binding.SetBindingType(BindingType.SINGLETON);
            }

            binding.SetValue(assetBundleInfo);

            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region ToAssetBundleCoroutineFromFile

        public static IBinding ToAssetBundleCoroutineFromFile(this IBinding binding, string url)
        {
            return binding.ToAssetBundleCoroutineFromFile(url, null);
        }

        /// <summary>
        /// 携程绑定 AssetBundle 资源，将 AssetBundleInfo 作为 binding 的值
        /// </summary>
        public static IBinding ToAssetBundleCoroutineFromFile(
            this IBinding binding,
            string url,
            Action<UnityEngine.Object> _loaded)
        {
            var assetBundleInfo = new AssetBundleInfo(url);
            CoroutineUtils.Instance.StartCoroutine(assetBundleInfo.GetCoroutineFromFile(_loaded));
            if (assetBundleInfo.asetBundle == null)
            {
                throw new Exceptions(
                    string.Format(Exceptions.ASSETBUNDLE_LOAD_FAILURE, url));
            }

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

        #region ToAssetBundleCoroutineFromCacheOrDownload

        public static IBinding ToAssetBundleCoroutineFromCacheOrDownload(this IBinding binding, string url)
        {
            return binding.ToAssetBundleCoroutineFromCacheOrDownload(url, null);
        }

        /// <summary>
        /// 携程通过 www.LoadFromCacheOrDownload 绑定 AssetBundle 资源，将 AssetBundleInfo 作为
        /// binding 的值.
        /// </summary>
        public static IBinding ToAssetBundleCoroutineFromCacheOrDownload(
            this IBinding binding,
            string url,
            Action<UnityEngine.Object> _loaded)
        {
            var assetBundleInfo = new AssetBundleInfo(url);
            CoroutineUtils.Instance.StartCoroutine(assetBundleInfo.LoadCoroutineFromCacheOrDownload(_loaded));
            if (assetBundleInfo.asetBundle == null)
            {
                throw new Exceptions(
                    string.Format(Exceptions.ASSETBUNDLE_LOAD_FAILURE, url));
            }

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
        /// 绑定一个多例的资源但并不进行实例化（用于不需要立即实例化的场景或声音等非 prefab 资源）
        /// </summary>
        public static IBinding ToResources(this IBinding binding, string[] names)
        {
            if (!TypeUtils.IsAssignable(typeof(UnityEngine.Object), binding.type))
            {
                throw new Exceptions(
                    string.Format(Exceptions.NON_SPECIFIED_TYPE, "UnityEngine.Object"));
            }

            binding.SetBindingType(BindingType.MULTITON);

            for (int i = 0; i < names.Length; i++)
            {
                var resource = Resources.Load(names[i]);
                if (resource == null)
                {
                    throw new Exceptions(
                        string.Format(Exceptions.RESOURCE_LOAD_FAILURE, names[i]));
                }

                binding.SetValue(resource);
            }

            binding.binder.Storing(binding);

            return binding;
        }

        #endregion

        #region Instantiate

        public static IBinding Instantiate(this IBinding binding)
        {
            return Instantiate(binding, null, 1, null);
        }

        public static IBinding Instantiate(
            this IBinding binding,
            string name)
        {
            return Instantiate(binding, null, 1, name);
        }

        public static IBinding Instantiate(
            this IBinding binding,
            Action<object> handle)
        {
            return Instantiate(binding, handle, 1, null);
        }

        public static IBinding Instantiate(
            this IBinding binding,
            Action<object> handle,
            string name)
        {
            return Instantiate(binding, handle, 1, name);
        }

        /// <summary>
        /// 实例化 binding 的 PrefabInfo 类型值指定次数，并提供一个可供操作实例化结果的委托
        /// 需注意 ToPrefab 方法自身会进行一次实例化，在其后再调用 Instantiate 方法会实例化出多个物体
        /// </summary>
        public static IBinding Instantiate(
            this IBinding binding,
            Action<object> handle,
            int times,
            string name = null,
            string gameobjectName = null)
        {
            if (binding.value is PrefabInfo)
            {
                InstantiatePrefabInfo(binding, handle, times, name, gameobjectName);
            }
            else if (binding.value is AssetBundleInfo)
            {
                InstantiateAssetBundleInfo(binding, handle, times, name, gameobjectName);
            }
            else
            {
                TryInstantiateObject(binding, handle, times, name, gameobjectName);
            }

            return binding;
        }

        #endregion

        #region ToUnload

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
            Caching.CleanCache();
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

        private static void InstantiatePrefabInfo(
            this IBinding binding,
            Action<object> handle,
            int times,
            string name = null,
            string gameobjectName = null)
        {
            var prefabInfo = (PrefabInfo)binding.value;
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
        }

        private static void InstantiateAssetBundleInfo(
            this IBinding binding,
            Action<object> handle,
            int times,
            string name = null,
            string gameobjectName = null)
        {
            AssetBundleInfo asi = (AssetBundleInfo)binding.value;

            if (!binding.type.Equals(typeof(AssetBundleInfo)))
            {
                Component[] componentList = new Component[times];

                for (int i = 0; i < times; i++)
                {
                    var gameObject = (GameObject)MonoBehaviour.Instantiate(asi.asetBundle.LoadAsset(name));

                    if (string.IsNullOrEmpty(gameobjectName))
                    {
                        gameObject.name = name;
                    }
                    else if (string.IsNullOrEmpty(gameobjectName) && !string.IsNullOrEmpty(name))
                    {
                        gameObject.name = gameobjectName;
                    }

                    var component = gameObject.GetComponent(binding.type);

                    if (component == null)
                    {
                        component = gameObject.AddComponent(binding.type);
                        componentList[i] = component;
                    }
                }

                if (handle != null) { handle(componentList); }
            }
            else
            {
                GameObject[] gameObjectList = new GameObject[times];

                for (int i = 0; i < times; i++)
                {
                    var gameObject = (GameObject)MonoBehaviour.Instantiate(asi.asetBundle.LoadAsset(name));

                    if (string.IsNullOrEmpty(gameobjectName))
                    {
                        gameObject.name = name;
                    }
                    else if (string.IsNullOrEmpty(gameobjectName) && !string.IsNullOrEmpty(name))
                    {
                        gameObject.name = gameobjectName;
                    }

                    gameObjectList[i] = gameObject;
                }

                if (handle != null) { handle(gameObjectList); }
            }
        }

        private static void TryInstantiateObject(
            this IBinding binding,
            Action<object> handle,
            int times,
            string name = null,
            string gameobjectName = null)
        {
            if (binding.value is UnityEngine.Object)
            {
                if (!binding.type.Equals(typeof(GameObject)))
                {
                    try
                    {
                        Component[] componentList = new Component[times];

                        for (int i = 0; i < times; i++)
                        {
                            var gameObject = (GameObject)MonoBehaviour.Instantiate((UnityEngine.Object)binding.value);


                            if (string.IsNullOrEmpty(gameobjectName))
                            {
                                gameObject.name = name;
                            }
                            else if (string.IsNullOrEmpty(gameobjectName) && !string.IsNullOrEmpty(name))
                            {
                                gameObject.name = gameobjectName;
                            }

                            var component = gameObject.GetComponent(binding.type);

                            if (component == null)
                            {
                                component = gameObject.AddComponent(binding.type);
                                componentList[i] = component;
                            }
                        }

                        if (handle != null) { handle(componentList); }
                    }
                    catch (Exception e) { throw (e); }
                }
                else
                {
                    try
                    {
                        GameObject[] gameObjectList = new GameObject[times];

                        for (int i = 0; i < times; i++)
                        {
                            var gameObject = (GameObject)MonoBehaviour.Instantiate((UnityEngine.Object)binding.value);


                            if (string.IsNullOrEmpty(gameobjectName))
                            {
                                gameObject.name = name;
                            }

                            else if (string.IsNullOrEmpty(gameobjectName) && !string.IsNullOrEmpty(name))
                            {
                                gameObject.name = gameobjectName;
                            }

                            gameObjectList[i] = gameObject;
                        }

                        if (handle != null) { handle(gameObjectList); }
                    }
                    catch (Exception e) { throw (e); }
                }
            }
            else
            {
                throw new Exceptions(
                    string.Format(Exceptions.NON_SPECIFIED_TYPE, "UnityEngine.Object"));
            }
        }

        #endregion

        #endregion
    }
}
