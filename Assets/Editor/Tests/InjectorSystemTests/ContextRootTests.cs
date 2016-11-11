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

using UnityEngine;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleContainer;
using SimpleContainer.Container;

namespace uMVVMCS_NUitTests
{
    [TestFixture]
    public class ContextRootTests : MonoBehaviour
    {
        /// <summary>
        /// 测试 ReflectionFactory.Create 方法创建的 type 属性是否正确创建
        /// </summary>
        [Test]
        public void SetupContainers_setInt_Correct()
        {
            //Arrange 
            //IContextRoot root = GameObject.Find("Main Camera").GetComponent<TestsRoot_a>() as IContextRoot;
            //Act
            //int num = root.container.Resolve<TestsFunctions>()
            //Assert
            //Assert.AreEqual(typeof(someClass_d), reflectionInfo.type);
        }
    }
}
