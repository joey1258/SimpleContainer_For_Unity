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
        /// 测试绑定到 TEMP 类型的无 id binding 会否被忽略，不保留在 binder 中
        /// 既然是临时 binding，保存在字典中又没有高效的删除方法，不如即用即弃，让GC自然回收
        /// 如需反复使用，不建议使用 TEMP 类型，或为其加上 id
        /// </summary>
        [Test]
        public void BindTempNullIdTo_ToSomeValue_BinderDoNotSave()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.Bind<object>().ToSelf();
            binder.Bind<object>().ToSelf();
            //Assert
            Assert.AreEqual(0, binder.GetAllBindings().Count);
        }

        /// <summary>
        /// 测试 ToSelf 方法是否使 TEMP 类型的 binding 的值约束转换为 SINGLE 类型
        /// 由于绑定自身的type作为自己的值的缘故，自身的值只有一个，所以值的类型也没有必要保留为 MULTIPLE
        /// </summary>
        [Test]
        public void BindTempNullId_ToSelf_ConstraintChangeTOSingle()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            IBinding binding = binder.Bind<object>().ToSelf();
            //Assert
            Assert.AreEqual(ConstraintType.SINGLE, binding.constraint);
        }

        /// <summary>
        /// 测试绑定实例到 TEMP 类型的 binding，其值约束是否任保持为 MULTIPLE 类型
        /// </summary>
        [Test]
        public void BindTempNullIdTo_Type_ConstraintStayMultiple()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            IBinding binding = binder.Bind<object>().To<Binding>();
            //Assert
            Assert.AreEqual(ConstraintType.MULTIPLE, binding.constraint);
        }

        /// <summary>
        /// 测试绑定实例的值到 TEMP 类型的 binding,binding 是否会自动转换为 SINGLETON 类型
        /// </summary>
        [Test]
        public void BindTempNullIdTo_Instance_BindingChangeTOSingleton()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.Bind<int>().To(1).As(1);
            //Assert
            Assert.AreEqual(BindingType.SINGLETON, binder.GetBinding<int>(1).bindingType);
        }

        /// <summary>
        /// 测试绑定实例的值到 TEMP 类型的 binding,其值约束是否会自动转换为 SINGLE 类型
        /// </summary>
        [Test]
        public void BindTempNullIdTo_Instance_ConstraintChangeTOSingle()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.Bind<int>().To(1).As(1);
            //Assert
            Assert.AreEqual(ConstraintType.SINGLE, binder.GetBinding<int>(1).constraint);
        }

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

        /// <summary>
        /// 测试 RemoveValue 是否正确的删除 binding 的值
        /// TEMP 类型的 binding 会将实例值转换为类型，移除后应该剩下 someClass_b
        /// </summary>
        [Test]
        public void RemoveValue_GetValueCount_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            someClass sc = new someClass();
            someClass scb = new someClass();
            someClass scc = new someClass();
            IBinding binding = binder.Bind<someClass>()
                .To(new List<object>() { sc, scb, scc }).As("A");
            //Act
            binding.RemoveValues(new List<object>() { sc })
                .RemoveValue(scc);
            //Assert
            Assert.AreEqual(scb, binding.value);
        }

        /// <summary>
        /// 测试链式 Bind 各种类型 To 各种值，最后 binder 中储存的 binding 是否正确
        /// 绑定的第一个值是 TEMP 类型且没有 id，因此不被 binder 储存，所以总数应该是3
        /// </summary>
        [Test]
        public void BindChain_EveryType_BinderCountCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder.Bind<someClass>()
                .ToSelf()
                .Bind<int>()
                .To(1)
                .BindSingleton<someClass>()
                .To(new someClass() { id = 10086 })
                .As("A")
                .BindFactory< someClass_b>()
                .To(new someClass() { id = 110 })
                .As("b");
            //Act
            int num = binder.GetAllBindings().Count;
            //Assert
            Assert.AreEqual(3, num);
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


