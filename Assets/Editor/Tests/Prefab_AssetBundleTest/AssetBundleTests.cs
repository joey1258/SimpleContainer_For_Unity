/**
 * This file is part of SimpleContainer.
 *
 * Licensed under The MIT License
 * For full copyright and license information, please see the MIT-LICENSE.txt
 * Redistributions of files must retain the above copyright notice.
 *
 * @copyright Joey1258
 * @link https://github.com/joey1258/ToluaContainer
 * @license http://www.opensource.org/licenses/mit-license.php MIT License
 */
 
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ToluaContainer;
using ToluaContainer.Container;

namespace uMVVMCS_NUitTests
{
    [TestFixture]
    public class AssetBundleTests
    {
        /// <summary>
        /// 测试 ToAssetBundleFromFile 方法绑定 AssetBundleInfo 是否成功
        /// </summary>
        [Test]
        public void ToAssetBundleFromFile_AssetBundleInfo_AssetBundleNoNull()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            IBinding binding = binder.Bind<AssetBundleInfo>().ToAssetBundleFromFile(Application.dataPath + "/Editor/Tests/Prefab_AssetBundleTest/cube.prefab.unity3d");
            //Assert
            Assert.AreEqual(
                true,
                ((AssetBundleInfo)binding.value).asetBundle != null);
            ((AssetBundleInfo)binding.value).Dispose(true);
        }

        /// <summary>
        /// 涉及到异步或携程的方法需要 mono 单例，因而无法在非运行时测试
        /// </summary>

        /// <summary>
        /// 测试 ToAssetBundleFromNewWWW 方法绑定 AssetBundleInfo 是否成功
        /// </summary>
        [Test]
        public void ToAssetBundleFromNewWWW_AssetBundleInfo_AssetBundleNoNull()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            IBinding binding = binder.Bind<AssetBundleInfo>().ToAssetBundleFromNewWWW(Application.dataPath + "/Editor/Tests/Prefab_AssetBundleTest/cube.prefab.unity3d");
            //Assert
            Assert.AreEqual(
                true,
                ((AssetBundleInfo)binding.value).asetBundle != null);
            ((AssetBundleInfo)binding.value).Dispose(true);
        }

        /// <summary>
        /// 测试 LoadFromCacheOrDownload 相关方法无法在非运行时测试
        /// </summary>
    }
}
