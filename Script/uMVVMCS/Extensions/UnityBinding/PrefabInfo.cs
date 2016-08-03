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
using System.Collections;

namespace uMVVMCS.DIContainer
{
    public class PrefabInfo
    {
        #region constructor

        public PrefabInfo(UnityEngine.Object prefab, string path, Type type)
        {
            _prefab = prefab;
            this.path = path;
            this.type = type;
        }

        public PrefabInfo(string path, Type type)
        {
            this.path = path;
            this.type = type;
        }

        #endregion

        #region property

        /// <summary>
        /// 资源对象
        /// </summary>
        public UnityEngine.Object prefab
        {
            get
            {
                if (_prefab == null) { ResourcesLoad(); }
                return _prefab;
            }
        }
        private UnityEngine.Object _prefab;

        /// <summary>
        /// prefab 上的组件
        /// </summary>
        public Type type { get; set; }

        /// <summary>
        /// 资源路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 资源生成次数计数
        /// </summary>
        public int refCount { get; set; }

        /// <summary>
        /// 资源对象是否已经加载
        /// </summary>
        public bool isLoaded
        {
            get { return prefab != null; }
        }

        #endregion

        #region functions

        /// <summary>
        /// 加载资源
        /// </summary>
        private void ResourcesLoad()
        {
            try
            {
                _prefab = UnityEngine.Resources.Load(path);
                if (_prefab == null)
                {
                    UnityEngine.Debug.Log("Resources Load Failure! path:" + path);
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e.ToString());
            }
        }

        /// <summary>
        /// 协程加载资源
        /// </summary>
        public IEnumerator GetCoroutineObject(Action<UnityEngine.Object> _loader)
        {
            while (true)
            {
                yield return null;
                if (_prefab == null) { ResourcesLoad(); yield return null; }
                if (_loader != null) _loader(_prefab);
                yield break;
            }
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> _loader)
        {
            return GetAsyncObject(_loader, null);
        }

        /// <summary>
        /// 异步加载资源(带进度条功能)
        /// </summary>
        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> _loader, Action<float> _progress)
        {
            // 如果 _prefab 不为空说明已经读取完成，执行 yield break 之后不再执行下面语句  
            if (_prefab != null) { _loader(_prefab); yield break; }
            
            UnityEngine.ResourceRequest _resRequest = UnityEngine.Resources.LoadAsync(path);

            // 进度判断值不能为1，否则会卡住
            while (_resRequest.progress < 0.9)
            {
                if (_progress != null)
                    _progress(_resRequest.progress);
                yield return null;
            }

            // 在0.9~1之间如果未完成继续读取
            while (!_resRequest.isDone)
            {
                if (_progress != null)
                    _progress(_resRequest.progress);
                yield return null;
            }

            _prefab = _resRequest.asset;

            if (_loader != null) { _loader(_prefab); }

            yield return _resRequest;
        }

        #endregion
    }
}