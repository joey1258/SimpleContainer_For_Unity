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
    public class InjectionTests : MonoBehaviour
    {
        /// <summary>
        /// 测试 ReflectionFactory.Create 方法创建的 type 属性是否正确创建
        /// </summary>
        [Test]
        public void Inject_VToVM_Correct()
        {
            //Arrange 
            IInjector injector = new Injector(new ReflectionCache(), new InjectionBinder());
            IReflectionCache cache = new ReflectionCache();
            testV tv = new testV(1);
            testVM tvm = new testVM(2);
            //Act
            injector.Inject(tvm);
            //Assert
            //Assert.AreEqual(typeof(someClass_d), reflectionInfo.type);
        }
    }

    public class testV
    {
        public int id;
        [Inject]
        public testVM VM;
        public testV(int id) { this.id = id; }
    }

    public class testVM
    {
        public int id;
        [Inject]
        public testV V;
        public testVM(int id) { this.id = id; }
    }
}
