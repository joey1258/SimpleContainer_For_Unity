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

namespace uMVVMCS
{
    public class AssetInfo
    {
        #region property

        /// <summary>
        /// 资源类型(Class)
        /// </summary>
        public Type assetType { get; set; }

        /// <summary>
        /// 资源路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 资源生成次数计数
        /// </summary>
        public int refCount { get; set; }

        /// <summary>
        /// 通过资源对象是否为空来判断是否已经加载
        /// </summary>
        public bool isLoaded
        {
            get { return _asset != null; }
        }

        /// <summary>
        /// 资源对象
        /// </summary>
        public UnityEngine.Object asset
        {
            get
            {
                if (_asset == null) { _ResourcesLoad(); }
                return _asset;
            }
        }
        private UnityEngine.Object _asset;

        #endregion

        #region functions

        /// <summary>
        /// 协程加载资源
        /// </summary>
        public IEnumerator GetCoroutineObject(Action<UnityEngine.Object> _loaded)
        {
            while (true)
            {
                yield return null;
                if (_asset == null) { _ResourcesLoad(); yield return null; }
                if (_loaded != null) _loaded(_asset);
                yield break;
            }
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        private void _ResourcesLoad()
        {
            try
            {
                _asset = UnityEngine.Resources.Load(path);
                if (_asset == null)
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
        /// 异步加载资源
        /// </summary>
        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> _loaded)
        {
            return GetAsyncObject(_loaded, null);
        }

        /// <summary>
        /// 异步加载资源(带进度条功能)
        /// </summary>
        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> _loaded, Action<float> _progress)
        {
            if (_asset != null) { _loaded(_asset); yield break; }
            
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

            // 
            _asset = _resRequest.asset;
            if (_loaded != null)
                _loaded(_asset);

            yield return _resRequest;
        }

        #endregion
    }
}

