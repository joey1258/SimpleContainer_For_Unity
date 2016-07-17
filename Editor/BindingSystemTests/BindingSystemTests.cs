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
    public class BindingSystemTests
    {
        /// <summary>
        /// 测试绑定两个同样的值到 TEMP 类型的无 id binding 会否被过滤掉，只保留1个
        /// </summary>
        [Test]
        public void BindTempNullIdTo_TwoSameValue_SameValueBeFiltered()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.Bind<object>().To(typeof(object));
            binder.Bind<object>().To(typeof(object));
            //Assert
            Assert.AreEqual(1, binder.GetAllBindings().Count);
        }

        /*/// <summary>
        /// 测试绑定实例的值到 TEMP 类型的 binding 其值是否会自动转换为 Type 类型
        /// </summary>
        [Test]
        public void BindTemp_ToInstanceValue_ConvertTOTypeValue()
        {
            //Arrange 
            IBinder binder = new Binder();
            IInfo testInfo = new Info(typeof(Binding), BindingType.TEMP);
            //Act
            binder.Bind<Info>().To(testInfo).As("test");
            //Assert
            Assert.AreEqual(true, binder.GetBinding<Info>("test").value is Type);
        }*/

        /// <summary>
        /// 测试绑定3个同样的值到 TEMP 类型的有 id binding 会全部保留
        /// </summary>
        [Test]
        public void BindTempToSelf_TwoSameValue_AllSave()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.Bind<Binding>().ToSelf().As("test1");
            binder.Bind<Binding>().ToSelf().As("test2");
            binder.Bind<Binding>().ToSelf().As("test3");
            //Assert
            Assert.AreEqual(3, binder.GetAllBindings().Count);
        }

        /// <summary>
        /// 测试绑定2个同样的值类型的值到 SINGLETON 类型的无 id binding 会否被过滤掉，只保留1个
        /// （值类型只能用 BindSingleton 类型 binding 来储存）
        /// </summary>
        [Test]
        public void BindSingletonNullIdTo_TwoSameValue_SameValueBeFiltered()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.BindSingleton<int>().To(1);
            binder.BindSingleton<int>().To(1);
            //Assert
            Assert.AreEqual(1, binder.GetAllBindings().Count);
        }

        /// <summary>
        /// 测试绑定3个同样的值到 SINGLETON 类型的有 id binding 会全部保留
        /// （值类型只能用 BindSingleton 类型 binding 来储存）
        /// </summary>
        [Test]
        public void BindSingletonTo_TwoSameValue_AllSave()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.BindSingleton<int>().To(1).As("test1");
            binder.BindSingleton<int>().To(1).As("test2");
            binder.BindSingleton<int>().To(1).As("test3");
            //Assert
            Assert.AreEqual(3, binder.GetAllBindings().Count);
        }

        /// <summary>
        /// 测试 BindSingleton、To 方法绑定的值取回后是否正确
        /// </summary>
        [Test]
        public void BindSingletonTo_GetValueByTypeAndId_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder.BindSingleton<string>().To("a").As("A");
            //Act
            string text = binder.GetBinding<string>("A").value as string;
            //Assert
            Assert.AreEqual("a", text);
        }

        /// <summary>
        /// 测试 BindFactory、To 方法绑定的值取回后是否正确(实例的 id 是否一致）
        /// </summary>
        [Test]
        public void BindFactoryTo_GetValueByTypeAndId_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder.BindFactory<someClass>().To(new someClass() { id = 10086 }).As("A");
            //Act
            int num = ((someClass)binder.GetBinding<someClass>("A").value).id;
            //Assert
            Assert.AreEqual(10086, num);
        }

        /*/// <summary>
        /// 测试 RemoveValue 是否正确的删除 binding 的值
        /// TEMP 类型的 binding 会将实例值转换为类型，移除后应该剩下 someClass_b
        /// </summary>
        [Test]
        public void RemoveValue_GetValueCount_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            IBinding binding = binder.Bind<someClass>()
                .To(new List<object>() {
                    new someClass(),
                    new someClass_b(),
                    new someClass_c() })
                .As("A");
            //Act
            binding.RemoveValues(new List<object>() { typeof(someClass) })
                .RemoveValue(typeof(someClass_c));
            //Assert
            Assert.AreEqual(typeof(someClass_b), binding.value);
        }*/

        /// <summary>
        /// 测试 BindSingleton To 多个值，后赋的值是否正确覆盖之前的值 
        /// </summary>
        [Test]
        public void BindSingletonTo_MultiValue_LastCoverBefore()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.BindSingleton<string>().To("c").To("b").To("a").As("A");
            //Assert
            Assert.AreEqual("a", binder.GetBinding<string>("A").value);
        }
    }

    public class someClass : IInjectionFactory
    {
        public int id;
        public object Create(InjectionContext context) { return this; }
    }

    public class someClass_b : someClass { }

    public class someClass_c : someClass { }
}


