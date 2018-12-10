/**
 * This file is part of SimpleContainer.
 *
 * Licensed under The MIT License
 * For full copyright and license information, please see the MIT-LICENSE.txt
 * Redistributions of files must retain the above copyright notice.
 *
 * @copyright Joey1258
 * @link https://github.com/joey1258
 * @license http://www.opensource.org/licenses/mit-license.php MIT License
 */

/**
* LoadFromMemory_NW：创建完成后只会在内存中创建较小的 SerializedFile，但后续 load 等操作时仍然需要进行
* IO 操作
*
* LoadFromMemory：加载Bundle文件并获取WWW对象，完成后会在内存中创建较大的WebStream（解压后的内容，通常
* 为原Bundle文件的4~5倍大小，纹理资源比例可能更大），但后续 load 等操作可以直接在内存中进行.
* 
* LoadFromMemory_LFCOD：加载 Bundle 文件并获取 WWW 对象，同时将解压形式的 Bundle 内容存入磁盘中作为缓
* 存（如果该 Bundle 已在缓存中，则省去这一步），完成后只会在内存中创建较小的 SerializedFile ，而后续的
* AssetBundle.Load 需要通过 IO 从磁盘中的缓存获取。
* 
* LoadFromMemory_NW 与 LoadFromMemory_LFCOD 的对比：
* 1.  LoadFromMemory_NW 后续的 Load 操作在内存中进行，相比 LoadFromMemory_LFCOD 的 IO 操作开销更小；
* 2.  LoadFromMemory_NW 不形成缓存文件，而 LoadFromMemory_LFCOD 则需要额外的磁盘空间存放缓存；
* 3.  new WWW 能通过 WWW.texture，WWW.bytes，WWW.audioClip 等接口直接加载外部资源，而 
*     WWW LoadFromCacheOrDownload 只能用于加载 AssetBundle
* 4.  LoadFromMemory_NW 每次加载都涉及到解压操作，而 LoadFromMemory_LFCOD 在第二次加载时就省去了解压的
*     开销；
* 5.  LoadFromMemory_NW 在内存中会有较大的 WebStream，而后者在内存中只有通常较小的 SerializedFile。
*     （此项为一般情况，但并不绝对，对于序列化信息较多的 Prefab，很可能出现 SerializedFile比WebStream
*     更大的情况）
*/

using UnityEngine;
using System;
using System.Collections;
using Utils;

namespace SimpleContainer.Container
{
    public class AssetBundleInfo
    {
        #region property

        /// <summary>
        /// 资源对象
        /// </summary>
        public AssetBundle asetBundle
        {
            get
            {
                if (_asetBundle == null) { LoadFromFile(); }
                return _asetBundle;
            }
        }
        private AssetBundle _asetBundle;

        /// <summary>
        /// 资源路径
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 资源对象是否已经加载
        /// </summary>
        public bool isLoaded
        {
            get { return _asetBundle != null; }
        }

        #endregion

        #region constructor

        public AssetBundleInfo(AssetBundle asetBundle, string url)
        {
            _asetBundle = asetBundle;
            this.url = url;
        }

        public AssetBundleInfo(string url)
        {
            this.url = url;
        }

        #endregion

        #region functions

        #region Load from file

        /// <summary>
        /// 从 IO 加载 AssetBundle
        /// </summary>
        public void LoadFromFile()
        {
            _asetBundle = AssetBundle.LoadFromFile(url);
            if (_asetBundle == null)
            {
                throw new Exceptions(
                    string.Format(Exceptions.ASSETBUNDLE_LOAD_FAILURE, url));
            }
        }

        #endregion

        #region load from memory nw

        /// <summary>
        /// 从内存加载 AssetBundle(new WWW)
        /// </summary>
        public IEnumerator LoadFromMemory_NW()
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.error != null)
            {
                throw new Exception("WWW download had an error:" + www.error);
            }

            _asetBundle = AssetBundle.LoadFromMemory(www.bytes);
            if (_asetBundle == null)
            {
                throw new Exceptions(
                    string.Format(Exceptions.ASSETBUNDLE_LOAD_FAILURE, url));
            }
        }

        #endregion

        #region load from cache or download

        /// <summary>
        /// 从内存加载 AssetBundle(WWW.LoadFromCacheOrDownload)
        /// </summary>
        public void LoadFromCacheOrDownload()
        {
            WWW www = WWW.LoadFromCacheOrDownload(url, 5);
            if (www.error != null)
            {
                throw new Exception("WWW download had an error:" + www.error);
            }

            _asetBundle = www.assetBundle;

            if (_asetBundle == null)
            {
                throw new Exceptions(
                    string.Format(Exceptions.ASSETBUNDLE_LOAD_FAILURE, url));
            }
        }

        #endregion

        /// <summary>
        /// 释放资源(当参数为假时，asset 内的数据将被清除，之后无法再从 asset 进行任何加载，但已经加载的保持不变，为真时则清除所有，即使已经加载了的一并清除，对它们的引用将会失效)
        /// </summary>
        public void Dispose(bool unloadAllLoadedObjects)
        {
            _asetBundle.Unload(unloadAllLoadedObjects);
            _asetBundle = null;
        }

        #endregion
    }
}
