
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

        /*private Dictionary<string, WWW> dicWwwObj;

        private WWW LoadResourcesWWW;*/

        #endregion

        private Dictionary<string, AssetInfo> infos;

        private ContextRoot root;

        #region constructor

        public ResourcesCache()
        {
            infos = new Dictionary<string, AssetInfo>();
            //dicWwwObj = new Dictionary<string, WWW>();
        }

        #region property

        /// <summary>
        /// 用 path 来索引的索引器
        /// </summary>
        public object this[string path] { get { return GetInfo(path).asset; } }

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

            if (!Contains(path))
            {
                AssetInfo _assetInfo = new AssetInfo();
                _assetInfo.path = path;
                infos.Add(path, _assetInfo);

                infos.Add(path, _assetInfo);
            }
        }

        /// <summary>
        /// 从缓存字典中移除指定的 Asset.
        /// </summary>
        public void Remove(string path)
        {
            if (Contains(path))
            {
                infos.Remove(path);
            }
        }

        /// <summary>
        /// 从缓存字典中获取指定的 Asset.
        /// </summary>
        public AssetInfo GetInfo(string path)
        {
            if (!Contains(path))
            {
                Add(path);
            }

            //计算已经开启了几次该类型资源
            infos[path].refCount++;
            return infos[path];
        }

        /// <summary>
        /// 返回当前缓存字典中是否存在指定类型的实例
        /// </summary>
        public bool Contains(string path)
        {
            return infos.ContainsKey(path);
        }

        /// <summary>
        /// 从内存中移除指定路径资源
        /// </summary>
        public void Unload(string path)
        {
            if (infos.ContainsKey(path))
            {
                Resources.UnloadAsset(infos[path].asset);
            }

            infos[path] = null;
            infos.Remove(path);
        }

        #endregion
    }
}
