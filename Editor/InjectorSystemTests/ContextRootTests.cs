using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using uMVVMCS;
using uMVVMCS.DIContainer;

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
