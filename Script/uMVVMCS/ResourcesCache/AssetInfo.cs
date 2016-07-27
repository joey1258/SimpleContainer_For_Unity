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
    /// <summary>
    /// 资源信息类
    /// </summary>
    public class AssetInfo
    {
        /// <summary>
        /// 资源对象
        /// </summary>
        private UnityEngine.Object _Object;

        /// <summary>
        /// 资源类型(Class)
        /// </summary>
        public Type AssetType { get; set; }

        /// <summary>
        /// 资源路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 资源生成次数计数
        /// </summary>
        public int RefCount { get; set; }

        /// <summary>
        /// 通过资源对象是否为空来判断是否已经加载
        /// </summary>
        public bool IsLoaded
        {
            get { return _Object != null; }
        }

        /// <summary>
        /// 资源对象
        /// </summary>
        public UnityEngine.Object AssetObject
        {
            get
            {
                if (_Object == null) { _ResourcesLoad(); }
                return _Object;
            }
        }

        /// <summary>
        /// 协程加载资源
        /// </summary>
        public IEnumerator GetCoroutineObject(Action<UnityEngine.Object> _loaded)
        {
            while (true)
            {
                yield return null;
                if (_Object == null) { _ResourcesLoad(); yield return null; }
                if (_loaded != null) _loaded(_Object);
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
                _Object = UnityEngine.Resources.Load(Path);
                if (_Object == null)
                    UnityEngine.Debug.Log("Resources Load Failure! Path:" + Path);
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
        /// 异步加载资源
        /// </summary>
        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> _loaded, Action<float> _progress)
        {
            // 如果不为空就代表还有东西要加载
            if (_Object != null) { _loaded(_Object); yield break; }

            // 为空就代表已经加载完毕
            UnityEngine.ResourceRequest _resRequest = UnityEngine.Resources.LoadAsync(Path);

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

            // ???
            _Object = _resRequest.asset;
            if (_loaded != null)
                _loaded(_Object);

            yield return _resRequest;
        }
    }
}

