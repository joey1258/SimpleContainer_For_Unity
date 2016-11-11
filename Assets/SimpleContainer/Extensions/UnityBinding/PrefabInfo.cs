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
using System.Collections;
using Utils;

namespace SimpleContainer.Container
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
        public int useCount { get; set; }

        /// <summary>
        /// 资源对象是否已经加载
        /// </summary>
        public bool isLoaded
        {
            get { return _prefab != null; }
        }

        #endregion

        #region functions

        /// <summary>
        /// 加载资源
        /// </summary>
        private void ResourcesLoad()
        {
            _prefab = UnityEngine.Resources.Load(path);
            if (_prefab == null)
            {
                throw new Exceptions(
                    string.Format(Exceptions.RESOURCES_LOAD_FAILURE, path));
            }
        }

        /// <summary>
        /// 协程加载资源
        /// </summary>
        public IEnumerator GetCoroutineObject(Action<UnityEngine.Object> handle)
        {
            while (true)
            {
                yield return null;
                if (_prefab == null) { ResourcesLoad(); yield return null; }
                if (handle != null) { handle(_prefab); }
                yield break;
            }
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> handle)
        {
            return GetAsyncObject(handle, null);
        }

        /// <summary>
        /// 异步加载资源(带进度条功能)
        /// </summary>
        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> handle, Action<float> progress)
        {
            // 如果 _prefab 不为空说明已经读取完成，执行 yield break 之后不再执行下面语句  
            if (_prefab != null) { handle(_prefab); yield break; }
            
            UnityEngine.ResourceRequest resRequest = UnityEngine.Resources.LoadAsync(path);

            while (!resRequest.isDone)
            {
                if (progress != null) { progress(resRequest.progress); }
                yield return null;
            }
            _prefab = resRequest.asset;

            if (handle != null) { handle(_prefab); }
            yield return resRequest;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void UnloadAsset()
        {
            UnityEngine.Resources.UnloadAsset(_prefab);
            _prefab = null;
        }

        #endregion
    }
}