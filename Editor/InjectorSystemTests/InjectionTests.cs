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
        /*/// <summary>
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
        }*/
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
