
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
using System;
using System.Collections.Generic;
using uMVVMCS.DIContainer;

namespace uMVVMCS
{
    /// <summary>
    /// 资源管理器 负责用字典管理资源信息AssetInfo，以及原始或异步、携程LOAD、实例化资源，但不对实例化后
    /// 的gameObject进行管理
    /// </summary>
    public class ResourcesCache
    {
        #region WWW Object

        private Dictionary<string, WWW> dicWwwObj;

        private WWW LoadResourcesWWW;

        #endregion

        private Dictionary<string, AssetInfo> infos;

        private ContextRoot root;

        #region constructor

        public ResourcesCache()
        {
            infos = new Dictionary<string, AssetInfo>();
            dicWwwObj = new Dictionary<string, WWW>();
        }

        #region property

        /// <summary>
        /// 用 path 来索引的索引器
        /// </summary>
        public ReflectionInfo this[string path] { get { return this.GetInfo(type); } }

        #endregion

        #endregion

        #region functions

        /// <summary>
        /// 为缓存字典添加指定的 Asset.
        /// </summary>
        public void Add(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            if (!this.Contains(path))
            {
                AssetInfo _assetInfo = new AssetInfo();
                _assetInfo.Path = path;
                infos.Add(path, _assetInfo);

                infos.Add(path, _assetInfo);

                //计算已经开启了几次该类型资源
                _assetInfo.RefCount++;
            }
        }

        /// <summary>
        /// 从缓存字典中移除指定的 Asset.
        /// </summary>
        public void Remove(string path)
        {
            if (this.Contains(path))
            {
                infos.Remove(path);
            }
        }

        /// <summary>
        /// 从缓存字典中获取指定的 Asset.
        /// </summary>
        public AssetInfo GetInfo(string path)
        {
            if (!this.Contains(path))
            {
                this.Add(path);
            }

            return this.infos[path];
        }

        /// <summary>
        /// 获取资源信息 重载1
        /// </summary>
        private AssetInfo GetAssetInfo(string _path)
        {
            return GetAssetInfo(_path, null);
        }

        /// <summary>
        /// 获取资源信息
        /// </summary>
        private AssetInfo GetAssetInfo(string _path, Action<UnityEngine.Object> _loaded)
        {
            if (string.IsNullOrEmpty(_path))
            {
                UnityEngine.Debug.LogError("Error: null _path name.");
                if (_loaded != null)
                    _loaded(null);
            }

            // Load Res....
            AssetInfo _assetInfo = null;
            // 如果字典中没有就新建，并将参数储存进去，并添加到字典 (TryGetValue成功则直接将获取到的值赋给形参)
            if (!infos.TryGetValue(_path, out _assetInfo))
            {
                _assetInfo = new AssetInfo();
                _assetInfo.Path = _path;
                infos.Add(_path, _assetInfo);
            }
            //计算已经开启了几次该类型资源
            _assetInfo.RefCount++;

            return _assetInfo;
        }

        #endregion




















        #region Load Resources

        /// <summary>
        /// 读取指定路径
        /// </summary>
        public UnityEngine.Object Load(string _path)
        {
            AssetInfo _assetInfo = GetAssetInfo(_path);

            if (_assetInfo != null)
                return _assetInfo.AssetObject;

            return null;
        }

        #endregion

        #region Load Async Resources

        /// <summary>
        /// 异步读取
        /// </summary>
        public void LoadAsync(string _path, Action<UnityEngine.Object> _loaded)
        {
            LoadAsync(_path, _loaded, null);
        }

        /// <summary>
        /// 异步读取（带配合进度条的委托） 重载1
        /// </summary>
        public void LoadAsync(string _path, Action<UnityEngine.Object> _loaded, Action<float> _progress)
        {
            AssetInfo _assetInfo = GetAssetInfo(_path, _loaded);

            if (_assetInfo != null)
                root.StartCoroutine(_assetInfo.GetAsyncObject(_loaded, _progress));
        }

        #endregion

        #region Load Resources & Instantiate Obj

        /// <summary>
        /// 读取并实例化
        /// </summary>
        public UnityEngine.Object LoadInstance(string _path)
        {
            UnityEngine.Object _obj = Load(_path);
            return Instantiate(_obj);
        }

        /// <summary>
        /// 协程读取并实例化
        /// </summary>
        public void LoadCoroutineInstance(string _path, Action<UnityEngine.Object> _loaded)
        {
            LoadCoroutine(_path, (_obj) => { Instantiate(_obj, _loaded); });
        }

        /// <summary>
        /// 异步读取并实例化
        /// </summary>
        public void LoadAsyncInstance(string _path, Action<UnityEngine.Object> _loaded)
        {
            LoadAsync(_path, (_obj) => { Instantiate(_obj, _loaded); });
        }

        /// <summary>
        /// 异步读取并实例化（带配合进度条的委托） 重载1
        /// </summary>
        public void LoadAsyncInstance(string _path, Action<UnityEngine.Object> _loaded, Action<float> _progress)
        {
            LoadAsync(_path, (_obj) => { Instantiate(_obj, _loaded); }, _progress);
        }

        #endregion

        #region Load Coroutine Resources

        /// <summary>
        /// 协程读取
        /// </summary>
        public void LoadCoroutine(string _path, Action<UnityEngine.Object> _loaded)
        {
            AssetInfo _assetInfo = GetAssetInfo(_path, _loaded);

            if (_assetInfo != null)
                root.StartCoroutine(_assetInfo.GetCoroutineObject(_loaded));
        }

        #endregion

        #region Get Asset Info & Instantiate Object

        /// <summary>
        /// 实例化物体 重载1
        /// </summary>
        private UnityEngine.Object Instantiate(UnityEngine.Object _obj)
        {
            return Instantiate(_obj, null);
        }

        /// <summary>
        /// 实例化物体
        /// </summary>
        private UnityEngine.Object Instantiate(UnityEngine.Object _obj, Action<UnityEngine.Object> _loaded)
        {
            UnityEngine.Object _retObj = null;
            if (_obj != null)
            {
                _retObj = UnityEngine.MonoBehaviour.Instantiate(_obj);

                if (_retObj != null)
                {
                    if (_loaded != null) { _loaded(_retObj); return null; }
                    return _retObj;
                }
                else { UnityEngine.Debug.LogError("Error: null Instantiate _retObj."); }
            }
            else { UnityEngine.Debug.LogError("Error: null Resources Load _obj."); }
            return null;
        }

        #endregion

        #region Unload Resources

        /// <summary>
        /// 从内存中移除指定路径资源
        /// </summary>
        public void Unload(string _path)
        {
            AssetInfo _assetInfo = GetAssetInfo(_path);

            if (infos.ContainsKey(_path))
                Resources.UnloadAsset(infos[_path].AssetObject);

            infos[_path] = null;
            infos.Remove(_path);
            //TODO:: AssetBundle unload
        }

        #endregion
    }
}
