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
        #region Bind & To & GetBinding

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
        /// 测试一次绑定多个无 id 的 binding 后 binder 保存数量是否正确
        /// 由于第一个绑定的是 TEMP 类型且值也是类型、并且没有 id，所以 binder 只储存了2个
        /// </summary>
        [Test]
        public void MultipleBindNUllId_BindingsCount_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder.MultipleBind(
                new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new List<object>() { typeof(someClass_b), 1, new someClass() });
            //Act
            int num = binder.GetAllBindings().Count;
            //Assert
            Assert.AreEqual(2, num);
        }

        #endregion

        #region To Chain 

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
                .BindFactory<someClass_b>()
                .To(new someClass() { id = 110 })
                .As("b");
            //Act
            int num = binder.GetAllBindings().Count;
            //Assert
            Assert.AreEqual(3, num);
        }

        /// <summary>
        /// 测试绑定多个无 id 的 binding 后可否控制对具体 binding 的赋值数量
        /// id 为2的 binding 的值有2个，id 为 5 的有3个，总数应该有5个
        /// </summary>
        [Test]
        public void MultipleBind_ToSingleAndMulpitleValue_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder.MultipleBind(
                new List<Type>() { typeof(someClass_b), typeof(object), typeof(someClass) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new List<object>() { typeof(someClass_b), ((object[])new object[] { 111, 222, 333 }), new someClass() })
                    .As(new List<object>() { null, 1, 2 })
                    .Bind<someClass>().ToSelf()
                    .MultipleBind(
                new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new List<object>() { typeof(someClass_b), 1, new someClass() })
                    .Bind<someClass>().To(new someClass()).As(3)
                    .MultipleBind(
                new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new List<object>() { typeof(someClass_b), new int[] { 444, 555, 666 }, new someClass() })
                    .As(new List<object>() { null, 4, 5 })
                    .Bind<someClass>().ToSelf();
            //Act
            int num = binder.GetBinding<someClass>(1).valueArray.Length +
                binder.GetBinding<someClass>(4).valueArray.Length;
            //Assert
            Assert.AreEqual(5, num);
        }

        #endregion

        #region Bind Chain

        /// <summary>
        /// 测试一次绑定多个无 id 的 binding 并赋值后再次绑定多个无 id binding，binder 保存数量是否正确
        /// 由于有两个 TEMP 类型且值也是类型、并且没有 id 的 binding，所以 binder 只储存了4个
        /// </summary>
        [Test]
        public void MultipleBindNUllIdTo_ChainMultipleBind_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder.MultipleBind(
                new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new List<object>() { typeof(someClass_b), 1, new someClass() })
                    .MultipleBind(
                new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new List<object>() { typeof(someClass_b), 1, new someClass() });
            //Act
            int num = binder.GetAllBindings().Count;
            //Assert
            Assert.AreEqual(4, num);
        }

        /// <summary>
        /// 测试绑定多个有、无 id 的 binding 往复链式操作，binder 保存数量是否正确
        /// 由于有3个 TEMP 类型且值也是类型、并且没有 id 的 binding，所以 binder 只储存了4个
        /// </summary>
        [Test]
        public void MultipleBind_ChainMultipleBind_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder.MultipleBind(
                new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new List<object>() { typeof(someClass_b), 1, new someClass() })
                    .As(new List<object>() { null, 1, 2 })
                    .MultipleBind(
                new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new List<object>() { typeof(someClass_b), 1, new someClass() })
                    .MultipleBind(
                new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new List<object>() { typeof(someClass_b), 1, new someClass() })
                    .As(new List<object>() { null, 3, 4 });
            //Act
            int num = binder.GetAllBindings().Count;
            //Assert
            Assert.AreEqual(6, num);
        }

        /// <summary>
        /// 测试绑定多个有或无 id 的 binding 并赋值后再次绑定单个有或无 id 的 binding，并多次
        /// 链式循环操作后 binder 保存数量是否正确
        /// </summary>
        [Test]
        public void MultipleBindTo_ChainNomalBind_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder.MultipleBind(
                new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new List<object>() { typeof(someClass_b), 1, new someClass() })
                    .As(new List<object>() { null, 1, 2 })
                    .Bind<someClass>().ToSelf()
                    .MultipleBind(
                new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new List<object>() { typeof(someClass_b), 1, new someClass() })
                    .Bind<someClass>().To(new someClass()).As(3)
                    .MultipleBind(
                new List<Type>() { typeof(someClass_b), typeof(int), typeof(someClass) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new List<object>() { typeof(someClass_b), 1, new someClass() })
                    .As(new List<object>() { null, 4, 5 })
                    .Bind<someClass>().ToSelf();
            //Act
            int num = binder.GetAllBindings().Count;
            //Assert
            Assert.AreEqual(7, num);
        }

        #endregion

        #region GetValue

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
        /// 测试一次绑定多个无 id 的 binding 取值后相加是否正确
        /// 虽然都是 TEMP 类型，但 To（object） 方法会自动转换 bindingType 和值约束类型
        /// </summary>
        [Test]
        public void MultipleBindNullId_ValueAdd_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder.MultipleBind(
                new List<Type>() { typeof(float), typeof(float), typeof(float) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.TEMP,
                    BindingType.TEMP })
                    .To(new List<object>() { 1f, 2f, 3.3f });

            IList<IBinding> bindings = binder.GetAllBindings();
            int length = bindings.Count;
            float num = 0;
            //Act
            for (int i = 0; i < length; i++)
            {
                num += (float)bindings[i].value;
            }

            //Assert
            Assert.AreEqual(6.3f, num);
        }

        /// <summary>
        /// 测试一次绑定多个 binding 取值后相加是否正确
        /// 虽然都是 TEMP 类型，但 To（object） 方法会自动转换 bindingType 和值约束类型
        /// </summary>
        [Test]
        public void MultipleBind_ValueAdd_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder.MultipleBind(
                new List<Type>() { typeof(int), typeof(int), typeof(float) },
                new List<BindingType>() {
                    BindingType.TEMP,
                    BindingType.TEMP,
                    BindingType.TEMP })
                    .To(new List<object>() { 1, 2, 3.3f })
                    .As(new List<object>() { 1, 2, 3 });
            //Act
            float num = (int)binder.GetBinding<int>(1).value +
                (int)binder.GetBinding<int>(2).value +
                (float)binder.GetBinding<float>(3).value;
            //Assert
            Assert.AreEqual(6.3f, num);
        }

        #endregion

        #region ReMove & UnBInd

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

        #endregion

    }

    public class someClass : IInjectionFactory
    {
        public int id;
        public object Create(InjectionContext context) { return this; }
    }

    public class someClass_b : someClass { }

    public class someClass_c : someClass { }
}


